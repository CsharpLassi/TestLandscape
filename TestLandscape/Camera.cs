using engenious;
using engenious.Content;
using engenious.Graphics;
using engenious.Helper;

namespace TestLandscape
{
    public class Camera : GameObject<Camera>
    {
        public Matrix View { get; protected set; }
        public Matrix Projection { get; protected set; }

        public bool IsOrtho { get; set; }
        
        public Vector3 Position { get; set; }
        public Vector3 LookAt { get; set; }
        public Vector3 Up { get; set; }

        protected override void OnDraw(RenderPass pass, GameTime time, GraphicsDevice device, Camera camera, SunLight sun, Matrix world, RenderTarget2D shadowMap, Matrix shadowProjView)
        {
            if (pass == RenderPass.CameraUpdate)
            {
                UpdateMatrix(device);
            }
            
        }

        public void UpdateMatrix(GraphicsDevice device)
        {
            if (!IsOrtho)
            {
                Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4,device.Viewport.AspectRatio, 0.1f, 500.0f);
            }
            else
            {
                Projection = Matrix.CreateOrthographicOffCenter(-40, 40, 40, -40, -500, 500);
            }
            
            View =  Matrix.CreateLookAt(Position,LookAt,Up);
        }
    }
}