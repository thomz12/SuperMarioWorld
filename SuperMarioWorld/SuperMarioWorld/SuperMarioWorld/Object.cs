using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SuperMarioWorld
{
    class Object : GameObject
    {
        //Does the object have a bounding box?
        private bool _blocking;

        public Object(Vector2 position) : base(position)
        {
            _blocking = true;
        }
    }
}
