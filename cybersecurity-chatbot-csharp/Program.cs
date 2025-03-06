using System;
using System.Media; // For playing audio
using System.Threading; // For simulating typing effect

namespace cybersecurity_chatbot_csharp
{
    class Program
    {
        static void Main(string[] args)
        {
            // Task 1: Voice Greeting
            PlayVoiceGreeting();

            // Task 2: Image Display
            DisplayAsciiArt();

            // Task 3: Text-Based Greeting and User Interaction
            string userName = GetUserName();
            DisplayWelcomeMessage(userName);

            // Task 4: Basic Response System
            StartChat(userName);

            // Task 5: Input Validation is handled within the StartChat method
        }

        // Method to play a voice greeting
        static void PlayVoiceGreeting()
        {
            try
            {
                // Load and play the WAV file
                SoundPlayer player = new SoundPlayer("welcome.wav");
                player.PlaySync(); // Play synchronously
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error playing voice greeting: " + ex.Message);
                Console.ResetColor();
            }
        }

        // Method to display ASCII art
        static void DisplayAsciiArt()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(@"
  _____           _      _        _____  _____  _____  _____  _____ 
 |  __ \         | |    | |      / ____|/ ____|/ ____|/ ____|/ ____|
 | |__) |__ _ ___| | __ | |__   | (___ | |    | |    | |    | (___  
 |  ___// _` / __| |/ / | '_ \   \___ \| |    | |    | |     \___ \ 
 | |  | (_| \__ \   <  | |_) |  ____) | |____| |____| |____ ____) |
 |_|   \__,_|___/_|\_\ |_.__/  |_____/ \_____|\_____|\_____|_____/ 
            ");
            Console.ResetColor();
        }

        // Method to get the user's name
        static string GetUserName()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("Enter your name: ");
            Console.ResetColor();
            string userName = Console.ReadLine();

            // Input validation for empty name
            while (string.IsNullOrWhiteSpace(userName))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Name cannot be empty. Please enter your name:");
                Console.ResetColor();
                userName = Console.ReadLine();
            }

            return userName;
        }

        // Method to display a welcome message
        static void DisplayWelcomeMessage(string userName)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\nHello, {userName}! Welcome to the Cybersecurity Awareness Bot.");
            Console.WriteLine("I'm here to help you stay safe online.\n");
            Console.ResetColor();
        }

        // Method to start the chat
        static void StartChat(string userName)
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("You can ask me about:\n- Password safety\n- Phishing\n- Safe browsing\n");
            Console.ResetColor();

            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write($"{userName}, what would you like to know about? (Type 'exit' to quit): ");
                Console.ResetColor();
                string userInput = Console.ReadLine().ToLower();

                // Task 5: Input Validation
                if (string.IsNullOrWhiteSpace(userInput))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("I didn’t quite understand that. Could you rephrase?");
                    Console.ResetColor();
                    continue;
                }

                if (userInput == "exit")
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine("Goodbye! Stay safe online!");
                    Console.ResetColor();
                    break;
                }

                // Task 4: Basic Response System
                switch (userInput)
                {
                    case "password":
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine("Use strong, unique passwords for each account. Avoid using personal details.");
                        Console.ResetColor();
                        break;

                    case "phishing":
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine("Be cautious of emails asking for personal information. Scammers often disguise themselves as trusted organizations.");
                        Console.ResetColor();
                        break;

                    case "safe browsing":
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine("Always check for 'https://' in the URL and avoid clicking on suspicious links.");
                        Console.ResetColor();
                        break;

                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("I didn’t quite understand that. Could you rephrase?");
                        Console.ResetColor();
                        break;
                }
            }
        }
    }
}