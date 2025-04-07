using System;
using System.Media;
using System.IO;
using System.Threading;
using System.Drawing;
using System.Text;
using System.Linq;

namespace cybersecurity_chatbot_csharp
{
    /// <summary>
    /// Handles all user interface components including:
    /// - Audio playback
    /// - ASCII art generation
    /// - Console formatting and display
    /// - User input collection
    /// </summary>
    public class UserInterface
    {
        // ASCII character gradient from darkest to lightest
        private static readonly char[] asciiChars = { '#', '8', '&', 'o', ':', '*', '.', ' ' };

        /// <summary>
        /// Plays the welcome audio greeting in WAV format
        /// </summary>
        public void PlayVoiceGreeting()
        {
            try
            {
                // Get application base directory
                var basePath = AppDomain.CurrentDomain.BaseDirectory;

                // Construct path to audio file
                string relativePath = Path.Combine("Audio", "welcome.wav");
                string audioPath = Path.GetFullPath(Path.Combine(basePath, relativePath));

                // Verify and play audio file
                if (File.Exists(audioPath))
                {
                    using (SoundPlayer player = new SoundPlayer(audioPath))
                    {
                        player.Load();  // Pre-load for smooth playback
                        player.PlaySync();  // Play synchronously
                    }
                }
                else
                {
                    DisplayError($"Audio file not found at: {audioPath}");
                }
            }
            catch (Exception ex)
            {
                DisplayError($"Error playing voice greeting: {ex.Message}");
            }
        }

        /// <summary>
        /// Generates and displays ASCII art from an image file
        /// </summary>
        public void DisplayAsciiArt()
        {
            try
            {
                // Get application base directory
                var imageBasePath = AppDomain.CurrentDomain.BaseDirectory;

                // Construct path to image file
                string imageRelativePath = Path.Combine("Images", "cybersecurity.jpg");
                string imagePath = Path.GetFullPath(Path.Combine(imageBasePath, imageRelativePath));

                // Verify image exists
                if (!File.Exists(imagePath))
                {
                    DisplayError($"ASCII art image not found at: {imagePath}");
                    return;
                }

                // Generate and display ASCII art
                string asciiArt = ConvertImageToAscii(imagePath, 100, 50);
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine(asciiArt);
                Console.ResetColor();
            }
            catch (Exception ex)
            {
                DisplayError($"Error generating ASCII art: {ex.Message}");
            }
        }

        /// <summary>
        /// Collects and validates the user's name through console input
        /// </summary>
        /// <returns>Validated user name</returns>
        public string GetUserName()
        {
            const int maxAttempts = 7;
            int attempts = 0;

            while (attempts < maxAttempts)
            {
                try
                {
                    // Prompt for input
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    Console.Write("Enter your name: ");
                    Console.ResetColor();

                    string userName = Console.ReadLine();

                    // Validate input
                    if (string.IsNullOrWhiteSpace(userName))
                    {
                        throw new ArgumentException("Name cannot be empty.");
                    }

                    if (userName.Any(c => !char.IsLetter(c) && !char.IsWhiteSpace(c)))
                    {
                        throw new ArgumentException("Only letters and spaces allowed");
                    }

                    return userName.Trim();
                }
                catch (Exception ex)
                {
                    attempts++;
                    DisplayError($"{ex.Message} (Attempt {attempts}/{maxAttempts})");
                }
            }

            // Fallback to default name if max attempts reached
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Using default name 'User'");
            Console.ResetColor();
            return "User";
        }

        /// <summary>
        /// Displays a personalized welcome message with decorative borders
        /// </summary>
        /// <param name="userName">The user's name to include in the message</param>
        public void DisplayWelcomeMessage(string userName)
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
                DisplayError($"Error displaying welcome message: {ex.Message}");
            }
        }

        /// <summary>
        /// Simulates typing effect by printing text with character delays
        /// </summary>
        /// <param name="text">Text to display</param>
        /// <param name="delay">Milliseconds delay between characters</param>
        public void TypeText(string text, int delay = 30)
        {
            foreach (char c in text)
            {
                Console.Write(c);
                Thread.Sleep(delay);
            }
            Console.WriteLine();
        }

        /// <summary>
        /// Displays error messages in red text
        /// </summary>
        /// <param name="message">Error message to display</param>
        public void DisplayError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        /// <summary>
        /// Converts an image file to ASCII art
        /// </summary>
        /// <param name="imagePath">Path to source image</param>
        /// <param name="width">Width in characters</param>
        /// <param name="height">Height in characters</param>
        /// <returns>ASCII art string</returns>
        private string ConvertImageToAscii(string imagePath, int width, int height)
        {
            using (Bitmap bitmap = new Bitmap(imagePath))
            {
                // Resize image to fit console
                Bitmap resizedImage = new Bitmap(bitmap, new Size(width, height));
                StringBuilder asciiArt = new StringBuilder();

                // Process each pixel
                for (int y = 0; y < resizedImage.Height; y++)
                {
                    for (int x = 0; x < resizedImage.Width; x++)
                    {
                        // Convert pixel to grayscale
                        Color pixelColor = resizedImage.GetPixel(x, y);
                        int grayValue = (int)(0.3 * pixelColor.R + 0.59 * pixelColor.G + 0.11 * pixelColor.B);

                        // Map grayscale to ASCII character
                        char asciiChar = MapGrayValueToAscii(grayValue);
                        asciiArt.Append(asciiChar);
                    }
                    asciiArt.AppendLine();
                }

                return asciiArt.ToString();
            }
        }

        /// <summary>
        /// Maps grayscale value (0-255) to ASCII character
        /// </summary>
        private char MapGrayValueToAscii(int grayValue)
        {
            grayValue = Clamp(grayValue, 0, 255);
            int index = grayValue * (asciiChars.Length - 1) / 255;
            return asciiChars[index];
        }

        /// <summary>
        /// Clamps value between min and max bounds
        /// </summary>
        private int Clamp(int value, int min, int max)
        {
            return (value < min) ? min : (value > max) ? max : value;
        }
    }
}