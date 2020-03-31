using System;
using System.Collections.Generic;
using AAI.Entity.MovingEntities;
using AAI.Pathing;
using Microsoft.Xna.Framework;

namespace AAI.behaviour
{
    class PathFollowing : SteeringBehaviour
    {
        // to do
        public PathFollowing(MovingEntity me) : base(me)
        {
        }

        public override Vector2 Calculate()
        {
            Queue<Edge> targetlist = ME.MyWorld.gameMap.commands;
            if (targetlist != null)
                for (int i = 0; i < targetlist.Count; i++)
                {
                    Edge edge = new Edge(new Vertex(0, 0, new Vector2(0, 0)), new Vertex(0, 0, new Vector2(0, 0)), 0.0);
                    if (i == 0)
                        edge = ME.MyWorld.gameMap.commands.Dequeue();

                    Vector2 target = edge.destination.position;

                    Vector2 DesiredVelocity = target - ME.Pos;
                    DesiredVelocity.Normalize();
                    DesiredVelocity *= ME.MaxSpeed;
                    Console.WriteLine(Vector2.Distance(ME.Pos, target));
                    if (Vector2.Distance(ME.Pos, target) <= 2)
                    {
                        edge = ME.MyWorld.gameMap.commands.Dequeue();
                        return DesiredVelocity - ME.Velocity;
                    }
                    return new Vector2(0, 0);
                }

            return new Vector2(0, 0);
        }
    }
}