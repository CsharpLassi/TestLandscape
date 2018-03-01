using System.Collections.Generic;
using engenious;
using engenious.Graphics;
using TestLandscape.Simulation;
using TestLandscape.Simulation.World;

namespace TestLandscape
{
    public class GameSimulation : DrawableGameComponent
    {
        public Scene CurrentScene { get; private set; }
        Queue<GameObject> updateObjects = new Queue<GameObject>();
       
        
        private readonly RenderSimulation renderSimulation;
        
        public GameSimulation(Game game) : base(game)
        {
            ScriptSimulation.Register();
            HeadSimulation.Register();
            GravitySimulation.Register();
            
            renderSimulation = RenderSimulation.Register();
            

        }

        public void LoadScene<T>()
            where T: Scene, new()
        {
            var newScene = new T();
            newScene.Load(this,Game.Content,GraphicsDevice);

            CurrentScene = newScene;
        }

        
        
        public override void Update(GameTime gameTime)
        {
            if (CurrentScene == null)
                return;

            GameSimulationComponent.BeginSimulation();
            
            foreach (var child in CurrentScene.Children)
            {
                if (!child.IsEnabled)
                    continue;
                
                updateObjects.Enqueue(child);
            }

            while (updateObjects.Count > 0)
            {
                var gameObject = updateObjects.Dequeue();

                GameSimulationComponent.Simulate(gameTime, gameObject);
                foreach (var child in gameObject.Children)
                {
                    if (!child.IsEnabled)
                        continue;
                    
                    updateObjects.Enqueue(child);
                    
                    
                }
            }
        }


        public override void Draw(GameTime gameTime)
        {
            if (CurrentScene == null)
                return;
            
            renderSimulation.Draw(GraphicsDevice,CurrentScene,gameTime);
        }
    }
}