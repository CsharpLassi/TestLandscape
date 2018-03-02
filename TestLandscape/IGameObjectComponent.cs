using engenious;
using engenious.Content;
using engenious.Graphics;

namespace TestLandscape
{
    public interface IGameObjectComponent : IGameId
    {    
        bool IsEnabled { get; set; }

        GameObject GameObject { get; }
        GameSimulation Simulation { get; }
        
        IGameObjectComponent Copy(GameObject gameObject, Scene scene,GameSimulation simulation);
        void Load(GameObject gameObject, Scene scene, GameSimulation simulation);
    }
}