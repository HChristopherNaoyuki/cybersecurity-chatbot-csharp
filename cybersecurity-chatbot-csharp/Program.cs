using System;
using System.Media;       // For audio playback functionality
using System.IO;         // For file system operations
using System.Threading;  // For timing/delays in the typing effect
using System.Collections.Generic;  // For Dictionary data structure
using System.Linq;       // For LINQ operations
using System.Drawing;    // For image processing (Bitmap class)
using System.Text;       // For StringBuilder class

namespace cybersecurity_chatbot_csharp
{
    /// <summary>
    /// Main class for the Cybersecurity Awareness Chatbot application.
    /// This comprehensive application combines multiple features:
    /// 1. Audio greeting playback
    /// 2. Dynamic ASCII art generation from images
    /// 3. Interactive cybersecurity education chatbot
    /// 4. Multi-topic response system with natural language processing
    /// </summary>
    class Program
    {
        /// <summary>
        /// ASCII character gradient from darkest to lightest.
        /// These characters are used to represent different brightness levels
        /// when converting images to ASCII art. The characters are ordered
        /// from most dense (darkest) to least dense (lightest).
        /// </summary>
        private static readonly char[] asciiChars = { '#', '8', '&', 'o', ':', '*', '.', ' ' };

        /// <summary>
        /// Main entry point for the application.
        /// Orchestrates the complete workflow:
        /// 1. Plays welcome audio greeting
        /// 2. Displays cybersecurity-themed ASCII art
        /// 3. Collects and validates user's name
        /// 4. Displays personalized welcome message
        /// 5. Enters main interactive chat loop
        /// </summary>
        /// <param name="args">Command line arguments (not used in this application)</param>
        static void Main(string[] args)
        {
            PlayVoiceGreeting();
            DisplayAsciiArt();
            string userName = GetUserName();
            DisplayWelcomeMessage(userName);
            StartChat(userName);
        }

        /// <summary>
        /// Clamps a value between specified minimum and maximum bounds.
        /// This is a safer alternative to Math.Clamp for older .NET versions.
        /// </summary>
        /// <param name="value">The value to clamp</param>
        /// <param name="min">Minimum allowable value</param>
        /// <param name="max">Maximum allowable value</param>
        /// <returns>
        /// Returns:
        /// - min if value < min
        /// - max if value > max
        /// - value otherwise
        /// </returns>
        static int Clamp(int value, int min, int max)
        {
            return (value < min) ? min : (value > max) ? max : value;
        }

        /// <summary>
        /// Plays a WAV format audio greeting synchronously.
        /// Features:
        /// - Automatic path resolution relative to executable
        /// - Proper resource disposal
        /// - Comprehensive error handling
        /// - User feedback for missing files
        /// </summary>
        static void PlayVoiceGreeting()
        {
            try
            {
                // Get the application's base directory
                var basePath = AppDomain.CurrentDomain.BaseDirectory;

                // Construct relative and full paths to the audio file
                string relativePath = Path.Combine("Audio", "welcome.wav");
                string audioPath = Path.GetFullPath(Path.Combine(basePath, relativePath));

                // Verify file exists before attempting playback
                if (File.Exists(audioPath))
                {
                    // Use using statement for proper disposal of SoundPlayer resources
                    using (SoundPlayer player = new SoundPlayer(audioPath))
                    {
                        player.Load();  // Pre-load the audio file into memory
                        player.PlaySync();  // Play synchronously (blocks until complete)
                    }
                }
                else
                {
                    // Display error message in red if file not found
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Audio file not found at: {audioPath}");
                    Console.ResetColor();
                }
            }
            catch (Exception ex)
            {
                // Handle any unexpected errors during audio playback
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Error playing voice greeting: {ex.Message}");
                Console.ResetColor();
            }
        }

        /// <summary>
        /// Generates and displays ASCII art from an image file.
        /// Key features:
        /// - Automatic path resolution
        /// - Dynamic image resizing
        /// - Grayscale conversion
        /// - Brightness-based character mapping
        /// - Comprehensive error handling
        /// </summary>
        static void DisplayAsciiArt()
        {
            try
            {
                // Get the application's base directory
                var imageBasePath = AppDomain.CurrentDomain.BaseDirectory;

                // Construct relative and full paths to the image file
                string imageRelativePath = Path.Combine("Images", "cybersecurity.jpg");
                string imagePath = Path.GetFullPath(Path.Combine(imageBasePath, imageRelativePath));

                // Validate the image exists before processing
                if (!File.Exists(imagePath))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("ASCII art image not found at: " + imagePath);
                    Console.ResetColor();
                    return;
                }

                // Set output dimensions (adjust these based on your console size)
                // Note: Larger values will produce more detailed but potentially
                // less readable output in smaller console windows
                int width = 100;   // Width in characters
                int height = 50;   // Height in characters

                // Generate the ASCII art from the image
                string asciiArt = ConvertImageToAscii(imagePath, width, height);

                // Display the ASCII art in dark green for better visibility
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine(asciiArt);
                Console.ResetColor();
            }
            catch (Exception ex)
            {
                // Handle any errors during ASCII art generation
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Error generating ASCII art: {ex.Message}");
                Console.ResetColor();
            }
        }

        /// <summary>
        /// Converts an image file to ASCII art.
        /// The conversion process:
        /// 1. Loads and resizes the source image
        /// 2. Converts each pixel to grayscale
        /// 3. Maps brightness values to ASCII characters
        /// 4. Builds the final ASCII string
        /// </summary>
        /// <param name="imagePath">Path to the source image file</param>
        /// <param name="width">Desired width in characters</param>
        /// <param name="height">Desired height in characters</param>
        /// <returns>String containing the generated ASCII art</returns>
        static string ConvertImageToAscii(string imagePath, int width, int height)
        {
            // Load the source image
            using (Bitmap bitmap = new Bitmap(imagePath))
            {
                // Create resized version for ASCII conversion
                // Note: Resizing is necessary to fit the console output and
                // to reduce processing time for large images
                Bitmap resizedImage = new Bitmap(bitmap, new Size(width, height));
                StringBuilder asciiArt = new StringBuilder();

                // Process each pixel in the resized image
                for (int y = 0; y < resizedImage.Height; y++)
                {
                    for (int x = 0; x < resizedImage.Width; x++)
                    {
                        // Get the color of the current pixel
                        Color pixelColor = resizedImage.GetPixel(x, y);

                        // Convert the pixel color to grayscale using luminosity method
                        // This formula accounts for human perception of different colors
                        int grayValue = (int)(0.3 * pixelColor.R + 0.59 * pixelColor.G + 0.11 * pixelColor.B);

                        // Map the grayscale value to an ASCII character
                        char asciiChar = MapGrayValueToAscii(grayValue);

                        // Append the character to our result
                        asciiArt.Append(asciiChar);
                    }
                    // Add new line at the end of each row of pixels
                    asciiArt.AppendLine();
                }

                return asciiArt.ToString();
            }
        }

        /// <summary>
        /// Maps grayscale values (0-255) to ASCII characters.
        /// Uses a predefined brightness gradient (asciiChars array).
        /// </summary>
        /// <param name="grayValue">Pixel brightness value (0-255)</param>
        /// <returns>ASCII character representing the brightness level</returns>
        static char MapGrayValueToAscii(int grayValue)
        {
            // Ensure the value is within valid range (0-255)
            grayValue = Clamp(grayValue, 0, 255);

            // Calculate the index in the asciiChars array
            // The formula distributes the 256 possible gray values
            // across the available ASCII characters
            int index = grayValue * (asciiChars.Length - 1) / 255;

            // Return the corresponding ASCII character
            return asciiChars[index];
        }

        /// <summary>
        /// Collects and validates the user's name through console input.
        /// Implements:
        /// - Multiple attempts with countdown
        /// - Strict character validation (letters and spaces only)
        /// - Empty input detection
        /// - Graceful fallback to default name
        /// </summary>
        /// <returns>Validated user name as string</returns>
        static string GetUserName()
        {
            const int maxAttempts = 7;  // Maximum allowed attempts
            int attempts = 0;           // Current attempt count

            while (attempts < maxAttempts)
            {
                try
                {
                    // Prompt for input with colored text
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    Console.Write("Enter your name: ");
                    Console.ResetColor();

                    // Read user input and trim whitespace
                    string userName = Console.ReadLine();

                    // Validate the input is not empty or whitespace
                    if (string.IsNullOrWhiteSpace(userName))
                    {
                        throw new ArgumentException("Name cannot be empty.");
                    }

                    // Validate only letters and spaces are present
                    if (userName.Any(c => !char.IsLetter(c) && !char.IsWhiteSpace(c)))
                    {
                        throw new ArgumentException("Only letters and spaces allowed");
                    }

                    // Return the validated and trimmed name
                    return userName.Trim();
                }
                catch (Exception ex)
                {
                    // Increment attempt counter and show error
                    attempts++;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Error: {ex.Message} (Attempt {attempts}/{maxAttempts})");
                    Console.ResetColor();
                }
            }

            // If max attempts reached, use default name
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Using default name 'User'");
            Console.ResetColor();
            return "User";
        }

        /// <summary>
        /// Displays a personalized welcome message with formatted borders.
        /// Features:
        /// - Incorporates the user's name
        /// - Decorative borders for visual appeal
        /// - Color formatting
        /// - Comprehensive error handling
        /// </summary>
        /// <param name="userName">The name to include in the welcome message</param>
        static void DisplayWelcomeMessage(string userName)
        {
            try
            {
                // Display welcome message with decorative borders
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.WriteLine("=======================================================================");
                Console.WriteLine($"Hello, {userName}! Welcome to the Cybersecurity Awareness Bot.");
                Console.WriteLine("I'm here to help you stay safe online.");
                Console.WriteLine("=======================================================================");
                Console.ResetColor();
            }
            catch (Exception ex)
            {
                // Handle any display errors
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Error displaying welcome message: {ex.Message}");
                Console.ResetColor();
            }
        }

        /// <summary>
        /// Simulates typing effect by printing text with character delays.
        /// Creates a more natural, conversational interface.
        /// </summary>
        /// <param name="text">The text to display</param>
        /// <param name="delay">Milliseconds delay between characters (default: 30ms)</param>
        static void TypeText(string text, int delay = 30)
        {
            // Print each character with a slight delay
            foreach (char c in text)
            {
                Console.Write(c);
                Thread.Sleep(delay);  // Pause between characters
            }
            Console.WriteLine();  // Move to next line after completion
        }

        /// <summary>
        /// Main chat loop that handles user interaction.
        /// Features:
        /// - Cybersecurity knowledge base
        /// - Multi-topic response system
        /// - Help and exit commands
        /// - Comprehensive error handling
        /// </summary>
        /// <param name="userName">User's name for personalization</param>
        static void StartChat(string userName)
        {
            try
            {
                // Initialize the cybersecurity knowledge base
                var responses = InitializeKnowledgeBase();

                // Display initial instructions
                Console.ForegroundColor = ConsoleColor.Cyan;
                TypeText("Type 'help' for topics or 'exit' to quit", 30);
                Console.ResetColor();

                // Main conversation loop
                while (true)
                {
                    // Display prompt with user's name
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.Write($"{userName}: ");
                    Console.ResetColor();

                    // Read and clean user input
                    string userInput = Console.ReadLine()?.Trim();

                    // Handle empty input
                    if (string.IsNullOrWhiteSpace(userInput))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        TypeText("Please enter your question.", 30);
                        Console.ResetColor();
                        continue;
                    }

                    // Process special commands (help/exit)
                    if (ProcessSpecialCommands(userInput, responses))
                        break;

                    // Process regular cybersecurity topics
                    ProcessUserInput(userInput, responses);
                }
            }
            catch (Exception ex)
            {
                // Handle any unexpected errors in the chat system
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Chat error: {ex.Message}");
                Console.ResetColor();
            }
        }

        /// <summary>
        /// Initializes the cybersecurity knowledge base dictionary.
        /// Contains responses organized by topic with detailed information.
        /// </summary>
        /// <returns>Dictionary of topics and responses</returns>
        static Dictionary<string, string> InitializeKnowledgeBase()
        {
            return new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                // General conversation responses
                { "how are you", "I'm functioning optimally! Ready to discuss cybersecurity." },
                { "purpose", "I provide cybersecurity education to help you stay safe online." },
                { "help", "I can explain: Passwords, 2FA, phishing, VPNs, Wi-Fi security, HTTPS, email safety" },

                // Password security responses
                { "password", "Strong passwords should:\n- Be 12+ characters\n- Mix character types\n- Avoid personal info\n- Be unique per account\n- Use a password manager" },

                // Two-factor authentication responses
                { "2fa", "Two-factor authentication adds security by requiring:\n1. Something you know (password)\n2. Something you have (phone/device)\nUse authenticator apps instead of SMS when possible" },

                // Phishing awareness responses
                { "phishing", "Phishing protection:\n- Verify sender addresses\n- Hover before clicking links\n- Never share credentials via email\n- Report suspicious messages" },
                
                // VPN usage responses
                { "vpn", "VPN benefits:\n- Encrypts all internet traffic\n- Essential on public Wi-Fi\n- Choose no-log providers\n- Doesn't provide complete anonymity" },

                // Wifi responses
                { "wifi", "Public Wi-Fi usage tips:\n- Avoid accessing sensitive accounts or making financial transactions\n- Use a VPN to encrypt your traffic\n- Disable file sharing and sharing of devices\n- Turn off automatic connection to Wi-Fi networks" },
                { "wi-fi", "Public Wi-Fi usage tips:\n- Avoid accessing sensitive accounts or making financial transactions\n- Use a VPN to encrypt your traffic\n- Disable file sharing and sharing of devices\n- Turn off automatic connection to Wi-Fi networks" },
                { "public wifi", "Public Wi-Fi usage tips:\n- Avoid accessing sensitive accounts or making financial transactions\n- Use a VPN to encrypt your traffic\n- Disable file sharing and sharing of devices\n- Turn off automatic connection to Wi-Fi networks" },
                { "public wi-fi", "Public Wi-Fi usage tips:\n- Avoid accessing sensitive accounts or making financial transactions\n- Use a VPN to encrypt your traffic\n- Disable file sharing and sharing of devices\n- Turn off automatic connection to Wi-Fi networks" },

                // Email security responses
                { "email", "Email safety tips:\n- Enable strong spam filters\n- Verify unusual requests\n- Don't open unexpected attachments\n- Review old emails periodically" }
            };
        }

        /// <summary>
        /// Processes special commands (help/exit).
        /// Returns true if the exit command was received.
        /// </summary>
        /// <param name="input">User input to check</param>
        /// <param name="responses">Knowledge base dictionary</param>
        /// <returns>True if exit command was received</returns>
        static bool ProcessSpecialCommands(string input, Dictionary<string, string> responses)
        {
            // Check for exit command
            if (input.Equals("exit", StringComparison.OrdinalIgnoreCase))
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                TypeText("Stay safe online! Goodbye.", 30);
                Console.ResetColor();
                return true;
            }

            // Check for help command
            if (input.Equals("help", StringComparison.OrdinalIgnoreCase))
            {
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                TypeText(responses["help"], 30);
                Console.ResetColor();
            }

            return false;
        }

        /// <summary>
        /// Processes user input to find and respond to cybersecurity topics.
        /// Features:
        /// - Multiple topics in one query
        /// - Ordered responses based on input
        /// - Unknown topic fallback
        /// </summary>
        /// <param name="input">User input string</param>
        /// <param name="responses">Knowledge base dictionary</param>
        static void ProcessUserInput(string input, Dictionary<string, string> responses)
        {
            // Find all matching topics in the order they appear in the input
            var matchedTopics = responses.Keys
                .Where(topic => input.IndexOf(topic, StringComparison.OrdinalIgnoreCase) >= 0)
                .OrderBy(topic => input.IndexOf(topic, StringComparison.OrdinalIgnoreCase))
                .ToList();

            if (matchedTopics.Count > 0)
            {
                // Display each matched topic with its response
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.Write("ChatBot: ");
                Console.ForegroundColor = ConsoleColor.Magenta;

                foreach (string topic in matchedTopics)
                {
                    TypeText($"{topic.ToUpper()} >> {responses[topic]}", 30);
                }
            }
            else
            {
                // No recognized topics found
                Console.ForegroundColor = ConsoleColor.Red;
                TypeText("I don't know about that topic. Try 'help' for options.", 30);
            }
            Console.ResetColor();
        }
    }
}