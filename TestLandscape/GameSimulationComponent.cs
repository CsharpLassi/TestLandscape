﻿using System;
using System.Collections.Generic;
using engenious;

namespace TestLandscape
{
    public abstract class GameSimulationComponent<TS,T> : GameSimulationComponent
        where TS: GameSimulationComponent<TS,T>,new()
        where T: GameObjectComponent<T>
    {
        public static readonly TS Instance = new TS();

        public static void Register()
        {
            Register<TS>();
        }

        protected sealed override void Update(GameTime time, GameObject gameObject)
        {
            if (gameObject.Components.TryGet<T>(out var component))
                Update(gameObject,component,time);
        }

        protected abstract void Update(GameObject gameObject, T component, GameTime time);
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
        
        public static void Simulate(GameTime time,GameObject gameObject)
        {
            foreach (var simulation in simulations)
            {
                simulation.Update(time,gameObject);
            }
        }

        protected abstract void Update(GameTime time, GameObject gameObject);

        protected abstract void BeginUpdate();
        
        public static void BeginSimulation()
        {
            foreach (var simulation in simulations)
            {
                simulation.BeginUpdate();
            }
        }
    }
}