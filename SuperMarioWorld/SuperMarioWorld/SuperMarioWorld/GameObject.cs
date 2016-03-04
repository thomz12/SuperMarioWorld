using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SuperMarioWorld
{
    class GameObject
    {
        public Rectangle boundingBox;
        public Vector2 position;

        public GameObject(Vector2 position)
        {
            this.position = position;
        }
    }
}
