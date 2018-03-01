using engenious;
using engenious.Content;
using engenious.Graphics;
using engenious.UserDefined;

namespace TestLandscape.Components.Models
{
    public abstract class ModelComponent<T> : DrawComponent<T>, IDrawComponent
        where T : ModelComponent<T>
    {
        private static bool isStaticLoaded;

        private static ModelEffect effect;

        public bool IsTransparent { get; set; }
        public bool HaveShadow { get; set; } = true;
        
        protected override void OnCopy(T component)
        {
            
        }

        protected override void OnLoad()
        {
            GameObject.CreateComponent<TranslationComponent>();
            LoadStatic(Manager);
        }

        private static void LoadStatic(ContentManager manager)
        {
            if (isStaticLoaded)
                return;
  
            effect = manager.Load<ModelEffect>("ModelEffect");

            isStaticLoaded = true;
        }
        
        protected void DrawModel(RenderPass pass,Model model,Matrix world,Camera camera, SunLight sun)
        {
            if (pass == RenderPass.Shadow && HaveShadow)
            {
                GraphicsDevice.RasterizerState = RasterizerState.CullCounterClockwise;
                effect.Shadow.Pass1.Apply();
                effect.Shadow.Pass1.Proj = camera.Projection;
                effect.Shadow.Pass1.View = camera.View;
                effect.Shadow.Pass1.World = world;
                model.Draw();
            }
            else if (pass == RenderPass.Normal && !IsTransparent)
            {
                GraphicsDevice.RasterizerState = RasterizerState.CullClockwise;
            
                effect.Main.Pass1.Apply();
                effect.Main.Pass1.AmbientColor = sun.AmbientColor;
                effect.Main.Pass1.DiffuseColor = sun.DiffuseColor;
                effect.Main.Pass1.DiffuseDirection = sun.DiffuseDirection;

                effect.Main.Pass1.Proj = camera.Projection;
                effect.Main.Pass1.View = camera.View;
                effect.Main.Pass1.World = world;
            
                model.Draw();
            }
            else if(pass == RenderPass.Transparent && IsTransparent)
            {
                GraphicsDevice.RasterizerState = RasterizerState.CullClockwise;
                effect.Transparent.Pass1.Apply();
                
                effect.Transparent.Pass1.Proj = camera.Projection;
                effect.Transparent.Pass1.View = camera.View;
                effect.Transparent.Pass1.World = world;
                
                model.Draw();
            }
            

        }
        
        //public abstract void Draw(RenderPass pass, GameTime time, Camera camera, SunLight sun, Matrix world, RenderTarget2D shadowMap, Matrix shadowProjView);
    }
}