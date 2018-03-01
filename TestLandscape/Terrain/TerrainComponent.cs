using engenious;
using engenious.Graphics;

namespace TestLandscape.Terrain
{
    public class TerrainComponent : DrawComponent<TerrainComponent>, IDrawComponent
    {
        protected override void OnLoad()
        {
            UseLevelOfDetail = false;
        }

        protected override void OnCopy(TerrainComponent component)
        {
            component.UseLevelOfDetail = UseLevelOfDetail;
        }

        public override void Draw(int step, RenderPass pass, GameTime time, Camera camera, SunLight sun, RenderTarget2D shadowMap, Matrix shadowProjView)
        {
            if (GameObject is Landscape landscape)
            {
                landscape.Draw(pass,time,camera,sun,landscape.GetWorldDrawMatrix(step),shadowMap,shadowProjView);
            }
        }
    }
}