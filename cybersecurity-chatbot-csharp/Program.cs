using System;
using System.Media;
using System.IO;
using System.Threading;
using System.Collections.Generic;

namespace cybersecurity_chatbot_csharp
{
    class Program
    {
        static void Main(string[] args)
        {
            // Task 1: Play voice greeting
            PlayVoiceGreeting();

            // Task 2: Display ASCII art
            DisplayAsciiArt();

            // Task 3: Get user's name and welcome them
            string userName = GetUserName();
            DisplayWelcomeMessage(userName);

            // Task 4: Start the chatbot conversation with expanded knowledge base
            StartChat(userName);
        }

        static void PlayVoiceGreeting()
        {
            try
            {
                var basePath = AppDomain.CurrentDomain.BaseDirectory;
                string relativePath = Path.Combine("Audio", "welcome.wav");
                string audioPath = Path.GetFullPath(Path.Combine(basePath, relativePath));

                if (File.Exists(audioPath))
                {
                    SoundPlayer player = new SoundPlayer(audioPath);
                    player.Load();
                    player.PlaySync();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Audio file not found at: {audioPath}");
                    Console.ResetColor();
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Error playing voice greeting: {ex.Message}");
                Console.ResetColor();
            }
        }

        static void DisplayAsciiArt()
        {
            try
            {
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine(@"
+-------------------------------------------------------------------------------------+
|                                                                                     |
|    ____ __      __ ______    _____ ______    _____   _____  ____ __    __ ______    |
|   / ___)) \    / ((_   _ \  / ___/(   __ \  / ____\ / ___/ / ___)) )  ( ((   __ \   |
|  / /     \ \  / /   ) (_) )( (__   ) (__) )( (___  ( (__  / /   ( (    ) )) (__) )  |
| ( (       \ \/ /    \   _/  ) __) (    __/  \___ \  ) __)( (     ) )  ( ((    __/   |
| ( (        \  /     /  _ \ ( (     ) \ \  _     ) )( (   ( (    ( (    ) )) \ \  _  |
|  \ \___     )(     _) (_) ) \ \___( ( \ \_))___/ /  \ \___\ \___ ) \__/ (( ( \ \_)) |
|   \____)   /__\   (______/   \____\)_) \__//____/    \____\\____)\______/ )_) \__/  |
|                                                                                     |
|   _____  ________ __      __                                                        |
|  (_   _)(___  ___)) \    / (                                                        |
|    | |      ) )    \ \  / /                                                         |
|    | |     ( (      \ \/ /                                                          |
|    | |      ) )      \  /                                                           |
|   _| |__   ( (        )(                                                            |
|  /_____(   /__\      /__\                                                           |
|                                                                                     |
|    ____  __    __   ____  ________  ______    ____  ________                        |
|   / ___)(  \  /  ) (    )(___  ___)(_   _ \  / __ \(___  ___)                       |
|  / /     \ (__) /  / /\ \    ) )     ) (_) )/ /  \ \   ) )                          |
| ( (       ) __ (  ( (__) )  ( (      \   _/( ()  () ) ( (                           |
| ( (      ( (  ) )  )    (    ) )     /  _ \( ()  () )  ) )                          |
|  \ \___   ) )( (  /  /\  \  ( (     _) (_) )\ \__/ /  ( (                           |
|   \____) /_/  \_\/__(  )__\ /__\   (______/  \____/   /__\                          |
|                                                                                     |
+-------------------------------------------------------------------------------------+
                ");
                Console.ResetColor();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Error displaying ASCII art: {ex.Message}");
                Console.ResetColor();
            }
        }

        static string GetUserName()
        {
            try
            {
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.Write("Enter your name: ");
                Console.ResetColor();

                string userName = Console.ReadLine();

                while (string.IsNullOrWhiteSpace(userName))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Name cannot be empty. Please enter your name:");
                    Console.ResetColor();
                    userName = Console.ReadLine();
                }

                return userName;
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Error getting user name: {ex.Message}");
                Console.ResetColor();
                return "User";
            }
        }

        static void DisplayWelcomeMessage(string userName)
        {
            try
            {
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.WriteLine("=======================================================================");
                Console.WriteLine($"Hello, {userName}! Welcome to the Cybersecurity Awareness Bot.");
                Console.WriteLine("I'm here to help you stay safe online.");
                Console.WriteLine("=======================================================================");
                Console.ResetColor();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Error displaying welcome message: {ex.Message}");
                Console.ResetColor();
            }
        }

        static void TypeText(string text, int delay = 30)
        {
            foreach (char c in text)
            {
                Console.Write(c);
                Thread.Sleep(delay);
            }
            Console.WriteLine();
        }

        static void StartChat(string userName)
        {
            try
            {
                // Expanded cybersecurity knowledge base
                Dictionary<string, string> responses = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                {
                    // General questions
                    { "how are you", "I'm doing great, thanks for asking! Ready to discuss cybersecurity?" },
                    { "purpose", "My purpose is to educate you about cybersecurity best practices and help you stay safe online." },
                    { "help", "I can help with: Passwords, 2FA, phishing, VPNs, public Wi-Fi, HTTPS, and email security. Just ask!" },

                    // Password Security
                    { "password",
                        @"- Use at least 12 characters (longer is better).
                        - Combine uppercase, lowercase, numbers and special characters.
                        - Avoid dictionary words or personal information.
                        - Use unique passwords for each account.
                        - Consider a password manager to generate/store passwords securely."
                    },

                    // Two-Factor Authentication
                    { "2fa",
                        @"- Adds an extra security layer beyond just passwords.
                        - Types include SMS codes, authenticator apps, and hardware tokens.
                        - Authenticator apps (Google/Microsoft Authenticator) are more secure than SMS.
                        - Backup codes should be stored securely.
                        - Required for all financial and sensitive accounts."
                    },
                    { "authentication",
                        @"- Adds an extra security layer beyond just passwords.
                        - Types include SMS codes, authenticator apps, and hardware tokens.
                        - Authenticator apps (Google/Microsoft Authenticator) are more secure than SMS.
                        - Backup codes should be stored securely.
                        - Required for all financial and sensitive accounts."
                    },
                    { "two factor",
                        @"- Adds an extra security layer beyond just passwords.
                        - Types include SMS codes, authenticator apps, and hardware tokens.
                        - Authenticator apps (Google/Microsoft Authenticator) are more secure than SMS.
                        - Backup codes should be stored securely.
                        - Required for all financial and sensitive accounts."
                    },

                    // Phishing Awareness
                    { "phishing",
                        @"- Never click links or download attachments from unexpected emails.
                        - Check sender addresses carefully for slight misspellings.
                        - Hover over links to preview the actual URL before clicking.
                        - Legitimate organizations won't ask for sensitive info via email.
                        - Report suspicious emails to your IT department or email provider."
                    },
                    { "spear phishing", "Spear phishing targets specific individuals with personalized messages. Always verify unusual requests through another channel." },
                    
                    // VPN Usage
                    { "vpn",
                        @"- Encrypts all internet traffic between your device and the VPN server.
                        - Essential when using public Wi-Fi networks.
                        - Choose a reputable VPN provider with a no-logs policy.
                        - Can help bypass geo-restrictions but choose servers carefully.
                        - Doesn't make you completely anonymous - other tracking methods exist."
                    },

                    // Public Wi-Fi Safety
                    { "public wi-fi",
                        @"- Assume all public Wi-Fi networks are potentially insecure.
                        - Never access banking or sensitive accounts without VPN.
                        - Disable file sharing and enable firewall.
                        - Use cellular data instead when possible for sensitive activities.
                        - Forget the network after use to prevent automatic reconnection."
                    },
                    { "wifi",
                        @"- Assume all public Wi-Fi networks are potentially insecure.
                        - Never access banking or sensitive accounts without VPN.
                        - Disable file sharing and enable firewall.
                        - Use cellular data instead when possible for sensitive activities.
                        - Forget the network after use to prevent automatic reconnection."
                    },

                    // HTTPS Security
                    { "https",
                        @"- Look for padlock icon in browser address bar.
                        - Indicates encrypted connection between browser and website.
                        - Doesn't guarantee the website itself is legitimate.
                        - Important for all login pages and form submissions.
                        - Consider HTTPS Everywhere browser extension for forced encryption."
                    },

                    // Email Security
                    { "email",
                        @"- Enable spam filters at maximum setting.
                        - Be wary of urgent requests for information or money transfers.
                        - Don't open unexpected attachments (even from known contacts).
                        - Verify unusual requests via another communication channel.
                        - Regularly review and clean up old emails containing sensitive information."
                    },
                };

                Console.ForegroundColor = ConsoleColor.Cyan;
                TypeText("Type 'help' to see topics I can discuss or 'exit' to quit.", 30);
                Console.ResetColor();

                while (true)
                {
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    Console.Write($"{userName}: ");
                    Console.ResetColor();

                    string userInput = Console.ReadLine()?.Trim();

                    if (string.IsNullOrWhiteSpace(userInput))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        TypeText("ChatBot: I didn't get that. Could you please rephrase?", 30);
                        Console.ResetColor();
                        continue;
                    }

                    if (userInput.Equals("exit", StringComparison.OrdinalIgnoreCase))
                    {
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        TypeText("ChatBot: Goodbye! Remember to practice good cybersecurity habits!", 30);
                        Console.ResetColor();
                        break;
                    }

                    bool responseFound = false;
                    foreach (var keyword in responses.Keys)
                    {
                        if (userInput.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            Console.ForegroundColor = ConsoleColor.DarkCyan;
                            Console.Write("ChatBot: ");
                            Console.ForegroundColor = ConsoleColor.Magenta;
                            TypeText(responses[keyword], 30);
                            Console.ResetColor();
                            responseFound = true;
                            break;
                        }
                    }

                    if (!responseFound)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkCyan;
                        Console.Write("ChatBot: ");
                        Console.ForegroundColor = ConsoleColor.Red;
                        TypeText("I'm not sure about that topic. Try asking about: passwords, 2FA, phishing, VPNs, or email security.", 30);
                        Console.ResetColor();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Error in chatbot: {ex.Message}");
                Console.ResetColor();
            }
        }
    }
}