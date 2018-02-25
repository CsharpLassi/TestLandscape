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
        public override void Load(ContentManager manager, GraphicsDevice device)
        {
            base.Load(manager, device);
            LoadStatic(manager);
        }

        private static void LoadStatic(ContentManager manager)
        {
            if (isStaticLoaded)
                return;

            effect = manager.Load<ModelEffect>("ModelEffect");

            isStaticLoaded = true;
        }

        protected void DrawModel(RenderPass pass,Model model,GraphicsDevice device,Matrix world,Camera camera, SunLight sun)
        {
            if (pass == RenderPass.Shadow)
            {
                device.RasterizerState = RasterizerState.CullCounterClockwise;
                effect.Shadow.Pass1.Apply();
                effect.Shadow.Pass1.Proj = camera.Projection;
                effect.Shadow.Pass1.View = camera.View;
                effect.Shadow.Pass1.World = world;
                model.Draw();
            }
            else if (pass == RenderPass.Normal)
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
            

        }
    }
}