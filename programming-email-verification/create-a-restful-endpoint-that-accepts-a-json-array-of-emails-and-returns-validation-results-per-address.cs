using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Aspose.Email.Tools.Verifications;
using Aspose.Email;

namespace EmailValidationApi
{
    public class ValidationResultDto
    {
        public string Email { get; set; }
        public ValidationResponseCode ReturnCode { get; set; }
        public string Message { get; set; }
    }

    class Program
    {
        static async Task Main(string[] args)
        {
            const string prefix = "http://localhost:5000/";
            using var listener = new HttpListener();
            listener.Prefixes.Add(prefix);

            try
            {
                listener.Start();
                Console.WriteLine($"Listening on {prefix}. Press Enter to stop.");

                var listeningTask = Task.Run(async () =>
                {
                    while (listener.IsListening)
                    {
                        HttpListenerContext context = null;
                        try
                        {
                            context = await listener.GetContextAsync();
                            _ = Task.Run(() => ProcessRequestAsync(context));
                        }
                        catch (HttpListenerException) // Listener stopped
                        {
                            break;
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Listener error: {ex.Message}");
                        }
                    }
                });

                Console.ReadLine(); // Wait for user to stop
                listener.Stop();
                await listeningTask;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unhandled exception: {ex.Message}");
            }
        }

        private static async Task ProcessRequestAsync(HttpListenerContext context)
        {
            try
            {
                if (context.Request.HttpMethod != "POST" ||
                    !context.Request.Url.AbsolutePath.Equals("/validate", StringComparison.OrdinalIgnoreCase))
                {
                    context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    await WriteResponseAsync(context.Response, "Endpoint not found.");
                    return;
                }

                // Read request body
                string requestBody;
                using (var reader = new System.IO.StreamReader(context.Request.InputStream, context.Request.ContentEncoding))
                {
                    requestBody = await reader.ReadToEndAsync();
                }

                // Deserialize JSON array of email strings
                List<string> emails;
                try
                {
                    emails = JsonSerializer.Deserialize<List<string>>(requestBody, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }
                catch (JsonException)
                {
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    await WriteResponseAsync(context.Response, "Invalid JSON payload.");
                    return;
                }

                if (emails == null)
                {
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    await WriteResponseAsync(context.Response, "Invalid JSON payload.");
                    return;
                }

                // Validate emails
                var validator = new EmailValidator();
                var results = new List<ValidationResultDto>();

                foreach (var email in emails)
                {
                    validator.Validate(email, out ValidationResult validationResult);
                    results.Add(new ValidationResultDto
                    {
                        Email = email,
                        ReturnCode = validationResult.ReturnCode,
                        Message = validationResult.Message
                    });
                }

                // Return JSON response
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.OK;
                await JsonSerializer.SerializeAsync(context.Response.OutputStream, results);
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                await WriteResponseAsync(context.Response, $"Error: {ex.Message}");
            }
            finally
            {
                context.Response.OutputStream.Close();
            }
        }

        private static async Task WriteResponseAsync(HttpListenerResponse response, string message)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(message);
            response.ContentType = "text/plain";
            response.ContentLength64 = buffer.Length;
            await response.OutputStream.WriteAsync(buffer, 0, buffer.Length);
        }
    }
}
