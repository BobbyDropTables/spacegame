using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AAI.Entity;
using AAI.Pathing;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AAI
{
    public class GameMap
    {
        public int WIDTH { get; set; }
        public int HEIGHT { get; set; }
        public int TILE_SIZE { get; set; }

        private Graph NavGraph;
        public List<Edge> commands;
        
        public GameMap(int width, int height, int tileSize = 40)
        {
            this.WIDTH = width;
            this.HEIGHT = height;
            this.TILE_SIZE = tileSize;

            this.canRender = true;
            this.commands = new List<Edge>();
            this.NavGraph = new Graph(this);

        }

        public void FloodFill(List<BaseGameEntity> statics)
        {
            //1.    Fill navGraph with only vertices

            //2.    Check collisions and change individual vertices that aren't reachable

            //3.    Add edges
            // NavGraph.Fill();

        }

        public List<Edge> PathingPipeline(Vector2 entityPosition, Vector2 targetPosition)
        {
            //1.    Reset current graph values
            NavGraph.Reset();
            entityPosition = BoundVector(entityPosition);
            targetPosition = BoundVector(targetPosition);

            //2.    Get closest visible Vertex from source and destination
            var source = NavGraph.ClosestVertexToPosition((int)entityPosition.X, (int)entityPosition.Y);
            var destination = NavGraph.ClosestVertexToPosition((int)targetPosition.X, (int)targetPosition.Y);

            //3.    Run A* and get path of vertices
            var aStar = new AStarSearch();
            var pathOfVertices = aStar.Search(source, destination);

            // Return null if no path is found
            if (pathOfVertices == null) 
                return null;
            
            //4.    Add new vertices at exact x,y position of source and destination
            //      Add these to path of vertices
            Vertex entity = new Vertex(-1,-1, entityPosition);
            Vertex target = new Vertex(-1, -1, targetPosition);

            pathOfVertices.Insert(0, entity);
            pathOfVertices.Add(target);

            //5.    Run algorithm through vertices to check if angles are the same
            //      Remove vertices that are angled the same and create new list of edges.
            var pathOfEdges = RemoveVerticesWithEqualDirection(pathOfVertices);

            // Return null if no path is found
            if (pathOfEdges == null || pathOfEdges.Count == 0)
                return null;

            //6.    Apply path smoothing 

            //7.    Return list of edges with fastest possible route.
            return pathOfEdges;

        }

        private List<Edge> RemoveVerticesWithEqualDirection(List<Vertex> path)
        {
            if (path.Count <= 0)
                return null;

            List<Edge> list = new List<Edge>();

            int previousAngle = int.MaxValue;
            Vertex begin = null;
            Vertex previous = null;

            for(int i = 0; i < path.Count; i++)
            {
                var current = path[i];

                if (i == path.Count - 1)
                    list.Add(new Edge(previous, current, 0));

                if (current == null)
                    list.Add(new Edge(begin, previous, 0));

                if (begin == null)
                {
                    begin = current;
                    previous = current;
                    continue;
                }
                // If there is no change between angles
                int currentAngle = (int)CalculateAngle(current, previous);
                if (previousAngle == int.MaxValue)
                    previousAngle = currentAngle;

                if (currentAngle == previousAngle)
                {
                    previous = current;
                    continue;
                }

                // If there is a change between angles
                list.Add(new Edge(begin, previous, 0));
                previousAngle = currentAngle;
                begin = previous;
                previous = current;
            }

            foreach (var edge in list)
            {
                edge.color = Color.Blue;
                edge.destination.color = Color.Blue;
                edge.source.color = Color.Blue;

            }
            return list;
        }

        private double CalculateAngle(Vertex first, Vertex second)
        {
            var deltaX = Math.Pow((second.x - first.x), 2);
            var deltaY = Math.Pow((second.y - first.y), 2);

            var radian = Math.Atan2((second.y - first.y), (second.x - first.x));
            var angle = (radian * (180 / Math.PI) + 360) % 360;

            return angle;
        }

        private Vector2 BoundVector(Vector2 position)
        {
            if (position.X < 0)
                position.X = 0;
            else if (position.X >= WIDTH)
                position.X = WIDTH;

            if (position.Y < 0)
                position.Y = 0;
            else if (position.Y >= HEIGHT)
                position.Y = HEIGHT;

            return position;
        }

        public int IndexToCoordinate(int index)
        {
            return (index + 1) * TILE_SIZE - (TILE_SIZE / 2);
        }

        private bool canRender;
        public void Render(SpriteBatch spriteBatch)
        {
            if (canRender)
            {
                NavGraph.Draw(spriteBatch);
                // canRender = false;
                if (commands != null)
                {
                    foreach (var current in commands)
                    {
                        current.Draw(spriteBatch);
                        current.source.DrawVertex(spriteBatch);
                        current.destination.DrawVertex(spriteBatch);

                    }
                }
                
            }
        }
    }
}
