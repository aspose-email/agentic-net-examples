using Aspose.Email;
using Aspose.Email.PersonalInfo;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // SharePoint REST endpoint (replace with actual URL)
            string sharepointSiteUrl = "https://sharepoint.example.com/sites/YourSite";
            string listTitle = "Contacts";
            string requestUrl = $"{sharepointSiteUrl}/_api/web/lists/getbytitle('{listTitle}')/items";

            // Guard against placeholder endpoint to avoid real network calls during CI
            if (sharepointSiteUrl.Contains("sharepoint.example.com"))
            {
                Console.WriteLine("Placeholder SharePoint URL detected. Skipping REST call.");
                return;
            }

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Accept", "application/json;odata=verbose");

                HttpResponseMessage response;
                try
                {
                    response = client.GetAsync(requestUrl).Result;
                }
                catch (HttpRequestException httpEx)
                {
                    Console.Error.WriteLine($"HTTP request failed: {httpEx.Message}");
                    return;
                }

                if (!response.IsSuccessStatusCode)
                {
                    Console.Error.WriteLine($"SharePoint request failed with status code: {response.StatusCode}");
                    return;
                }

                string jsonResponse = response.Content.ReadAsStringAsync().Result;

                using (JsonDocument document = JsonDocument.Parse(jsonResponse))
                {
                    JsonElement root = document.RootElement;

                    if (root.TryGetProperty("d", out JsonElement dElement) &&
                        dElement.TryGetProperty("results", out JsonElement resultsElement) &&
                        resultsElement.ValueKind == JsonValueKind.Array)
                    {
                        List<Contact> contacts = new List<Contact>();

                        foreach (JsonElement item in resultsElement.EnumerateArray())
                        {
                            Contact contact = new Contact();

                            // Title -> DisplayName
                            if (item.TryGetProperty("Title", out JsonElement titleElement) &&
                                titleElement.ValueKind == JsonValueKind.String)
                            {
                                contact.DisplayName = titleElement.GetString();
                            }

                            // Email -> EmailAddresses
                            if (item.TryGetProperty("Email", out JsonElement emailElement) &&
                                emailElement.ValueKind == JsonValueKind.String)
                            {
                                string email = emailElement.GetString();
                                if (!string.IsNullOrEmpty(email))
                                {
                                    contact.EmailAddresses.Add(new EmailAddress(email));
                                }
                            }

                            // Phone -> PhoneNumbers
                            if (item.TryGetProperty("Phone", out JsonElement phoneElement) &&
                                phoneElement.ValueKind == JsonValueKind.String)
                            {
                                string phone = phoneElement.GetString();
                                if (!string.IsNullOrEmpty(phone))
                                {
                                    contact.PhoneNumbers.Add(new PhoneNumber
                                    {
                                        Number = phone,
                                        Category = PhoneNumberCategory.Company
                                    });
                                }
                            }

                            contacts.Add(contact);
                        }

                        // Output loaded contacts
                        foreach (Contact loadedContact in contacts)
                        {
                            Console.WriteLine($"Name: {loadedContact.DisplayName}");
                            foreach (EmailAddress emailAddr in loadedContact.EmailAddresses)
                            {
                                Console.WriteLine($"  Email: {emailAddr.Address}");
                            }
                            foreach (PhoneNumber phoneNum in loadedContact.PhoneNumbers)
                            {
                                Console.WriteLine($"  Phone ({phoneNum.Category}): {phoneNum.Number}");
                            }
                        }
                    }
                    else
                    {
                        Console.Error.WriteLine("Unexpected JSON structure received from SharePoint.");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
