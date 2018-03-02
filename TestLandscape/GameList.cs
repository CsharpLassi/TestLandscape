using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;

namespace TestLandscape
{
    public class GameList<T> : IEnumerable<T>
        where T : IGameId
    {
        protected Action<T> addItem;
        protected Action<T> removeItem;
        
        protected readonly HashSet<int> Ids = new HashSet<int>();
        protected readonly Dictionary<int,T> Sets = new Dictionary<int, T>();

        //TODO:Lock ???
        protected readonly Queue<T> AddQueue = new Queue<T>();
        protected readonly Queue<T> RemoveQueue = new Queue<T>();
        
        private int enumerableSemaphore = 0;
        
        private object changeLockObject = new object();
        
        public void Clear()
        {
            lock (changeLockObject)
            {
                Ids.Clear();
                Sets.Clear();
                AddQueue.Clear();
                RemoveQueue.Clear();
            }
        }

        public GameList()
        {
            
        }
        
        public GameList(Action<T> addItem, Action<T> removeItem)
        {
            this.addItem = addItem;
            this.removeItem = removeItem;
        }
        
        public bool Add(T component)
        {
            if (!Ids.Contains(component.Id))
            {
                Ids.Add(component.Id);

                lock (changeLockObject)
                {
                    if (enumerableSemaphore == 0)
                    {
                        Sets.Add(component.Id, component);
                        addItem?.Invoke(component);
                    }
                    else
                        AddQueue.Enqueue(component);

                }
                return true;
            }
            
            return false;
        }

        
        public bool Remove(T component)
        {
            if (Ids.Contains(component.Id))
            {
                Ids.Remove(component.Id);

                lock (changeLockObject)
                {
                    if (enumerableSemaphore == 0)
                    {
                        Sets.Remove(component.Id);
                        removeItem?.Invoke(component);
                    }
                    else
                        RemoveQueue.Enqueue(component);

                    return true;
                }
                
                
            }
            return false;
        }
        
        private void UpdateQueues()
        {
            lock (changeLockObject)
            {
                while (RemoveQueue.Count > 0)
                {
                    var item = RemoveQueue.Dequeue();
                    Sets.Remove(item.Id);
                    addItem?.Invoke(item);
                }
            
                while (AddQueue.Count > 0)
                {
                    var item = AddQueue.Dequeue();
                    Sets.Add(item.Id,item);
                    removeItem?.Invoke(item);
                }
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