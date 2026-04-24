using System;
using Aspose.Email;
using Aspose.Email.Tools.Search;

class Program
{
    static void Main()
    {
        try
        {
            // Phrase to search for in the message body
            string phrase = "Important";

            // Initialize the query builder
            MailQueryBuilder builder = new MailQueryBuilder();

            // Set the body contains criteria (case‑insensitive)
            builder.Body.Contains(phrase, true);

            // Build the MailQuery object
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
