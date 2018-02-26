using System;
using System.Collections.Generic;
using engenious.Content;
using engenious.Graphics;

namespace TestLandscape
{
    public class GameObjectCollection : List<GameObject>
    {
        public T Create<T>(ContentManager manager,GraphicsDevice device,Scene scene,Action<T> fill)
            where T : GameObject,new()
        {
            var gameObject = new T();
            gameObject.Load(manager, device, scene);
            
            this.Add(gameObject);

            fill?.Invoke(gameObject);
            
            return gameObject;
        }
        
        public T Create<T>(ContentManager manager,GraphicsDevice device,Scene scene)
            where T : GameObject,new()
        {
            return Create<T>(manager, device, scene, null);
        }
    }
}