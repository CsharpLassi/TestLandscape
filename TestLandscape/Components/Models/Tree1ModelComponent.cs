using engenious;
using engenious.Content;
using engenious.Graphics;

namespace TestLandscape.Components.Models
{
    public class Tree1ModelComponent : ModelComponent<Tree1ModelComponent>
    {
        private static bool isStaticLoaded;
        private static Model logModel;
        private static Model leavesModel;
        
        protected override void OnLoad()
        {
            base.OnLoad();
            LoadStatic(Manager);
        }

        private static void LoadStatic(ContentManager manager)
        {
            if (isStaticLoaded)
                return;
            logModel = manager.Load<Model>("trees/tree1/log");
            leavesModel = manager.Load<Model>("trees/tree1/leaves");
            
            isStaticLoaded = true;
        }
        
        public override void Draw(RenderPass pass, GameTime time, Camera camera, SunLight sun, Matrix world,
            RenderTarget2D shadowMap, Matrix shadowProjView)
        {
            DrawModel(pass,logModel,world,camera,sun);
        }


    }
}