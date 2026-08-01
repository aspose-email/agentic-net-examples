using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace GraphSubscriptionExample
{
    class Program
    {
        static async Task Main(string[] args)
        {
            try
            {
                string accessToken = await GetAccessTokenAsync();
                if (string.IsNullOrEmpty(accessToken))
                {
                    Console.WriteLine("Failed to acquire access token.");
                    return;
                }

                var subscriptionId = await CreateSubscriptionAsync(accessToken);
                if (!string.IsNullOrEmpty(subscriptionId))
                {
                    Console.WriteLine($"Subscription created successfully. Id: {subscriptionId}");
                }
                else
                {
                    Console.WriteLine("Subscription creation failed.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        // Placeholder: Acquire an OAuth2 access token for Microsoft Graph.
        private static async Task<string> GetAccessTokenAsync()
        {
            // TODO: Replace with actual token acquisition logic (e.g., client credentials flow).
            // Return a placeholder token for compilation.
            await Task.CompletedTask;
            return "YOUR_ACCESS_TOKEN";
        }

        private static async Task<string> CreateSubscriptionAsync(string accessToken)
        {
            using var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var subscription = new
            {
                changeType = "created,updated",
                notificationUrl = "https://yourapp.example.com/api/notifications", // Must be HTTPS and reachable
                resource = "communications/messages", // Adjust to the actual product update resource
                expirationDateTime = DateTime.UtcNow.AddMinutes(30).ToString("o"),
                clientState = "secretClientValue"
            };

            var content = new StringContent(JsonSerializer.Serialize(subscription), Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync("https://graph.microsoft.com/v1.0/subscriptions", content);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Failed to create subscription. Status: {response.StatusCode}");
                var error = await response.Content.ReadAsStringAsync();
                Console.WriteLine(error);
                return null;
            }

            var responseBody = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(responseBody);
            if (doc.RootElement.TryGetProperty("id", out var idProp))
            {
                return idProp.GetString();
            }

            return null;
        }
    }
}
