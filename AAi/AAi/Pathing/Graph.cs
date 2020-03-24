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
        private GameMap gameMap;

        public Graph(GameMap gamemap)
        {
            this.gameMap = gamemap;
            this.vertices = new Vertex[gameMap.WIDTH, gameMap.HEIGHT];

            for (int x = 0; x < gameMap.WIDTH; x++)
            {
                for (int y = 0; y < gameMap.HEIGHT; y++)
                {
                    vertices[x, y] = new Vertex(x, y, new Vector2(gamemap.IndexToCoordinate(x), gamemap.IndexToCoordinate(y)));
                }
            }

            // Fill();
        }

        public Vertex GetVertex(int x, int y)
        {
            if ((x < 0 || x >= gameMap.WIDTH) || (y < 0 || y >= gameMap.HEIGHT))
            {
                Console.WriteLine("GetVertex: Out of bounds. [x: " + x + ", y: " + y + "]");
                return null;
            }
            else
            {
                Vertex v = vertices[x, y];
                if(v.canTraverse)
                    return  v;
            }

            return null;
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

            if (!(x < 0) || !(x >= gameMap.WIDTH))
            {
                double temp = (double) x / gameMap.TILE_SIZE;
                posX = (int)(Math.Ceiling(temp)) - 1;

                if (posX < 0)
                    posX = 0;
                if (posX >= gameMap.WIDTH)
                    posX = gameMap.WIDTH - 1;
            }
            if (!(y < 0) || !(y >= gameMap.HEIGHT))
            {
                double temp = (double)y / gameMap.TILE_SIZE;
                posY = (int)(Math.Ceiling(temp)) - 1;

                if (posY < 0)
                    posY = 0;
                if (posY >= gameMap.HEIGHT)
                    posY = gameMap.HEIGHT - 1;

            }

            return GetVertex(posX, posY);
        }

        public void Fill()
        {
            for (int x = 0; x < gameMap.WIDTH; x++)
            {
                for (int y = 0; y < gameMap.HEIGHT; y++)
                {
                    Vertex v = GetVertex(x, y);
                    if(v != null)
                        FillPerimeter(v);
                }
            }
        }
        private void FillPerimeter(Vertex source)
        {
            if (source == null)
                return;
            if (!source.canTraverse)
                return;

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

                    if ((indexX >= 0 && indexX < gameMap.WIDTH)
                        && (indexY >= 0 && indexY < gameMap.HEIGHT))
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
        
        // TODO
        public void Draw(SpriteBatch sb)
        {
            for (int x = 0; x < gameMap.WIDTH; x++)
            {
                for (int y = 0; y < gameMap.HEIGHT; y++)
                {
                    Vertex v = GetVertex(x, y);
                    // if(v != null)
                        v.Draw(sb);
                }
            }
        }

        // TODO
        public static void DrawPath(List<Vertex> path)
        {
            foreach (var current in path)
            {
                current.color = Color.Blue;
            }
        }

        // TODO:
        // NOT HERE
        public static void DrawLine(SpriteBatch spriteBatch, Vector2 begin, Vector2 end, Color color, int width = 1)
        {
            Rectangle r = new Rectangle((int)begin.X, (int)begin.Y, (int)(end - begin).Length() + width, width);
            Vector2 v = Vector2.Normalize(begin - end);
            float angle = (float)Math.Acos(Vector2.Dot(v, -Vector2.UnitX));
            if (begin.Y > end.Y) angle = MathHelper.TwoPi - angle;
        
            Texture2D Pixel = TextureStorage.Textures["Pixel"];
        
            spriteBatch.Draw(Pixel, r, null, color, angle, Vector2.Zero, SpriteEffects.None, 0);
        }

    }
}
