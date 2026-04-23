using System;
using Aspose.Email;
using Aspose.Email.Tools.Search;

class Program
{
    static void Main()
    {
        try
        {
            // Initialize the query builder
            MailQueryBuilder builder = new MailQueryBuilder();

            // Add a case‑sensitive condition on the Subject field.
            // The second parameter 'false' specifies case‑sensitive matching.
            builder.Subject.Contains("Invoice", false);

            // Retrieve the constructed query
            MailQuery query = builder.GetQuery();

            // Display the generated query string
            Console.WriteLine("Generated query: " + query.ToString());
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
