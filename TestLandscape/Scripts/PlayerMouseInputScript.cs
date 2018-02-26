using engenious;
using engenious.Input;
using TestLandscape.Components;

namespace TestLandscape.Scripts
{
    public class PlayerMouseInputScript : Script<PlayerMouseInputScript>
    {
        public Vector2 mouseOldPoistion;
        
        public override void Update(GameTime gameTime)
        {
            var mouse = engenious.Input.Mouse.GetState();

            HeadComponent head;
            
            if (!GameObject.Components.TryGet(out head))
                return;
            
            if (mouse.IsButtonDown(MouseButton.Left))
            {      
                if (!Scene.Game.IsMouseVisible)
                {
                    var position = new Vector2(mouse.X,mouse.Y) - mouseOldPoistion;
                    head.Angle -= position.X / 100;
                    head.Tilt -= position.Y / 100;
                }
                
                mouseOldPoistion = new Vector2(mouse.X,mouse.Y);
                Scene.Game.IsMouseVisible = false;
            }
            else
            {
                Scene.Game.IsMouseVisible = true;
            }
            
        }

        protected override void OnCopy(PlayerMouseInputScript component)
        {
        }
    }
}