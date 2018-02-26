using System;
using engenious;
using engenious.Input;
using TestLandscape.Components;

namespace TestLandscape.Scripts
{
    public class PlayerMoveInputScript : Script<PlayerMoveInputScript>
    {
        public override void Update(GameTime gameTime)
        {
            var state = Keyboard.GetState();
            var fac = 10;

            TranslationComponent trans;
            HeadComponent head;

            if (!GameObject.Components.TryGet(out trans))
                return;
            
            if (!GameObject.Components.TryGet(out head))
                return;
            
            if (state.IsKeyDown(Keys.W))
            {
                trans.Position += new Vector3((float)Math.Cos(head.Angle),-(float)Math.Sin(head.Angle),0) *0.1f * fac;
            }
            
            if (state.IsKeyDown(Keys.S))
            {
                trans.Position -= new Vector3((float)Math.Cos(head.Angle),-(float)Math.Sin(head.Angle),0) *0.1f * fac;
            }
            
            if (state.IsKeyDown(Keys.A))
            {
                trans.Position -= new Vector3((float)Math.Sin(head.Angle),(float)Math.Cos(head.Angle),0) *0.1f * fac;
            }
            
            if (state.IsKeyDown(Keys.D))
            {
                trans.Position += new Vector3((float)Math.Sin(head.Angle),(float)Math.Cos(head.Angle),0) *0.1f * fac;
            }

            if (state.IsKeyDown(Keys.Space))
            {
                trans.Position += new Vector3(0,0,1)*0.1f;
            }
            
            if (state.IsKeyDown(Keys.ShiftLeft))
            {
                trans.Position += new Vector3(0,0,-1)*0.1f;
            }
        }

        protected override void OnCopy(PlayerMoveInputScript component)
        {
            
        }
    }
}