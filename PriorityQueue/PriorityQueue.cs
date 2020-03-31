using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace PriorityQueue
{
    public class PriorityQueue<T> : IPriorityQueue<T>
        where T : IComparable<T>
    {
        private const int DEFAULT_CAPACITY = 100;
        private T[] array; // The internal heap array

        public int Size { get; private set; }

        public PriorityQueue() : this(DEFAULT_CAPACITY) { }

        public PriorityQueue(int capacity)
        {
            this.array = new T[capacity];
        }

        public void Clear() => this.Size = 0;

        private void EnsureCapacity(int capacity)
        {
            if (this.array.Length > capacity) { return; }

            int newCapacity = this.array.Length << 1 >= 0 ? this.array.Length << 1 : int.MaxValue;

            Array.Resize(ref this.array, newCapacity);
        }

        public void Add(T x)
        {
            AddFreely(x);

            PercolateUp(this.Size);
        }

        public void AddRange(IEnumerable<T> items)
        {
            foreach (T item in items) { AddFreely(item); }

            BuildHeap();
        }

        // Removes the smallest item in the priority queue
        public T Remove()
        {
            if (this.Size == 0) { throw new PriorityQueueEmptyException(); }

            T min = this.array[1];
            T last = this.array[this.Size];

            this.array[1] = last;

            PercolateDown(1);

            this.Size--;

            return min;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void Swap(IList<T> array, int index1, int index2)
        {
            T temp = array[index1];
            array[index1] = array[index2];
            array[index2] = temp;
        }

        public void PercolateUp(int nodeIndex)
        {
            while (true)
            {
                int parentIndex = nodeIndex >> 1;

                //    If node has no parent, do nothing
                if (parentIndex < 1) { return; }

                //    If node is in it's correct place, do nothing
                if (this.array[parentIndex].CompareTo(this.array[nodeIndex]) <= 0) return;

                //    Swap parent & node
                Swap(this.array, parentIndex, nodeIndex);
                nodeIndex = parentIndex;
            }
        }

        public void PercolateDown(int nodeIndex)
        {
            T node;
            
            while (true)
            {
                node = this.array[nodeIndex];

                int rightChildIndex = nodeIndex << 1;
                int leftChildIndex = rightChildIndex + 1;

                //    Determine smallest child;
                //    If right is out of bounds
                //        stop
                //    If left is out of bounds or left is bigger than right
                //        It's right
                //    Else
                //        It's left
                int minChildIndex = leftChildIndex;

                if (rightChildIndex > this.Size) { return; }

                if (leftChildIndex > this.Size || this.array[leftChildIndex].CompareTo(this.array[rightChildIndex]) > 0)
                {
                    minChildIndex = rightChildIndex;
                }
                
                //    If node is in it's correct place, do nothing
                if (node.CompareTo(this.array[minChildIndex]) <= 0) { return; }

                //    Swap parent & node
                Swap(this.array, nodeIndex, minChildIndex);
                nodeIndex = minChildIndex;
            }
        }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            int layerSize = 2;

            for (int i = 1; i < this.array.Length; i++)
            {
                if (i == layerSize)
                {
                    layerSize <<= 1;
                    stringBuilder.Append("\n");
                }

                stringBuilder.Append(this.array[i] + " ");
            }

            return stringBuilder.ToString(); //string.Join(" ", this.array.Where((comparable, i) => i > 0 && i <= this.Size));
        }

        public void AddFreely(T x)
        {
            EnsureCapacity(this.Size + 1);
            this.array[this.Size + 1] = x;

            this.Size++;
        }


        private int GetDepth()
        {
            return (int) Math.Log(this.Size, 2);
        }

        public void BuildHeap()
        {
            int currentLevel = GetDepth();

            while (currentLevel >= 0)
            {
                int minIndex = currentLevel > 0 ? 2 << (currentLevel - 1) : 1;
                int levelSize = minIndex;

                IEnumerable<int> levelIndexes = Enumerable.Range(minIndex, levelSize);

                foreach (int levelIndex in levelIndexes) { PercolateDown(levelIndex); }

                currentLevel--;
            }
        }
    }
}