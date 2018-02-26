using engenious;
using engenious.Content;
using engenious.Graphics;
using TestLandscape.Components;
using TestLandscape.Models;

namespace TestLandscape.Scenes
{
    public class BasicScene : Scene
    {
        public BasicScene(Game game) : base(game)
        {
        }
        
        public override void Load(ContentManager manager,GraphicsDevice device)
        {
            base.Load(manager,device);
            Children.Create<Tree1Object>(manager, device, this).TranslationComponent.Position =
                new Vector3(30, 30, -12);
            
            Children.Create<Landscape>(manager, device, this)
                .CreateComponent<ScalingComponent>(s => s.Scaling = new Vector3(5,5,100));

            Children.Create<Player>(manager, device, this);
        }



    }
}