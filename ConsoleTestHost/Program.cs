using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COMP306_FeedbackService;
using System.ServiceModel;

namespace ConsoleTestHost
{
    class Program
    {
        static void Main(string[] args)
        {
            ServiceHost host = new ServiceHost(typeof(FeedbackService));

            host.Open();

            Console.WriteLine("The FeedbackService is up and running.\nPress any key to exit");
            Console.ReadKey(true);

            host.Close();
        }
    }
}
