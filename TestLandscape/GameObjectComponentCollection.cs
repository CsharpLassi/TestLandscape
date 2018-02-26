using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using engenious;
using TestLandscape.Scenes;

namespace TestLandscape
{
    public class GameObjectComponentCollection : GameList<IGameObjectComponent>
    {
        public T CreateOrGet<T>(GameObject gameObject,Scene scene,Action<T> fill)
            where T: GameObjectComponent<T>,new()
        {
            if (TryGet<T>(out T result))
            {
                return result;
            }
            
            var component = new T();
            component.Load(gameObject,scene);
            Add(component);
            
            fill?.Invoke(component);
            
            return component;
        }
        
        public T CreateOrGet<T>(GameObject gameObject,Scene scene)
            where T: GameObjectComponent<T>,new()
        {
            return CreateOrGet<T>(gameObject, scene, null);
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