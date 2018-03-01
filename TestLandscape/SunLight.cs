using engenious;
using engenious.Content;
using engenious.Graphics;

namespace TestLandscape
{
    public class SunLight : GameObject<SunLight>
    {
        public class SunShadowCamera : Camera
        {
            public SunLight SunLight { get; internal set; }

            protected override void OnLoad()
            {
                base.OnLoad();
                IsOrtho = true;
            }

            protected override void OnDraw(GameTime time, Camera camera, SunLight sun, RenderTarget2D shadowMap, Matrix shadowProjView)
            {
                LookAt = camera.Position;
                Position = camera.Position - SunLight.DiffuseDirection;
                Up = Vector3.UnitY;
            }
        }
        
        public Color AmbientColor { get; set; }
        public Color DiffuseColor { get; set; }
        public Vector3 DiffuseDirection { get; set; }

        public SunShadowCamera ShadowCamera { get; private set; }

        protected override  void OnLoad()
        {
            base.OnLoad();

            ShadowCamera = CreateObject<SunShadowCamera>();
            ShadowCamera.SunLight = this;
            
            AmbientColor = Color.LightGray;
            DiffuseColor = Color.White;
            DiffuseDirection = new Vector3(0, 1, -1);
        }
    }
}