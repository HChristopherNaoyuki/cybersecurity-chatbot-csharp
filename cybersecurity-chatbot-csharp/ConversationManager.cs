using System;
using System.Collections;

namespace cybersecurity_chatbot_csharp
{
    /// <summary>
    /// Manages the core conversation logic of the Cybersecurity Chatbot.
    /// Handles user input processing, sentiment analysis, response generation,
    /// and maintains conversation state.
    /// </summary>
    public class ConversationManager
    {
        private readonly KnowledgeBase knowledgeBase;
        private readonly MemoryManager memory;
        private readonly UserInterface ui;

        /// <summary>
        /// Initializes a new instance of the ConversationManager
        /// </summary>
        /// <param name="kb">KnowledgeBase instance for accessing responses</param>
        /// <param name="mm">MemoryManager instance for storing user context</param>
        /// <param name="ui">UserInterface instance for handling I/O</param>
        public ConversationManager(KnowledgeBase kb, MemoryManager mm, UserInterface ui)
        {
            this.knowledgeBase = kb ?? throw new ArgumentNullException(nameof(kb));
            this.memory = mm ?? throw new ArgumentNullException(nameof(mm));
            this.ui = ui ?? throw new ArgumentNullException(nameof(ui));
        }

        /// <summary>
        /// Main entry point that starts and manages the conversation loop
        /// </summary>
        public void StartChat()
        {
            try
            {
                // Display initial instructions
                ui.TypeText("Type 'help' for topics or 'exit' to quit", 30);

                // Main conversation loop
                while (true)
                {
                    ProcessUserInput();
                }
            }
            catch (Exception ex)
            {
                ui.DisplayError($"A system error occurred: {ex.Message}");
                ui.TypeText("Restarting conversation...", 30);
                StartChat(); // Restart conversation on critical error
            }
        }

        /// <summary>
        /// Handles a single cycle of user input and bot response
        /// </summary>
        private void ProcessUserInput()
        {
            // Display prompt and get input
            string userInput = GetUserInput();

            // Handle empty input
            if (string.IsNullOrWhiteSpace(userInput))
            {
                ui.DisplayError("Please enter your question.");
                return;
            }

            // Check for exit command
            if (IsExitCommand(userInput))
            {
                HandleExit();
                return;
            }

            // Check for help command
            if (IsHelpCommand(userInput))
            {
                DisplayHelp();
                return;
            }

            // Process regular input
            ProcessRegularInput(userInput);
        }

        /// <summary>
        /// Gets and validates user input from console
        /// </summary>
        private string GetUserInput()
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write($"{memory.UserName}: ");
            Console.ResetColor();

            return Console.ReadLine()?.Trim() ?? string.Empty;
        }

        /// <summary>
        /// Checks if input is an exit command
        /// </summary>
        private bool IsExitCommand(string input)
        {
            return input.Equals("exit", StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Handles the exit command gracefully
        /// </summary>
        private void HandleExit()
        {
            ui.TypeText("Stay safe online! Goodbye.", 30);
            Environment.Exit(0); // Clean exit
        }

        /// <summary>
        /// Checks if input is a help command
        /// </summary>
        private bool IsHelpCommand(string input)
        {
            return input.Equals("help", StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Displays help information with available topics
        /// </summary>
        private void DisplayHelp()
        {
            ui.TypeText("I can help with these cybersecurity topics:", 20);

            ArrayList topics = knowledgeBase.GetAllTopics();
            if (topics != null && topics.Count > 0)
            {
                foreach (string topic in topics)
                {
                    if (!string.IsNullOrEmpty(topic))
                    {
                        Console.WriteLine($"- {topic}");
                    }
                }
            }

            ui.TypeText("\nYou can say things like:", 20);
            Console.WriteLine("• \"Tell me about password safety\"");
            Console.WriteLine("• \"How do I recognize phishing emails?\"");
            Console.WriteLine("• \"I'm interested in privacy\"");
        }

        /// <summary>
        /// Processes regular user input (non-command)
        /// </summary>
        private void ProcessRegularInput(string input)
        {
            // Step 1: Analyze sentiment
            string sentiment = DetectSentiment(input);

            // Step 2: Extract meaningful keywords
            ArrayList keywords = ExtractKeywords(input);

            // Step 3: Check for interest expression
            if (TryHandleInterestExpression(input, keywords))
            {
                return;
            }

            // Step 4: Handle remembered interest if no keywords
            if (keywords.Count == 0 && memory.HasInterest())
            {
                HandleRememberedInterest();
                return;
            }

            // Step 5: Generate and display response
            GenerateResponse(input, sentiment, keywords);
        }

        /// <summary>
        /// Detects sentiment from user input using keyword analysis
        /// </summary>
        private string DetectSentiment(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return "neutral";

            input = input.ToLower();

            // Sentiment keywords dictionary
            var sentimentKeywords = new System.Collections.Generic.Dictionary<string, string[]>
            {
                { "worried", new[] { "worried", "concerned", "scared" } },
                { "positive", new[] { "happy", "excited", "great" } },
                { "negative", new[] { "angry", "frustrated", "annoyed" } },
                { "curious", new[] { "what", "how", "explain", "?" } }
            };

            foreach (var sentiment in sentimentKeywords)
            {
                if (sentiment.Value == null) continue;

                foreach (var keyword in sentiment.Value)
                {
                    if (!string.IsNullOrEmpty(keyword) && input.Contains(keyword))
                    {
                        return sentiment.Key;
                    }
                }
            }

            return "neutral";
        }

        /// <summary>
        /// Extracts important keywords from user input
        /// </summary>
        private ArrayList ExtractKeywords(string input)
        {
            ArrayList keywords = new ArrayList();

            if (string.IsNullOrWhiteSpace(input))
                return keywords;

            string[] words = input.Split(new[] { ' ', ',', '.', '?', '!' },
                StringSplitOptions.RemoveEmptyEntries);

            if (words == null || words.Length == 0)
                return keywords;

            foreach (string word in words)
            {
                string cleanWord = word.ToLower().Trim();
                if (cleanWord.Length > 2 && !knowledgeBase.ShouldIgnoreWord(cleanWord))
                {
                    keywords.Add(cleanWord);
                }
            }

            return keywords;
        }

        /// <summary>
        /// Handles expressions of interest in specific topics
        /// </summary>
        private bool TryHandleInterestExpression(string input, ArrayList keywords)
        {
            if (!input.ToLower().Contains("interested in"))
                return false;

            ArrayList topics = knowledgeBase.GetAllTopics();
            if (topics == null || topics.Count == 0)
                return false;

            foreach (string topic in topics)
            {
                if (input.ToLower().Contains(topic.ToLower()))
                {
                    memory.RememberInterest(topic);
                    ui.TypeText($"I'll remember you're interested in {topic}. ", 20);

                    string response = knowledgeBase.GetResponse(topic);
                    if (!string.IsNullOrEmpty(response))
                    {
                        ui.TypeText(response, 20);
                    }
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Handles conversation when no keywords but remembered interest exists
        /// </summary>
        private void HandleRememberedInterest()
        {
            if (!memory.HasInterest() || string.IsNullOrEmpty(memory.UserInterest))
                return;

            string response = knowledgeBase.GetResponse(memory.UserInterest);
            if (!string.IsNullOrEmpty(response))
            {
                ui.TypeText($"Since you're interested in {memory.UserInterest}, here's more: ", 20);
                ui.TypeText(response, 20);
            }
        }

        /// <summary>
        /// Generates and displays response to user input
        /// </summary>
        private void GenerateResponse(string input, string sentiment, ArrayList keywords)
        {
            // Get sentiment-based opening
            string sentimentResponse = GetSentimentResponse(sentiment);

            // Find matching topics
            ArrayList matchedTopics = FindMatchingTopics(keywords);

            // Display response
            if (matchedTopics.Count > 0)
            {
                DisplayMatchedTopics(sentimentResponse, matchedTopics);
            }
            else
            {
                ui.DisplayError(sentimentResponse + "I'm not sure about that topic. Try 'help' for options.");
            }
        }

        /// <summary>
        /// Gets appropriate sentiment-based opening phrase
        /// </summary>
        private string GetSentimentResponse(string sentiment)
        {
            switch (sentiment)
            {
                case "worried": return "I understand this can be concerning. ";
                case "positive": return "Great! ";
                case "negative": return "I'm sorry you're feeling frustrated. ";
                case "curious": return "That's a great question! ";
                default: return "";
            }
        }

        /// <summary>
        /// Finds all topics that match the extracted keywords
        /// </summary>
        private ArrayList FindMatchingTopics(ArrayList keywords)
        {
            ArrayList matchedTopics = new ArrayList();

            if (keywords == null || keywords.Count == 0)
                return matchedTopics;

            foreach (string keyword in keywords)
            {
                string response = knowledgeBase.GetResponse(keyword);
                if (!string.IsNullOrEmpty(response))
                {
                    matchedTopics.Add(new string[] { keyword, response });
                }
            }

            return matchedTopics;
        }

        /// <summary>
        /// Displays matched topics with proper formatting
        /// </summary>
        private void DisplayMatchedTopics(string sentimentResponse, ArrayList matchedTopics)
        {
            if (matchedTopics == null || matchedTopics.Count == 0)
                return;

            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.Write("ChatBot: ");
            Console.ForegroundColor = ConsoleColor.Magenta;

            if (!string.IsNullOrEmpty(sentimentResponse))
            {
                ui.TypeText(sentimentResponse, 20);
            }

            foreach (string[] topic in matchedTopics)
            {
                if (topic != null && topic.Length >= 2)
                {
                    ui.TypeText($"{topic[0]?.ToUpper()} >> {topic[1]}", 20);
                }
            }
            // Reset color after displaying response
            Console.ResetColor();
        }
    }
}