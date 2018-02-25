using System;
using engenious;
using engenious.Helper;
using engenious.Input;

namespace TestLandscape.Scripts
{
    public class CameraMovement : GameObjectComponent
    {
        private double angle;
        private float tilt;
        private Vector3 position;
        
        private Vector2 mouseOldPoistion;
        

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
                if (!Scene.Game.IsMouseVisible)
                {
                    var position = new Vector2(mouse.X,mouse.Y) - mouseOldPoistion;
                    angle -= position.X / 100;
                    tilt -= position.Y / 100;
                }
                
                mouseOldPoistion = new Vector2(mouse.X,mouse.Y);
                Scene.Game.IsMouseVisible = false;
            }
            else
            {
                Scene.Game.IsMouseVisible = true;
            }
            
            var height = (float)Math.Sin(tilt);
            float distance = (float)Math.Cos(tilt);

            var lookX = (float)Math.Cos(angle) * distance;
            var lookY = -(float)Math.Sin(angle) * distance;

            float strafeX = (float)Math.Cos(angle + MathHelper.PiOver2);
            float strafeY = -(float)Math.Sin(angle + MathHelper.PiOver2);

            
            var cameraUpVector = Vector3.Cross(new Vector3(strafeX, strafeY, 0), new Vector3(lookX, lookY, height));


            var camera = GameObject.OfType<Camera>();
            camera.Position = position;
            camera.LookAt = new Vector3(
                position.X + lookX,
                position.Y + lookY,
                position.Z + height);
            camera.Up = -cameraUpVector;
        }
    }
}