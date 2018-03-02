using System;
using System.ComponentModel.Design;
using System.Threading;
using engenious;
using engenious.Content;
using engenious.Graphics;

namespace TestLandscape
{
    public static class GameObjectComponentManager
    {
        private static int globalId = 0;
        
        public static int GetNextId()
        {
            return Interlocked.Increment(ref globalId);
        }
    }
    
    public abstract class GameObjectComponent<T> : IGameObjectComponent , IDisposable
        where T : GameObjectComponent<T>
    {
        public GameObject GameObject { get; private set; }
        protected Scene Scene { get; private set; }

        public static readonly int ComponentId = GameObjectComponentManager.GetNextId();
        public int Id => ComponentId;
        
        public bool IsEnabled { get; set; } = true;

        public GameSimulation Simulation { get; private set; }
        
        public void Load(GameObject gameObject, Scene scene,GameSimulation simulation)
        {
            GameObject = gameObject;
            Scene = scene;
            Simulation = simulation;
            OnLoad();
        }

        protected virtual void OnLoad()
        {

        }

        public IGameObjectComponent Copy(GameObject gameObject, Scene scene, GameSimulation simulation)
        {
            var newComponent = (T)Activator.CreateInstance(this.GetType());
            newComponent.GameObject = gameObject;
            newComponent.Scene = scene;
            newComponent.Simulation = simulation;
            
            OnCopy(newComponent);

            return newComponent;
        }

        protected abstract void OnCopy(T component);

        public sealed override int GetHashCode()
        {
            return Id;
        }

        public void Dispose()
        {
            GameObject.Components.Remove(this);
        }
    }
}