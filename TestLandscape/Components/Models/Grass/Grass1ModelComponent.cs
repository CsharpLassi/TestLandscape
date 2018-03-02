using engenious;
using engenious.Content;
using engenious.Graphics;
using engenious.Helper;

namespace TestLandscape.Components.Models.Grass
{
    public class Grass1ModelComponent : ModelComponent<Grass1ModelComponent>
    {
        private static Model grassModel;
        private static bool isLoaded;
        
        private static object lockObject = new object();
        
        protected override void OnLoad()
        {
            base.OnLoad();
            OnStaticLoad(Simulation.Manager);

            IsTransparent = false;
            HasShadow = true;
        }

        private static void OnStaticLoad(ContentManager manager)
        {
            lock (lockObject)
            {
                if (isLoaded)
                    return;
                
                isLoaded = true;
            }
            
            grassModel = manager.Load<Model>("grass/grass1");
            
        }

        public override void Draw(int step, RenderPass pass, GameTime time, Camera camera, SunLight sun, RenderTarget2D shadowMap,
            Matrix shadowProjView)
        {
            var matrix = GameObject.GetWorldDrawMatrix(step) * Matrix.CreateRotationX(MathHelper.Pi) 
                         * Matrix.CreateScaling(0.1f,0.1f,0.1f);
            
            DrawModel(pass,grassModel,matrix,camera,sun);
        }
    }
}