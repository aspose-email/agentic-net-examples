using System;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Clients.Google;

namespace AsposeEmailExample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Initialize Gmail client (replace placeholders with real values)
                IGmailClient gmailClient = GmailClient.GetInstance(
                    "YOUR_ACCESS_TOKEN",
                    "user@example.com");

                // Retrieve color information
                ColorsInfo colorsInfo = gmailClient.GetColors();

                // Display last update time
                Console.WriteLine("Colors last updated: " + colorsInfo.Updated);

                // Process calendar colors
                Console.WriteLine("Calendar Colors:");
                Dictionary<string, Colors> calendarColors = colorsInfo.Calendar;
                foreach (KeyValuePair<string, Colors> kvp in calendarColors)
                {
                    Console.WriteLine($"  ID: {kvp.Key}, Color: {kvp.Value}");
                }

                // Process event colors
                Console.WriteLine("Event Colors:");
                Dictionary<string, Colors> eventColors = colorsInfo.Event;
                foreach (KeyValuePair<string, Colors> kvp in eventColors)
                {
                    Console.WriteLine($"  ID: {kvp.Key}, Color: {kvp.Value}");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Error: " + ex.Message);
            }
        }
    }
}