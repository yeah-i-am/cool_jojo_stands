using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cool_jojo_stands.Stands
{
    public class Fist
    {
        public Fist(int direction, int time, Vector2 position)
        {
            this.direction = direction;
            this.time = time;
            this.position = position;
        }
        
        public int time;
        public Vector2 position;
        public int direction;
    }
}
