using System.Diagnostics;
using engenious;
using engenious.Content;
using engenious.Graphics;

namespace TestLandscape
{
    public class Scene
    {
        
        
        public GameObjectCollection Children { get; set; } = new GameObjectCollection();

        public Camera Camera { get; set; }
        public SunLight SunLight { get; set; }

        public GameSimulation Simulation { get; private set; }
        
        public void Load(GameSimulation simulation,ContentManager manager,GraphicsDevice device)
        {
            Simulation = simulation;
            
            Camera = Children.Create<Camera>(manager,device, this);
            SunLight = Children.Create<SunLight>(manager, device, this);
            
            OnLoad(manager,device);
        }

        public virtual void OnLoad(ContentManager manager, GraphicsDevice device)
        {

        }
    }
}