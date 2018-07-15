using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace testingUnitTest
{
    public class ResultClass
    {
        public int max;
        public int left;
        public int right;

        public ResultClass()
        {
            max = 0;
            left = 0;
            right = 0;
        }

        public ResultClass CompareResult(int newLeft, int newRight)
        {
            if (newRight - newLeft + 1 > this.max)
            {
                max = newRight - newLeft + 1;
                left = newLeft;
                right = newRight;
            }
            return this;
        }
    }

    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
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

        public int SelectionSort(int[] input)
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

        public int BubbleSort(int[] input)
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

        public int BubbleSortWithFlag(int[] input)
        {
            int count = 0;

            for (int i = 0; i < input.Length - 1; i++)
            {
                bool needSwap = false;
                for (int j = 0; j < input.Length - i - 1; j++)
                {
                    count++;

                    if (input[j+1] < input [j])
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
    }
}
