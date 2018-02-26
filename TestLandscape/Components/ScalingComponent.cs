using System.ComponentModel;
using engenious;

namespace TestLandscape.Components
{
    public class ScalingComponent : GameObjectComponent<ScalingComponent>
    {
        public Vector3 Scaling = Vector3.One;
        public Matrix Matrix => Matrix.CreateScaling(Scaling);
        
        protected override void OnCopy(ScalingComponent component)
        {
            component.Scaling = Scaling;
        }
    }
}