using System;
using System.Collections.Generic;
using AAI.Entity;
using AAI.Entity.MovingEntities;
using AAI.Entity.staticEntities;
using AAI.View;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AAI.behaviour
{
    internal class WallAvoidance : SteeringBehaviour
    {
        private readonly float        _feelerLength;
        private          Vector2      feeler;
        private          Vector2      steeringForce;
        public WallAvoidance(MovingEntity me, float feelerLength) : base(me)
        {
            _feelerLength = feelerLength;
        }

        public override Vector2 Calculate()
        {
            feeler = ME.Pos + _feelerLength * ME.Heading;
            steeringForce = new Vector2(0, 0);
            List<BaseGameEntity> staticEntities = ME.MyWorld.walls;
            // Run through walls
            foreach (Wall w in staticEntities)
                // Check for intersection
                if (w.Intersects(ME.Pos, feeler))
                {
                    //get all intersectslines
                    List<Line> intersectsline = w.getIntersectsline(ME.Pos, feeler);
                    //Is this closest found so far? If so keep record
                    Vector2 closestVector2 = new Vector2(int.MaxValue, int.MaxValue);
                    Line    closestLine    = new Line(new Vector2(), new Vector2());
                    foreach (Line line in intersectsline)
                        if (Vector2.Distance(ME.Pos, line.first) > Vector2.Distance(ME.Pos, line.second))
                        {
                            if (Vector2.Distance(ME.Pos, closestVector2) > Vector2.Distance(ME.Pos, line.first))
                            {
                                closestVector2 = line.first;
                                closestLine    = line;
                            }
                        }
                        else
                        {
                            if (Vector2.Distance(ME.Pos, closestVector2) > Vector2.Distance(ME.Pos, line.second))
                            {
                                closestVector2 = line.second;
                                closestLine    = line;
                            }
                        }

                    // Create a force in the direction of the wall normal, with a magnitude of the overshoot
                    Vector2 temp = Vector2.Normalize(closestLine.first - closestLine.second);
                    Vector2 normal = new Vector2(0, 0)
                    {
                        X = -temp.Y,
                        Y = temp.X
                    };
                    //steeringForce = normal * OverShoot;
                    steeringForce = 3.0f * (-2 * Vector2.Dot(ME.Velocity, normal) * normal + ME.Velocity);
                    return steeringForce;
                }

            return new Vector2(0, 0);
        }

        public override void DebugDraw(SpriteBatch spriteBatch, float scale)
        {
            Vector2 Start = ME.Pos;
            Vector2 End   = feeler;
            //Console.WriteLine("Start:{0}\tEnd:{1}", Start, End);
            Vector2 edge = End - Start;
            // calculate angle to rotate line
            float angle =
                (float) Math.Atan2(edge.Y, edge.X);
            Texture2D Texture = TextureStorage.Textures["Line"];
            Vector2   origin  = new Vector2(0);
            spriteBatch.Draw(Texture,
                             new Rectangle(
                                           (int) Start.X,
                                           (int) Start.Y,
                                           (int) edge.Length(),
                                           1),
                             null,
                             Color.Yellow,
                             angle,
                             new Vector2(0, 0.5f),
                             SpriteEffects.None,
                             0);


            Start = ME.Pos;
            End   = ME.Pos + steeringForce * scale;
            edge = End - Start;
            // calculate angle to rotate line
            angle =
                (float) Math.Atan2(edge.Y, edge.X);
            origin = new Vector2(0);
            spriteBatch.Draw(Texture,
                             new Rectangle(
                                           (int) Start.X,
                                           (int) Start.Y,
                                           (int) edge.Length(),
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

            float invBot = 1.0f / Bot;
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