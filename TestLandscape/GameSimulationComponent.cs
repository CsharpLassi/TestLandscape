using System;
using System.Collections.Generic;
using engenious;

namespace TestLandscape
{
    public abstract class GameSimulationComponent<TS,T> : GameSimulationComponent
        where TS: GameSimulationComponent<TS,T>,new()
        where T: GameObjectComponent<T>
    {
        public static readonly TS Instance = new TS();

        public GameList<T> Components { get; } = new GameList<T>();
        
        public static TS Register()
        {
            return Register<TS>();
        }

        public sealed override void Add(IGameObjectComponent component)
        {
            if (component is T tComponent)
            {
                Components.Add(tComponent);
            }
        }

        public override void Remove(IGameObjectComponent component)
        {
            if (component is T tComponent)
            {
                Components.Remove(tComponent);
            }
        }

        public sealed override void Update(GameTime time)
        {
            foreach (var component in Components)
            {
                Update(component,time);
            }
        }

        protected abstract void Update( T component, GameTime time);
    }

    public abstract class GameSimulationComponent
    {
        private static HashSet<GameSimulationComponent> simulations = new HashSet<GameSimulationComponent>();
        
        protected static T Register<T>()
            where T : GameSimulationComponent,new()
        {
            var simulationComponent = new T();
            
            simulations.Add(simulationComponent);

            return simulationComponent;
        }

        public abstract void BeginUpdate(GameTime time);
        public abstract void Update(GameTime time);

        public abstract void Add(IGameObjectComponent component);

        public abstract void Remove(IGameObjectComponent component);
    }
}