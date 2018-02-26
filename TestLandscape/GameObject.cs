using System;
using System.ComponentModel;
using engenious;
using engenious.Content;
using engenious.Graphics;
using TestLandscape.Components;

namespace TestLandscape
{
    public class GameObject
    {
        public GameObjectCollection Children { get; set; } = new GameObjectCollection();
        public GameObjectComponentCollection Components { get; set; } = new GameObjectComponentCollection();
        
        public virtual void Load(ContentManager manager,GraphicsDevice device,Scene scene)
        {
        }
        
        public void Update(GameTime time)
        {
            foreach (var component in Components)
            {
                component.Update(time);
            }

            OnUpdate();
            
            foreach (var child in Children)
            {
                child.Update(time);
            }
        }

        protected virtual void OnUpdate()
        {
            
        }
        
        private bool firstDraw = true;
        protected TranslationComponent translationComponent;
        
        public void Draw(RenderPass pass, GameTime time, GraphicsDevice device, Camera camera, SunLight sun, Matrix world,
            RenderTarget2D shadowMap, Matrix shadowProjView)
        {
            if (firstDraw)
            {
                translationComponent = Components.Get<TranslationComponent>();
                firstDraw = false;
            }


            if (translationComponent != null)
                world = world * translationComponent.Matrix;
            
            OnDrawChildren(pass,time,device, camera, sun, world, shadowMap, shadowProjView);

            if (Components.TryGet<ScalingComponent>(out var component ))
            {
                world = world * component.Matrix;
            }
            
            OnDraw(pass,time,device, camera, sun, world, shadowMap, shadowProjView);
        }

        protected virtual void OnDrawChildren(RenderPass pass,GameTime time,GraphicsDevice device, Camera camera, SunLight sun, Matrix world,
            RenderTarget2D shadowMap, Matrix shadowProjView)
        {
            foreach (var child in Children)
            {
                child.Draw(pass, time, device, camera, sun, world,shadowMap,shadowProjView);
            }
        }


        protected virtual void OnDraw(RenderPass pass, GameTime time, GraphicsDevice device, Camera camera, SunLight sun, Matrix world,
            RenderTarget2D shadowMap, Matrix shadowProjView)
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
    }
    
    public abstract class GameObject<GT> : GameObject
        where GT: GameObject<GT>,new()
    {

        public GameObject<GT> Create<T>(Scene scene,Action<T> fill)
            where T: GameObjectComponent,new()
        {
            var component = Components.Create<T>(this, scene);
            
            fill?.Invoke(component);
            return this;
        }
        
        
    }
}