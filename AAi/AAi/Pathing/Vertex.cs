using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AAI.View;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AAI.Pathing
{
    public class Vertex
    {
        public List<Edge> adjacent { get; }
        public int x { get; set; }
        public int y { get; set; }

        public bool visited { get; set; }
        public Vertex previous;

        public double g { get; set; }
        public double h { get; set; }
        public double f { get; set; }

        public Vector2 position { get; set; }
        public Texture2D Texture;
        public Color color { get; set; }

        public Vertex(int x, int y, Vector2 pos)
        {
            this.x = x;
            this.y = y;

            this.position = pos;
            this.adjacent = new List<Edge>();

            this.g = Double.PositiveInfinity;
            this.h = Double.PositiveInfinity;
            this.f = Double.PositiveInfinity;

            this.previous = null;
            this.visited = false;
            this.color = Color.Yellow;
        }

        public void Reset()
        {
            this.g = Double.PositiveInfinity;
            this.h = Double.PositiveInfinity;
            this.f = Double.PositiveInfinity;

            color = Color.Yellow;
            previous = null;
            visited = false;
        }

        public void Draw(SpriteBatch sb)
        {

            //Draw dot
            Texture = TextureStorage.Textures["Vertex"];
            sb.Draw(Texture,
                new Rectangle((int)position.X - 3, (int)position.Y - 3, (int)7, (int)7),
                null,
                this.color);

            if (this.adjacent.Count > 0)
            {
                foreach (Edge current in adjacent)
                {
                    //Draw Line
                    current.Draw(sb);
                }
            }
        }
    }
}
