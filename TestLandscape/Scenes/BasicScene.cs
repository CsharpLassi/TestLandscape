﻿using engenious;
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
            var landscape = Children.Create<Landscape>(Simulation, this, i => i.Scaling = new Vector3(1, 1, 1));

            Children.Create<Player>(Simulation, this);
        }



    }
}