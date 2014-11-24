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

                ConsoleFeedbackServiceReference.FeedbackServiceClient client =
                    new ConsoleFeedbackServiceReference.FeedbackServiceClient("BasicHttpBinding_IFeedbackService");
                try
                {

                    System.Net.ServicePointManager.ServerCertificateValidationCallback +=
                      (se, cert, chain, sslerror) => { return true; };
                    client.ClientCredentials.Windows.ClientCredential.Domain = Environment.MachineName; // "JohnnyWalker";
                    client.ClientCredentials.Windows.ClientCredential.UserName = "student";
                    client.ClientCredentials.Windows.ClientCredential.Password = "password";

                    // test to get current user
                    string user="";
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
                    Console.WriteLine(string.Format("\nUser[{0}], adding new feedback:{1} to course id:{2}.", user,feedback, course_id));
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

                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
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
