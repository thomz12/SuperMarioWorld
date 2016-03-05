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

        //If a block is enabled it can do an action TRUE when for ex. Mystery block has emptied its contents.
        protected bool used;

        public Object(Vector2 position) : base(position)
        {
            _blocking = true;
            used = false;
        }
    }
}
