using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Asteroids
{
    public static class Physics
    {
        public static float RandomRotation()
        {
            return (float)(Random.Shared.NextDouble() * Math.PI * 2.0f);
        }
       public static float DistanceBetween(Vector2 a, Vector2 b)
        {
            return (float)(Math.Sqrt(Math.Abs(a.X - b.X) * Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y) * Math.Abs(a.Y - b.Y)));
        }
        public static bool RadiusCollisionBetween(Entity a, Entity b)
        {
            float x, y, distance;
            float ax = a.Position.X;
            float bx = b.Position.X;
            float ay = a.Position.Y;
            float by = b.Position.Y;
            float ar = a.Radius;
            float br = b.Radius;
            x = Math.Abs(a.Position.X - b.Position.X);
            y = Math.Abs(a.Position.Y - b.Position.Y);
            distance = (float)Math.Sqrt(x * x + y * y);
            return distance < a.Radius + b.Radius;

            //more efficient version which squares the left side in stead of sqrting the right side
            //if (ax > bx) x = ax - bx; else x = bx - ax;
            //if (ay > by) y = ay - by; else y = by - ay;
            //return (x * x + y * y) < ((ar + br) * (ar + br));
        }
        public static void CollisionBounce(GameTime gameTime, Entity a, Entity b)
        {
            //physics equations
            float aX, aY, bX, bY;
            aX = (a.Velocity.X * (a.Mass - b.Mass) + (2 * b.Mass * b.Velocity.X)) / (a.Mass + b.Mass);
            aY = (a.Velocity.Y * (a.Mass - b.Mass) + (2 * b.Mass * b.Velocity.Y)) / (a.Mass + b.Mass);
            bX = (b.Velocity.X * (b.Mass - a.Mass) + (2 * a.Mass * a.Velocity.X)) / (a.Mass + b.Mass);
            bY = (b.Velocity.Y * (b.Mass - a.Mass) + (2 * a.Mass * a.Velocity.Y)) / (a.Mass + b.Mass);
            a.SetVelocity(new Vector2(aX, aY));
            b.SetVelocity(new Vector2(bX, bY));
        }
    }
}
