using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;

namespace Fundamentals
{
    [TestFixture]
    public class TestAlgorithms
    {
        private int SelectionSort(int[] input)
        {
            int count = 0;
            for (int i = 0; i < input.Length - 1; i++)
            {
                int minPos = i;
                for (int j = i + 1; j < input.Length; j++)
                {
                    if (input[j] < input[minPos])
                        minPos = j;
                    count++;
                }

                int min = input[minPos];
                input[minPos] = input[i];
                input[i] = min;
            }
            return count;
        }

        private int BubbleSort(int[] input)
        {
            int count = 0;
            for (int i = 0; i < input.Length - 1; i++)
            {
                for (int j = 0; j < input.Length - i - 1; j++)
                {
                    if (input[j + 1] < input[j])
                    {
                        count++;

                        int min = input[j + 1];
                        input[j + 1] = input[j];
                        input[j] = min;
                    }
                }
            }
            return count;
        }

        private int BubbleSortWithFlag(int[] input)
        {
            int count = 0;

            for (int i = 0; i < input.Length - 1; i++)
            {
                bool needSwap = false;
                for (int j = 0; j < input.Length - i - 1; j++)
                {
                    count++;

                    if (input[j + 1] < input[j])
                    {
                        int min = input[j + 1];
                        input[j + 1] = input[j];
                        input[j] = min;
                    }
                }
                if (!needSwap)
                    break;
            }

            return count;
        }

        private int InsertionSort(int[] input)
        {
            int count = 0;

            for (int i = 1; i < input.Length; i++)
            {
                int value = input[i];

                for (int j = i - 1; j >= 0; j--)
                {
                    count++;

                    if (input[j] > value)
                    {
                        input[j + 1] = input[j];

                        if (j == 0)
                            input[0] = value;

                        continue;
                    }

                    input[j + 1] = value;
                    break;
                }
            }

            return count;
        }

        public int MergeSort(int[] input, int start, int end)
        {
            int count = 0;

            if (start >= end)
                return 1;

            int mid = (start + end) / 2;
            count += MergeSort(input, start, mid);
            count += MergeSort(input, mid + 1, end);

            count += MergeSortedSubArrays(input, start, mid, end);
            return count;
        }

        private int MergeSortedSubArrays(int[] input, int start, int mid, int end)
        {
            int count = 0;

            int[] result = new int[end + 1 - start];
            int i = start, j = mid + 1, k = 0;

            while (i <= mid && j <= end)
            {
                count++;

                if (input[i] < input[j])
                {
                    result[k] = input[i];
                    i++;
                }
                else
                {
                    result[k] = input[j];
                    j++;
                }
                k++;
            }

            for (; i <= mid; i++, k++, count++)
                result[k] = input[i];
            for (; j <= end; j++, k++, count++)
                result[k] = input[j];

            for (i = 0; i < result.Length; i++, count++)
                input[start + i] = result[i];

            return count;
        }

        private int QuickSort(int[] input, int start, int end)
        {
            int count = 0;

            if (start >= end)
                return 1;

            int i = start, temp;
            for (int j = start; j < end; j++)
            {
                count++;
                if (input[j] < input[end])
                {
                    if (i != j)
                    {
                        temp = input[j];
                        input[j] = input[i];
                        input[i] = temp;
                    }
                    if (i != end - 1)
                        i++;
                }
            }

            if (input[i] > input[end])
            {
                temp = input[end];
                input[end] = input[i];
                input[i] = temp;
            }

            count += QuickSort(input, start, i);
            count += QuickSort(input, i + 1, end);

            return count;
        }

        [Test]
        public void TestSorting()
        {
            int size = 5000;

            int[] selection = new int[size], bubble = new int[size], insertion = new int[size], merge = new int[size], quick = new int[size];
            Random random = new Random();
            for (int i = 0; i < size; i++)
            {
                selection[i] = random.Next(1, size * 4);
                bubble[i] = random.Next(1, size * 4);
                insertion[i] = random.Next(1, size * 4);
                merge[i] = random.Next(1, size * 4);
                quick[i] = random.Next(1, size * 4);
            }
            int selectionCount = 0, bubbleCount = 0, bubbleWFCount = 0, insertionCount = 0, mergeCount = 0, quickCount = 0;

            selectionCount = this.SelectionSort(selection);
            bubbleCount = this.BubbleSort(bubble);
            bubbleWFCount = this.BubbleSortWithFlag(bubble);
            insertionCount = this.InsertionSort(insertion);
            mergeCount = this.MergeSort(merge, 0, merge.Length - 1);
            quickCount = this.QuickSort(quick, 0, quick.Length - 1);
        }
    }
}
