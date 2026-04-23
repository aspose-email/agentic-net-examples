using System;
using Aspose.Email;
using Aspose.Email.Tools.Search;

class Program
{
    static void Main()
    {
        try
        {
            // Create a MailQueryBuilder instance
            MailQueryBuilder builder = new MailQueryBuilder();

            // Add a case‑sensitive filter on the sender's display name
            // The 'From' field contains the sender information; set ignoreCase to false for case‑sensitivity
            builder.From.Contains("John Doe", false);

            // Build the MailQuery object
            MailQuery query = builder.GetQuery();

            // Output the generated query string
            Console.WriteLine("Generated MailQuery:");
            Console.WriteLine(query.ToString());
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
