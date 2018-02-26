using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;

namespace TestLandscape
{
    public class GameList<T> : IEnumerable<T>
        where T : IGameId
    {
        protected readonly HashSet<int> Ids = new HashSet<int>();
        protected readonly Dictionary<int,T> Sets = new Dictionary<int, T>();

        //TODO:Lock ???
        protected readonly Queue<T> AddQueue = new Queue<T>();
        protected readonly Queue<T> RemoveQueue = new Queue<T>();
        
        private int enumerableSemaphore = 0;
        
        public bool Add(T component)
        {
            if (!Ids.Contains(component.Id))
            {
                Ids.Add(component.Id);
                
                if (enumerableSemaphore == 0)
                    Sets.Add(component.Id,component);
                else
                    AddQueue.Enqueue(component);
                
                
                return true;
            }
            
            return false;
        }

        public bool Remove(T component)
        {
            if (Ids.Contains(component.Id))
            {
                Ids.Remove(component.Id);
                
                if (enumerableSemaphore == 0)
                    Sets.Remove(component.Id);
                else
                    RemoveQueue.Enqueue(component);
                
                return true;
            }
            return false;
        }
        
        private void UpdateQueues()
        {
            while (RemoveQueue.Count > 0)
            {
                var item = RemoveQueue.Dequeue();
                Sets.Remove(item.Id);
            }
            
            while (AddQueue.Count > 0)
            {
                var item = AddQueue.Dequeue();
                Sets.Add(item.Id,item);
            }
        }
        
        public IEnumerator<T> GetEnumerator()
        {
            Interlocked.Increment(ref enumerableSemaphore);
            
            foreach (var gameObjectComponent in Sets)
            {
                yield return gameObjectComponent.Value;
            }

            var result = Interlocked.Decrement(ref enumerableSemaphore);
            if (result == 0)
            {
                UpdateQueues();
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}