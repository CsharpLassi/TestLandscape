using System.Collections.Generic;
using engenious;
using engenious.Graphics;
using TestLandscape.Simulation;
using TestLandscape.Simulation.World;

namespace TestLandscape
{
    public class GameSimulation : DrawableGameComponent
    {
        private RenderTarget2D shadowMap;
        
        public Scene CurrentScene { get; private set; }
        Queue<GameObject> updateObjects = new Queue<GameObject>();
        
        private readonly List<IDrawComponent> drawComponents = new List<IDrawComponent>();
        
        public GameSimulation(Game game) : base(game)
        {
            ScriptSimulation.Register();
            HeadSimulation.Register();
            GravitySimulation.Register();
            
            shadowMap = new RenderTarget2D(GraphicsDevice,1024*4,1024*4,PixelInternalFormat.DepthComponent32);
            shadowMap.SamplerState = SamplerState.LinearClamp;
        }

        public void LoadScene<T>()
            where T: Scene, new()
        {
            var newScene = new T();
            newScene.Load(this,Game.Content,GraphicsDevice);

            CurrentScene = newScene;
        }

        
        
        public override void Update(GameTime gameTime)
        {   
            drawComponents.Clear();
            foreach (var child in CurrentScene.Children)
            {
                if (!child.IsEnabled)
                    continue;
                
                updateObjects.Enqueue(child);
            }

            while (updateObjects.Count > 0)
            {
                var gameObject = updateObjects.Dequeue();

                foreach (var component in gameObject.Components)
                {
                    if (component is IDrawComponent drawComponent)
                    {
                        drawComponents.Add(drawComponent);
                    }
                }
                
                GameSimulationComponent.Simulate(gameTime, gameObject);
                foreach (var child in gameObject.Children)
                {
                    if (!child.IsEnabled)
                        continue;
                    
                    updateObjects.Enqueue(child);
                    
                    
                }
            }
        }


        public override void Draw(GameTime gameTime)
        {
            Matrix bias = new Matrix(0.5f, 0.0f, 0.0f, 0.5f,
                0.0f, 0.5f, 0.0f, 0.5f,
                0.0f, 0.0f, 0.5f, 0.5f,
                0.0f, 0.0f, 0.0f, 1.0f);

            //Camera
            foreach (var drawComponent in drawComponents)
            {
                drawComponent.Draw(RenderPass.CameraUpdate, gameTime,
                    CurrentScene.Camera, CurrentScene.SunLight, Matrix.Identity, null, Matrix.Identity);
            }

            //Shadow

            GraphicsDevice.SetRenderTarget(shadowMap);
            GraphicsDevice.Clear(Color.White);
            var shadowCamera = CurrentScene.SunLight.ShadowCamera;
            var shadowProjView = bias * shadowCamera.Projection * shadowCamera.View;
            foreach (var drawComponent in drawComponents)
            {
                drawComponent.Draw(RenderPass.Shadow, gameTime,
                    shadowCamera, CurrentScene.SunLight, Matrix.Identity, null, Matrix.Identity);
            }


            //Normal
            GraphicsDevice.SetRenderTarget(null);
            GraphicsDevice.Clear(Color.CornflowerBlue);

            foreach (var component in drawComponents)
            {
                component.Draw(RenderPass.Normal, gameTime,
                    CurrentScene.Camera, CurrentScene.SunLight, Matrix.Identity, shadowMap,
                    shadowProjView);
            }
            
            //Transparent
            foreach (var component in drawComponents)
            {
                component.Draw(RenderPass.Transparent, gameTime,
                    CurrentScene.Camera, CurrentScene.SunLight, Matrix.Identity, shadowMap,
                    shadowProjView);
            }
            
        }
    }
}