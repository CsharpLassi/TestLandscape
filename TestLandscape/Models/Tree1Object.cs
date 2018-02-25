using engenious;
using engenious.Content;
using engenious.Graphics;

namespace TestLandscape.Models
{
    public class Tree1Object : ModelObject<Tree1Object>
    {
        private static bool isStaticLoaded;
        private static Model logModel;
        private static Model leavesModel;
        
        public override void Load(ContentManager manager, GraphicsDevice device)
        {
            base.Load(manager, device);
            LoadStatic(manager);
        }

        private static void LoadStatic(ContentManager manager)
        {
            if (isStaticLoaded)
                return;
            logModel = manager.Load<Model>("trees/tree1/log");
            leavesModel = manager.Load<Model>("trees/tree1/leaves");
            
            isStaticLoaded = true;
        }

        protected override void OnDraw(RenderPass pass, GameTime time, GraphicsDevice device, Camera camera, SunLight sun, Matrix world, RenderTarget2D shadowMap, Matrix shadowProjView)
        {
            DrawModel(pass, logModel, device ,world * Matrix.CreateScaling(0.1f,0.1f,0.1f),camera,sun);
            DrawModel(pass, leavesModel, device, world,camera,sun);
        }
    }
}