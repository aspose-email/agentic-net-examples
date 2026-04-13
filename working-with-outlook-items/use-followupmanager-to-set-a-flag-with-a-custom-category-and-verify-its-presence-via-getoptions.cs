using Aspose.Email;
using System;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            // Create a new MAPI message (from, to, subject, body)
            using (MapiMessage message = new MapiMessage("sender@example.com", "recipient@example.com", "Test Subject", "Test body"))
            {
                // Set a follow‑up flag with a request string
                FollowUpManager.SetFlag(message, "Follow up");

                // Add a custom category to the message
                FollowUpManager.AddCategory(message, "CustomCategory");

                // Retrieve the follow‑up options to verify the category
                FollowUpOptions options = FollowUpManager.GetOptions(message);

                if (!string.IsNullOrEmpty(options.Categories) && options.Categories.Contains("CustomCategory"))
                {
                    Console.WriteLine("Custom category successfully added.");
                }
                else
                {
                    Console.WriteLine("Custom category not found.");
                }

                // Output the flag request for reference
                Console.WriteLine("Flag request: " + options.FlagRequest);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
