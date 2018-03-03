using engenious;
using engenious.Content;
using engenious.Graphics;
using engenious.UserDefined;

namespace TestLandscape.Components.Models
{
    public abstract class ModelComponent<T> : DrawComponent<T>
        where T : ModelComponent<T>
    {
        

        private static bool isStaticLoaded;

        private static ModelEffect effect;
        private static Model model;
        private static readonly object lockObject = new object();

        private GraphicsDevice device;
        private readonly string modelPath;

        private Matrix scaleMatrix = Matrix.CreateScaling(Vector3.One);
        private Matrix rotateMatrix = Matrix.Identity;
        private Matrix translationMatrix = Matrix.Identity;
        
        protected Matrix ScaleMatrix
        {
            get { return scaleMatrix; }
            set
            {
                scaleMatrix = value; 
                UpdateMatrix();
            }
        }

        protected Matrix RotateMatrix
        {
            get { return rotateMatrix; }
            set
            {
                rotateMatrix = value; 
                UpdateMatrix();
            }
        }

        protected Matrix TranslationMatrix
        {
            get { return translationMatrix; }
            set
            {
                translationMatrix = value; 
                UpdateMatrix();
            }
        }


        private Matrix drawMatrix = Matrix.Identity;


        

        public ModelComponent(string modelPath)
        {
            this.modelPath = modelPath;
            UpdateMatrix();
        }

        private void UpdateMatrix()
        {
            drawMatrix = translationMatrix * scaleMatrix * rotateMatrix;
        }
        
        protected override void OnCopy(T component)
        {
            
        }

        protected override void OnLoad()
        {
            GameObject.CreateComponent<TranslationComponent>();
            LoadStatic(Simulation.Game.Content,modelPath);

            device = Simulation.GraphicsDevice;
        }

        private static void LoadStatic(ContentManager manager,string modelPath)
        {
            lock (lockObject)
            {
                if (isStaticLoaded)
                    return;
  
                isStaticLoaded = true;
            }
           
            
            effect = manager.Load<ModelEffect>("ModelEffect");
            model = manager.Load<Model>(modelPath);


        }

        public sealed override void Draw(int step, RenderPass pass, GameTime time, Camera camera, SunLight sun, RenderTarget2D shadowMap,
            Matrix shadowProjView)
        {
            var world = GameObject.GetWorldDrawMatrix(step) * drawMatrix;
            
            if (pass == RenderPass.Shadow && HasShadow)
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

        //public abstract void Draw(RenderPass pass, GameTime time, Camera camera, SunLight sun, Matrix world, RenderTarget2D shadowMap, Matrix shadowProjView);
    }
}