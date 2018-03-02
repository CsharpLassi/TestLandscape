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
        public override void OnLoad()
        {   
            Children.Create<Landscape>(Simulation, this)
                .CreateComponent<ScalingComponent>(s => s.Scaling = new Vector3(5,5,100));

            Children.Create<Player>(Simulation, this);
        }



    }
}