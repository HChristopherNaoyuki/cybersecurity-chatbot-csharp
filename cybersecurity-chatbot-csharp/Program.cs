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

            // Task 4: Start the chatbot conversation
            StartChat(userName);
        }

        // Method to play a voice greeting
        static void PlayVoiceGreeting()
        {
            try
            {
                // Get the base directory of the application
                var basePath = AppDomain.CurrentDomain.BaseDirectory;

                // Combine the base directory with the relative path to the audio file
                string relativePath = Path.Combine("Audio", "welcome.wav");

                // Get the absolute path by resolving the relative path
                string audioPath = Path.GetFullPath(Path.Combine(basePath, relativePath));

                // Check if the file exists at the resolved path
                if (File.Exists(audioPath))
                {
                    // Load and play the WAV file
                    SoundPlayer player = new SoundPlayer(audioPath);
                    player.Load(); // Ensure the file is loaded before playing
                    player.PlaySync(); // Play synchronously
                }
                else
                {
                    // Display an error message if the audio file is not found
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Audio file not found at: {audioPath}");
                    Console.ResetColor();
                }
            }
            catch (Exception ex)
            {
                // Handle any errors that occur while playing the voice greeting
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Error playing voice greeting: {ex.Message}");
                Console.ResetColor();
            }
        }

        // Method to display ASCII art
        static void DisplayAsciiArt()
        {
            try
            {
                // Set the console text color to Dark Green for the ASCII art
                Console.ForegroundColor = ConsoleColor.DarkGreen;

                // Display the cybersecurity-themed ASCII art
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

                // Reset the console text color to default
                Console.ResetColor();
            }
            catch (Exception ex)
            {
                // Handle any errors that occur while displaying the ASCII art
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Error displaying ASCII art: {ex.Message}");
                Console.ResetColor();
            }
        }

        // Method to get the user's name
        static string GetUserName()
        {
            try
            {
                // Prompt the user to enter their name
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("Enter your name: ");
                Console.ResetColor();

                // Read the user's input
                string userName = Console.ReadLine();

                // Input validation: Ensure the name is not empty
                while (string.IsNullOrWhiteSpace(userName))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Name cannot be empty. Please enter your name:");
                    Console.ResetColor();
                    userName = Console.ReadLine();
                }

                // Return the validated user name
                return userName;
            }
            catch (Exception ex)
            {
                // Handle any errors that occur while getting the user's name
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Error getting user name: {ex.Message}");
                Console.ResetColor();
                return "User"; // Default name if an error occurs
            }
        }

        // Method to display a welcome message
        static void DisplayWelcomeMessage(string userName)
        {
            try
            {
                // Set the console text color to Yellow for the welcome message
                Console.ForegroundColor = ConsoleColor.Yellow;

                // Display a welcome message with the user's name
                Console.WriteLine("=======================================================================");
                Console.WriteLine($"Hello, {userName}! Welcome to the Cybersecurity Awareness Bot.");
                Console.WriteLine("I'm here to help you stay safe online.");
                Console.WriteLine("=======================================================================");

                // Reset the console text color to default
                Console.ResetColor();
            }
            catch (Exception ex)
            {
                // Handle any errors that occur while displaying the welcome message
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Error displaying welcome message: {ex.Message}");
                Console.ResetColor();
            }
        }

        // Method to simulate typing effect
        static void TypeText(string text, int delay = 50)
        {
            // Loop through each character in the text
            foreach (char c in text)
            {
                // Print the character to the console
                Console.Write(c);

                // Simulate a typing delay
                Thread.Sleep(delay);
            }

            // Move to the next line after typing the text
            Console.WriteLine();
        }

        // Method to start the chat
        static void StartChat(string userName)
        {
            try
            {
                // Define keywords and responses for cybersecurity topics
                Dictionary<string, string> responses = new Dictionary<string, string>
                {
                    // General questions
                    { "how are you", "I'm doing great, thanks for asking!" },
                    { "purpose", "My purpose is to keep you safe online." },

                    // Cybersecurity topics
                    { "password", "A strong password should be at least 12 characters long, with a mix of letters, numbers, and symbols." },
                    { "2fa", "Two-factor authentication adds an extra layer of security. Always enable it when possible." },
                    { "phishing", "Phishing is when attackers trick you into revealing sensitive information via fake emails or websites." },
                    { "spear phishing", "Spear phishing targets specific individuals or organizations to steal sensitive data." },
                    { "email", "Always verify the sender before opening links or attachments in emails." },
                    { "scam", "If you suspect an email is a scam, don't click anything. Report it and delete it." },
                    { "vpn", "A VPN encrypts your internet traffic, making it safer from hackers." },
                    { "public wi-fi", "Avoid using public Wi-Fi for sensitive transactions unless you're using a VPN." },
                    { "https", "HTTPS websites encrypt data between your browser and the server, keeping it safe from attackers." }
                };

                // Display exit prompt in Yellow
                Console.ForegroundColor = ConsoleColor.Yellow;
                TypeText("Type 'exit' to quit the chat.", 30);
                Console.ResetColor();

                // Start the chatbot conversation loop
                while (true)
                {
                    // Prompt the user for input
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write($"{userName}: ");
                    Console.ResetColor();

                    // Read the user's input and convert it to lowercase
                    string userInput = Console.ReadLine()?.Trim().ToLower();

                    // Input validation: Check for empty input
                    if (string.IsNullOrWhiteSpace(userInput))
                    {
                        Console.ForegroundColor = ConsoleColor.Red; // Error message in Red
                        TypeText("ChatBot: I did not quite understand that. Could you rephrase?", 30);
                        Console.ResetColor();
                        continue; // Skip to the next iteration of the loop
                    }

                    // Check if the user wants to exit the chat
                    if (userInput == "exit")
                    {
                        Console.ForegroundColor = ConsoleColor.Magenta; // Chatbot responses in Pink
                        TypeText("ChatBot: Goodbye! Stay safe online!", 30);
                        Console.ResetColor();
                        break; // Exit the loop and end the chat
                    }

                    // Check if the input contains any known keyword(s)
                    bool responseFound = false;
                    foreach (var keyword in responses.Keys)
                    {
                        if (userInput.Contains(keyword))
                        {
                            // Display the corresponding response for the keyword
                            Console.ForegroundColor = ConsoleColor.Magenta; // Chatbot responses in Pink
                            TypeText($"ChatBot: {responses[keyword]}", 30);
                            Console.ResetColor();
                            responseFound = true;
                            break; // Exit the loop after finding a matching keyword
                        }
                    }

                    // If no keyword was found, ask the user to rephrase
                    if (!responseFound)
                    {
                        Console.ForegroundColor = ConsoleColor.Red; // Error message in Red
                        TypeText("ChatBot: I did not quite understand that. Could you rephrase?", 30);
                        Console.ResetColor();
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle any errors that occur during the chat
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Error in chatbot: {ex.Message}");
                Console.ResetColor();
            }
        }
    }
}