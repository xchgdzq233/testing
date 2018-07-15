using FFX_P8_Image.Client.FFXImageServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FFX_P8_Image.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            List<string> result = new List<string>();
            //FFXImageServiceClient proxy = new FFXImageServiceClient("NetTcpBinding_IFFXImageService");
            ImageProxy proxy = new ImageProxy("NetTcpBinding_IFFXImageService");
            proxy.ClientCredentials.Windows.ClientCredential.UserName = "BUILTIN\\Administrators";
            try
            {
                result = proxy.GetTicketsList("aticket").ToList();
            }
            catch(Exception ex)
            {
                Console.WriteLine("Failed to load server data. " + ex.Message);
            }
            finally
            {
                proxy.Close();
            }

            foreach (string s in result)
                Console.WriteLine(s);
            Console.ReadKey();
        }
    }
}
