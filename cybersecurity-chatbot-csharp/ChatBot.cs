using System;

namespace cybersecurity_chatbot_csharp
{
    /// <summary>
    /// Main orchestrator class for the Cybersecurity Awareness Chatbot application.
    /// Coordinates all major components and manages the application lifecycle.
    /// 
    /// Responsibilities:
    /// - Initializes all subsystem components
    /// - Manages the high-level application flow
    /// - Serves as the entry point for chatbot functionality
    /// </summary>
    public class ChatBot
    {
        // Core subsystem components
        private readonly KnowledgeBase knowledgeBase;
        private readonly UserInterface ui;
        private readonly ConversationManager conversation;
        private readonly MemoryManager memory;

        /// <summary>
        /// Initializes a new instance of the ChatBot class with all required dependencies.
        /// 
        /// Design Pattern:
        /// - Uses Dependency Injection to promote loose coupling and testability
        /// - Follows the Composition Root pattern for object graph construction
        /// </summary>
        public ChatBot()
        {
            // Initialize subsystems in recommended order:
            // 1. UI (needed first for any output)
            // 2. Knowledge Base (foundational data)
            // 3. Memory (persistence layer)
            // 4. Conversation (main logic, depends on all others)
            ui = new UserInterface();
            knowledgeBase = new KnowledgeBase();
            memory = new MemoryManager();
            conversation = new ConversationManager(knowledgeBase, memory, ui);
        }

        /// <summary>
        /// Main execution method that runs the chatbot workflow.
        /// 
        /// Execution Flow:
        /// 1. Plays welcome audio greeting
        /// 2. Displays ASCII art header
        /// 3. Collects and stores user's name
        /// 4. Displays personalized welcome
        /// 5. Enters main conversation loop
        /// 
        /// Error Handling:
        /// - Any exceptions bubble up to Program.cs for centralized handling
        /// </summary>
        public void Run()
        {
            // 1. Multimedia welcome sequence
            ui.PlayVoiceGreeting();
            ui.DisplayAsciiArt();

            // 2. User identification
            memory.UserName = ui.GetUserName();
            ui.DisplayWelcomeMessage(memory.UserName);

            // 3. Core interaction loop
            conversation.StartChat();
        }
    }
}