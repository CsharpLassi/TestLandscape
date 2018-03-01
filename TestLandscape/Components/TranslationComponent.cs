using engenious;

namespace TestLandscape.Components
{
    public class TranslationComponent : GameObjectComponent<TranslationComponent>
    {
        private Vector3 position;
        private Vector3 rotation;
        private Vector3 scaling = Vector3.One;

        private bool isChanged = true;
        
        public Vector3 Position
        {
            get { return position; }
            set
            {
                position = value;
                isChanged = true;
            }
        }

        public Vector3 Rotation
        {
            get { return rotation; }
            set
            {
                rotation = value;
                isChanged = true;
            }
        }

        public Vector3 Scaling
        {
            get
            {
                return scaling; 
                
            }
            set
            {
                scaling = value;
                isChanged = true;

            }
        }

        private Matrix matrix = Matrix.Identity;

        public Matrix Matrix
        {
            get
            {
                if (!isChanged)
                    return matrix;
                
                matrix =  Matrix.CreateTranslation(position)
                        * Matrix.CreateScaling(scaling) 
                        * Matrix.CreateRotationX(rotation.X)  
                        * Matrix.CreateRotationY(rotation.Y)  
                        * Matrix.CreateRotationZ(rotation.Z);

                isChanged = false;

                return matrix;

            }
        }

        protected override void OnCopy(TranslationComponent component)
        {
            component.Position = component.Position;
            component.Rotation = component.Rotation;
            component.Scaling = component.Scaling;
        }
    }
}