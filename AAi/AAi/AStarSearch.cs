using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AAI.Pathing;

namespace AAI
{
    class AStarSearch
    {

        public List<Vertex> Search(Vertex source, Vertex destination)
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

        /**
         * Check if vertex equals the destination
         * @return true if vertices are equal
         * @return false if vertices are not equal
         */
        private bool IsDestination(Vertex check, Vertex destination)
        {
            if (check.x == destination.x && check.y == destination.y)
                return true;
            else
                return false;
        }

        /**
         * Provide a global sense of distance between two vertices
         * Implemented as Manhattan variant.
         */
        public int Heuristics(Vertex source, Vertex destination)
        {
            int x = (int)Math.Abs(destination.x - source.x);
            int y = (int)Math.Abs(destination.y - source.y);
            return x + y;
        }
    }
}
