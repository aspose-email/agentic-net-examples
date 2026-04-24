using System;
using Aspose.Email;
using Aspose.Email.Tools.Search;

class Program
{
    static void Main()
    {
        try
        {
            // Define the date after which messages should be filtered
            DateTime filterDate = new DateTime(2023, 1, 1);

            // Build the mail query using MailQueryBuilder
            MailQueryBuilder builder = new MailQueryBuilder();

            // Create a query for messages sent on or after the specified date
            MailQuery dateQuery = builder.SentDate.Since(filterDate);

            // Retrieve the query string representation
            string queryString = dateQuery.ToString();

            Console.WriteLine("MailQuery to filter messages sent after {0:d}: {1}", filterDate, queryString);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
