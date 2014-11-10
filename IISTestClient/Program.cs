using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IISTestClient
{
    class Program
    {
        static void Main(string[] args)
        {
            PermissiveCertificatePolicy.Enact("CN=HTTPS-Server");

            try
            {
                IISFeedbackServiceReference.FeedbackServiceClient client = new IISFeedbackServiceReference.FeedbackServiceClient("WS2007HttpBinding_IFeedbackService");

                client.ClientCredentials.Windows.ClientCredential.Domain = "JohnnyWalker";
                client.ClientCredentials.Windows.ClientCredential.UserName = "student";
                client.ClientCredentials.Windows.ClientCredential.Password = "password";
                //client.ClientCredentials.Windows.ClientCredential.UserName = "visitor";
                //client.ClientCredentials.Windows.ClientCredential.Password = "visitor";

                // slove error : Could not establish trust relationship for the SSL/TLS secure channel with authority 'localhost'.

                //method 1:
                //Trust all certificates, this is not a good practice as it completely ignores the server certificate and tells the service point manager that whatever certificate is fine
                //System.Net.ServicePointManager.ServerCertificateValidationCallback +=
                //    (se, cert, chain, sslerror) =>  {return true; };

                // method 2:
                // trust sender
                System.Net.ServicePointManager.ServerCertificateValidationCallback
                    = ((sender, cert, chain, errors) => cert.Subject.Contains("JohnnyWalker")); //Servername

                string user = client.GetCurrentUser();
                Console.WriteLine("You are : {0}", user);

                var r = client.GetAllCourse();
                foreach (var c in r)
                {
                    Console.WriteLine("ID:{0},   Code:{1},   Title:{2}.", c.ID, c.Code, c.Title);
                }

                client.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.ReadLine();
        }
    }
}
