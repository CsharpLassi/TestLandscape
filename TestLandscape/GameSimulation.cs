using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        
        private Stopwatch updateStopWatch = new Stopwatch();  
        private Stopwatch drawStopwatch = new Stopwatch();
        
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
            updateStopWatch.Restart();
            
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

            Console.WriteLine("Update:" + updateStopWatch.ElapsedMilliseconds);
        }


        public override void Draw(GameTime gameTime)
        {
            drawStopwatch.Restart();
            if (CurrentScene == null)
                return;
            
            renderSimulation.Draw(GraphicsDevice,CurrentScene,gameTime);
            Console.WriteLine("Draw:" + drawStopwatch.ElapsedMilliseconds);
        }
    }
}