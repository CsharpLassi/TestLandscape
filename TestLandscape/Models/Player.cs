using engenious;
using engenious.Content;
using engenious.Graphics;
using TestLandscape.Components;
using TestLandscape.Scripts;

namespace TestLandscape.Models
{
    public class Player : GameObject<Player>
    {
        public override void Load(ContentManager manager, GraphicsDevice device, Scene scene)
        {
            Create<TranslationComponent>(scene, null);
            Create<HeadComponent>(scene,null);

            Create<PlayerMouseInputScript>(scene, null);
            Create<PlayerMoveInputScript>(scene, null);

            
            var placeBox = Children.Create<GameObject>(manager, device, scene);
            placeBox.Components.Create<TranslationComponent>(placeBox, scene);
            placeBox.Children.Create<Tree1Object>(manager, device, scene);

            Create<PlaceItemScript>(scene, i => i.PlayeBox = placeBox);
        }

        protected override void OnDrawChildren(RenderPass pass, GameTime time, GraphicsDevice device, Camera camera, SunLight sun, Matrix world,
            RenderTarget2D shadowMap, Matrix shadowProjView)
        {
            base.OnDrawChildren(pass, time, device, camera, sun, world, shadowMap, shadowProjView);
        }
    }
}