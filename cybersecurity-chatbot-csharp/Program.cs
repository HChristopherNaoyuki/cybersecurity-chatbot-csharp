using System;

namespace cybersecurity_chatbot_csharp
{
    /// <summary>
    /// Main entry point for the Cybersecurity Awareness Chatbot application.
    /// Creates and initializes the ChatBot instance.
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            // Initialize and run the chatbot
            ChatBot bot = new ChatBot();
            bot.Run();
        }
    }
}