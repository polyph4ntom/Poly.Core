using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Poly.GameFramework.Core
{
    public abstract class FPolySubsystemOwner<TSubsystemType>
        where TSubsystemType : FPolySubsystem
    {
        internal readonly Dictionary<Type, TSubsystemType> registeredSubsystems = new();

        private readonly TaskCompletionSource<bool> isSubsystemsReady = new();
        internal Task WaitForSubsystems() => isSubsystemsReady.Task;

        protected internal virtual async Task InitializeAsync()
        {
            var subsystemTypes = await FPolyGameplayStatics.LoadSubsystems<TSubsystemType>();
            foreach (var type in subsystemTypes)
            {
                var subsystem = (TSubsystemType) Activator.CreateInstance(type);
                await subsystem.InitializeSubsystem();
                PostSubsystemInitialize(subsystem);
                subsystem.PostInitializeSubsystem();
                registeredSubsystems.Add(type, subsystem);
            }
            isSubsystemsReady.SetResult(true);
        }

        protected internal virtual void PostSubsystemInitialize(TSubsystemType subsystem)
        {
        
        }

        protected internal virtual void Dispose()
        {
            foreach (var subsystem in registeredSubsystems.Values)
            {
                subsystem.DisposeSubsystem();
            }
            
            registeredSubsystems.Clear();
        }
        
        public TSubsystem GetSubsystem<TSubsystem>() where TSubsystem : TSubsystemType
        {
            if (registeredSubsystems.TryGetValue(typeof(TSubsystem), out var subsystem))
            {
                return (TSubsystem) subsystem;
            }

            return null;
        }
    }
}

namespace Poly.GameFramework.Core
{
    public abstract class APolyMonoSubsystemOwner<TSubsystemType> : MonoBehaviour
        where TSubsystemType : FPolyTickableSubsystem
    {
        internal readonly Dictionary<Type, TSubsystemType> registeredSubsystems = new();
        
        private readonly TaskCompletionSource<bool> isSubsystemsReady = new();
        internal Task WaitForSubsystems() => isSubsystemsReady.Task;

        protected internal virtual async Task InitializeAsync()
        {
            var subsystemTypes = await FPolyGameplayStatics.LoadSubsystems<TSubsystemType>();
            foreach (var type in subsystemTypes)
            {
                var subsystem = (TSubsystemType) Activator.CreateInstance(type);
                await subsystem.InitializeSubsystem();
                PostSubsystemInitialize(subsystem);
                subsystem.PostInitializeSubsystem();
                registeredSubsystems.Add(type, subsystem);
            }
            isSubsystemsReady.SetResult(true);
        }

        protected internal virtual void PostSubsystemInitialize(TSubsystemType subsystem)
        {
            
        }
        
        protected internal virtual void Dispose()
        {
            foreach (var subsystem in registeredSubsystems.Values)
            {
                subsystem.DisposeSubsystem();
            }
            
            registeredSubsystems.Clear();
        }

        protected internal virtual void Update()
        {
            foreach (var subsystem in registeredSubsystems.Values)
            {
                var dT = Time.deltaTime;
                subsystem.PreTick(dT);
                subsystem.Tick(dT);
            }
        }

        protected internal virtual void FixedUpdate()
        {
            foreach (var subsystem in registeredSubsystems.Values)
            {
                subsystem.FixedTick(Time.fixedDeltaTime);
            }
        }

        protected internal virtual void LateUpdate()
        {
            foreach (var subsystem in registeredSubsystems.Values)
            {
                subsystem.LateTick(Time.deltaTime);
            }
        }
        
        public TSubsystem GetSubsystem<TSubsystem>() where TSubsystem : TSubsystemType
        {
            if (registeredSubsystems.TryGetValue(typeof(TSubsystem), out var subsystem))
            {
                return (TSubsystem) subsystem;
            }

            return null;
        }
    }
}