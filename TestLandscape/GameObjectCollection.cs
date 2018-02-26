﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using engenious.Content;
using engenious.Graphics;

namespace TestLandscape
{
    public class GameObjectCollection : GameList<GameObject>
    {
        
        public T Create<T>(ContentManager manager,GraphicsDevice device,Scene scene,GameObject parent,Action<T> fill)
            where T : GameObject,new()
        {
            var gameObject = new T();
            gameObject.Parent = parent;
            gameObject.Load(manager, device, scene);
            
            this.Add(gameObject);

            fill?.Invoke(gameObject);
            
            return gameObject;
        }
        
        public T Create<T>(ContentManager manager, GraphicsDevice device, Scene scene, GameObject parent)
            where T : GameObject, new()
        {
            return Create<T>(manager, device, scene, parent, null);
        }
        
        public T Create<T>(ContentManager manager, GraphicsDevice device, Scene scene, Action<T> fill)
            where T : GameObject, new()
        {
            return Create<T>(manager, device, scene, null, fill);
        }
        
        public T Create<T>(ContentManager manager, GraphicsDevice device, Scene scene)
            where T : GameObject, new()
        {
            return Create<T>(manager, device, scene, null, null);
        }
    }
}