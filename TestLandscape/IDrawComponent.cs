using engenious;
using engenious.Graphics;

namespace TestLandscape
{
    public interface IDrawComponent : IGameObjectComponent , IGameId
    {
        void Draw(int step, RenderPass pass, GameTime time, Camera camera, SunLight sun,
            RenderTarget2D shadowMap, Matrix shadowProjView);

        bool UseLevelOfDetail { get; }
    }
}