using engenious;

namespace TestLandscape.Simulation
{
    public class ScriptSimulation : GameSimulationComponent
    {
        public static void Register()
        {
            Register<ScriptSimulation>();
        }
        
        protected override void Update(GameTime time, GameObject gameObject)
        {
            foreach (var component in gameObject.Components)
            {
                if (component is IScript script)
                {
                    script.Update(time);
                }
            }
        }
    }
}