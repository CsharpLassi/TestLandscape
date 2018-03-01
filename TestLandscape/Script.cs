using engenious;
using TestLandscape.Simulation;

namespace TestLandscape
{
    public abstract class Script<T> : GameObjectComponent<T> , IScript
        where T : Script<T>
    {
        public virtual void Update(GameTime gameTime)
        {
            
        }
    }
}