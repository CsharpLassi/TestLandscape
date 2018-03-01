using engenious;
using engenious.Graphics;

namespace TestLandscape
{
    public interface IDrawComponent
    {
        void Draw(RenderPass pass, GameTime time, Camera camera, SunLight sun, Matrix world,
            RenderTarget2D shadowMap, Matrix shadowProjView);
    }
}