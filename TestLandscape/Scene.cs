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
        
        public void Load(GameSimulation simulation)
        {
            Simulation = simulation;
            
            Camera = Children.Create<Camera>(simulation, this);
            SunLight = Children.Create<SunLight>(simulation, this);
            
            OnLoad();
        }

        public virtual void OnLoad()
        {

        }
    }
}