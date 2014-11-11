using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTestClient
{
    class Program
    {
        static void Main(string[] args)
        {
            PermissiveCertificatePolicy.Enact("CN=HTTPS-Server");

            try
            {
                ConsoleFeedbackServiceReference.FeedbackServiceClient client = 
                    new ConsoleFeedbackServiceReference.FeedbackServiceClient("BasicHttpBinding_IFeedbackService");

                client.ClientCredentials.Windows.ClientCredential.Domain = "JohnnyWalker";
                client.ClientCredentials.Windows.ClientCredential.UserName = "student";
                client.ClientCredentials.Windows.ClientCredential.Password = "password";
                //client.ClientCredentials.Windows.ClientCredential.UserName = "visitor";
                //client.ClientCredentials.Windows.ClientCredential.Password = "visitor";

                string user = client.GetCurrentUser();
                Console.WriteLine("You are : {0}", user);

                var r = client.GetAllCourse();
                foreach (var c in r)
                {
                    Console.WriteLine("ID:{0},   Code:{1},   Title:{2}.",c.ID,c.Code,c.Title);
                }

                client.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.ReadLine();
        }
    }
}
