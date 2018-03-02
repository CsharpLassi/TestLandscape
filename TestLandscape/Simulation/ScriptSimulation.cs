using engenious;

namespace TestLandscape.Simulation
{
    public class ScriptSimulation : GameSimulationComponent
    {
        public static ScriptSimulation Register()
        {
            return Register<ScriptSimulation>();
        }

        protected readonly GameList<IScript> scripts = new GameList<IScript>();
        
        public override void BeginUpdate(GameTime time)
        {
            
        }

        public override void Update(GameTime time)
        {

            foreach (var script in scripts)
            {
                script.Update(time);
            }
            
        }

        public override void Add(IGameObjectComponent component)
        {
            if (component is IScript script)
            {
                scripts.Add(script);
            }
        }

        public override void Remove(IGameObjectComponent component)
        {
            if (component is IScript script)
            {
                scripts.Remove(script);
            }
        }
    }
}