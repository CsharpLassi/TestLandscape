﻿using engenious;
using TestLandscape.Components;
using TestLandscape.Components.Models.Grass;

namespace TestLandscape.Models.Grass
{
    public class Grass1ModelObject : GameObject<Grass1ModelObject>
    {
        protected override void OnLoad()
        {
            CreateComponent<Grass1ModelComponent>();
        }
        
        public static Grass1ModelObject Create(GameObject parent,Vector3 position)
        {
            var newObject = parent.CreateObject<Grass1ModelObject>();
            if (newObject.Components.TryGet<TranslationComponent>(out var trans))
            {
                trans.Position = position;
            }

            return newObject;
        }
    }
}