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

            WCFServiceReference.FeedbackServiceClient client = new WCFServiceReference.FeedbackServiceClient("WS2007HttpBinding_IFeedbackService");
            try
            {

                client.ClientCredentials.Windows.ClientCredential.Domain = Environment.MachineName; // "AlexLiu-PC";
                //client.ClientCredentials.Windows.ClientCredential.UserName = "alexliu";
                //client.ClientCredentials.Windows.ClientCredential.Password = "Password";
                client.ClientCredentials.Windows.ClientCredential.UserName = "student";
                client.ClientCredentials.Windows.ClientCredential.Password = "password";
                //client.ClientCredentials.Windows.ClientCredential.UserName = "visitor";
                //client.ClientCredentials.Windows.ClientCredential.Password = "visitor";

                // slove error : Could not establish trust relationship for the SSL/TLS secure channel with authority 'localhost'.

                //method 1:
                //Trust all certificates, this is not a good practice as it completely ignores the server certificate and tells the service point manager that whatever certificate is fine
                System.Net.ServicePointManager.ServerCertificateValidationCallback +=
                    (se, cert, chain, sslerror) =>  {return true; };

                // method 2:
                // trust sender
                //System.Net.ServicePointManager.ServerCertificateValidationCallback
                //    = ((sender, cert, chain, errors) => cert.Subject.Contains("AlexLiu-PC")); //Servername

                // test to get current user
                string user = "";
                try
                {
                    Console.WriteLine("Loading current user...");
                    user = client.GetCurrentUser();
                    Console.WriteLine("You are : {0}\n", user);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: failed to get current user. please add user to group [FeedbackUsers]");
                    Console.WriteLine(ex.Message);
                }


                // test to call get all course
                Console.WriteLine("\nLoadin all courses:");
                var r = client.GetAllCourse();
                foreach (var c in r)
                {
                    Console.WriteLine("ID:{0},   Code:{1},   Title:{2}.", c.ID, c.Code, c.Title);
                }

                // test to call post feedback
                string feedback = "some text";
                int course_id = 1;
                Console.WriteLine(string.Format("\nUser[{0}], adding new feedback:{1} to course id:{2}.", user, feedback, course_id));
                try
                {
                    int result = client.PostFeedbackByCourseID(course_id, feedback, false);
                    if (result > 0)
                        Console.WriteLine("New feedback added.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: add feedback failed.  (*please make sure user exists in TStudent and TStudentCourse)");
                    Console.WriteLine(ex.Message);
                }

                client.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                client.Close();
            }
            Console.Write("\nPress enter to exit.");
            Console.ReadLine();
        }
    }
}
