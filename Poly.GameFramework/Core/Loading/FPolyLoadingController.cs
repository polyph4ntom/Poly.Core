using System.Collections;
using System.Threading.Tasks;
using Poly.Common;
using Poly.Log;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Poly.GameFramework.Core
{
    public class FPolyLoadingController
    {
        private readonly TaskCompletionSource<bool> isLoadingSceneReady = new();
        
        private APolyLoadingComponent loadingComponent;
        private Camera loadingSceneCamera;
        
        internal Task WaitForLoadingScene() => isLoadingSceneReady.Task;

        public FPolyLoadingController()
        {
            SceneManager.sceneLoaded += OnSceneLoaded; 
            SceneManager.LoadScene(FPolyGameFrameworkCoreAssembly.LOADING_SCENE_INDEX, LoadSceneMode.Additive);
        }
        
        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (scene.buildIndex != FPolyGameFrameworkCoreAssembly.LOADING_SCENE_INDEX)
            {
                return;
            }

            foreach (var obj in scene.GetRootGameObjects())
            {
                if (loadingComponent == null)
                {
                    loadingComponent = obj.GetComponent<APolyLoadingComponent>();
                }

                if (loadingSceneCamera == null)
                {
                    loadingSceneCamera = obj.GetComponent<Camera>();
                }
            }
            
            if (loadingComponent == null)
            {
                FPolyLog.Error("Poly.GameFramework.Core", "OnSceneLoaded called with no APolyLoadingComponent! on a LoadingScene. It should be the first object in the hierarchy!");
                return;
            }
            
            if (loadingSceneCamera == null)
            {
                FPolyLog.Error("Poly.GameFramework.Core", "OnSceneLoaded called with no Camera! on a LoadingScene. It should be the second object in the hierarchy!");
                return;
            }

            loadingComponent.HideWorld(true);
            loadingSceneCamera.gameObject.SetActive(true);
            SceneManager.sceneLoaded -= OnSceneLoaded;
        
            isLoadingSceneReady.SetResult(true);
        }

        public void EndLoadingProcess(bool immediate)
        {
            loadingComponent.StartCoroutine(DelayActivation(1));
        }

        public void RequestNewLevel(FPolyRequestNewLevel evt)
        {
            loadingComponent.SetLevelData(evt.LevelName, evt.LevelDescription);
            loadingComponent.HideWorld();

            loadingComponent.StartCoroutine(LoadNewLevel(evt.World, evt.RequestedScene.ScenePath));
        }

        private IEnumerator LoadNewLevel(APolyWorldBase world, string levelPath)
        {
            yield return new WaitForSeconds(0.5f);
            world.Invalidate();
            
            var currentScene = SceneManager.GetActiveScene();
            yield return SceneManager.UnloadSceneAsync(currentScene);
        
            loadingSceneCamera.gameObject.SetActive(true);
        
            var loadOp = SceneManager.LoadSceneAsync(levelPath, LoadSceneMode.Additive);
            yield return loadOp;

            SceneManager.SetActiveScene(SceneManager.GetSceneByPath(levelPath));
        }

        private IEnumerator DelayActivation(float delay)
        {
            loadingComponent.FillProgressBar();
            loadingSceneCamera.gameObject.SetActive(false);
            yield return new WaitForSeconds(delay);
            loadingComponent.ShowWorld();
        }
    }
}
