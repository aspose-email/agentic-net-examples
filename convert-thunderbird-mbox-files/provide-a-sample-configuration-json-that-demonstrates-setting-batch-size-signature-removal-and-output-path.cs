using System;
using System.Text.Json;
using System.Text.Json.Serialization;

class Program
{
    public class ConversionConfig
    {
        [JsonPropertyName("batchSize")]
        public int BatchSize { get; set; }

        [JsonPropertyName("removeSignature")]
        public bool RemoveSignature { get; set; }

        [JsonPropertyName("outputPath")]
        public string OutputPath { get; set; }
    }

    static void Main()
    {
        var config = new ConversionConfig
        {
            BatchSize = 100,
            RemoveSignature = true,
            OutputPath = "C:\\ConvertedEmails"
        };

        var options = new JsonSerializerOptions { WriteIndented = true };
        string json = JsonSerializer.Serialize(config, options);
        Console.WriteLine("Sample configuration JSON:");
        Console.WriteLine(json);
    }
}
