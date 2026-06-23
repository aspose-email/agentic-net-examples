using System;
using System.Collections.Generic;
using System.IO;
using Aspose.Email;
using Aspose.Email.PersonalInfo;

namespace AsposeEmailAzureBlobSample
{
    // Dummy classes to simulate Azure Blob SDK without external dependencies
    class DummyBlobContainerClient
    {
        private readonly string _containerPath;

        public DummyBlobContainerClient(string connectionString, string containerName)
        {
            // Use a local folder to simulate a container
            _containerPath = Path.Combine(Path.GetTempPath(), "BlobContainers", containerName);
        }

        public void CreateIfNotExists()
        {
            if (!Directory.Exists(_containerPath))
                Directory.CreateDirectory(_containerPath);
        }

        public DummyBlobClient GetBlobClient(string blobName) => new DummyBlobClient(Path.Combine(_containerPath, blobName));
    }

    class DummyBlobClient
    {
        private readonly string _blobPath;

        public DummyBlobClient(string blobPath) => _blobPath = blobPath;

        public void Upload(Stream content, DummyBlobUploadOptions options)
        {
            // Ensure the directory exists
            var dir = Path.GetDirectoryName(_blobPath);
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            // Write the stream to a file (simulating blob upload)
            using (var fileStream = new FileStream(_blobPath, FileMode.Create, FileAccess.Write))
            {
                content.CopyTo(fileStream);
            }

            // Simulate setting metadata (write to a sidecar file)
            if (options?.Metadata != null && options.Metadata.Count > 0)
            {
                var metaPath = _blobPath + ".metadata";
                using (var writer = new StreamWriter(metaPath))
                {
                    foreach (var kvp in options.Metadata)
                        writer.WriteLine($"{kvp.Key}={kvp.Value}");
                }
            }

            // Access tier is ignored in this dummy implementation
        }
    }

    class DummyBlobUploadOptions
    {
        public IDictionary<string, string> Metadata { get; set; }
        public string AccessTier { get; set; } // Not used, kept for compatibility
    }

    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Azure Blob storage connection settings (placeholders)
                string connectionString = "YourConnectionString";
                string containerName = "contacts";
                string blobName = "contact.vcf";

                // Guard against placeholder connection string
                if (string.IsNullOrWhiteSpace(connectionString) || connectionString.Contains("YourConnectionString"))
                {
                    Console.Error.WriteLine("Azure Blob connection string is not configured. Skipping upload.");
                    return;
                }

                // Create dummy container client
                DummyBlobContainerClient client = null;
                try
                {
                    client = new DummyBlobContainerClient(connectionString, containerName);
                    client.CreateIfNotExists();
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create or access dummy Blob container: {ex.Message}");
                    return;
                }

                // Prepare a contact
                Contact contact = new Contact
                {
                    DisplayName = "John Doe",
                    CompanyName = "Example Corp",
                    JobTitle = "Software Engineer"
                };
                contact.EmailAddresses.Add(new EmailAddress("john.doe@example.com"));

                // Save contact to a memory stream (no file system access)
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    try
                    {
                        contact.Save(memoryStream);
                        memoryStream.Position = 0;
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to save contact to stream: {ex.Message}");
                        return;
                    }

                    // Prepare Blob client for the specific blob
                    DummyBlobClient blobClient = null;
                    try
                    {
                        blobClient = client.GetBlobClient(blobName);
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to get BlobClient: {ex.Message}");
                        return;
                    }

                    // Set metadata and access tier (simulated)
                    var uploadOptions = new DummyBlobUploadOptions
                    {
                        Metadata = new Dictionary<string, string>
                        {
                            { "ContactId", contact.Id?.ToString() ?? "0" }
                        },
                        AccessTier = "Cool"
                    };

                    // Upload the stream to the dummy Blob storage
                    try
                    {
                        blobClient.Upload(memoryStream, uploadOptions);
                        Console.WriteLine($"Contact uploaded successfully to simulated blob '{blobName}'.");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to upload contact to simulated Blob: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
            }
        }
    }
}
