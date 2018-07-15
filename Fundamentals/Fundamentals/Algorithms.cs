using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Fundamentals
{
    [TestClass]
    public class Algorithms
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

        [TestMethod]
        public void TestSorting()
        {
            int size = 5000;

            int[] selection = new int[size], bubble = new int[size];
            Random selectionR = new Random(), bubbleR = new Random();
            int selectionC = 0, bubbleC = 0, bubbleCF;
            for (int i = 0; i < size; i++)
            {
                selection[i] = selectionR.Next(1, size * 4);
                bubble[i] = bubbleR.Next(1, size * 4);
            }

            selectionC = this.SelectionSort(selection);
            bubbleC = this.BubbleSort(bubble);
            bubbleCF = this.BubbleSortWithFlag(bubble);
        }
    }
}
