using engenious;
using engenious.Content;
using engenious.Graphics;
using TestLandscape.Components;
using TestLandscape.Components.Models;
using TestLandscape.Components.Models.Grass;
using TestLandscape.Components.Models.Objects;
using TestLandscape.Scripts;
using TestLandscape.Scripts.World;

namespace TestLandscape.Models
{
    public class Player : GameObject<Player>
    {
        protected override void OnLoad()
        {
            CreateComponent<TranslationComponent>(t => t.Position = new Vector3(0,0,10));
            CreateComponent<HeadComponent>();

            CreateComponent<PlayerMouseInputScript>();
            CreateComponent<PlayerMoveInputScript>();

            CreateComponent<GravityCompononent>();
            
            //var placeBox = CreateObject<GameObject>();
            //placeBox.CreateComponent<TranslationComponent>();
            //placeBox.CreateComponent<BarrelModelComponent>();

            //CreateComponent<PlaceItemScript>(i => i.PlaceBox = placeBox);
        }
    }
}