using System;
using System.Media;
using System.IO;
using System.Threading;
using System.Collections.Generic;
using System.Linq;

namespace cybersecurity_chatbot_csharp
{
    /// <summary>
    /// Main class for the Cybersecurity Awareness Chatbot application.
    /// This console-based chatbot educates users about cybersecurity best practices
    /// through interactive conversations and responds to multiple security-related
    /// topics in a single user query.
    /// </summary>
    class Program
    {
        /// <summary>
        /// Application entry point that orchestrates the chatbot workflow.
        /// Execution sequence:
        /// 1. Plays welcome audio greeting
        /// 2. Displays cybersecurity-themed ASCII art
        /// 3. Collects and validates user's name
        /// 4. Displays personalized welcome message
        /// 5. Enters main chat loop
        /// </summary>
        static void Main(string[] args)
        {
            PlayVoiceGreeting();
            DisplayAsciiArt();
            string userName = GetUserName();
            DisplayWelcomeMessage(userName);
            StartChat(userName);
        }

        /// <summary>
        /// Plays a WAV format audio greeting synchronously.
        /// Handles cases where audio file is missing or playback fails.
        /// Audio file path is constructed relative to the executable location.
        /// </summary>
        static void PlayVoiceGreeting()
        {
            try
            {
                var basePath = AppDomain.CurrentDomain.BaseDirectory;
                string relativePath = Path.Combine("Audio", "welcome.wav");
                string audioPath = Path.GetFullPath(Path.Combine(basePath, relativePath));

                if (File.Exists(audioPath))
                {
                    using (SoundPlayer player = new SoundPlayer(audioPath))
                    {
                        player.Load();
                        player.PlaySync();
                    }
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

        /// <summary>
        /// Displays a cybersecurity-themed ASCII art banner with colored output.
        /// The art serves as a visual header for the chatbot interface.
        /// </summary>
        static void DisplayAsciiArt()
        {
            try
            {
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine(@"
                ██████╗ ██╗   ██╗███████╗██████╗ ███████╗ ██████╗ ██╗   ██╗███████╗
                ██╔══██╗╚██╗ ██╔╝██╔════╝██╔══██╗██╔════╝██╔═══██╗██║   ██║██╔════╝
                ██████╔╝ ╚████╔╝ █████╗  ██████╔╝███████╗██║   ██║██║   ██║█████╗  
                ██╔══██╗  ╚██╔╝  ██╔══╝  ██╔══██╗╚════██║██║▄▄ ██║██║   ██║██╔══╝  
                ██████╔╝   ██║   ███████╗██║  ██║███████║╚██████╔╝╚██████╔╝███████╗
                ╚═════╝    ╚═╝   ╚══════╝╚═╝  ╚═╝╚══════╝ ╚══▀▀═╝  ╚═════╝ ╚══════╝
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

        /// <summary>
        /// Collects and validates user's name through console input.
        /// Validates that name contains only English alphabet characters and spaces.
        /// Implements strict input validation to ensure proper formatting.
        /// Provides default value if input fails validation after multiple attempts.
        /// </summary>
        /// <returns>Validated user name as string</returns>
        static string GetUserName()
        {
            const int maxAttempts = 7;
            int attempts = 0;

            while (attempts < maxAttempts)
            {
                try
                {
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    Console.Write("Enter your name: ");
                    Console.ResetColor();

                    string userName = Console.ReadLine();

                    if (string.IsNullOrWhiteSpace(userName))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Name cannot be empty. Please try again.");
                        Console.ResetColor();
                        attempts++;
                        continue;
                    }

                    if (userName.Any(c => !char.IsLetter(c) && !char.IsWhiteSpace(c)))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Invalid characters detected. Only letters and spaces are allowed.");
                        Console.ResetColor();
                        attempts++;
                        continue;
                    }

                    return userName.Trim();
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Error getting user name: {ex.Message}");
                    Console.ResetColor();
                    attempts++;
                }
            }

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Maximum attempts reached. Using default name 'User'.");
            Console.ResetColor();
            return "User";
        }

        /// <summary>
        /// Displays personalized welcome message with formatted borders.
        /// Incorporates the user's name for personalized greeting.
        /// </summary>
        /// <param name="userName">The name to include in welcome message</param>
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

        /// <summary>
        /// Simulates typing effect by printing text with character-by-character delay.
        /// Creates more natural conversation flow and improves readability.
        /// </summary>
        /// <param name="text">Text to display</param>
        /// <param name="delay">Milliseconds delay between characters (default: 30ms)</param>
        static void TypeText(string text, int delay = 30)
        {
            foreach (char c in text)
            {
                Console.Write(c);
                Thread.Sleep(delay);
            }
            Console.WriteLine();
        }

        /// <summary>
        /// Initializes and maintains the main chat conversation loop.
        /// Handles user input, processes commands, and manages conversation flow.
        /// Implements exit command and help functionality.
        /// </summary>
        /// <param name="userName">User's name for personalized interaction</param>
        static void StartChat(string userName)
        {
            try
            {
                // Comprehensive knowledge base of cybersecurity topics
                var responses = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                {
                    // General conversation
                    { "how are you", "I'm doing great, thanks for asking! Ready to discuss cybersecurity?" },
                    { "purpose", "My purpose is to educate you about cybersecurity best practices and help you stay safe online." },
                    { "help", "I can help with: Passwords, 2FA, phishing, VPNs, public Wi-Fi, HTTPS, and email security. Just ask!" },

                    // Password Security
                    { "password", "Use at least 12 characters (longer is better). Combine uppercase, lowercase, numbers and special characters. Avoid dictionary words or personal information. Use unique passwords for each account. Consider a password manager to generate/store passwords securely." },

                    // Two-Factor Authentication
                    { "2fa", "Adds an extra security layer beyond just passwords. Types include SMS codes, authenticator apps, and hardware tokens. Authenticator apps (Google/Microsoft Authenticator) are more secure than SMS. Backup codes should be stored securely. Required for all financial and sensitive accounts." },
                    { "authentication", "Adds an extra security layer beyond just passwords. Types include SMS codes, authenticator apps, and hardware tokens. Authenticator apps (Google/Microsoft Authenticator) are more secure than SMS. Backup codes should be stored securely. Required for all financial and sensitive accounts." },
                    { "two factor", "Adds an extra security layer beyond just passwords. Types include SMS codes, authenticator apps, and hardware tokens. Authenticator apps (Google/Microsoft Authenticator) are more secure than SMS. Backup codes should be stored securely. Required for all financial and sensitive accounts." },

                    // Phishing Awareness
                    { "phishing", "Never click links or download attachments from unexpected emails. Check sender addresses carefully for slight misspellings. Hover over links to preview the actual URL before clicking. Legitimate organizations won't ask for sensitive info via email. Report suspicious emails to your IT department or email provider." },
                    { "spear phishing", "Spear phishing targets specific individuals with personalized messages. Always verify unusual requests through another channel." },
                    
                    // VPN Usage
                    { "vpn", "Encrypts all internet traffic between your device and the VPN server. Essential when using public Wi-Fi networks. Choose a reputable VPN provider with a no-logs policy. Can help bypass geo-restrictions but choose servers carefully. Doesn't make you completely anonymous - other tracking methods exist." },

                    // Public Wi-Fi Safety
                    { "public wi-fi", "Assume all public Wi-Fi networks are potentially insecure. Never access banking or sensitive accounts without VPN. Disable file sharing and enable firewall. Use cellular data instead when possible for sensitive activities. Forget the network after use to prevent automatic reconnection." },
                    { "wi-fi", "Assume all public Wi-Fi networks are potentially insecure. Never access banking or sensitive accounts without VPN. Disable file sharing and enable firewall. Use cellular data instead when possible for sensitive activities. Forget the network after use to prevent automatic reconnection." },
                    { "wifi", "Assume all public Wi-Fi networks are potentially insecure. Never access banking or sensitive accounts without VPN. Disable file sharing and enable firewall. Use cellular data instead when possible for sensitive activities. Forget the network after use to prevent automatic reconnection." },

                    // HTTPS Security
                    { "https", "Look for padlock icon in browser address bar. Indicates encrypted connection between browser and website. Doesn't guarantee the website itself is legitimate. Important for all login pages and form submissions. Consider HTTPS Everywhere browser extension for forced encryption." },

                    // Email Security
                    { "email", "Enable spam filters at maximum setting. Be wary of urgent requests for information or money transfers. Don't open unexpected attachments (even from known contacts). Verify unusual requests via another communication channel. Regularly review and clean up old emails containing sensitive information." },
                    { "scam", "Enable spam filters at maximum setting. Be wary of urgent requests for information or money transfers. Don't open unexpected attachments (even from known contacts). Verify unusual requests via another communication channel. Regularly review and clean up old emails containing sensitive information." }
                };

                Console.ForegroundColor = ConsoleColor.Cyan;
                TypeText("Type 'help' to see topics I can discuss or 'exit' to quit.", 30);
                Console.ResetColor();

                while (true)
                {
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
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
                        Console.ForegroundColor = ConsoleColor.DarkCyan;
                        Console.Write("ChatBot: ");
                        Console.ResetColor();
                        TypeText("Goodbye! Remember to practice good cybersecurity habits!", 30);
                        break;
                    }

                    if (userInput.Equals("help", StringComparison.OrdinalIgnoreCase))
                    {
                        Console.ForegroundColor = ConsoleColor.DarkCyan;
                        Console.Write("ChatBot: ");
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        TypeText(responses["help"], 30);
                        Console.ResetColor();
                        continue;
                    }

                    ProcessUserInput(userInput, responses);
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Error in chatbot: {ex.Message}");
                Console.ResetColor();
            }
        }

        /// <summary>
        /// Processes user input to identify and respond to all cybersecurity keywords.
        /// Implements ordered response generation based on keyword appearance in input.
        /// Handles cases where no recognized keywords are found.
        /// </summary>
        /// <param name="userInput">Raw user input string</param>
        /// <param name="responses">Knowledge base dictionary</param>
        static void ProcessUserInput(string userInput, Dictionary<string, string> responses)
        {
            // Find all matching keywords in order of appearance
            var matchedKeywords = responses.Keys
                .Where(keyword => userInput.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0)
                .OrderBy(keyword => userInput.IndexOf(keyword, StringComparison.OrdinalIgnoreCase))
                .ToList();

            if (matchedKeywords.Count > 0)
            {
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.Write("ChatBot: ");
                Console.ForegroundColor = ConsoleColor.Magenta;

                bool isFirstResponse = true;
                foreach (string keyword in matchedKeywords)
                {
                    if (!isFirstResponse)
                    {
                        Console.WriteLine();
                    }

                    TypeText($"About {keyword} >> {responses[keyword]}", 30);
                    isFirstResponse = false;
                }

                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.Write("ChatBot: ");
                Console.ForegroundColor = ConsoleColor.Red;
                TypeText("I'm not sure about that topic. Try asking about: passwords, 2FA, phishing, VPNs, or email security.", 30);
                Console.ResetColor();
            }
        }
    }
}