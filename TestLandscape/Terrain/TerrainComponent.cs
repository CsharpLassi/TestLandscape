using engenious;
using engenious.Graphics;

namespace TestLandscape.Terrain
{
    public class TerrainComponent : GameObjectComponent<TerrainComponent>, IDrawComponent
    {
        protected override void OnCopy(TerrainComponent component)
        {
            
        }

        public void Draw(RenderPass pass, GameTime time, Camera camera, SunLight sun, Matrix world, RenderTarget2D shadowMap, Matrix shadowProjView)
        {
            if (GameObject is Landscape landscape)
            {
                landscape.Draw(pass,time,camera,sun,world,shadowMap,shadowProjView);
            }
        }
    }
}