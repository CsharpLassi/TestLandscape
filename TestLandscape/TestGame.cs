using System;
using System.Collections.Generic;
using System.IO;
using engenious;
using engenious.Graphics;
using engenious.Helper;
using engenious.Input;
using engenious.UserDefined;

namespace TestLandscape
{
    public class TestGame : Game
    { 
        private Vector3 position;
        private Vector3 direction;

        private float tilt;
        private float angle;

        private RenderTarget2D shadowMap;

        private Vector2 mouseOldPoistion;


        private Model tree1;
        private ModelEffect modelEffect;
        
        
        private float height;
        private float lookX;
        private float lookY;
        private Vector3 cameraUpVector;

        private SpriteBatch batch;

        private Landscape landScape;
        
        
        public TestGame()
        {
            position = new Vector3(0,10,3);
            direction = new Vector3(0,1,0);
            
            tilt = 0.0f;
            angle = +MathHelper.PiOver2;
        }
        
        public override void LoadContent()
        {
            base.LoadContent();

            modelEffect = Content.Load<ModelEffect>("ModelEffect");
            tree1 = Content.Load<Model>("trees/tree1");
            
            shadowMap = new RenderTarget2D(GraphicsDevice,1024*4,1024*4,PixelInternalFormat.DepthComponent32);
            shadowMap.SamplerState = SamplerState.LinearClamp;
            
            batch = new SpriteBatch(GraphicsDevice);
            
            landScape = new Landscape(GraphicsDevice);
            landScape.Load(Content);
            
            
            
            //effect = new BasicEffect(GraphicsDevice);
        }

        public override void Update(GameTime gameTime)
        {
            
            
            var state = Keyboard.GetState();
            var mouse = engenious.Input.Mouse.GetState();

            var fac = 10;
            
            if (state.IsKeyDown(Keys.W))
            {
                position += new Vector3((float)Math.Cos(angle),-(float)Math.Sin(angle),0) *0.1f * fac;
            }
            
            if (state.IsKeyDown(Keys.S))
            {
                position -= new Vector3((float)Math.Cos(angle),-(float)Math.Sin(angle),0) *0.1f * fac;
            }
            
            if (state.IsKeyDown(Keys.A))
            {
                position -= new Vector3((float)Math.Sin(angle),(float)Math.Cos(angle),0) *0.1f * fac;
            }
            
            if (state.IsKeyDown(Keys.D))
            {
                position += new Vector3((float)Math.Sin(angle),(float)Math.Cos(angle),0) *0.1f * fac;
            }

            if (state.IsKeyDown(Keys.Space))
            {
                position += new Vector3(0,0,1)*0.1f;
            }
            
            if (state.IsKeyDown(Keys.ShiftLeft))
            {
                position += new Vector3(0,0,-1)*0.1f;
            }
            
            if (mouse.IsButtonDown(MouseButton.Left))
            {
                if (!IsMouseVisible)
                {
                    var position = new Vector2(mouse.X,mouse.Y) - mouseOldPoistion;
                    angle -= position.X / 100;
                    tilt -= position.Y / 100;
                }
                
                mouseOldPoistion = new Vector2(mouse.X,mouse.Y);
                IsMouseVisible = false;
            }
            else
            {
                IsMouseVisible = true;
            }
            
            height = (float)Math.Sin(tilt);
            float distance = (float)Math.Cos(tilt);

            lookX = (float)Math.Cos(angle) * distance;
            lookY = -(float)Math.Sin(angle) * distance;

            float strafeX = (float)Math.Cos(angle + MathHelper.PiOver2);
            float strafeY = -(float)Math.Sin(angle + MathHelper.PiOver2);

            
            cameraUpVector = Vector3.Cross(new Vector3(strafeX, strafeY, 0), new Vector3(lookX, lookY, height));

            landScape.Update();
        }

        public override void Draw(GameTime gameTime)
        {
                  
            DrawShadow();
            DrawNormal();
        }

        private Matrix shadowViewProj = Matrix.Identity;
        Matrix bias = new Matrix(0.5f,0.0f,0.0f,0.5f,
            0.0f,0.5f,0.0f,0.5f,
            0.0f,0.0f,0.5f,0.5f,
            0.0f,0.0f,0.0f,1.0f);
        
        public void DrawShadow()
        {
            GraphicsDevice.SetRenderTarget(shadowMap);
            GraphicsDevice.Clear(Color.White);
            
            var dir = new Vector3(0, 1, -1);
            dir.Normalize();
            
            
            var view = Matrix.CreateLookAt(position - dir, position,Vector3.UnitY);
            
            var proj = Matrix.CreateOrthographicOffCenter(-40,40,40,-40,-500,500);

            shadowViewProj = bias * proj * view;
            
            landScape.DrawShadow(Matrix.CreateScaling(5,5,100), view,proj);
            
            
            GraphicsDevice.RasterizerState = RasterizerState.CullCounterClockwise;
            modelEffect.Shadow.Pass1.Apply();
            modelEffect.Shadow.Pass1.Proj = proj;
            modelEffect.Shadow.Pass1.View = view;
            modelEffect.Shadow.Pass1.World = Matrix.CreateTranslation(30, 30, -12);
            tree1.Draw();
            
        }
        
        public void DrawNormal()
        {
            GraphicsDevice.SetRenderTarget(null);
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            
            GraphicsDevice.Clear(Color.CornflowerBlue);
            
            Color ambientColor = Color.LightGray;
            Color dirColor = Color.White;
            
            var dir = new Vector3(0, 1, -1);
            dir.Normalize();
            
            var view = Matrix.CreateLookAt(
                position,
                new Vector3(
                    position.X + lookX,
                    position.Y + lookY,
                    position.Z + height),
                -cameraUpVector);
            
            var proj = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4,GraphicsDevice.Viewport.AspectRatio, 0.1f, 500.0f);
            
            landScape.Draw(Matrix.CreateScaling(5,5,100), view,proj,shadowViewProj,shadowMap
                ,ambientColor,dirColor,dir);


            GraphicsDevice.RasterizerState = RasterizerState.CullClockwise;
            
            modelEffect.Main.Pass1.Apply();
            modelEffect.Main.Pass1.AmbientColor = ambientColor;
            modelEffect.Main.Pass1.DiffuseColor = dirColor;
            modelEffect.Main.Pass1.DiffuseDirection = dir;
            modelEffect.Main.Pass1.Proj = proj;
            modelEffect.Main.Pass1.View = view;
            modelEffect.Main.Pass1.ShadowMap = shadowMap;
            modelEffect.Main.Pass1.shadowViewProj = shadowViewProj;
            modelEffect.Main.Pass1.World = Matrix.CreateTranslation(30, 30, -12f);
            tree1.Draw();      
        }
    }
}