using System;
using System.Collections.Generic;
using System.Linq;

namespace GameAI.Pathfinding.PriorityQueue
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
            if (Size == 0) { throw new PriorityQueueEmptyException(); }

            T min = this.array[1];
            T last = this.array[this.Size];

            this.array[1] = last;

            PercolateDown(1);

            this.Size--;

            return min;
        }

        private void Swap(int index1, int index2)
        {
            T temp = this.array[index1];
            this.array[index1] = this.array[index2];
            this.array[index2] = temp;
        }

        public void PercolateUp(int nodeIndex)
        {
            int parentIndex = nodeIndex >> 1;

            //    If node has no parent, do nothing
            if (parentIndex < 1) { return; }

            //    If node is in it's correct place, do nothing
            if (this.array[parentIndex].CompareTo(this.array[nodeIndex]) <= 0) return;

            //    Swap parent & node
            Swap(parentIndex, nodeIndex);
            PercolateUp(parentIndex);
        }

        public void PercolateDown(int nodeIndex)
        {
            T node = this.array[nodeIndex];

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

            if (rightChildIndex > Size) { return; }

            if (leftChildIndex > Size || this.array[leftChildIndex].CompareTo(this.array[rightChildIndex]) > 0) { minChildIndex = rightChildIndex; }

            T minChild = this.array[minChildIndex];

            //    If node is in it's correct place, do nothing
            if (node.CompareTo(minChild) <= 0) { return; }

            //    Swap parent & node
            Swap(nodeIndex, minChildIndex);
            PercolateDown(minChildIndex);
        }

        public override string ToString()
        {
            return string.Join(" ", this.array.Where((comparable, i) => i > 0 && i <= Size));
        }

        public void AddFreely(T x)
        {
            EnsureCapacity(Size + 1);
            this.array[this.Size + 1] = x;

            this.Size++;
        }


        private int GetDepth()
        {
            return (int) Math.Log(Size, 2);
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