using engenious;

namespace TestLandscape.Components
{
    public class TranslationComponent : GameObjectComponent
    {
        public Vector3 Position { get; set; }
        public Vector3 Rotation { get; set; }
        public Vector3 Scaling { get; set; } = Vector3.One;

        public TranslationComponent()
        {
            
        }
        
        public Matrix Matrix => Matrix.CreateScaling(Scaling) 
                                * Matrix.CreateRotationX(Rotation.X)  
                                * Matrix.CreateRotationY(Rotation.Y)  
                                * Matrix.CreateRotationZ(Rotation.Z)  
                                * Matrix.CreateTranslation(Position);
    }
}