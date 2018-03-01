using engenious;
using engenious.Content;
using engenious.Graphics;
using TestLandscape.Components;
using TestLandscape.Components.Models;
using TestLandscape.Scripts;
using TestLandscape.Scripts.World;

namespace TestLandscape.Models
{
    public class Player : GameObject<Player>
    {
        protected override void OnLoad()
        {
            CreateComponent<TranslationComponent>();
            CreateComponent<HeadComponent>();

            CreateComponent<PlayerMouseInputScript>();
            CreateComponent<PlayerMoveInputScript>();

            CreateComponent<GravityCompononent>();
            
            var placeBox = CreateObject<GameObject>();
            placeBox.CreateComponent<TranslationComponent>();
            placeBox.CreateComponent<Tree1ModelComponent>();

            CreateComponent<PlaceItemScript>(i => i.PlaceBox = placeBox);
        }
    }
}