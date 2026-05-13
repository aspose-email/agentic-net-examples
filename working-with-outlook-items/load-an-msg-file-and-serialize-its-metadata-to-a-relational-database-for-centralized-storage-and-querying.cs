using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Path to the MSG file (replace with actual path)
            string msgFilePath = "sample.msg";

            // Verify that the MSG file exists; create a placeholder if it does not
            if (!File.Exists(msgFilePath))
            {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage(
                        "from@example.com",
                        "to@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(msgFilePath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }

                Console.Error.WriteLine($"MSG file not found: {msgFilePath}");
                return;
            }

            // Load the MSG file
            MapiMessage msg;
            try
            {
                msg = MapiMessage.Load(msgFilePath);
            }
            catch (Exception loadEx)
            {
                Console.Error.WriteLine($"Failed to load MSG file: {loadEx.Message}");
                return;
            }

            // Extract metadata
            string subject = msg.Subject ?? string.Empty;
            string senderEmail = msg.SenderEmailAddress ?? string.Empty;
            string senderName = msg.SenderName ?? string.Empty;
            DateTime? deliveryTime = msg.DeliveryTime;
            string body = msg.Body ?? string.Empty;

            // Connection string to the relational database (replace with actual connection string)
            string connectionString = "your_connection_string";

            // Guard against placeholder connection strings
            if (string.IsNullOrWhiteSpace(connectionString) || connectionString.Contains("your_connection_string"))
            {
                Console.Error.WriteLine("Please provide a valid database connection string.");
                // For demonstration purposes, we'll just output the extracted metadata.
                Console.WriteLine("\n--- Extracted Message Metadata ---");
                Console.WriteLine($"Subject      : {subject}");
                Console.WriteLine($"Sender Email : {senderEmail}");
                Console.WriteLine($"Sender Name  : {senderName}");
                Console.WriteLine($"Delivery Time: {(deliveryTime.HasValue ? deliveryTime.Value.ToString() : "N/A")}");
                Console.WriteLine($"Body         : {body}");
                return;
            }

            // NOTE: Actual database interaction code would go here.
            // Since System.Data.SqlClient is not available without adding a NuGet package,
            // the database insertion is omitted to keep the example self‑contained.
            // Replace the following block with proper ADO.NET code using your preferred provider.

            Console.WriteLine("\n--- Extracted Message Metadata (Ready for DB insertion) ---");
            Console.WriteLine($"Subject      : {subject}");
            Console.WriteLine($"Sender Email : {senderEmail}");
            Console.WriteLine($"Sender Name  : {senderName}");
            Console.WriteLine($"Delivery Time: {(deliveryTime.HasValue ? deliveryTime.Value.ToString() : "N/A")}");
            Console.WriteLine($"Body         : {body}");
            Console.WriteLine("\n[Database insertion logic would be executed here.]");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
