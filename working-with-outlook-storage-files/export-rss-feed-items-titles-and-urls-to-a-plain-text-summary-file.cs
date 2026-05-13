using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        try
        {
            // RSS feed URL – replace with a real URL when not a placeholder
            string feedUrl = "https://example.com/rss.xml";

            // Guard against placeholder URLs to avoid external calls during CI
            if (feedUrl.Contains("example.com"))
            {
                Console.Error.WriteLine("Feed URL appears to be a placeholder. Skipping download.");
                return;
            }

            // Output file path for the plain‑text summary
            string outputPath = "rss_summary.txt";

            // Ensure the output directory exists
            string outputDirectory = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            // Download the RSS feed content
            using (HttpClient httpClient = new HttpClient())
            {
                Task<string> downloadTask = httpClient.GetStringAsync(feedUrl);
                string rssContent = downloadTask.GetAwaiter().GetResult();

                // Parse the RSS XML
                XDocument rssDocument = XDocument.Parse(rssContent);
                IEnumerable<XElement> itemElements = rssDocument.Descendants("item");

                // Write titles and links to the summary file
                using (StreamWriter writer = new StreamWriter(outputPath, false))
                {
                    foreach (XElement item in itemElements)
                    {
                        XElement titleElement = item.Element("title");
                        XElement linkElement = item.Element("link");

                        string title = titleElement != null ? titleElement.Value.Trim() : "(no title)";
                        string link = linkElement != null ? linkElement.Value.Trim() : "(no link)";

                        writer.WriteLine(title);
                        writer.WriteLine(link);
                        writer.WriteLine(); // blank line between items
                    }
                }
            }

            Console.WriteLine($"RSS summary written to '{outputPath}'.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
