using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Poly.Events;

namespace Poly.GameFramework.Core
{
    public class FPolyGameplayStatics
    {
        internal static async Task<List<Type>> LoadSubsystems<TSubsystemType>()
            where TSubsystemType : FPolySubsystem
        {
            return await Task.Run(() =>
            {
                var baseType = typeof(TSubsystemType);
                var allTypes = AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(a =>
                    {
                        try { return a.GetTypes(); }
                        catch (ReflectionTypeLoadException ex) { return ex.Types.Where(t => t != null); }
                    })
                    .ToList();
                
                var types = allTypes
                    .Where(t =>
                        baseType.IsAssignableFrom(t) &&
                        !t.IsAbstract &&
                        t != baseType && allTypes.All(sub => sub.BaseType != t))
                    .ToList();

                return types;
            });
        }

        public static T GetGameInstance<T>(APolyMonoBehaviour contextObject)
            where T : FPolyGameInstanceBase
        {
            if (contextObject == null)
            {
                return null;
            }

            var worldBase = contextObject.GetWorld<APolyWorldBase>();

            if (worldBase == null)
            {
                return null;
            }

            return worldBase.GetGameInstance<T>();
        }

        public static T GetLocalPlayer<T>(APolyMonoBehaviour contextObject, int playerIdx)
            where T : FPolyLocalPlayerBase
        {
            if (contextObject == null)
            {
                return null;
            }

            var worldBase = contextObject.GetWorld<APolyWorldBase>();
            
            if (worldBase == null)
            {
                return null;
            }

            var playerController = worldBase.GetPlayerController(playerIdx);

            if (playerController == null)
            {
                return null;
            }

            return playerController.GetLocalPlayer<T>();
        }

        public static T GetPlayerController<T>(APolyMonoBehaviour contextObject, int playerIdx)
            where T : APolyPlayerControllerBase
        {
            if (contextObject == null)
            {
                return null;
            }

            var worldBase = contextObject.GetWorld<APolyWorldBase>();
            
            if (worldBase == null)
            {
                return null;
            }

            return worldBase.GetPlayerController<T>(playerIdx);
        }

        public static FPolyEventBus GetEventBus(APolyMonoBehaviour contextObject)
        {
            if (contextObject == null)
            {
                return null;
            }
            
            var worldBase = contextObject.GetWorld<APolyWorldBase>();
            
            if (worldBase == null)
            {
                return null;
            }

            return worldBase.WorldEventBus;
        }
        
        public static T GetGameInstance<T>(FPolyWorldSubsystem contextObject)
            where T : FPolyGameInstanceBase
        {
            if (contextObject == null)
            {
                return null;
            }

            var worldBase = contextObject.GetWorld<APolyWorldBase>();

            if (worldBase == null)
            {
                return null;
            }

            return worldBase.GetGameInstance<T>();
        }

        public static T GetLocalPlayer<T>(FPolyWorldSubsystem contextObject, int playerIdx)
            where T : FPolyLocalPlayerBase
        {
            if (contextObject == null)
            {
                return null;
            }

            var worldBase = contextObject.GetWorld<APolyWorldBase>();
            
            if (worldBase == null)
            {
                return null;
            }

            var playerController = worldBase.GetPlayerController(playerIdx);

            if (playerController == null)
            {
                return null;
            }

            return playerController.GetLocalPlayer<T>();
        }

        public static T GetPlayerController<T>(FPolyWorldSubsystem contextObject, int playerIdx)
            where T : APolyPlayerControllerBase
        {
            if (contextObject == null)
            {
                return null;
            }

            var worldBase = contextObject.GetWorld<APolyWorldBase>();
            
            if (worldBase == null)
            {
                return null;
            }

            return worldBase.GetPlayerController<T>(playerIdx);
        }
    }
}
