using System;
using System.Collections.Generic;
using System.Linq;
using AAI.Entity;
using AAI.Entity.MovingEntities;
using AAI.Entity.staticEntities;
using AAI.world;
using Microsoft.Xna.Framework;

namespace AAI.behaviour
{
    internal class WallAvoidance : SteeringBehaviour
    {
        private readonly float         _feelerLength;
        private          List<Vector2> _feelers;
        private          MovingEntity  _parent;

        public WallAvoidance(MovingEntity me, float feelerLength) : base(me)
        {
            _feelerLength = feelerLength;
        }

        public override Vector2 Calculate()
        {
            // Create feelers first
            CreateFeelers();

            float distToThisIP    = 0.0f;
            float distToClosestIP = float.MaxValue;

            // This will hold an index into the vector of walls
            int closestWall = -1;

            // Some vectors for returning steerforce and temporary vectors
            Vector2 steeringForce = new Vector2(0, 0);
            Vector2 point         = new Vector2(0, 0);
            Vector2 closestPoint  = new Vector2(0, 0);
            World   gameWorld     = ME.MyWorld;

            // Examine each feeler in turn
            for (int feeler = 0; feeler < _feelers.Count; feeler++)
            {
                List<BaseGameEntity> StaticEntites = ME.MyWorld.Entities.Where(entity => entity is Wall).ToList();
                // Run through walls
                for (int wall = 0; wall < StaticEntites.Count; wall++)
                {
                    // Check for intersection
                    if (LineIntersection2D(ME.Pos,
                                                      _feelers[feeler],
                                                      ((Wall)StaticEntites[wall]).Start,
                                                      ((Wall)StaticEntites[wall]).End,
                                                      ref distToThisIP,
                                                      ref point))
                    {
                        // Is this closest found so far? If so keep record
                        if (distToThisIP < distToClosestIP)
                        {
                            distToClosestIP = distToThisIP;
                            closestWall = wall;
                            closestPoint = point;
                        }
                    }
                }
                // If intersect detected calculate a force to steer away
                if (closestWall >= 0)
                {
                    Vector2 OverShoot = _feelers[feeler] - closestPoint;

                    // Create a force in the direction of the wall normal, with a magnitude of the overshoot
                    Vector2 temp = Vector2.Normalize(((Wall)StaticEntites[closestWall]).End - ((Wall)StaticEntites[closestWall]).Start);
                    Vector2 normal = new Vector2(0, 0);
                    normal.X = -temp.Y;
                    normal.Y = temp.X;
                    steeringForce = normal * OverShoot;
                }
            }

            return steeringForce;
        }

        private void CreateFeelers()
        {
            // First clear list
            _feelers = new List<Vector2>();

            // Create front feeler
            _feelers.Add(ME.Pos + _feelerLength * ME.Velocity);

            // Create left feeler
            float sin = (float) Math.Sin(MathHelper.ToRadians(-111));
            float cos = (float) Math.Cos(MathHelper.ToRadians(-111));

            float tx = ME.Velocity.X;
            float ty = ME.Velocity.Y;

            Vector2 temp = new Vector2(cos * tx - sin * ty, sin * tx + cos * ty);
            _feelers.Add(ME.Pos + _feelerLength / 1.5f * temp);

            // Create right feeler
            sin = (float) Math.Sin(MathHelper.ToRadians(111));
            cos = (float) Math.Cos(MathHelper.ToRadians(111));

            tx = ME.Velocity.X;
            ty = ME.Velocity.Y;

            temp = new Vector2(cos              * tx - sin * ty, sin * tx + cos * ty);
            _feelers.Add(ME.Pos + _feelerLength / 1.5f * temp);
        }

        public static bool LineIntersection2D(Vector2 A, Vector2 B, Vector2 C, Vector2 D, ref float dist,
                                              ref Vector2 point)
        {
            float rTop = (A.Y - C.Y) * (D.X - C.X) - (A.X - C.X) * (D.Y - C.Y);
            float rBot = (B.X - A.X) * (D.Y - C.Y) - (B.Y - A.Y) * (D.X - C.X);

            float sTop = (A.Y - C.Y) * (B.X - A.X) - (A.X - C.X) * (B.Y - A.Y);
            float sBot = (B.X - A.X) * (D.Y - C.Y) - (B.Y - A.Y) * (D.X - C.X);

            if (rBot == 0 || sBot == 0)
                //lines are parallel
                return false;

            float  r = rTop / rBot;
            double s = sTop / sBot;

            if (r > 0 && r < 1 && s > 0 && s < 1)
            {
                dist  = Vector2.Distance(A, B) * r;
                point = A + r * (B - A);

                return true;
            }

            dist = 0;
            return false;
        }
    }
}