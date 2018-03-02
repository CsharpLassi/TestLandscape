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
        
        public bool UseLevelOfDetail { get; protected set; } = true;

        public bool IsVisible { get; protected set; } = true;

        public bool IsTransparent { get; protected set; }

        public bool HasShadow { get; protected set; }

        public bool IsCamera { get; protected set; }
    }
}