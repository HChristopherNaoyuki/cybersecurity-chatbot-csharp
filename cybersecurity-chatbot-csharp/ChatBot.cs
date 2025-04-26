using System;

namespace cybersecurity_chatbot_csharp
{
    /// <summary>
    /// Main orchestrator class for the Cybersecurity Awareness Chatbot application.
    /// Implements the Facade pattern to provide a simplified interface to the chatbot's subsystems.
    /// 
    /// Responsibilities:
    /// - Coordinates all major components (UI, Knowledge, Memory, Conversation)
    /// - Manages the high-level application lifecycle
    /// - Handles initialization of all subsystems
    /// - Provides the main execution entry point
    /// 
    /// Design Principles:
    /// - Single Responsibility: Only coordinates, contains no business logic
    /// - Dependency Injection: Requires all subsystems in constructor
    /// - Loose Coupling: Subsystems communicate through interfaces
    /// </summary>
    public class ChatBot
    {
        private readonly KnowledgeBase _knowledgeBase;
        private readonly UserInterface _ui;
        private readonly ConversationManager _conversation;
        private readonly MemoryManager _memory;

        /// <summary>
        /// Initializes a new instance of the ChatBot class.
        /// Uses the Composition Root pattern for dependency construction.
        /// </summary>
        public ChatBot()
        {
            _ui = new UserInterface();
            _knowledgeBase = new KnowledgeBase();
            _memory = new MemoryManager();
            _conversation = new ConversationManager(_knowledgeBase, _memory, _ui);
        }

        /// <summary>
        /// Main execution method that runs the chatbot workflow.
        /// Implements the Template Method pattern for the fixed execution sequence.
        /// </summary>
        public void Run()
        {
            try
            {
                ExecuteWelcomeSequence();
                ExecuteUserIdentification();
                ExecuteMainConversation();
            }
            catch (Exception ex)
            {
                _ui.DisplayError($"Fatal error: {ex.Message}");
                Environment.Exit(1);
            }
        }

        private void ExecuteWelcomeSequence()
        {
            _ui.PlayVoiceGreeting();
            _ui.DisplayAsciiArt();
        }

        private void ExecuteUserIdentification()
        {
            _memory.UserName = _ui.GetUserName();
            _ui.DisplayWelcomeMessage(_memory.UserName);
        }

        private void ExecuteMainConversation()
        {
            _ui.TypeText("Type 'help' for topics or 'exit' to quit.", 30);
            _conversation.StartChat();
        }
    }
}