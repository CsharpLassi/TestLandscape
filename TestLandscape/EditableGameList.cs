using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace TestLandscape
{
    public class EditableGameList<T> : IEnumerable<T>
    {
        private readonly List<T> Sets = new List<T>();
        
        protected readonly Queue<T> AddQueue = new Queue<T>();
        protected readonly Queue<T> RemoveQueue = new Queue<T>();
        
        private int enumerableSemaphore = 0;
        
        public void Clear()
        {
            Sets.Clear();
            AddQueue.Clear();
            RemoveQueue.Clear();
        }

        public void Add(T item)
        {
            if (enumerableSemaphore == 0)
                Sets.Add(item);
            else
                AddQueue.Enqueue(item);
        }
        
        public void Remove(T item)
        {
            if (enumerableSemaphore == 0)
                Sets.Remove(item);
            else
                RemoveQueue.Enqueue(item);
        }
        
        private void UpdateQueues()
        {
            while (RemoveQueue.Count > 0)
            {
                var item = RemoveQueue.Dequeue();
                Sets.Remove(item);
            }
            
            while (AddQueue.Count > 0)
            {
                var item = AddQueue.Dequeue();
                Sets.Add(item);
            }
        }
        
        public IEnumerator<T> GetEnumerator()
        {
            Interlocked.Increment(ref enumerableSemaphore);
            
            foreach (var item in Sets)
            {
                yield return item;
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