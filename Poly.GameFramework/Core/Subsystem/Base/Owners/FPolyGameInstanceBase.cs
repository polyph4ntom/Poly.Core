using System;
using System.Threading.Tasks;
using Poly.Common;
using Poly.Events;
using Poly.Log;
using Poly.Settings;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Poly.GameFramework.Core
{
    public class FPolyGameInstanceBase : FPolySubsystemOwner<FPolyGameInstanceSubsystem>
    {
        internal FPolyLocalPlayerBase localPlayer;
    
        public FPolyEventBus PersistentEventBus { get; private set; }

        protected internal void InitializeInternalClasses(OPolyCoreClassSet coreClassSet)
        {
            localPlayer = (FPolyLocalPlayerBase) Activator.CreateInstance(coreClassSet.LocalPlayer.Type);
            PersistentEventBus = new FPolyEventBus();
        }

        protected internal override async Task InitializeAsync()
        {
            try
            {
                await localPlayer.InitializeAsync();
                await base.InitializeAsync();
            }
            catch (Exception e)
            {
                FPolyLog.Error("Poly.GameFramework.Core", "FPolyGameInstanceBase init failed: " + e);
                throw;
            }
        }
    
        protected internal async Task<FPolyWorldContext> EnsureAppReadyThenSpawnNewWorld(Scene scene, OPolyCoreClassSet coreClassSet)
        {
            try
            {
                var worldContext = new FPolyWorldContext(scene.path);
            
                await Task.WhenAll(FPolyDevSettingsDatabase.WaitForDatabase(), localPlayer.WaitForSubsystems());
                await SpawnNewWorld(worldContext, coreClassSet);
            
                return worldContext;
            }
            catch (Exception e)
            {
                FPolyLog.Error("Poly.GameFramework", "Spawn new world failed: " + e);
                throw;
            }
        }
    
        private async Task SpawnNewWorld(FPolyWorldContext worldContext, OPolyCoreClassSet coreClassSet)
        {
            var worldObject = new GameObject("PolyWorld");
            worldObject.transform.SetAsFirstSibling();

            var world = (APolyWorldBase) worldObject.AddComponent(coreClassSet.World.Type);
            worldContext.SetCurrentWorld(world);

            try
            {
                await world.InitializeAsync();
                
                world.PrepareWorld(this, worldContext.ScenePath, coreClassSet.PolyPawnsToSpawn);
                localPlayer.PreparePlayerController(world, coreClassSet.PlayerController);
            
                await world.PrepareWorldLoop(0);
            }
            catch (Exception e)
            {
                FPolyLog.Error("Poly.GameFramework", "App initialization failed: " + e);
                throw;
            }
        }

        protected internal override void Dispose()
        {
            localPlayer.Dispose();
            base.Dispose();
        }

        protected internal override void PostSubsystemInitialize(FPolyGameInstanceSubsystem subsystem)
        {
            subsystem.SetOwner(this);
        }
    }
}