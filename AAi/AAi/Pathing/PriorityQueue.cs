using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AAI.Pathing
{
    /**
     * Priority queue implemented as a binary heap.
     * Fixed sized array used as container.
     * If the array is full, double its size.
     */
    class PriorityQueue
    {
        private int CurrentSize;
        private Vertex[] Heap;

        public PriorityQueue()
        {
            this.CurrentSize = 0;
            this.Heap = new Vertex[1];
        }

        public PriorityQueue(Vertex[] collection)
        {
            CurrentSize = collection.Length;
            Heap = new Vertex[(CurrentSize + 2) * 11 / 10];

            int i = 1;

            foreach (var current in collection)
            {
                Heap[i++] = current;
            }

            BuildHeap();
        }

        public int Size()
        {
            return this.CurrentSize;
        }

        public void Clear()
        {
            this.CurrentSize = 0;
        }

        private void DoubleArray()
        {
            int newSize = Heap.Length * 2;

            Vertex[] tmp = new Vertex[newSize];

            for (int i = 0; i < CurrentSize + 1; i++)
            {
                tmp[i] = Heap[i];
            }

            this.Heap = tmp;

            // Console.WriteLine("Current Heap Array size: " + Heap.Length);
        }
        /**
         * Returns the smallest item in the priority queue
         * @return the smallest item
         * @throws
         */
        public Vertex Element()
        {
            return Heap[1];
        }

        /**
         * Adds an item to the priority queue
         * @param a Vertex
         * @return true
         */
        public bool Add(Vertex v)
        {
            if (CurrentSize + 1 == Heap.Length)
                DoubleArray();

            // Percolate up
            int hole = ++CurrentSize;
            Heap[0] = v;

            for (; v.f < Heap[hole / 2].f; hole /= 2)
            {
                Heap[hole] = Heap[hole / 2];
            }

            Heap[hole] = v;

            return true;
        }

        /**
         * Removes the smallest item in the priority queue
         * @return the smallest item
         */
        public Vertex Remove()
        {
            Vertex minItem = Element();
            Heap[1] = Heap[CurrentSize--];
            PercolateDown(1);

            return minItem;
        }

        /**
         * Percolate down in the heap.
         * @param hole the index at which the percolate begins.
         */
        private void PercolateDown(int hole)
        {
            int child;
            Vertex tmp = Heap[hole];

            for (; hole * 2 <= CurrentSize; hole = child)
            {
                child = hole * 2;
                if (child != CurrentSize && Heap[child + 1].f < Heap[child].f)
                    child++;
                if (Heap[child].f < tmp.f)
                    Heap[hole] = Heap[child];
                else
                    break;
            }
            Heap[hole] = tmp;
        }

        /**
         * Establish heap order property from an arbitrary arrangement
         * of items.
         */
        private void BuildHeap()
        {
            for (int i = CurrentSize / 2; i > 0; i--)
            {
                PercolateDown(i);
            }
        }

        public void PrintAll()
        {
            for (int i = 1; i < CurrentSize; i++)
            {
                Console.WriteLine("Value: " + Heap[i].f + " at index " + i + ".");
            }
        }
    }
}
