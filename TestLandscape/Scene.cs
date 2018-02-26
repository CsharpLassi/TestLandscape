using System.Diagnostics;
using engenious;
using engenious.Content;
using engenious.Graphics;

namespace TestLandscape
{
    public class Scene
    {
        public readonly Game Game;

        private RenderTarget2D shadowMap;
        
        public Scene(Game game)
        {
            Game = game;
        }
        
        public GameObjectCollection Children { get; set; } = new GameObjectCollection();

        public Camera Camera { get; set; }
        public SunLight SunLight { get; set; }
        
        public virtual void Load(ContentManager manager,GraphicsDevice device)
        {
            shadowMap = new RenderTarget2D(device,1024*4,1024*4,PixelInternalFormat.DepthComponent32);
            shadowMap.SamplerState = SamplerState.LinearClamp;
            
            Camera = Children.Create<Camera>(manager,device, this);
            SunLight = Children.Create<SunLight>(manager, device, this);
        }

        public virtual void Update(GameTime time)
        {
            foreach (var child in Children)
            {
                child.Update(time);
            }
        }
        
        Matrix bias = new Matrix(0.5f,0.0f,0.0f,0.5f,
            0.0f,0.5f,0.0f,0.5f,
            0.0f,0.0f,0.5f,0.5f,
            0.0f,0.0f,0.0f,1.0f);
        
        public virtual void Draw(GameTime time,GraphicsDevice device)
        {
            //Camera
            //device.SetRenderTarget(shadowMap);
            foreach (var child in Children)
            {
                child.Draw(RenderPass.CameraUpdate, time, device, Camera , SunLight , Matrix.Identity,null,Matrix.Identity);
            }
            
            //Shadow
            device.SetRenderTarget(shadowMap);
            device.Clear(Color.White);
            var shadowCamera = SunLight.ShadowCamera;
            var shadowProjView = bias * shadowCamera.Projection * shadowCamera.View;
            foreach (var child in Children)
            {
                child.Draw(RenderPass.Shadow, time, device, shadowCamera , SunLight , Matrix.Identity,null,Matrix.Identity);
            }
            
            //Normal
            device.SetRenderTarget(null);
            device.Clear(Color.CornflowerBlue);
            foreach (var child in Children)
            {
                child.Draw(RenderPass.Normal, time, device, Camera, SunLight , Matrix.Identity,shadowMap,shadowProjView);
            }
            
            //Transparent
            foreach (var child in Children)
            {
                child.Draw(RenderPass.Transparent, time, device, Camera, SunLight , Matrix.Identity,shadowMap,shadowProjView);
            }
            
        }
    }
}