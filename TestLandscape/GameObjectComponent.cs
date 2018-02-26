using System;
using System.ComponentModel.Design;
using System.Threading;
using engenious;

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
    
    public abstract class GameObjectComponent<T> : IGameObjectComponent
        where T : GameObjectComponent<T>
    {
        protected GameObject GameObject { get; private set; }
        protected Scene Scene { get; private set; }

        public static readonly int ComponentId = GameObjectComponentManager.GetNextId();
        public int Id => ComponentId;
        
        public bool IsEnabled { get; set; } = true;

        public void Load(GameObject gameObject, Scene scene)
        {
            GameObject = gameObject;
            Scene = scene;
            OnLoad();
        }

        protected virtual void OnLoad()
        {

        }

        public virtual void Update(GameTime gameTime)
        {

        }

        public IGameObjectComponent Copy(GameObject gameObject, Scene scene)
        {
            var newComponent = (T)Activator.CreateInstance(this.GetType());
            newComponent.GameObject = gameObject;
            newComponent.Scene = scene;
            
            OnCopy(newComponent);

            return newComponent;
        }

        protected abstract void OnCopy(T component);

        public sealed override int GetHashCode()
        {
            return Id;
        }
    }
}