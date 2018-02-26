using engenious;

namespace TestLandscape
{
    public interface IGameObjectComponent : IGameId
    {    
        bool IsEnabled { get; set; }

        IGameObjectComponent Copy(GameObject gameObject, Scene scene);
        void Load(GameObject gameObject, Scene scene);
        void Update(GameTime gameTime);
    }
}