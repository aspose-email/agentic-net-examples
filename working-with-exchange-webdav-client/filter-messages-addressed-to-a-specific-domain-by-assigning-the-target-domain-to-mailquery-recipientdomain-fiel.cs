using Aspose.Email;
using System;
using Aspose.Email.Tools.Search;

class Program
{
    static void Main()
    {
        try
        {
            // Define the target domain to filter recipients.
            string targetDomain = "example.com";

            // Build a mail query that matches messages where the TO field contains the target domain.
            MailQueryBuilder builder = new MailQueryBuilder();
            builder.To.Contains(targetDomain);

            // Retrieve the constructed query.
            MailQuery query = builder.GetQuery();

            // Output the generated query string.
            Console.WriteLine("Generated MailQuery: " + query);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
