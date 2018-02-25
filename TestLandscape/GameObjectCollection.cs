using System;
using System.Collections.Generic;
using engenious.Content;
using engenious.Graphics;

namespace TestLandscape
{
    public class GameObjectCollection : List<GameObject>
    {
        public T Create<T>(ContentManager manager,GraphicsDevice device,Action<T> fill)
            where T : GameObject,new()
        {
            var gameObject = new T();
            gameObject.Load(manager, device);
            
            this.Add(gameObject);

            fill?.Invoke(gameObject);
            
            return gameObject;
        }
        
        public T Create<T>(ContentManager manager,GraphicsDevice device)
            where T : GameObject,new()
        {
            return Create<T>(manager, device, null);
        }
    }
}