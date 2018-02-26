using System;
using engenious;
using engenious.Helper;
using engenious.Input;

namespace TestLandscape.Components
{
    public class HeadComponent : GameObjectComponent<HeadComponent>
    {
        public Vector3 Position = new Vector3(0,0,1);
        
        public float Tilt;
        public float Angle;
         
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

            float strafeX = -(float)Math.Sin(Angle);
            float strafeY = -(float)Math.Cos(Angle);

            
            var cameraUpVector = Vector3.Cross(new Vector3(strafeX, strafeY, 0), new Vector3(lookX, lookY, height));

            Scene.Camera.Position = position;
            Scene.Camera.LookAt = new Vector3(
                position.X + lookX,
                position.Y + lookY,
                position.Z + height);
            
            Scene.Camera.Up = -cameraUpVector;
        }

        protected override void OnCopy(HeadComponent component)
        {
            component.Position = Position;
            component.Angle = Angle;
            component.Tilt = Tilt;
        }
    }
}