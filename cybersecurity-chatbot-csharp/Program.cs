using System;
using System.Media;
using System.Threading;

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
                SoundPlayer player = new SoundPlayer("E:\\Projects\\Source Code\\Visual Studio 2022\\cybersecurity-chatbot-csharp\\cybersecurity-chatbot-csharp\\welcome.wav");
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
 _____         _                                                _  _            _____  _             _    _             _   
/  __ \       | |                                              (_)| |          /  __ \| |           | |  | |           | |  
| /  \/ _   _ | |__    ___  _ __  ___   ___   ___  _   _  _ __  _ | |_  _   _  | /  \/| |__    __ _ | |_ | |__    ___  | |_ 
| |    | | | || '_ \  / _ \| '__|/ __| / _ \ / __|| | | || '__|| || __|| | | | | |    | '_ \  / _` || __|| '_ \  / _ \ | __|
| \__/\| |_| || |_) ||  __/| |   \__ \|  __/| (__ | |_| || |   | || |_ | |_| | | \__/\| | | || (_| || |_ | |_) || (_) || |_ 
 \____/ \__, ||_.__/  \___||_|   |___/ \___| \___| \__,_||_|   |_| \__| \__, |  \____/|_| |_| \__,_| \__||_.__/  \___/  \__|
         __/ |                                                           __/ |                                              
        |___/                                                           |___/                                               
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

        // Method to simulate typing effect
        static void TypeText(string text, int delay = 50)
        {
            foreach (char c in text)
            {
                Console.Write(c);
                Thread.Sleep(delay); // Simulate typing delay
            }
            Console.WriteLine(); // Move to the next line after typing
        }

        // Method to start the chat
        static void StartChat(string userName)
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            TypeText("You can ask me about:\n- Password safety\n- Phishing\n- Safe browsing\n", 30);
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
                    TypeText("I didn’t quite understand that. Could you rephrase?", 30);
                    Console.ResetColor();
                    continue;
                }

                if (userInput == "exit")
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    TypeText("Goodbye! Stay safe online!", 30);
                    Console.ResetColor();
                    break;
                }

                // Task 4: Basic Response System
                switch (userInput)
                {
                    case "how are you?":
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        TypeText("I'm just a bot, but I'm here to help you stay safe online!", 30);
                        Console.ResetColor();
                        break;

                    case "what's your purpose?":
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        TypeText("My purpose is to educate you about cybersecurity and help you stay safe online.", 30);
                        Console.ResetColor();
                        break;

                    case "what can i ask you about?":
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        TypeText("You can ask me about:\n- Password safety\n- Phishing\n- Safe browsing", 30);
                        Console.ResetColor();
                        break;

                    case "password":
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        TypeText("Use strong, unique passwords for each account. Avoid using personal details.", 30);
                        Console.ResetColor();
                        break;

                    case "phishing":
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        TypeText("Be cautious of emails asking for personal information. Scammers often disguise themselves as trusted organizations.", 30);
                        Console.ResetColor();
                        break;

                    case "safe browsing":
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        TypeText("Always check for 'https://' in the URL and avoid clicking on suspicious links.", 30);
                        Console.ResetColor();
                        break;

                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        TypeText("I didn’t quite understand that. Could you rephrase?", 30);
                        Console.ResetColor();
                        break;
                }
            }
        }
    }
}