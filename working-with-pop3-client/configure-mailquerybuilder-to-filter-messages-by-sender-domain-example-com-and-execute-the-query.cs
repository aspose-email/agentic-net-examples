using System;
using Aspose.Email;
using Aspose.Email.Tools.Search;

class Program
{
    static void Main()
    {
        try
        {
            // Create a new MailQueryBuilder instance
            MailQueryBuilder builder = new MailQueryBuilder();

            // Add a condition to filter messages where the sender's address contains the domain "example.com"
            // The second parameter 'true' makes the search case‑insensitive
            builder.From.Contains("example.com", true);

            // Retrieve the built query
            MailQuery query = builder.GetQuery();

            // Output the generated query string
            Console.WriteLine("Generated query: " + query.ToString());
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
