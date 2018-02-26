using System;
using engenious;
using engenious.Helper;
using engenious.Input;

namespace TestLandscape.Components
{
    public class HeadComponent : GameObjectComponent
    {
        public Vector3 Position = new Vector3(0,0,1);
        
        public float Tilt;
        public double Angle;
         
        public override void Update(GameTime gameTime)
        {
            TranslationComponent trans;
            if (!GameObject.Components.TryGet<TranslationComponent>(out trans))
                return;

            var position = Position + trans.Position;
            
            var height = (float)Math.Sin(Tilt);
            float distance = (float)Math.Cos(Tilt);

            var lookX = (float)Math.Cos(Angle) * distance;
            var lookY = -(float)Math.Sin(Angle) * distance;

            float strafeX = (float)Math.Cos(Angle + MathHelper.PiOver2);
            float strafeY = -(float)Math.Sin(Angle + MathHelper.PiOver2);

            
            var cameraUpVector = Vector3.Cross(new Vector3(strafeX, strafeY, 0), new Vector3(lookX, lookY, height));

            Scene.Camera.Position = position;
            Scene.Camera.LookAt = new Vector3(
                position.X + lookX,
                position.Y + lookY,
                position.Z + height);
            
            Scene.Camera.Up = -cameraUpVector;
        }
    }
}