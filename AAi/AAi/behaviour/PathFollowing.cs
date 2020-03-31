using System;
using System.Collections.Generic;
using AAI.Entity.MovingEntities;
using AAI.Pathing;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AAI.behaviour
{
    class PathFollowing : SteeringBehaviour
    {

        Edge edge = new Edge(new Vertex(0, 0, new Vector2(0, 0)), new Vertex(0, 0, new Vector2(0, 0)), 0.0);
        public PathFollowing(MovingEntity me) : base(me)
        {
        }

        public override Vector2 Calculate()
        {
            Queue<Edge> targetlist = ME.MyWorld.gameMap.PathingPipeline(ME.Pos, ME.MyWorld.Target.Pos, ME.MyWorld.walls);
            Console.WriteLine(targetlist);
            if (targetlist != null)
                for (int i = 0; i < targetlist.Count; i++)
                {
                    
                    if (i == 0)
                        edge = targetlist.Dequeue();

                    Vector2 target = edge.destination.position;

                    Vector2 DesiredVelocity = target - ME.Pos;
                    DesiredVelocity.Normalize();
                    DesiredVelocity *= ME.MaxSpeed;
                    Console.WriteLine(Vector2.Distance(ME.Pos, target));
                    if (Vector2.Distance(ME.Pos, target) <= 2)
                    {
                        edge = ME.MyWorld.gameMap.commands.Dequeue();
                        return new Vector2(0, 0);
                    }
                    return DesiredVelocity - ME.Velocity;
                }

            return new Vector2(0, 0);
        }

        public override void DebugDraw(SpriteBatch spriteBatch, float scale)
        {
            throw new NotImplementedException();
        }
    }
}