using engenious;
using engenious.Content;
using engenious.Graphics;

namespace TestLandscape
{
    public interface IGameObjectComponent : IGameId
    {    
        bool IsEnabled { get; set; }

        GameObject GameObject { get; }
        
        IGameObjectComponent Copy(GameObject gameObject, Scene scene,ContentManager manager, GraphicsDevice device);
        void Load(GameObject gameObject, Scene scene, ContentManager manager, GraphicsDevice device);
    }
}