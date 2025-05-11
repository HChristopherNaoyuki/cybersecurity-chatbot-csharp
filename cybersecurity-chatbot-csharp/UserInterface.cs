using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading;

namespace cybersecurity_chatbot_csharp
{
    /// <summary>
    /// Handles all user interface components and multimedia presentation.
    /// Implements the Facade pattern to simplify complex UI operations.
    /// </summary>
    public class UserInterface
    {
        private static readonly char[] AsciiChars = { '#', '8', '&', 'o', ':', '*', '.', ' ' };

        /// <summary>
        /// Plays the welcome audio greeting in WAV format.
        /// </summary>
        public void PlayVoiceGreeting()
        {
            try
            {
                string audioPath = GetResourcePath("Audio", "welcome.wav");

                if (File.Exists(audioPath))
                {
                    using (SoundPlayer player = new SoundPlayer(audioPath))
                    {
                        player.Load();  // Pre-load for smooth playback
                        player.PlaySync();
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
        /// Generates and displays ASCII art from an image file.
        /// </summary>
        public void DisplayAsciiArt()
        {
            try
            {
                string imagePath = GetResourcePath("Images", "cybersecurity.png");

                if (!File.Exists(imagePath))
                {
                    DisplayError($"ASCII art image not found at: {imagePath}");
                    return;
                }

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
        /// Collects and validates the user's name through console input.
        /// </summary>
        public string GetUserName()
        {
            const int maxAttempts = 3;
            int attempts = 0;

            while (attempts < maxAttempts)
            {
                try
                {
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    Console.Write("Enter your name: ");
                    Console.ResetColor();

                    string userName = Console.ReadLine()?.Trim();

                    if (string.IsNullOrWhiteSpace(userName))
                    {
                        throw new ArgumentException("Name cannot be empty.");
                    }

                    if (userName.Any(c => !char.IsLetter(c) && !char.IsWhiteSpace(c)))
                    {
                        throw new ArgumentException("Only letters and spaces allowed");
                    }

                    return userName;
                }
                catch (Exception ex)
                {
                    attempts++;
                    DisplayError($"{ex.Message} (Attempt {attempts}/{maxAttempts})");
                }
            }

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Using default name 'User'");
            Console.ResetColor();
            return "User";
        }

        /// <summary>
        /// Displays a personalized welcome message with decorative borders.
        /// </summary>
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
        /// Simulates typing effect by printing text with character delays.
        /// </summary>
        public void TypeText(string text, int delay = 30)
        {
            if (string.IsNullOrEmpty(text)) return;

            foreach (char c in text)
            {
                Console.Write(c);
                Thread.Sleep(delay);
            }
            Console.WriteLine();
        }

        /// <summary>
        /// Displays error messages in standardized red text.
        /// </summary>
        public void DisplayError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        /// <summary>
        /// Gets the full path for a resource file
        /// </summary>
        private string GetResourcePath(string folder, string fileName)
        {
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            string relativePath = Path.Combine(folder, fileName);
            return Path.GetFullPath(Path.Combine(basePath, relativePath));
        }

        /// <summary>
        /// Converts an image to ASCII art
        /// </summary>
        private string ConvertImageToAscii(string imagePath, int width, int height)
        {
            using (Bitmap bitmap = new Bitmap(imagePath))
            {
                Bitmap resizedImage = new Bitmap(bitmap, new Size(width, height));
                StringBuilder asciiArt = new StringBuilder();

                for (int y = 0; y < resizedImage.Height; y++)
                {
                    for (int x = 0; x < resizedImage.Width; x++)
                    {
                        Color pixelColor = resizedImage.GetPixel(x, y);
                        int grayValue = (int)(0.3 * pixelColor.R + 0.59 * pixelColor.G + 0.11 * pixelColor.B);
                        char asciiChar = MapGrayValueToAscii(grayValue);
                        asciiArt.Append(asciiChar);
                    }
                    asciiArt.AppendLine();
                }

                return asciiArt.ToString();
            }
        }

        /// <summary>
        /// Maps a grayscale value to an ASCII character
        /// </summary>
        private char MapGrayValueToAscii(int grayValue)
        {
            grayValue = Math.Min(Math.Max(grayValue, 0), 255);
            int index = grayValue * (AsciiChars.Length - 1) / 255;
            return AsciiChars[index];
        }
    }
}