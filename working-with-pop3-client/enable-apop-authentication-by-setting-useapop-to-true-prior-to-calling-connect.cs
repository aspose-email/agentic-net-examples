using Aspose.Email;
using System;
using Aspose.Email.Clients.Pop3;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder connection parameters
            string host = "pop3.example.com";
            int port = 110;
            string username = "user@example.com";
            string password = "password";

            // Skip real network calls when placeholders are used
            if (host.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder host detected. Skipping POP3 connection.");
                return;
            }

            // Create POP3 client and set connection properties
            using (Pop3Client client = new Pop3Client())
            {
                client.Host = host;
                client.Port = port;
                client.Username = username;
                client.Password = password;

                // Enable APOP authentication if the property exists
                var useApopProp = typeof(Pop3Client).GetProperty("UseApop");
                if (useApopProp != null && useApopProp.CanWrite)
                {
                    useApopProp.SetValue(client, true);
                }
                else
                {
                    // Fallback: try setting an AuthenticationMode enum to APOP
                    var authModeProp = typeof(Pop3Client).GetProperty("AuthenticationMode");
                    if (authModeProp != null && authModeProp.CanWrite)
                    {
                        var enumType = authModeProp.PropertyType;
                        var apopValue = Enum.Parse(enumType, "Apop", true);
                        authModeProp.SetValue(client, apopValue);
                    }
                }

                // Connect/authenticate using the available method
                var connectMethod = typeof(Pop3Client).GetMethod("Connect", Type.EmptyTypes);
                if (connectMethod != null)
                {
                    connectMethod.Invoke(client, null);
                }
                else
                {
                    var authMethod = typeof(Pop3Client).GetMethod("Authenticate", Type.EmptyTypes);
                    if (authMethod != null)
                    {
                        authMethod.Invoke(client, null);
                    }
                }

                // Example operation: retrieve and display message count
                int messageCount = client.GetMessageCount();
                Console.WriteLine($"Message count: {messageCount}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
