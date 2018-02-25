using engenious;
using engenious.Content;
using engenious.Graphics;
using engenious.UserDefined;

namespace TestLandscape
{
    public abstract class ModelObject<T> : GameObject<T> 
        where T : GameObject<T>, new()
    {
        private static bool isStaticLoaded;

        private static ModelEffect effect;

        public bool IsTransparent { get; set; }
        public bool HaveShadow { get; set; } = true;
        
        public override void Load(ContentManager manager, GraphicsDevice device)
        {
            base.Load(manager, device);
            LoadStatic(manager,device);
        }

        private static void LoadStatic(ContentManager manager,GraphicsDevice device)
        {
            if (isStaticLoaded)
                return;
  
            effect = manager.Load<ModelEffect>("ModelEffect");

            isStaticLoaded = true;
        }

        protected void DrawModel(RenderPass pass,Model model,GraphicsDevice device,Matrix world,Camera camera, SunLight sun)
        {
            if (pass == RenderPass.Shadow && HaveShadow)
            {
                device.RasterizerState = RasterizerState.CullCounterClockwise;
                effect.Shadow.Pass1.Apply();
                effect.Shadow.Pass1.Proj = camera.Projection;
                effect.Shadow.Pass1.View = camera.View;
                effect.Shadow.Pass1.World = world;
                model.Draw();
            }
            else if (pass == RenderPass.Normal && !IsTransparent)
            {
                device.RasterizerState = RasterizerState.CullClockwise;
            
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
                device.RasterizerState = RasterizerState.CullClockwise;
                effect.Transparent.Pass1.Apply();
                
                effect.Transparent.Pass1.Proj = camera.Projection;
                effect.Transparent.Pass1.View = camera.View;
                effect.Transparent.Pass1.World = world;
                
                model.Draw();
            }
            

        }
    }
}