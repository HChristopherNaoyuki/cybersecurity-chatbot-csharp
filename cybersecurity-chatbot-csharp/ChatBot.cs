using System;

namespace cybersecurity_chatbot_csharp
{
    /// <summary>
    /// Main orchestrator class for the Cybersecurity Awareness Chatbot.
    /// Manages the complete application lifecycle and subsystem coordination.
    /// </summary>
    public class ChatBot
    {
        private readonly KnowledgeBase _knowledgeBase;
        private readonly UserInterface _ui;
        private readonly ConversationManager _conversation;
        private readonly MemoryManager _memory;

        /// <summary>
        /// Initializes a new ChatBot instance.
        /// Sets up all subsystems in proper dependency order.
        /// </summary>
        public ChatBot()
        {
            _ui = new UserInterface();
            _knowledgeBase = new KnowledgeBase();
            _memory = new MemoryManager(); // No default username set here
            _conversation = new ConversationManager(_knowledgeBase, _memory, _ui);
        }

        /// <summary>
        /// Runs the main chatbot workflow.
        /// Implements the complete user interaction sequence.
        /// </summary>
        public void Run()
        {
            try
            {
                ExecuteWelcomeSequence();
                ExecuteUserIdentification(); // Now properly sets username
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
            // Get and set actual username from user input
            _memory.UserName = _ui.GetUserName();
            _ui.DisplayWelcomeMessage(_memory.UserName);
        }

        private void ExecuteMainConversation()
        {
            _ui.TypeText("Type 'help' for topics or 'exit' to quit", 30);
            _conversation.StartChat();
        }
    }
}