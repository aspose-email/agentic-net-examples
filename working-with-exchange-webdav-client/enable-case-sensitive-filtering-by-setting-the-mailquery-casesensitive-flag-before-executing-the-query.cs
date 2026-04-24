using System;
using Aspose.Email;
using Aspose.Email.Tools.Search;

class Program
{
    static void Main()
    {
        try
        {
            // Build a case‑sensitive query using the Contains method with ignoreCase = false
            MailQueryBuilder builder = new MailQueryBuilder();
            builder.From.Contains("John.Doe@example.com", false);
            MailQuery query = builder.GetQuery();

            Console.WriteLine("Generated query: " + query.ToString());
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
