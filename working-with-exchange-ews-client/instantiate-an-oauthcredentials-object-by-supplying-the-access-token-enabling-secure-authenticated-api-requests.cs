using Aspose.Email;
using System;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // NOTE: The Aspose.Email version referenced does not contain an OAuthCredentials class.
            // When available, instantiate it with the access token, e.g.:
            // var credentials = new OAuthCredentials("your_access_token");
            // For now, you may use OAuthNetworkCredential as an alternative:
            // var credentials = new Aspose.Email.Clients.OAuthNetworkCredential("your_access_token");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
