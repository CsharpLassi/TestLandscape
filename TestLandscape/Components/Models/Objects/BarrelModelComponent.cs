using engenious;

namespace TestLandscape.Components.Models.Objects
{
    public class BarrelModelComponent : ModelComponent<BarrelModelComponent>
    {
        public BarrelModelComponent() 
            : base("objects/barrel")
        {
            ScaleMatrix = Matrix.CreateScaling(new Vector3(0.1f,0.1f,0.05f));
            EnableColor = false;
        }
    }
}