using engenious;
using engenious.Content;
using engenious.Graphics;
using TestLandscape.Components;
using TestLandscape.Scripts;

namespace TestLandscape.Models
{
    public class Player : GameObject<Player>
    {
        public override void OnLoad()
        {
            CreateComponent<TranslationComponent>();
            CreateComponent<HeadComponent>();

            CreateComponent<PlayerMouseInputScript>();
            CreateComponent<PlayerMoveInputScript>();

            
            var placeBox = CreateObject<GameObject>();
            placeBox.CreateComponent<TranslationComponent>();
            placeBox.CreateObject<Tree1Object>();

            CreateComponent<PlaceItemScript>(i => i.PlaceBox = placeBox);
        }
    }
}