using System;
using engenious;

namespace TestLandscape
{
    public abstract class GameObjectComponent
    {
        protected GameObject GameObject { get; private set; }
        protected Scene Scene { get; private set; }

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

        public GameObjectComponent Copy(GameObject gameObject, Scene scene)
        {
            var newComponent = (GameObjectComponent)Activator.CreateInstance(this.GetType());
            
            newComponent.GameObject = gameObject;
            newComponent.Scene = scene;
            OnCopy(newComponent);

            return newComponent;
        }

        protected abstract void OnCopy(GameObjectComponent component);
    }

    public abstract class GameObjectComponent<T> : GameObjectComponent
        where T : GameObjectComponent<T>
    {
        protected override void OnCopy(GameObjectComponent component)
        {
            OnCopy((T)component);
        }

        public abstract void OnCopy(T component);
    }
}