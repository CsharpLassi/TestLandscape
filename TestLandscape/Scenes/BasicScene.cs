using engenious;
using engenious.Content;
using engenious.Graphics;
using TestLandscape.Components;
using TestLandscape.Models;
using TestLandscape.Terrain;

namespace TestLandscape.Scenes
{
    public class BasicScene : Scene
    {  
        public override void OnLoad(ContentManager manager,GraphicsDevice device)
        {   
            Children.Create<Landscape>(manager, device, this)
                .CreateComponent<ScalingComponent>(s => s.Scaling = new Vector3(5,5,100));

            Children.Create<Player>(manager, device, this);
        }



    }
}