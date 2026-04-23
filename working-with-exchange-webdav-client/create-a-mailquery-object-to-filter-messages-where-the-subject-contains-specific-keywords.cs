using Aspose.Email;
using System;
using Aspose.Email.Tools.Search;

class Program
{
    static void Main()
    {
        try
        {
            // Initialize the query builder.
            MailQueryBuilder builder = new MailQueryBuilder();

            // Filter messages where the Subject contains the keyword "Invoice".
            // The second argument (true) makes the search case‑insensitive.
            builder.Subject.Contains("Invoice", true);

            // Build the MailQuery object.
            MailQuery mailQuery = builder.GetQuery();

            // Display the generated query string.
            Console.WriteLine("Generated MailQuery: " + mailQuery.ToString());
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
