using System;
using engenious;
using TestLandscape.Components;

namespace TestLandscape.Scripts
{
    public class PlaceItemScript : Script
    {
        public GameObject PlayeBox;
        public int Distance = 5;

        public override void Update(GameTime gameTime)
        {
            TranslationComponent boxPosition;
            HeadComponent playerHead;
            
            if (!PlayeBox.Components.TryGet(out boxPosition))
                return;

            if (!GameObject.Components.TryGet(out playerHead))
                return;
            
            boxPosition.Position = new Vector3((float)Math.Cos(playerHead.Angle)*Distance
                ,-(float)Math.Sin(playerHead.Angle)*Distance,(float)Math.Sin(playerHead.Tilt)*Distance);
            
            
        }
    }
}