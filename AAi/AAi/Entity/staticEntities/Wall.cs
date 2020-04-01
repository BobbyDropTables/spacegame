using System;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using AAI.View;
using AAI.world;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AAI.Entity.staticEntities
{
    struct Line
    {
        public Vector2 first { get; set; }
        public Vector2 second { get; set; }


        public Line(Vector2 first, Vector2 second)
        {
            this.first = first;
            this.second = second;
        }
    }

    class Wall : StaticEntity
    {
        public Line[] lines { get; }

        private int _width;
        private Color _color;

        public Wall(Vector2 first, Vector2 second, Vector2 third, Vector2 fourth, World w,
            Color color) : base(first, w)
        {
            lines = new Line[4];

            lines[0] = new Line(first, second);
            lines[1] = new Line(second, third);
            lines[2] = new Line(third, fourth);
            lines[3] = new Line(fourth, first);

        }

        public bool Intersects(Vector2 first, Vector2 second)
        {
            foreach (var line in lines)
            {
                if (GameMap.LineIntersection2D(line.first, line.second, first, second))
                {
                    Console.WriteLine("LINE INTERSECTS WITH OBJECT AT: " + line.first + ", " + line.second);
                    Console.WriteLine("FROM ENTITY: " + first + ", " + second);
                    
                    return true;
                }
            }

            return false;
        }

        public bool IsWithin(Vector2 pos)
        {
            int x1 = (int)lines[0].first.X;
            int x2 = (int)lines[0].second.X;
            int y1 = (int) lines[3].second.Y;
            int y2 = (int)lines[3].first.Y;

            if ((pos.X >= x1 && pos.X <= x2) && (pos.Y >= y1 && pos.Y <= y2))
                return true;

            return false;
        }

        public override void Render(SpriteBatch spriteBatch)
        {
            foreach (var line in lines)
            {
                Graph.DrawLine(spriteBatch, line.first, line.second, this._color);
            }
        }
    }
}
