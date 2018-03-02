using System;
using System.Collections.Generic;
using System.Diagnostics;
using engenious;
using engenious.Content;
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
           
        private readonly List<GameSimulationComponent> gameSimulationComponents = new List<GameSimulationComponent>();

        public ContentManager Manager => Game.Content;
        
           
        
        public GameSimulation(Game game) : base(game)
        {
            gameSimulationComponents.Add(ScriptSimulation.Register());
            gameSimulationComponents.Add(HeadSimulation.Register());
            gameSimulationComponents.Add(GravitySimulation.Register());

            renderSimulation = RenderSimulation.Register();
            
            gameSimulationComponents.Add(renderSimulation);
        }

        public void LoadScene<T>()
            where T: Scene, new()
        {
            var newScene = new T();
            newScene.Load(this);

            CurrentScene = newScene;
        }

        
        
        public override void Update(GameTime gameTime)
        {
            if (CurrentScene == null)
                return;

            foreach (var component in gameSimulationComponents)
            {
                component.BeginUpdate(gameTime);
            }

            foreach (var component in gameSimulationComponents)
            {
                component.Update(gameTime);
            }
        }


        public override void Draw(GameTime gameTime)
        {
            if (CurrentScene == null)
                return;
            
            renderSimulation.Draw(GraphicsDevice,CurrentScene,gameTime);
        }

        private readonly object registerLockObject = new object();
        
        public void RegisterComponent(IGameObjectComponent component)
        {
            lock (registerLockObject)
            {
                foreach (var simulationComponent in gameSimulationComponents)
                {
                    simulationComponent.Add(component);
                }
            }
        }
        
        
        public void RemoveComponent(IGameObjectComponent component)
        {
            lock (registerLockObject)
            {
                foreach (var simulationComponent in gameSimulationComponents)
                {
                    simulationComponent.Remove(component);
                }
            }
        }
    }
}