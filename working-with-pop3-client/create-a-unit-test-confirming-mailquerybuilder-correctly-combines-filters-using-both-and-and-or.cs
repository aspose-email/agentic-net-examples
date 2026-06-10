using Aspose.Email;
using System;
using System.Diagnostics;
using Aspose.Email.Tools.Search;

class Program
{
    static void Main()
    {
        try
        {
            // Build first part of the query (implicit AND of two conditions)
            MailQueryBuilder builder = new MailQueryBuilder();
            builder.From.Contains("alice@example.com", true);
            builder.Subject.Contains("report", true);
            MailQuery andQuery = builder.GetQuery();

            // Build second part of the query (single condition)
            MailQueryBuilder otherBuilder = new MailQueryBuilder();
            otherBuilder.To.Contains("bob@example.com", true);
            MailQuery toQuery = otherBuilder.GetQuery();

            // Combine the two parts using OR
            MailQuery combinedQuery = builder.Or(andQuery, toQuery);

            // Simple verification: the combined query string should contain both '&' (AND) and '|' (OR)
            string queryString = combinedQuery.ToString();

            bool containsAnd = queryString.Contains("&");
            bool containsOr = queryString.Contains("|");

            Debug.Assert(containsAnd, "Combined query should contain an AND operator.");
            Debug.Assert(containsOr, "Combined query should contain an OR operator.");

            if (containsAnd && containsOr)
            {
                Console.WriteLine("MailQueryBuilder correctly combines AND and OR filters.");
                Console.WriteLine("Resulting query: " + queryString);
            }
            else
            {
                Console.Error.WriteLine("MailQueryBuilder failed to combine filters as expected.");
                Console.Error.WriteLine("Resulting query: " + queryString);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
