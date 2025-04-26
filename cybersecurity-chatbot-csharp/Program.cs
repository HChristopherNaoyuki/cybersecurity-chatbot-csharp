using System;

namespace cybersecurity_chatbot_csharp
{
    /// <summary>
    /// Main entry point for the Cybersecurity Awareness Chatbot application.
    /// Implements the Exception Shield pattern for top-level error handling.
    /// 
    /// Responsibilities:
    /// - Creates and initializes the ChatBot instance
    /// - Provides global exception handling
    /// - Ensures clean application shutdown
    /// </summary>
    class Program
    {
        /// <summary>
        /// Application entry point with global exception handling.
        /// </summary>
        static void Main()
        {
            try
            {
                var bot = new ChatBot();
                bot.Run();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Critical error: {ex.Message}");
                Console.ResetColor();
                Environment.Exit(1);
            }
        }
    }
}