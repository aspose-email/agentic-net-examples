using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            string msgFilePath = "message.msg";

            // Ensure the MSG file exists before attempting to load it
            if (!File.Exists(msgFilePath))
            {
                Console.Error.WriteLine($"The file '{msgFilePath}' does not exist.");
                return;
            }

            // Load the MSG file using the correct API
            using (MapiMessage message = MapiMessage.Load(msgFilePath))
            {
                // Retrieve the categories assigned to the message
                string[] categories = message.Categories;

                if (categories != null && categories.Length > 0)
                {
                    Console.WriteLine("Message Categories:");
                    foreach (string category in categories)
                    {
                        Console.WriteLine($"- {category}");
                    }
                }
                else
                {
                    Console.WriteLine("No categories are assigned to this message.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
