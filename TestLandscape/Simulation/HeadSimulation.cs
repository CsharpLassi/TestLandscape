using System;
using engenious;
using TestLandscape.Components;

namespace TestLandscape.Simulation
{
    public class HeadSimulation : GameSimulationComponent<HeadSimulation,HeadComponent>
    {
        protected override void Update(GameObject gameObject, HeadComponent component ,GameTime time)
        {
            TranslationComponent trans;
            if (!gameObject.Components.TryGet<TranslationComponent>(out trans))
                return;

            var position = component.Position + trans.Position;
            
            var height = (float)Math.Sin(component.Tilt);
            float distance = (float)Math.Cos(component.Tilt);

            var lookX = (float)Math.Cos(component.Angle) * distance;
            var lookY = -(float)Math.Sin(component.Angle) * distance;

            float strafeX = -(float)Math.Sin(component.Angle);
            float strafeY = -(float)Math.Cos(component.Angle);

            
            var cameraUpVector = Vector3.Cross(new Vector3(strafeX, strafeY, 0), new Vector3(lookX, lookY, height));

            gameObject.Scene.Camera.Position = position;
            gameObject.Scene.Camera.LookAt = new Vector3(
                position.X + lookX,
                position.Y + lookY,
                position.Z + height);
            
            gameObject.Scene.Camera.Up = -cameraUpVector;
        }
    }
}