using System;
using engenious;
using engenious.Input;
using TestLandscape.Components;

namespace TestLandscape.Scripts
{
    public class PlaceItemScript : Script<PlaceItemScript>
    {
        public GameObject PlaceBox;
        public int Distance = 5;

        public float Rotation = 0;
        
        public override void Update(GameTime gameTime)
        {
            var mouseState = Mouse.GetState();
            if (mouseState.RightButton == ButtonState.Pressed)
            {
                foreach (var child in PlaceBox.Children)
                {
                    var copy = child.Copy();
                    if (copy.Components.TryGet<TranslationComponent>(out var translation))
                    {
                        Matrix translationMatrix = PlaceBox.GetGlobalWorldMatrix();
                        translation.Position = translationMatrix.Translation;
                        translation.Rotation = new Vector3(0,0,Rotation);
                        copy.Parent = null;
                    
                        Scene.Children.Add(copy);
                    }
                }
     
                PlaceBox.IsEnabled = false;
                IsEnabled = false;
                return;
            }

            Rotation = mouseState.Wheel/10.0f;
            
            TranslationComponent boxPosition;
            HeadComponent playerHead;
            
            if (!PlaceBox.Components.TryGet(out boxPosition))
                return;

            if (!GameObject.Components.TryGet(out playerHead))
                return;
            
            
            boxPosition.Position = new Vector3((float)Math.Cos(playerHead.Angle)*Distance
                ,-(float)Math.Sin(playerHead.Angle)*Distance,(float)Math.Sin(playerHead.Tilt)*Distance);
            boxPosition.Rotation = new Vector3(0,0,Rotation);
        }

        protected override void OnCopy(PlaceItemScript component)
        {
            component.Distance = Distance;
        }
    }
}