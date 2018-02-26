using System;
using System.Collections.Generic;
using System.Linq;
using TestLandscape.Scenes;

namespace TestLandscape
{
    public class GameObjectComponentCollection : List<GameObjectComponent>
    {
        public T Create<T>(GameObject gameObject,Scene scene,Action<T> fill)
            where T: GameObjectComponent,new()
        {
            var component = new T();
            component.Load(gameObject,scene);
            Add(component);
            
            fill?.Invoke(component);
            
            return component;
        }
        
        public T Create<T>(GameObject gameObject,Scene scene)
            where T: GameObjectComponent,new()
        {
            return Create<T>(gameObject, scene, null);
        }

        public T Get<T>()
        {
            return this.OfType<T>().FirstOrDefault();
        }

        public bool TryGet<T>(out T component)
        {
            component =  this.OfType<T>().FirstOrDefault();

            return component != null;
        }
    }
}