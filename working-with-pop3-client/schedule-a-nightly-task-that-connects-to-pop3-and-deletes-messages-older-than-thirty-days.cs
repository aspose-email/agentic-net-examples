using System;
using System.Collections.Generic;
using System.Threading;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Pop3;

namespace Pop3CleanupSample
{
    class Program
    {
        static void Main()
        {
            try
            {
                ScheduleNightlyCleanup();
                Console.WriteLine("POP3 cleanup scheduled. Press any key to exit.");
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unhandled exception: {ex.Message}");
            }
        }

        private static void ScheduleNightlyCleanup()
        {
            // Calculate time until next midnight
            DateTime now = DateTime.Now;
            DateTime nextMidnight = now.Date.AddDays(1);
            TimeSpan dueTime = nextMidnight - now;

            Timer timer = null;
            timer = new Timer(state =>
            {
                try
                {
                    DeleteMessagesOlderThanThirtyDays();
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Scheduled task error: {ex.Message}");
                }

                // Reschedule for the next day
                timer.Change(TimeSpan.FromDays(1), Timeout.InfiniteTimeSpan);
            }, null, dueTime, Timeout.InfiniteTimeSpan);
        }

        private static void DeleteMessagesOlderThanThirtyDays()
        {
            // Placeholder POP3 connection settings
            string host = "pop3.example.com";
            int port = 110;
            string username = "user@example.com";
            string password = "password";

            // Skip real network calls when placeholders are used
            if (host.Contains("example.com"))
            {
                Console.WriteLine("Skipping POP3 operation due to placeholder credentials.");
                return;
            }

            try
            {
                using (Pop3Client client = new Pop3Client(host, port, username, password, SecurityOptions.Auto))
                {
                    // Retrieve the list of messages
                    Pop3MessageInfoCollection messages = client.ListMessages();

                    foreach (Pop3MessageInfo messageInfo in messages)
                    {
                        // Delete messages older than 30 days
                        if (messageInfo.Date < DateTime.Now.AddDays(-30))
                        {
                            client.DeleteMessage(messageInfo.SequenceNumber);
                        }
                    }

                    // Commit deletions to the server
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"POP3 operation failed: {ex.Message}");
            }
        }
    }
}
