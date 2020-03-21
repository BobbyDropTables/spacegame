using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using AAI.Pathing;
using AAI.View;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AAI
{
    class Graph
    {
        private Vertex[,] vertices;
        private int WIDTH;
        private int HEIGHT;

        private const int TILE_SIZE = 40;

        public Graph(int width, int height)
        {
            this.WIDTH = width / TILE_SIZE;
            this.HEIGHT = height / TILE_SIZE;

            this.vertices = new Vertex[WIDTH, HEIGHT];

            for (int x = 0; x < WIDTH; x++)
            {
                for (int y = 0; y < HEIGHT; y++)
                {
                    vertices[x, y] = new Vertex(x, y, new Vector2(IndexToCoordinate(x), IndexToCoordinate(y)));
                }
            }

            Fill();
        }

        private Vertex GetVertex(int x, int y)
        {
            if ((x < 0 || x >= WIDTH) || (y < 0 || y >= HEIGHT))
            {
                Console.WriteLine("GetVertex: Out of bounds. [x: " + x + ", y: " + y + "]");
                return null;
            }
            else
            {
                return  vertices[x, y];
            }
        }

        public void Reset()
        {
            foreach (var current in vertices)
            {
                current.Reset();
            }
        }
        public void AddEdge(Vector2 source, Vector2 destination, double cost)
        {
            Vertex v = GetVertex((int)source.X, (int)source.Y);
            Vertex w = GetVertex((int)destination.X, (int)destination.Y);

            if (!(v == null || w == null))
                v.adjacent.Add(new Edge(v, w, cost));
        }

        public Vertex ClosestVertexToPosition(int x, int y)
        {
            int posX = 0;
            int posY = 0;

            if (!(x < 0) || !(x >= WIDTH))
            {
                double temp = (double) x / TILE_SIZE;
                posX = (int)(Math.Ceiling(temp)) - 1;

                if (posX < 0)
                    posX = 0;
                if (posX >= WIDTH)
                    posX = WIDTH - 1;
            }
            if (!(y < 0) || !(y >= HEIGHT))
            {
                double temp = (double)y / TILE_SIZE;
                posY = (int)(Math.Ceiling(temp)) - 1;

                if (posY < 0)
                    posY = 0;
                if (posY >= HEIGHT)
                    posY = HEIGHT - 1;

            }

            return GetVertex(posX, posY);
        }

        private void Fill()
        {
            for (int x = 0; x < WIDTH; x++)
            {
                for (int y = 0; y < HEIGHT; y++)
                {
                    FillPerimeter(vertices[x,y]);
                }
            }
        }
        private void FillPerimeter(Vertex source)
        {
            for (int x = -1; x < 2; x++)
            {
                for (int y = -1; y < 2; y++)
                {
                    if (x == 0 && y == 0)
                        continue;
                    
                    int indexX = source.x + x;
                    int indexY = source.y + y;

                    int temp = Math.Abs(x + y);
                    double distance = 1;
                    if (temp != 1)
                        distance = 1.4;

                    if ((indexX >= 0 && indexX < WIDTH)
                        && (indexY >= 0 && indexY < HEIGHT))
                    {
                        AddEdge(
                            new Vector2(source.x, source.y),
                            new Vector2(indexX, indexY),
                            distance
                        );
                    }
                }
            }
        }
        public int IndexToCoordinate(int index)
        {
            return (index + 1) * TILE_SIZE - (TILE_SIZE / 2);
        }

        public int Heuristics(Vertex source, Vertex destination)
        {
            int x = (int)Math.Abs(destination.x - source.x);
            int y = (int)Math.Abs(destination.y - source.y);
            return x + y;
        }

        public void Draw(SpriteBatch sb)
        {
            for (int x = 0; x < WIDTH; x++)
            {
                for (int y = 0; y < HEIGHT; y++)
                {
                    vertices[x, y].Draw(sb);
                }
            }
        }

        public List<Vertex> A_Star(Vertex source, Vertex destination)
        {
            PriorityQueue frontier = new PriorityQueue();

            List<Vertex> path = new List<Vertex>();
            source.g = 0;
            frontier.Add(source);

            while (frontier.Size() > 0)
            {
                Vertex current = frontier.Remove();

                if (IsDestination(current, destination))
                    return BuildPathFromEnd(source, destination);

                foreach (var neighbor in current.adjacent)
                {
                    var next = neighbor.destination;
                    double newCost = current.g + neighbor.cost;

                    if (newCost < next.g)
                    {
                        next.g = newCost;
                        double heuristics = Heuristics(next, destination);
                        next.h = heuristics;
                        next.previous = current;
                        double priority = newCost + heuristics;
                        next.f = priority;

                        if (next.visited == false)
                        {
                            frontier.Add(next);
                            next.visited = true;
                        }
                    }
                }
            }

            // Return null if no path is found
            return null;
        }

        public static void DrawPath(List<Vertex> path)
        {
            foreach (var current in path)
            {
                current.color = Color.Blue;
            }
        }

        /**
         * Builds a path from end to start Vertex
         * @return List of vertices with path
         */
        private List<Vertex> BuildPathFromEnd(Vertex start, Vertex end)
        {
            List<Vertex> path = new List<Vertex>();

            for (Vertex current = end; current != start; current = current.previous)
            {
                path.Add(current);
            }

            path.Reverse();
            return path;
        }

        private bool IsDestination(Vertex check, Vertex destination)
        {
            if (check.x == destination.x && check.y == destination.y)
                return true;
            else
                return false;
        }

        public static void DrawLine(SpriteBatch spriteBatch, Vector2 begin, Vector2 end, Color color, int width = 1)
        {
            Rectangle r = new Rectangle((int)begin.X, (int)begin.Y, (int)(end - begin).Length() + width, width);
            Vector2 v = Vector2.Normalize(begin - end);
            float angle = (float)Math.Acos(Vector2.Dot(v, -Vector2.UnitX));
            if (begin.Y > end.Y) angle = MathHelper.TwoPi - angle;

            Texture2D Pixel = TextureStorage.Textures["Vertex"];

            spriteBatch.Draw(Pixel, r, null, color, angle, Vector2.Zero, SpriteEffects.None, 0);
        }
    }
}
