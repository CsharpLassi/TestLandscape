using System;
using System.Collections.Generic;
using System.Threading;
using engenious;
using engenious.Graphics;

namespace TestLandscape.Simulation
{
    public class RenderSimulation : GameSimulationComponent
    {
        private RenderTarget2D shadowMap;
        
        private readonly Matrix bias = new Matrix(0.5f, 0.0f, 0.0f, 0.5f,
                                                    0.0f, 0.5f, 0.0f, 0.5f,
                                                    0.0f, 0.0f, 0.5f, 0.5f,
                                                    0.0f, 0.0f, 0.0f, 1.0f);
        
        private readonly EditableGameList<IDrawComponent> drawComponents = new EditableGameList<IDrawComponent>();
        
        private int currentStep;
        
        public static RenderSimulation Register()
        {
            return Register<RenderSimulation>();
        }

        protected override void BeginUpdate()
        {
            currentStep++;
            drawComponents.Clear();
        }
        
        protected override void Update(GameTime gameTime, GameObject gameObject)
        {
            foreach (var component in gameObject.Components)
            {
                if (component is IDrawComponent drawComponent)
                {
                    drawComponents.Add(drawComponent);
                }
            }
        }



        public void Draw(GraphicsDevice device,Scene scene,GameTime gameTime)
        {
            if (shadowMap == null)
            {
                shadowMap = new RenderTarget2D(device,1024*4,1024*4,PixelInternalFormat.DepthComponent32);
                shadowMap.SamplerState = SamplerState.LinearClamp;
            }
            


            //Camera
            foreach (var drawComponent in drawComponents)
            {
                drawComponent.Draw(currentStep, RenderPass.CameraUpdate, gameTime,
                    scene.Camera, scene.SunLight, null, Matrix.Identity);
            }

            var cameraPosition = scene.Camera.Position;
            
            
            //LevelOfDetail
            foreach (var drawComponent in drawComponents)
            {
                if (!drawComponent.UseLevelOfDetail)
                    continue;
                
                var matrix = drawComponent.GameObject.GetWorldDrawMatrix(currentStep);


                var distance = Vector3.DistanceSquared(cameraPosition, matrix.Translation);
                if (distance > 1000)
                {
                    drawComponents.Remove(drawComponent);
                }
            }
            
            
            //Shadow

            device.SetRenderTarget(shadowMap);
            device.Clear(Color.White);
            var shadowCamera = scene.SunLight.ShadowCamera;
            var shadowProjView = bias * shadowCamera.Projection * shadowCamera.View;
            foreach (var drawComponent in drawComponents)
            {
                drawComponent.Draw(currentStep, RenderPass.Shadow, gameTime,
                    shadowCamera, scene.SunLight, null, Matrix.Identity);
            }


            //Normal
            device.SetRenderTarget(null);
            device.Clear(Color.CornflowerBlue);

            foreach (var drawComponent in drawComponents)
            {
                drawComponent.Draw(currentStep, RenderPass.Normal, gameTime,
                    scene.Camera, scene.SunLight, shadowMap,
                    shadowProjView);
            }
            
            //Transparent
            foreach (var drawComponent in drawComponents)
            {
                drawComponent.Draw(currentStep, RenderPass.Transparent, gameTime,
                    scene.Camera, scene.SunLight, shadowMap,
                    shadowProjView);
            }
        }
    }
}