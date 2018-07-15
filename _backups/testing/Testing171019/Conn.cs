using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Testing171019
{
    public class Conn
    {
        private static readonly Conn instance = new Conn();

        static Conn() { }

        private Conn() { }

        public static Conn Instance
        {
            get
            {
                return instance;
            }
        }

        public void PrintNumber(String input = "", int waitTime = 5000)
        {
            Console.WriteLine(String.Format("Entering [PrintNumber] for {0}", input));
            Task.Delay(waitTime);
            Console.WriteLine(String.Format("Existing [PrintNumber] for {0}", input));
        }
    }
}
