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
        private Edge edge =
            new Edge(new Vertex(0, 0, new Vector2(0, 0)), new Vertex(0, 0, new Vector2(0, 0)), 0.0);

        private Queue<Edge> Path;
        private Queue<Edge> OldPath;
        private bool        start;
        private int         switchcase;
        private Vector2     target;
        public bool Finished = false;

        public PathFollowing(SmartEntity me, Queue<Edge> path, float force = 2f) : base(me, force)
        {
            target  = me.Pos;
            Path = path;
        }

        public override Vector2 Calculate()
        {
            if (start)
                if (Path != OldPath)
                    update();

            SeekBehaviour seek = new SeekBehaviour(ME, target);
            if (Vector2.Distance(ME.Pos, target) < 2)
                update();
            return seek.Calculate();
        }

        private void update()
        {
            if (Path != null)
            {
                if (Path == OldPath)
                {
                    if (switchcase >= Path.Count)
                    {
                        target = edge.destination.position;
                        Finished = true;
                    }
                    else
                    {
                        Edge[] targetlist = Path.ToArray();
                        edge   = targetlist[switchcase];
                        target = edge.destination.position;
                        switchcase++;
                    }
                }
                else
                {
                    OldPath    = Path;
                    switchcase = 0;
                    Edge[] targetlist = Path.ToArray();
                    edge   = targetlist[switchcase];
                    target = edge.destination.position;
                    start  = true;
                    Finished = false;
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
            if (Path != null)
            {
                foreach (var current in Path)
                {
                    current.Draw(spriteBatch);
                    current.source.DrawVertex(spriteBatch);
                    current.destination.DrawVertex(spriteBatch);

                }
            }
        }
    }
}