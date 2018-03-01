using engenious;

namespace TestLandscape
{
    public interface IScript : IGameObjectComponent
    {
        void Update(GameTime gameTime);
    }
}