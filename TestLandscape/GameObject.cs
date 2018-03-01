﻿using System;
using System.ComponentModel;
using System.Dynamic;
using System.Threading;
using engenious;
using engenious.Content;
using engenious.Graphics;
using TestLandscape.Components;

namespace TestLandscape
{
    public class GameObject : IGameId
    {
        private static int globalId;
        
        private GameObject parent;
        
        public GameObjectCollection Children { get; set; } = new GameObjectCollection();
        public GameObjectComponentCollection Components { get; set; } = new GameObjectComponentCollection();

        public bool IsEnabled { get; set; } = true;


        
        public int Id { get; } = Interlocked.Increment(ref globalId);
        
        public GameObject Parent
        {
            get { return parent; }
            set
            {
                if (parent != value)
                {
                    parent = value;
                    parent.Children.Remove(this);
                }
            }
        }

        public Scene Scene { get; private set; }
        
        protected ContentManager Manager { get; private set; }
        protected GraphicsDevice GraphicsDevice { get; private set; }
        
        public void Load(ContentManager manager,GraphicsDevice device,Scene scene)
        {
            Scene = scene;
            Manager = manager;
            GraphicsDevice = device;
            
            OnLoad();
        }

        protected virtual void OnLoad()
        {
            
        }

        
        public T OfType<T>()
            where T: GameObject
        {
            if (this is T value)
            {
                return value;
            }
            return null;
        }
        
        public T CreateObject<T>(Action<T> fill = null) 
            where T : GameObject,new()
        {
            var newObject = Children.Create<T>(Manager, GraphicsDevice, Scene, this, fill);
            return newObject;
        }
        
        public T CreateComponent<T>(Action<T> fill = null) 
            where T : GameObjectComponent<T>,new()
        {
            var newObject = Components.CreateOrGet<T>(this, Scene, Manager, GraphicsDevice, fill);
            return newObject;
        }

        public Matrix GetGlobalWorldMatrix()
        {
            Matrix parentWorld = Parent?.GetGlobalWorldMatrix() ?? Matrix.Identity;

            if (Components.TryGet<TranslationComponent>(out var translationComponent))
                return parentWorld * translationComponent.Matrix;

            return parentWorld;
        }

        public GameObject Copy()
        {
            var newObject = (GameObject)Activator.CreateInstance(this.GetType());
            foreach (var component in Components)
            {
                newObject.Components.Add(component.Copy(newObject,Scene));
            }

            foreach (var child in Children)
            {
                newObject.Children.Add(child.Copy());
            }
            
            return newObject;
        }
    }
    
    public abstract class GameObject<GT> : GameObject
        where GT: GameObject<GT>,new()
    {
 
    }
}