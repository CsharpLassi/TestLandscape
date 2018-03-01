using engenious;
using TestLandscape.Components;

namespace TestLandscape.Scripts.World
{
    public class GravityCompononent : GameObjectComponent<GravityCompononent>
    {
        public float Gravity = 9.81f;
        
        protected override void OnCopy(GravityCompononent component)
        {
            component.Gravity = 9.81f;
        }
    }
}