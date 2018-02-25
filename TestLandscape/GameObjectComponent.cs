using engenious;

namespace TestLandscape
{
    public class GameObjectComponent
    {
        protected GameObject GameObject { get; private set; }
        protected Scene Scene { get; private set; }
        
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
    }
}