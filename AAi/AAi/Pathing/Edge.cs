using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AAI.Pathing
{
    public class Edge
    {
        public double cost { get; }
        private Vertex source { get; }
        public Vertex destination { get; }
        public Texture2D Texture;
        public Color color { get; set; }

        public Edge(Vertex source, Vertex destination, double cost)
        {
            this.source = source;
            this.destination = destination;
            this.cost = cost;
            this.color = Color.Yellow;
        }
        public void Draw(SpriteBatch sb)
        {
            Graph.DrawLine(
                sb,
                new Vector2(source.position.X, source.position.Y),
                new Vector2(destination.position.X, destination.position.Y),
                this.color
            );
        }
    }
}
