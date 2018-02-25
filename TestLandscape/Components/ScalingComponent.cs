using System.ComponentModel;
using engenious;

namespace TestLandscape.Components
{
    public class ScalingComponent : GameObjectComponent
    {
        public Vector3 Scaling { get; set; }
        public Matrix Matrix => Matrix.CreateScaling(Scaling);
    }
}