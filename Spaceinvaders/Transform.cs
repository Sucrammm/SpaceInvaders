using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Spaceinvaders
{
    public class Transform
    {
        public Vector2 position;
        public Vector2 direction;
        public float speed;
        public float size;

        public Transform(Vector2 position, Vector2 direction, float speed, float size)
        {
            this.position = position;
            this.direction = direction;
            this.speed = speed;
            this.size = size;
        }
    }
}
