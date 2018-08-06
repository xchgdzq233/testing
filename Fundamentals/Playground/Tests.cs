using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace Playground
{
    [TestFixture]
    public class Tests
    {
        IApp app;
        Platform platform;

        public Tests(Platform platform)
        {
            this.platform = platform;
        }

        //[SetUp]
        //public void BeforeEachTest()
        //{
        //    app = AppInitializer.StartApp(platform);
        //}

        [Test]
        public void AppLaunches()
        {
            List<List<int>> input = new List<List<int>>() { new List<int>() { 1, 2, 3 } };
            int i = 0;
            Assert.IsTrue(((i == (input.Count - 1) / 2) || (i == (input[0].Count - 1) / 2)) && (i % 2 == 1));
        }

        public List<int> spiralOrder(List<List<int>> input)
        {
            List<int> result = new List<int>();

            for (int i = 0; i <= input.Count / 2 && i <= input[0].Count / 2; i++)
            {
                for (int j = i; j < input[0].Count - 1 - i; j++)
                    result.Add(input[i][j]);
                for (int j = i; j < input.Count - 1 - i; j++)
                    result.Add(input[j][input[0].Count - 1 - i]);
                for (int j = input[0].Count - 1 - i; j > 0; j--)
                    result.Add(input[input.Count - 1 - i][j]);
                for (int j = input.Count - 1 - i; j > 0; j--)
                    result.Add(input[j][input.Count - 1 - i]);
            }

            return result;
        }
    }
}
