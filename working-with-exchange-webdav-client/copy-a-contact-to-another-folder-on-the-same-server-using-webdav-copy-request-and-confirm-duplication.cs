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
            // Server connection details (replace with real values)
            string serviceUrl = "https://exchange.example.com/exchange/username";
            string username = "user@example.com";
            string password = "password";

            // Skip external calls when placeholder credentials are used
            if (serviceUrl.Contains("example.com") ||
                username.Contains("example.com") ||
                password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            // Prepare HttpClient with Basic authentication
            using var httpClient = new HttpClient();
            var authToken = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{username}:{password}"));
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authToken);
            httpClient.DefaultRequestHeaders.Add("Accept", "application/xml");

            // -----------------------------------------------------------------
            // Step 1: Create a new contact (placeholder – normally you would POST
            //         an XML/JSON representation of the contact to the Contacts folder)
            // -----------------------------------------------------------------
            // For demonstration, assume the contact was created and we have its URI.
            // In a real scenario you would POST to mailboxInfo.ContactsUri and parse the
            // Location header from the response.
            string contactsFolderUri = $"{serviceUrl}/Contacts";
            string newContactUri = $"{contactsFolderUri}/JohnDoeContact.vcf"; // placeholder URI

            Console.WriteLine($"Assumed contact created at URI: {newContactUri}");

            // -----------------------------------------------------------------
            // Step 2: Copy the contact to the same folder (or another folder) using WebDAV COPY
            // -----------------------------------------------------------------
            // Destination folder – using the same Contacts folder to create a duplicate
            string destinationFolderUri = contactsFolderUri; // could be a different folder URI

            // Build the COPY request
            var copyRequest = new HttpRequestMessage(new HttpMethod("COPY"), newContactUri);
            // The Destination header must contain the full URI where the resource will be copied.
            // Adding a new name to avoid conflict (e.g., appending "_Copy").
            string copiedContactUri = $"{destinationFolderUri}/JohnDoeContact_Copy.vcf";
            copyRequest.Headers.Add("Destination", copiedContactUri);
            // Overwrite header is optional; set to "T" to allow overwriting if needed.
            copyRequest.Headers.Add("Overwrite", "F");

            Console.WriteLine($"Sending COPY request to duplicate contact to: {copiedContactUri}");

            using var copyResponse = await httpClient.SendAsync(copyRequest);
            if (copyResponse.IsSuccessStatusCode)
            {
                Console.WriteLine("Contact duplication confirmed.");
                Console.WriteLine($"Copied contact URI: {copiedContactUri}");
            }
            else
            {
                Console.Error.WriteLine($"Failed to duplicate the contact. Status: {(int)copyResponse.StatusCode} {copyResponse.ReasonPhrase}");
                string responseBody = await copyResponse.Content.ReadAsStringAsync();
                if (!string.IsNullOrWhiteSpace(responseBody))
                {
                    Console.Error.WriteLine("Response body:");
                    Console.Error.WriteLine(responseBody);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
