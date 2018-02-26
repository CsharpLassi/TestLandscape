﻿using engenious;
using engenious.Content;
using engenious.Graphics;

namespace TestLandscape
{
    public class SunLight : GameObject<SunLight>
    {
        public class SunShadowCamera : Camera
        {
            public SunLight SunLight { get; internal set; }

            public override void Load(ContentManager manager, GraphicsDevice device, Scene scene)
            {
                IsOrtho = true;
            }

            protected override void OnDraw(RenderPass pass, GameTime time, GraphicsDevice device, Camera camera, SunLight sun, Matrix world,
                RenderTarget2D shadowMap, Matrix shadowProjView)
            {
                if (pass == RenderPass.CameraUpdate)
                {
                    LookAt = camera.Position;
                    Position = camera.Position - SunLight.DiffuseDirection;
                    Up = Vector3.UnitY;
                }
                base.OnDraw(pass,time,device,camera,sun,world,shadowMap,shadowProjView);
            }
        }
        
        public Color AmbientColor { get; set; }
        public Color DiffuseColor { get; set; }
        public Vector3 DiffuseDirection { get; set; }

        public SunShadowCamera ShadowCamera { get; private set; }
        
        public override void Load(ContentManager manager, GraphicsDevice device, Scene scene)
        {
            ShadowCamera = Children.Create<SunShadowCamera>(manager, device, scene);
            ShadowCamera.SunLight = this;
            
            AmbientColor = Color.LightGray;
            DiffuseColor = Color.White;
            DiffuseDirection = new Vector3(0, 1, -1);
        }
    }
}