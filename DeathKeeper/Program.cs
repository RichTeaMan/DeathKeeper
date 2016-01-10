using Cache;
using CommandLineParser;
using DeathKeeper.Wdq;
using DeathKeeper.WikiData;
using DeathKeeper.WikiData.Human;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DeathKeeper
{
    class Program
    {
        const string WdqResponse = "WdqResponse.json";

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
        }


        [ClCommand("FillCache")]
        public static void FillCache()
        {
            var errors = new ConcurrentBag<Tuple<int, Exception>>();
            var responseBody = File.ReadAllText(WdqResponse);
            var wdqRequestor = new WdqRequestor();

            Console.WriteLine("Getting human instance references.");
            var wdqResult = wdqRequestor.ResultFromString(responseBody);
            Console.WriteLine("Found {0} human instances.", wdqResult.items.Length);

            var wikiDataRequestor = WikiDataRequestor.Create();

            int count = 0;

            var partitions = new List<IEnumerable<int>>();


            Parallel.ForEach(wdqResult.items, new ParallelOptions() { MaxDegreeOfParallelism = 20 }, id =>
            {
                try
                {
                    var humanUrl = wikiDataRequestor.GetEntityUrl(id);
                    wikiDataRequestor.WebCache.FillCache(humanUrl);
                    var c = Interlocked.Increment(ref count);
                    if (c % 100 == 0)
                    {
                        Console.Write("\r{0}/{1} completed.", c, wdqResult.items.Count());
                    }
                }
                catch (Exception ex)
                {
                    var t = new Tuple<int, Exception>(id, ex);
                    errors.Add(t);
                }
            });
            Console.WriteLine();
            Console.WriteLine("{0} humans found.", count);
            Console.WriteLine("Cache fill finished.");
        }

        [ClCommand("GetAllPersons")]
        public static void GetAllPersons()
        {
            var humans = new List<Human>();
            var errors = new List<Tuple<int, Exception>>();
            var responseBody = File.ReadAllText(WdqResponse);
            var wdqRequestor = new WdqRequestor();
            
            Console.WriteLine("Getting human instance references.");
            var wdqResult = wdqRequestor.ResultFromString(responseBody);
            Console.WriteLine("Found {0} human instances.", wdqResult.items.Length);

            var humanFactory = new HumanFactory();
            foreach (var id in wdqResult.items)
            {
                try
                {
                    if (humans.Count % 10 == 0)
                    {
                        Console.Write("\r{0}/{1} completed.", humans.Count, wdqResult.items.Count());
                    }
                    var human = humanFactory.FromEntityId(id);
                    humans.Add(human);

                }
                catch(Exception ex)
                {
                    var t = new Tuple<int, Exception>(id, ex);
                    errors.Add(t);
                }
            }
            Console.WriteLine();
            Console.WriteLine("{0} humans found.", humans.Count);
            Console.WriteLine("Writing reports.");

            var livingSb = new StringBuilder();
            livingSb.AppendFormat("Name\tDoB\tAge\tLink");
            livingSb.AppendLine();
            var living = humans.Where(h => h.DateOfBirth != null && h.DateOfDeath == null).OrderByDescending(h => h.Age());
            foreach(var l in living)
            {
                livingSb.AppendFormat("{0}\t{1}\t{2}\t{3}", l.Label, l.DateOfBirth, l.Age(), l.WikiLink);
                livingSb.AppendLine();
            }
            File.WriteAllText("Living.txt", livingSb.ToString());
            Console.WriteLine("Reports finished.");
        }
    }
}
