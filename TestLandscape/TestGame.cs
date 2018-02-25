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
        private BasicScene scene;

        private ModelEffect modelEffect;
        
        public TestGame()
        {
            
        }
        
        public override void LoadContent()
        {
            base.LoadContent();
            
            
            
            scene = new BasicScene(this);
            scene.Load(Content,GraphicsDevice);
        }

        public override void Update(GameTime gameTime)
        {
         
            scene.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            scene.Draw(gameTime,GraphicsDevice);
        }
    }
}