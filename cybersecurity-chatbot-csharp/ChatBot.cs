using System.Collections;

namespace cybersecurity_chatbot_csharp
{
    /// <summary>
    /// Main controller class that orchestrates all chatbot functionality.
    /// Initializes all components and manages the application flow.
    /// </summary>
    public class ChatBot
    {
        private readonly KnowledgeBase knowledgeBase;
        private readonly UserInterface ui;
        private readonly ConversationManager conversation;
        private readonly MemoryManager memory;

        /// <summary>
        /// Constructor that initializes all components
        /// </summary>
        public ChatBot()
        {
            ui = new UserInterface();
            knowledgeBase = new KnowledgeBase();
            memory = new MemoryManager();
            conversation = new ConversationManager(knowledgeBase, memory, ui);
        }

        /// <summary>
        /// Main execution method that runs the chatbot workflow
        /// </summary>
        public void Run()
        {
            // 1. Play welcome audio greeting
            ui.PlayVoiceGreeting();

            // 2. Display ASCII art header
            ui.DisplayAsciiArt();

            // 3. Get and store user's name
            memory.UserName = ui.GetUserName();

            // 4. Display personalized welcome message
            ui.DisplayWelcomeMessage(memory.UserName);

            // 5. Start main conversation loop
            conversation.StartChat();
        }
    }
}