using System;
using System.Collections.Generic;
using System.Net;

namespace AsposeEmailDnsCacheSample
{
    // Simple DNS MX record cache with a 10‑minute expiration.
    internal static class DnsCache
    {
        // Cache entry holding MX records and their expiration time.
        private sealed class CacheEntry
        {
            public List<string> MxRecords { get; set; }
            public DateTime ExpiryUtc { get; set; }
        }

        private static readonly Dictionary<string, CacheEntry> _cache = new Dictionary<string, CacheEntry>(StringComparer.OrdinalIgnoreCase);
        private static readonly object _lock = new object();

        // Retrieves MX records for the specified domain, using the cache when possible.
        public static List<string> GetMxRecords(string domain)
        {
            if (string.IsNullOrEmpty(domain))
            {
                throw new ArgumentException("Domain name must be provided.", nameof(domain));
            }

            // Try to return a cached value.
            lock (_lock)
            {
                CacheEntry cachedEntry;
                if (_cache.TryGetValue(domain, out cachedEntry) && cachedEntry.ExpiryUtc > DateTime.UtcNow)
                {
                    return cachedEntry.MxRecords;
                }
            }

            // Cache miss – perform a fresh lookup.
            List<string> mxRecords = PerformMxLookup(domain);

            // Store the result in the cache for ten minutes.
            lock (_lock)
            {
                _cache[domain] = new CacheEntry
                {
                    MxRecords = mxRecords,
                    ExpiryUtc = DateTime.UtcNow.AddMinutes(10)
                };
            }

            return mxRecords;
        }

        // Performs the actual MX lookup. Aspose.Email does not expose a direct MX query,
        // so this placeholder uses System.Net.Dns to resolve the domain's A records.
        // Replace with a proper MX query implementation when available.
        private static List<string> PerformMxLookup(string domain)
        {
            List<string> result = new List<string>();

            try
            {
                // Resolve the host to ensure the domain exists.
                // This does not retrieve MX records; it is a placeholder.
                IPHostEntry hostEntry = Dns.GetHostEntry(domain);
                if (hostEntry != null && hostEntry.AddressList != null && hostEntry.AddressList.Length > 0)
                {
                    // As a simple fallback, add the domain itself as a mail server.
                    result.Add(domain);
                }
            }
            catch (Exception ex)
            {
                // In a real implementation, handle DNS errors appropriately.
                Console.Error.WriteLine($"Failed to resolve MX records for '{domain}': {ex.Message}");
            }

            return result;
        }
    }

    internal class Program
    {
        private static void Main(string[] args)
        {
            try
            {
                // Example domains to look up.
                string[] domains = new string[] { "example.com", "contoso.com" };

                foreach (string domain in domains)
                {
                    List<string> mxRecords = DnsCache.GetMxRecords(domain);
                    Console.WriteLine($"MX records for {domain}:");
                    if (mxRecords.Count == 0)
                    {
                        Console.WriteLine("  (none found)");
                    }
                    else
                    {
                        foreach (string mx in mxRecords)
                        {
                            Console.WriteLine($"  {mx}");
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
}
