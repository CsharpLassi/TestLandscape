using System.Collections.Generic;
using engenious;
using engenious.Graphics;
using TestLandscape.Components;

namespace TestLandscape
{
    public abstract class DrawComponent<T> : GameObjectComponent<T>, IDrawComponent 
        where T : DrawComponent<T>
    {
        public abstract void Draw(int step, RenderPass pass, 
            GameTime time, Camera camera, SunLight sun, RenderTarget2D shadowMap, Matrix shadowProjView);
        
        public bool UseLevelOfDetail { get; set; } = true;
        
        public bool IsStatic { get; set; }

        public bool IsTransparent { get; set; }

        public bool HasShadow { get;  set; }

        public bool IsCamera { get; protected set; }
    }
}