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

            // Add a filter for messages received today (SentDate equals today)
            MailQuery query = builder.SentDate.On(DateTime.Today);

            // Output the generated query (its string representation)
            Console.WriteLine("Generated MailQuery for today's messages:");
            Console.WriteLine(query.ToString());
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
