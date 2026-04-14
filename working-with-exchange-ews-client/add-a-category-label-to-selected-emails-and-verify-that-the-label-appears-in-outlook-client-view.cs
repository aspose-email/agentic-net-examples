using System.Collections.Generic;
using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;
using Aspose.Email.Calendar;

class Program
{
    static void Main()
    {
        try
        {
            // Define the path to the MSG file.
            string msgFilePath = "sample.msg";

            // Ensure the file exists; if not, create a minimal placeholder message.
            if (!File.Exists(msgFilePath))
            {
                try
                {
                    // Create a simple MapiMessage and save it as a placeholder.
                    MapiMessage placeholderMessage = new MapiMessage("sender@example.com", "recipient@example.com", "Sample Subject", "Sample body.");
                    placeholderMessage.Save(msgFilePath);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine("Failed to create placeholder MSG file: " + ex.Message);
                    return;
                }
            }

            // Load the existing message.
            MapiMessage message;
            try
            {
                message = MapiMessage.Load(msgFilePath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Failed to load MSG file: " + ex.Message);
                return;
            }

            // Add a category to the message.
            try
            {
                FollowUpManager.AddCategory(message, "MyCategory");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Failed to add category: " + ex.Message);
                return;
            }

            // Save the updated message back to the file.
            try
            {
                message.Save(msgFilePath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Failed to save MSG file: " + ex.Message);
                return;
            }

            // Verify that the category was added by reading the categories list.
            try
            {
                MapiMessage verifyMessage = MapiMessage.Load(msgFilePath);
                System.Collections.Generic.IList<string> categories = FollowUpManager.GetCategories(verifyMessage);
                Console.WriteLine("Categories on the message:");
                foreach (string category in categories)
                {
                    Console.WriteLine("- " + category);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Failed to verify categories: " + ex.Message);
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Unexpected error: " + ex.Message);
        }
    }
}
