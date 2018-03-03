using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        
        private readonly List<IDrawComponent> drawComponents = new List<IDrawComponent>();
        private readonly List<IDrawComponent> cameraComponents = new List<IDrawComponent>();
        
        private int currentStep;
        
        private readonly Stopwatch watch = new Stopwatch();
        
        public static RenderSimulation Register()
        {
            return Register<RenderSimulation>();
        }

        public override void BeginUpdate(GameTime gameTime)
        {
            currentStep++;
        }
        
        public override void Update(GameTime gameTime)
        {
            
        }

        public override void Add(IGameObjectComponent component)
        {   
            if (component is IDrawComponent drawComponent)
            {
                if (drawComponent.IsCamera)
                    cameraComponents.Add(drawComponent);

                drawComponents.Add(drawComponent);
            }
        }

        public override void Remove(IGameObjectComponent component)
        {
            if (component is IDrawComponent drawComponent)
            {
                cameraComponents.Remove(drawComponent);
                drawComponents.Remove(drawComponent);
            }
        }


        public void Draw(GraphicsDevice device,Scene scene,GameTime gameTime)
        {
            if (shadowMap == null)
            {
                shadowMap = new RenderTarget2D(device,1024*4,1024*4,PixelInternalFormat.DepthComponent32);
                shadowMap.SamplerState = SamplerState.LinearClamp;
            }
            

            watch.Restart();
            //Camera
            foreach (var cameraComponent in cameraComponents)
            {
                cameraComponent.Draw(currentStep, RenderPass.CameraUpdate, gameTime,
                    scene.Camera, scene.SunLight, null, Matrix.Identity);
            }

            Console.WriteLine("Camera:" + watch.ElapsedMilliseconds);
            
            var cameraPosition = scene.Camera.Position;
                      
            //Shadow

            watch.Restart();
            device.SetRenderTarget(shadowMap);
            device.Clear(Color.White);
            var shadowCamera = scene.SunLight.ShadowCamera;
            var shadowProjView = bias * shadowCamera.Projection * shadowCamera.View;
            foreach (var drawComponent in drawComponents)
            {
                if (!drawComponent.IsEnabled || !drawComponent.HasShadow)
                    continue;
                
                drawComponent.Draw(currentStep, RenderPass.Shadow, gameTime,
                    shadowCamera, scene.SunLight, null, Matrix.Identity);
            }
            Console.WriteLine("Shadow:" + watch.ElapsedMilliseconds);

            watch.Restart();
            //Normal
            device.SetRenderTarget(null);
            device.Clear(Color.CornflowerBlue);
            foreach (var drawComponent in drawComponents)
            {
                if (!drawComponent.IsEnabled)
                    continue;
                
                drawComponent.Draw(currentStep, RenderPass.Normal, gameTime,
                    scene.Camera, scene.SunLight, shadowMap,
                    shadowProjView);
            }
            Console.WriteLine("Normal:" + watch.ElapsedMilliseconds);
            
            watch.Restart();
            //Transparent
            foreach (var drawComponent in drawComponents)
            {
                if (!drawComponent.IsEnabled || !drawComponent.IsTransparent)
                    continue;
                
                drawComponent.Draw(currentStep, RenderPass.Transparent, gameTime,
                    scene.Camera, scene.SunLight, shadowMap,
                    shadowProjView);
            }
            Console.WriteLine("Trans:" + watch.ElapsedMilliseconds);
        }
    }
}