using CommandLineParser;
using DeathKeeper.WikiData.Human;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeathKeeper
{
    class Program
    {
        static void Main(string[] args)
        {
            MethodInvoker command = null;
            try
            {
                command = ClCommandAttribute.GetCommand(typeof(Program), args);

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error parsing command:");
                Console.WriteLine(ex);
            }
            if (command != null)
            {
                try
                {
                    command.Invoke();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error running command:");
                    Console.WriteLine(ex);

                    var inner = ex.InnerException;
                    while (inner != null)
                    {
                        Console.WriteLine(inner);
                        Console.WriteLine();
                        inner = inner.InnerException;
                    }

                    Console.WriteLine(ex.StackTrace);
                }
            }
        }

        [ClCommand("GetPerson")]
        public static void GetPersons(
            [ClArgs("id")]
            string[] ids
            )
        {
            var humanFactory = new HumanFactory();
            foreach (var id in ids)
            {
                int intId;
                if (int.TryParse(id, out intId))
                {
                    var human = humanFactory.FromEntityId(intId);
                    Console.WriteLine("{0}:", intId);
                    Console.WriteLine("{0}", human);
                }
                else
                {
                    Console.WriteLine("{0} is not an integer.", id);
                }
            }
            Console.ReadKey();
        }
    }
}
