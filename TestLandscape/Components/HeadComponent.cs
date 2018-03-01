using System;
using System.Collections;
using engenious;
using engenious.Helper;
using engenious.Input;

namespace TestLandscape.Components
{
    public class HeadComponent : GameObjectComponent<HeadComponent>
    {
        public Vector3 Position = new Vector3(0,0,1);
        
        public float Tilt;
        public float Angle;

        protected override void OnCopy(HeadComponent component)
        {
            component.Position = Position;
            component.Angle = Angle;
            component.Tilt = Tilt;       
        }
    }
}