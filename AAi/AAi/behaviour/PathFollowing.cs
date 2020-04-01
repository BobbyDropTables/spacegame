using System;
using System.Collections.Generic;
using AAI.Entity.MovingEntities;
using AAI.Pathing;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AAI.behaviour
{
    internal class PathFollowing : SteeringBehaviour
    {
        private readonly Vehicle vehicle;

        private Edge edge =
            new Edge(new Vertex(0, 0, new Vector2(0, 0)), new Vertex(0, 0, new Vector2(0, 0)), 0.0);

        private Queue<Edge> OldPath;
        private bool        start;
        private int         switchcase;
        private Vector2     target;
        public bool finished = false;

        public PathFollowing(Vehicle me) : base(me)
        {
            vehicle = me;
            target  = me.Pos;
        }

        public override Vector2 Calculate()
        {
            if (start)
                if (vehicle.Path != OldPath)
                    update();

            SeekBehaviour seek = new SeekBehaviour(ME, target);
            if (Vector2.Distance(ME.Pos, target) < 2)
                update();
            return seek.Calculate();
        }

        private void update()
        {
            if (vehicle.Path != null)
            {
                if (vehicle.Path == OldPath)
                {
                    if (switchcase >= vehicle.Path.Count)
                    {
                        target = edge.destination.position;
                        finished = true;
                    }
                    else
                    {
                        Edge[] targetlist = vehicle.Path.ToArray();
                        edge   = targetlist[switchcase];
                        target = edge.destination.position;
                        switchcase++;
                    }
                }
                else
                {
                    OldPath    = vehicle.Path;
                    switchcase = 0;
                    Edge[] targetlist = vehicle.Path.ToArray();
                    edge   = targetlist[switchcase];
                    target = edge.destination.position;
                    start  = true;
                    finished = false;
                }
            }
            else
            {
                edge = new Edge(new Vertex((int) ME.Pos.X / 40, (int) ME.Pos.Y / 40, ME.Pos),
                                new Vertex((int) ME.Pos.X / 40, (int) ME.Pos.Y / 40, ME.Pos), 0);
                target = ME.Pos;
            }
        }

        public override void DebugDraw(SpriteBatch spriteBatch, float scale)
        {
            throw new NotImplementedException();
        }
    }
}