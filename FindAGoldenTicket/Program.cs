using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace FindAGoldenTicket
{
    class Program
    {
        private static bool _ticketFound = false;

        static void Main(string[] args)
        {
            //Console.WriteLine("1: Single Threaded");
            //Console.WriteLine("2. Muli-Threaded");
            //Console.Write("Single Threaded or Multi-Threaded? (1 or 2)");
            //var selection = Console.ReadLine();

            Console.WriteLine($"{Environment.NewLine}Creating Ticket");
            var tickets = CreateTicket();

            Console.Write("How many threads:");
            var threadCountInput = Console.ReadLine();

            Console.WriteLine("Single Threaded.");
            var startTimeSingle = DateTime.Now;
            FindTicket(tickets);
            var endTimeSingle = DateTime.Now;

            var startTimeMulti = DateTime.Now;
            DateTime endTimeMulti = DateTime.Now;
            if (int.TryParse(threadCountInput, out var threadCount))
            {
                _ticketFound = false;
                var j = 0;
                var groups = tickets.GroupBy(item => j++ % threadCount).Select(part => part.ToHashSet()).ToHashSet();
                //var groupCount = 0;
                //foreach (var @group in groups)
                //{
                //    Console.WriteLine($"Group: {groupCount}: {@group.Count()}, First: {@group.First().Key}");
                //    groupCount++;
                //}

                //Console.ReadLine();
                var threads = new Dictionary<int, Thread>();
                Console.WriteLine($"{Environment.NewLine}{Environment.NewLine}Starting Multi Thread-----------------------------------------------------------------------------");
                startTimeMulti = DateTime.Now;
                for (var i = 0; i < threadCount; i++)
                {
                    var i1 = i;
                    threads[i] = new Thread(() =>
                    {
                        var result = FindTicket(groups.ElementAt(i1), i1);
                        if (result)
                        {
                            endTimeMulti = DateTime.Now;
                        }
                    });
                    threads[i].Start();
                }

                while (threads.Any(c => c.Value.IsAlive))
                {
                    System.Threading.Thread.Sleep(10);
                }
            }

            Console.WriteLine($"Single Thread: Elapsed Time: {(endTimeSingle - startTimeSingle).TotalSeconds} seconds.");
            Console.WriteLine($"Multi Thread: Elapsed Time: {(endTimeMulti - startTimeMulti).TotalSeconds} seconds.");
            Console.WriteLine("Press any key to exit....");
            Console.ReadLine();

        }

        private static bool FindTicket(IEnumerable<KeyValuePair<string, bool>> tickets, int threadId = 0)
        {
            foreach (var ticket in tickets)
            {
                //System.Threading.Thread.Sleep(50);
                if (ticket.Value)
                {
                    _ticketFound = true;
                    Console.WriteLine($"Thread: {threadId:0#} FOUND The GOLDEN TICKET!!! - Serial Number: {ticket.Key}");
                    return true;
                }

                if (_ticketFound)
                {
                    return false;
                }
                Console.WriteLine($"Thread: {threadId:0#} NOT a ticket - Serial Number: {ticket.Key}");
            }

            return false;
        }

        static Dictionary<string, bool> CreateTicket(int count = 2500)
        {
            var result = new Dictionary<string, bool>();

            var random = new Random();
            var elementId = count-1; //random.Next(0, count - 1);
            for (var i = 0; i < count; i++)
            {
                var serialNumber = Guid.NewGuid().ToString("D");
                if (i == elementId)
                {
                    //Console.WriteLine($"Ticket Serial Number: {serialNumber}");
                   // Console.ReadLine();
                }

                result[serialNumber] = i == elementId;
            }
            return result;
        }
    }
}
