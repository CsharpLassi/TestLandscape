using engenious;

namespace TestLandscape.Components
{
    public class TranslationComponent : GameObjectComponent<TranslationComponent>
    {
        public Vector3 Position;
        public Vector3 Rotation;
        public Vector3 Scaling = Vector3.One;

        public TranslationComponent()
        {
            
        }
        
        public Matrix Matrix => Matrix.CreateTranslation(Position)
                                * Matrix.CreateScaling(Scaling) 
                                * Matrix.CreateRotationX(Rotation.X)  
                                * Matrix.CreateRotationY(Rotation.Y)  
                                * Matrix.CreateRotationZ(Rotation.Z);

        public override void OnCopy(TranslationComponent component)
        {
            component.Position = component.Position;
            component.Rotation = component.Rotation;
            component.Scaling = component.Scaling;
        }
    }
}