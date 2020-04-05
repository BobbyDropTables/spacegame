using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using AAI.Entity;
using AAI.Entity.staticEntities;
using AAI.Pathing;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AAI
{
    public class GameMap
    {
        public int SCREEN_WIDTH { get; }
        public int SCREEN_HEIGHT { get; }

        public int TILE_SIZE { get; }
        public int INDEX_WIDTH { get; }
        public int INDEX_HEIGHT { get; }

        private Graph NavGraph;
        public Queue<Edge> commands;
        public Queue<Edge> astar;


        public GameMap(int width, int height, int tileSize = 40)
        {
            this.SCREEN_WIDTH = width;
            this.SCREEN_HEIGHT = height;
            this.TILE_SIZE = tileSize;

            this.INDEX_WIDTH = SCREEN_WIDTH / TILE_SIZE;
            this.INDEX_HEIGHT = SCREEN_HEIGHT / TILE_SIZE;
                
            this.canRender = true;
            this.commands = new Queue<Edge>();
            this.NavGraph = new Graph(this);

        }

        public void FloodFill(List<BaseGameEntity> statics)
        {
            //1.    Fill navGraph with only vertices

            //2.    Check collisions and change individual vertices that aren't reachable
            List<BaseGameEntity> staticEntities = statics.Where(current => current is Wall).ToList();
            for (int x = 0; x < INDEX_WIDTH; x++)
            {
                for (int y = 0; y < INDEX_HEIGHT; y++)
                {
                    Vertex current = NavGraph.GetVertex(x, y);
                    if (current != null)
                    {
                        foreach (Wall wall in staticEntities)
                        {
                            // Check if entity collides with Vertex
                            if(wall.IsWithin(current.position))
                            {
                                current.canTraverse = false;
                            }
                        }
                    }
                }
            }
            //3.    Add edges
            NavGraph.Fill(staticEntities);
        }

        public Queue<Edge> PathingPipeline(Vector2 entityPosition, Vector2 targetPosition, List<BaseGameEntity> statics)
        {
            //1.    Reset current graph values
            NavGraph.Reset();
            entityPosition = BoundVector(entityPosition);
            targetPosition = BoundVector(targetPosition);
            // Make sure the list of static entities contains only Wall objects
            List<BaseGameEntity> staticEntities = statics.Where(current => current is Wall).ToList();

            //2.    Get closest visible Vertex from source and destination
            var source = NavGraph.ClosestVertexToPosition((int)entityPosition.X, (int)entityPosition.Y);
            var destination = NavGraph.ClosestVertexToPosition((int)targetPosition.X, (int)targetPosition.Y);

            if (source == null || destination == null)
                return null;

            //3.    Run A* and get path of vertices
            var aStar = new AStarSearch();
            var pathOfVertices = new List<Vertex>();
            
            pathOfVertices = aStar.Search(source, destination);
            
            // Return null if no path is found
            if (pathOfVertices == null) 
                return null;
            
            //4.    Add new vertices at exact x,y position of source and destination
            //      Add these to path of vertices
            Vertex entity = new Vertex(-1,-1, entityPosition);
            Vertex target = new Vertex(-1, -1, targetPosition);

            pathOfVertices.Insert(0, entity);
            pathOfVertices.Add(target);

            // TODO: remove
            astar = BuildEdgeQueue(pathOfVertices);

            //5.    Run algorithm through vertices to check if angles are the same
            //      Remove vertices that are angled the same and create new list of edges.
            pathOfVertices = RemoveVerticesWithEqualDirection(pathOfVertices);

            // Return null if no path is found
            if (pathOfVertices == null || pathOfVertices.Count == 0)
                return null;

            //6.    Apply path smoothing 
            pathOfVertices = SmoothPath(pathOfVertices, staticEntities);

            //7.    Return list of edges with fastest possible route.
            var result = BuildEdgeQueue(pathOfVertices);
            return result;

        }

        private Queue<Edge> BuildEdgeQueue(List<Vertex> vertices)
        {
            List<Edge> edges = new List<Edge>();
            Vertex first = null;
            Vertex second = null;

            for (int i = 0; i < vertices.Count; i++)
            {
                if (first == null)
                {
                    first = vertices[i];
                    continue;
                }

                if (second == null)
                {
                    second = vertices[i];

                    Edge temp = new Edge(first, second, 0);
                    temp.color = Color.Red;
                    temp.source.color = Color.Red;
                    temp.destination.color = Color.Red;
                    edges.Add(temp);
                    first = second;
                    second = null;
                }
            }
            Queue<Edge> result = new Queue<Edge>();
            foreach (var edge in edges)
            {
                result.Enqueue(edge);
            }
            //Console.WriteLine("V: " + vertices.Count + ", E: " + result.Count);
            return result;
        }

        /**
         * Smooth a given path of vertices.
         * Check if a new path can be plotted by creating a line.
         * Check if that line intersects with any existing objects.
         */
        private List<Vertex> SmoothPath(List<Vertex> input, List<BaseGameEntity> statics)
        {
            if (input == null || statics == null)
                return null;
            
            List<Vertex> result = new List<Vertex>();

            if (input.Count < 2)
            {
                foreach (var vertex in input)
                {
                    result.Add(vertex);
                }

                return result;
            }
            Stack<Vertex> stack = new Stack<Vertex>();
            input.Reverse();

            // stack.Push(input[0]);
            foreach (var current in input)
            {
                stack.Push(current);
            }

            Vertex A = null;
            Vertex B = null;
            Vertex C = null;
            
            bool ready = false;
            while (!ready)
            {
                // Pop three vertices off stack
                A = stack.Pop();
                B = stack.Pop();

                if(stack.Count != 0)
                    C = stack.Pop();
                else // If third vertex is null add A and B to list
                {
                    result.Add(A);
                    result.Add(B);
                    ready = true;
                }

                var Line = new Tuple<Vector2, Vector2>(A.position, C.position);

                bool intersects = false;
                // Check if line intersects with a static object
                foreach (Wall wall in statics)
                {
                    if(wall.Intersects(Line.Item1, Line.Item2))
                    {
                        intersects = true;
                        
                        break;
                    }
                }

                if (intersects)
                {
                    result.Add(A);
                    stack.Push(C);
                    stack.Push(B);
                }
                else
                {
                    stack.Push(C);
                    stack.Push(A);
                }
            }

            return result;
        }
        /**
         * Algorithm to remove vertices with the same direction.
         * @return A list of vertices
         * @return null if path encounters an error.
         */
        private List<Vertex> RemoveVerticesWithEqualDirection(List<Vertex> path)
        {
            if (path.Count <= 0)
                return null;

            List<Edge> list = new List<Edge>();

            int previousAngle = int.MaxValue;
            Vertex begin = null;
            Vertex previous = null;

            for (int i = 0; i < path.Count; i++)
            {
                var current = path[i];

                if (i == path.Count - 1)
                {
                    list.Add(new Edge(begin, previous, 0));
                    list.Add(new Edge(previous, current, 0));
                    break;
                }

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

            // Convert edges to vertices
            List<Vertex> result = new List<Vertex>();
            for (int i = 0; i < list.Count; i++)
            {
                var current = list[i];
                if (i == 0)
                {
                    result.Add(current.source);
                }
                result.Add(current.destination);
            }
            return result;
        }

        /**
         * Calculate the angle between two vertices
         * @return the angle in radians between 0 and 360
         */
        private double CalculateAngle(Vertex first, Vertex second)
        {
            var deltaX = Math.Pow((second.x - first.x), 2);
            var deltaY = Math.Pow((second.y - first.y), 2);

            var radian = Math.Atan2((second.y - first.y), (second.x - first.x));
            var angle = (radian * (180 / Math.PI) + 360) % 360;

            return angle;
        }

        /**
         * Check whether a vector fits inside the screen size.
         * @return a Vector2 with a bounded position if vector was outside screen size.
         */
        private Vector2 BoundVector(Vector2 position)
        {
            if (position.X < 0)
                position.X = 0;
            else if (position.X >= SCREEN_WIDTH)
                position.X = SCREEN_WIDTH;

            if (position.Y < 0)
                position.Y = 0;
            else if (position.Y >= SCREEN_HEIGHT)
                position.Y = SCREEN_HEIGHT;

            return position;
        }

        /**
         * Check whether a vertex fits inside the screen size.
         * @return a vertex with a bounded position if vector was outside screen size.
         */
        private Vertex BoundVertex(Vertex position)
        {
            int newX = (int)position.position.X;
            int newY = (int)position.position.Y;
            if (position.position.X < 0)
                newX = 0;
            else if (position.position.X >= SCREEN_WIDTH)
                newX = SCREEN_WIDTH;

            if (position.position.Y < 0)
                newY = 0;
            else if (position.position.Y >= SCREEN_HEIGHT)
                newY= SCREEN_HEIGHT;

            position.position = new Vector2(newX, newY);
            return position;
        }

        public static bool LineIntersection2D(Vector2 l1p1, Vector2 l1p2, Vector2 l2p1, Vector2 l2p2)
        {
            float q = (l1p1.Y - l2p1.Y) * (l2p2.X - l2p1.X) - (l1p1.X - l2p1.X) * (l2p2.Y - l2p1.Y);
            float d = (l1p2.X - l1p1.X) * (l2p2.Y - l2p1.Y) - (l1p2.Y - l1p1.Y) * (l2p2.X - l2p1.X);

            if (d == 0)
            {
                return false;
            }

            float r = q / d;

            q = (l1p1.Y - l2p1.Y) * (l1p2.X - l1p1.X) - (l1p1.X - l2p1.X) * (l1p2.Y - l1p1.Y);
            float s = q / d;

            if (r < 0 || r > 1 || s < 0 || s > 1)
            {
                return false;
            }

            return true;
        }
        //public static bool LineIntersection2D(Vector2 A, Vector2 B, Vector2 C, Vector2 D)
        //{
        //    float rTop = (A.Y - C.Y) * (D.X - C.X) - (A.X - C.X) * (D.Y - C.Y);
        //    float sTop = (A.Y - C.Y) * (B.X - A.X) - (A.X - C.X) * (B.Y - A.Y);

        //    float Bot = (B.X - A.X) * (D.Y - C.Y) - (B.Y - A.Y) * (D.X - C.X);

        //    if (Bot == 0) //parallel
        //        return false;

        //    float invBot = 1.0f / Bot;
        //    float r = rTop * invBot;
        //    float s = sTop * invBot;

        //    if (r > 0 && r < 1 && s > 0 && s < 1)
        //        return true; //lines intersect

        //    //lines do not intersect
        //    return false;
        //}

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
                // if (astar != null)
                // {
                //     foreach (var current in astar)
                //     {
                //         current.source.color = Color.Blue;
                //         current.destination.color = Color.Blue;
                //         current.color = Color.Blue;
                //
                //         current.Draw(spriteBatch);
                //         current.source.DrawVertex(spriteBatch);
                //         current.destination.DrawVertex(spriteBatch);
                //
                //     }
                // }

            }
        }
    }
}
