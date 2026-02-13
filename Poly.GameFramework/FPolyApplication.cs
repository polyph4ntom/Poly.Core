using System;
using System.Threading.Tasks;
using Poly.Common;
using Poly.GameFramework.Core;
using Poly.Log;
using Poly.Settings;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Poly.GameFramework
{
    internal static class FPolyApplication
    {
        internal static FPolyGameInstanceBase GameInstance { get; private set; }
        internal static FPolyLoadingController LoadingController { get; private set; }
        internal static FPolyWorldContext WorldContext { get; private set; }
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void OnAppStarted()
        {
            
#if UNITY_EDITOR
            var disableArchitecture = EditorPrefs.GetBool(FPolyGameFrameworkCoreAssembly.DISABLE_ARCHITECTURE);
            if (disableArchitecture)
            {
                return;
            }
#endif
            LoadingController = new FPolyLoadingController();

            SceneManager.activeSceneChanged += OnActiveSceneChanged;
            AppDomain.CurrentDomain.ProcessExit += OnProcessExit;
        }

        private static void OnActiveSceneChanged(Scene oldScene, Scene newScene)
        {
            if (newScene.buildIndex == FPolyGameFrameworkCoreAssembly.LOADING_SCENE_INDEX)
            {
                return;
            }
            
            _ = OnActiveSceneChangedAsync(newScene);
        }

        private static void OnProcessExit(object sender, EventArgs e)
        {
            WorldContext?.Invalidate();
            GameInstance?.Dispose();
        }

        private static async Task OnActiveSceneChangedAsync(Scene scene)
        {
            try
            {
                await Task.WhenAll(LoadingController.WaitForLoadingScene(), FPolyDevSettingsDatabase.WaitForDatabase());
                var coreClassSet = FPolyDevSettingsDatabase.Get<FPolyCoreSettings>().RetrieveSet(scene.path);
                if (GameInstance == null)
                {
                    GameInstance = (FPolyGameInstanceBase) Activator.CreateInstance(coreClassSet.GameInstance.Type);
                    GameInstance.InitializeInternalClasses(coreClassSet);
                    GameInstance.PersistentEventBus.Subscribe<FPolyRequestNewLevel>(LoadingController, LoadingController.RequestNewLevel);
                    await GameInstance.InitializeAsync();
                }
                
                WorldContext?.Invalidate();
                WorldContext = await GameInstance.EnsureAppReadyThenSpawnNewWorld(scene, coreClassSet);
            }
            catch (Exception e)
            {
                FPolyLog.Error("Poly.GameFramework","OnActiveSceneChangedAsync failed: " + e);
                throw;
            }
        }
    }
}