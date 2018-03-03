using engenious;
using engenious.Content;
using engenious.Graphics;
using engenious.Helper;

namespace TestLandscape.Components.Models.Grass
{
    public class Grass1ModelComponent : ModelComponent<Grass1ModelComponent>
    {

        public Grass1ModelComponent() 
            : base("grass/grass1")
        {
            IsTransparent = false;
            HasShadow = true;

            RotateMatrix = Matrix.CreateRotationX(MathHelper.Pi);
            ScaleMatrix = Matrix.CreateScaling(0.1f,0.1f,0.1f);
        }
        
        
        protected override void OnLoad()
        {
            base.OnLoad();


        }
    }
}