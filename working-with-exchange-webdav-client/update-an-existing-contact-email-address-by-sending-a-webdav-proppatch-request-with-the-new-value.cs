using Aspose.Email.PersonalInfo;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

class Program
{
    static async Task Main()
    {
        try
        {
            // Exchange WebDAV service URL (e.g., https://exchange.example.com/exchange)
            string serviceUrl = "https://exchange.example.com/exchange";
            string username = "user@example.com";
            string password = "password";

            // URI of the contact to be updated.
            // Adjust the path according to your Exchange folder structure.
            string contactUri = "/exchange/user@example.com/contacts/CONTACT_ID.eml";

            // Skip external calls when placeholder credentials are used
            if (serviceUrl.Contains("example.com") ||
                username.Contains("example.com") ||
                password == "password" ||
                contactUri.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            // PROPPATCH XML to update the primary email address.
            string propPatchXml = @"<?xml version=""1.0"" encoding=""utf-8""?>
<D:propertyupdate xmlns:D=""DAV:"" xmlns:Z=""urn:schemas-microsoft-com:"">
  <D:set>
    <D:prop>
      <Z:EmailAddress>new.email@example.com</Z:EmailAddress>
    </D:prop>
  </D:set>
</D:propertyupdate>";

            using var httpClient = new HttpClient();

            // Set Basic Authentication header
            var authToken = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{username}:{password}"));
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authToken);

            // Build the full request URI
            var requestUri = new Uri(new Uri(serviceUrl), contactUri);

            // Create PROPPATCH request
            var request = new HttpRequestMessage(new HttpMethod("PROPPATCH"), requestUri)
            {
                Content = new StringContent(propPatchXml, Encoding.UTF8, "application/xml")
            };

            // Send request
            HttpResponseMessage response = await httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Contact email address updated successfully.");
            }
            else
            {
                Console.Error.WriteLine($"Failed to update contact. Status: {(int)response.StatusCode} {response.ReasonPhrase}");
                string responseBody = await response.Content.ReadAsStringAsync();
                Console.Error.WriteLine("Response body:");
                Console.Error.WriteLine(responseBody);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
