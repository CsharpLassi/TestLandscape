using System;
using System.Collections.Generic;
using System.IO;
using engenious;
using engenious.Graphics;
using engenious.Helper;
using engenious.Input;
using engenious.UserDefined;
using TestLandscape.Scenes;

namespace TestLandscape
{
    public class TestGame : Game
    { 
        private GameSimulation simulation;

        //private ModelEffect modelEffect;
        
        public TestGame()
        {
            simulation = new GameSimulation(this);
            Components.Add(simulation);
        }
        
        public override void LoadContent()
        {
            base.LoadContent();

            simulation.LoadScene<BasicScene>();
            
        }
    }
}