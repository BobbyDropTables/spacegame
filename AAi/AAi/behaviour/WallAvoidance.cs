using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using AAI.Entity;
using AAI.Entity.MovingEntities;
using AAI.Entity.staticEntities;
using AAI.View;
using AAI.world;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AAI.behaviour
{
    internal class WallAvoidance : SteeringBehaviour
    {
        private readonly float         _feelerLength;
        private Vector2 feeler;
        private          MovingEntity  _parent;
        private Vector2 steeringForce;

        public WallAvoidance(MovingEntity me, float feelerLength) : base(me)
        {
            _feelerLength = feelerLength;
        }

        public override Vector2 Calculate()
        {
            // Create feeler first
            feeler = ME.Pos + _feelerLength * ME.Velocity;
            steeringForce = new Vector2(0, 0);
            float distToThisIP    = 0.0f;
            float distToClosestIP = float.MaxValue;

            // This will hold an index into the vector of walls
            int closestWall = -1;

            // Some vectors for returning steerforce and temporary vectors
            
            Vector2 point         = new Vector2(0, 0);

            // Examine each feeler in turn
                List<BaseGameEntity> staticEntities = ME.MyWorld.Entities.Where(entity => entity is Wall).ToList();
                // Run through walls
                for (int wall = 0; wall < staticEntities.Count; wall++)
                {
                    // Check for intersection
                    if (LineIntersection2D(ME.Pos,
                                           feeler,
                                           ((Wall) staticEntities[wall]).Start,
                                           ((Wall) staticEntities[wall]).End,
                                           ref distToThisIP, ref point))
                    {
                        //Is this closest found so far? If so keep record
                        if (distToThisIP < distToClosestIP)
                        {
                            distToClosestIP = distToThisIP;
                            closestWall     = wall;
                        }
                    }

                    // If intersect detected calculate a force to steer away
                    if (closestWall >= 0)
                    {
                        // Create a force in the direction of the wall normal, with a magnitude of the overshoot
                        Vector2 temp = Vector2.Normalize(((Wall)staticEntities[closestWall]).End -
                                                         ((Wall)staticEntities[closestWall]).Start);
                        Vector2 normal = new Vector2(0, 0)
                        {
                            X = -temp.Y,
                            Y = temp.X,
                        };
                        //steeringForce = normal * OverShoot;
                        steeringForce = 3.0f * (-2 * (Vector2.Dot(ME.Velocity, normal)) * normal + ME.Velocity);

                    }
            }
            
            return steeringForce;
        }

        public override void DebugDraw(SpriteBatch spriteBatch, float scale)
        {
            var Start = ME.Pos;
            var End   = ME.Pos + steeringForce * scale;
            Console.WriteLine("Start:{0}\tEnd:{1}", Start, End);
            Vector2 edge = End - Start;
            // calculate angle to rotate line
            float angle =
                (float)Math.Atan2(edge.Y, edge.X);
            var Texture = TextureStorage.Textures["Line"];
            var origin  = new Vector2(0);
            spriteBatch.Draw(Texture,
                             new Rectangle(
                                           (int)Start.X,
                                           (int)Start.Y,
                                           (int)edge.Length(),
                                           1),
                             null,
                             Color.Yellow,
                             angle,
                             new Vector2(0, 0.5f),
                             SpriteEffects.None,
                             0);
        }

        private bool LineIntersection2D(Vector2 A,
                                        Vector2 B,
                                        Vector2 C,
                                        Vector2 D, ref float dist, ref Vector2 point)
        {
            float rTop = (A.Y - C.Y) * (D.X - C.X) - (A.X - C.X) * (D.Y - C.Y);
            float sTop = (A.Y - C.Y) * (B.X - A.X) - (A.X - C.X) * (B.Y - A.Y);

            float Bot = (B.X - A.X) * (D.Y - C.Y) - (B.Y - A.Y) * (D.X - C.X);

            if (Bot == 0) //parallel
                return false;

            float invBot = 1.0f  / Bot;
            float r      = rTop * invBot;
            float s      = sTop * invBot;

            if (r > 0 && r < 1 && s > 0 && s < 1)
            {
                dist  = Vector2.Distance(A, B) * r;
                point = A + r * (B - A);
                //lines intersect
                return true;
            }

            //lines do not intersect
            return false;
        }
    }
}