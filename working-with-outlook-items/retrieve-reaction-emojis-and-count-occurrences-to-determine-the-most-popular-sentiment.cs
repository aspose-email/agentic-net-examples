using System;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Clients.Pop3;

namespace ReactionEmojiCounter
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Placeholder connection settings
                string host = "pop3.example.com";
                int port = 110;
                string username = "username";
                string password = "password";

                // Guard against placeholder credentials to avoid external calls
                if (host.Contains("example.com") || username.Equals("username", StringComparison.OrdinalIgnoreCase) || password.Equals("password", StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine("Skipping POP3 connection due to placeholder credentials.");
                    return;
                }

                // Emoji list to track
                string[] emojis = new string[] { "😀", "😢", "👍", "❤️", "😂" };
                Dictionary<string, int> emojiCounts = new Dictionary<string, int>();
                foreach (string emoji in emojis)
                {
                    emojiCounts[emoji] = 0;
                }

                // Connect to POP3 server and process messages
                try
                {
                    using (Pop3Client client = new Pop3Client(host, port, username, password))
                    {
                        // Validate credentials
                        try
                        {
                            client.ValidateCredentials();
                        }
                        catch (Exception credEx)
                        {
                            Console.Error.WriteLine("Credential validation failed: " + credEx.Message);
                            return;
                        }

                        // Get mailbox info
                        Pop3MailboxInfo mailboxInfo = client.GetMailboxInfo();
                        int messageCount = mailboxInfo.MessageCount;

                        for (int i = 1; i <= messageCount; i++)
                        {
                            try
                            {
                                using (MailMessage message = client.FetchMessage(i))
                                {
                                    string body = message.Body;
                                    if (string.IsNullOrEmpty(body))
                                    {
                                        continue;
                                    }

                                    foreach (string emoji in emojis)
                                    {
                                        int occurrences = 0;
                                        int index = body.IndexOf(emoji, StringComparison.Ordinal);
                                        while (index != -1)
                                        {
                                            occurrences++;
                                            index = body.IndexOf(emoji, index + emoji.Length, StringComparison.Ordinal);
                                        }
                                        if (occurrences > 0)
                                        {
                                            emojiCounts[emoji] += occurrences;
                                        }
                                    }
                                }
                            }
                            catch (Exception fetchEx)
                            {
                                Console.Error.WriteLine($"Failed to fetch message #{i}: {fetchEx.Message}");
                                // Continue with next message
                            }
                        }
                    }
                }
                catch (Exception clientEx)
                {
                    Console.Error.WriteLine("POP3 client error: " + clientEx.Message);
                    return;
                }

                // Determine most popular emoji
                string mostPopularEmoji = null;
                int highestCount = 0;
                foreach (KeyValuePair<string, int> pair in emojiCounts)
                {
                    if (pair.Value > highestCount)
                    {
                        highestCount = pair.Value;
                        mostPopularEmoji = pair.Key;
                    }
                }

                if (mostPopularEmoji == null || highestCount == 0)
                {
                    Console.WriteLine("No reaction emojis were found in the mailbox.");
                }
                else
                {
                    Console.WriteLine($"Most popular reaction emoji: {mostPopularEmoji} (appeared {highestCount} times)");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Unhandled exception: " + ex.Message);
            }
        }
    }
}
