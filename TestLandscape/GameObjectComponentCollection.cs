﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using engenious;
using engenious.Content;
using engenious.Graphics;
using TestLandscape.Scenes;

namespace TestLandscape
{
    public class GameObjectComponentCollection : GameList<IGameObjectComponent>
    {
        public GameObjectComponentCollection()
        {
            addItem = addCallback;
            removeItem = removeCallback;
        }

        private void removeCallback(IGameObjectComponent obj)
        {
            obj.Simulation.RemoveComponent(obj);
        }

        private void addCallback(IGameObjectComponent obj)
        {
            obj.Simulation.RegisterComponent(obj);
        }


        public T CreateOrGet<T>(GameObject gameObject,Scene scene,GameSimulation simulation,Action<T> fill)
            where T: GameObjectComponent<T>,new()
        {
            if (TryGet<T>(out T result))
            {
                return result;
            }
            
            var component = new T();
            component.Load(gameObject,scene,simulation);
            Add(component);
            
            fill?.Invoke(component);
            
            return component;
        }
        
        public T CreateOrGet<T>(GameObject gameObject,Scene scene,GameSimulation simulation)
            where T: GameObjectComponent<T>,new()
        {
            return CreateOrGet<T>(gameObject, scene, simulation, null);
        }

        public bool TryGet<T>(out T component)
            where T: GameObjectComponent<T>
        {
            var id = GameObjectComponent<T>.ComponentId;
            if (Ids.Contains(id))
            {
                component =  (T)Sets[id];
                return true;
            }

            component = null;
            return false;
        }



    }
}