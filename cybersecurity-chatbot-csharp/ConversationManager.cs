using System;
using System.Collections.Generic;
using System.Linq;

namespace cybersecurity_chatbot_csharp
{
    /// <summary>
    /// Orchestrates all conversation logic for the chatbot.
    /// Implements natural language processing, sentiment analysis, and dynamic responses.
    /// 
    /// Key Features:
    /// - Keyword recognition
    /// - Sentiment detection
    /// - Context-aware responses
    /// - Interest tracking
    /// - Command processing
    /// 
    /// Design Patterns:
    /// - State (conversation flow)
    /// - Strategy (response generation)
    /// - Chain of Responsibility (command processing)
    /// </summary>
    public class ConversationManager
    {
        // Dependencies
        private readonly KnowledgeBase _knowledgeBase;
        private readonly MemoryManager _memory;
        private readonly UserInterface _ui;

        // Configuration
        private readonly Dictionary<string, string[]> _sentimentKeywords = new Dictionary<string, string[]>
        {
            ["worried"] = new[] { "worried", "concerned", "scared", "afraid" },
            ["positive"] = new[] { "happy", "excited", "great", "good" },
            ["negative"] = new[] { "angry", "frustrated", "annoyed", "upset" },
            ["curious"] = new[] { "what", "how", "explain", "?", "tell me" }
        };

        private readonly string[] _exitCommands = { "exit", "quit", "bye" };
        private readonly string[] _helpCommands = { "help", "options", "topics" };
        private readonly string[] _favoritesCommands = { "favorites", "my favorites" };

        /// <summary>
        /// Initializes a new ConversationManager instance.
        /// 
        /// Parameters:
        /// - knowledgeBase: Source of response content
        /// - memory: User state manager
        /// - ui: Interface for input/output
        /// 
        /// Exceptions:
        /// - ArgumentNullException for null dependencies
        /// </summary>
        public ConversationManager(KnowledgeBase knowledgeBase, MemoryManager memory, UserInterface ui)
        {
            _knowledgeBase = knowledgeBase ?? throw new ArgumentNullException(nameof(knowledgeBase));
            _memory = memory ?? throw new ArgumentNullException(nameof(memory));
            _ui = ui ?? throw new ArgumentNullException(nameof(ui));
        }

        /// <summary>
        /// Starts and manages the main conversation loop.
        /// 
        /// Features:
        /// - Self-healing error handling
        /// - Persistent until exit command
        /// - Help guidance
        /// - Graceful termination
        /// </summary>
        public void StartChat()
        {
            try
            {
                _ui.TypeText("Type 'help' for topics, 'favorites' to manage favorites, or 'exit' to quit", 30);

                while (true)
                {
                    ProcessUserInput();
                }
            }
            catch (Exception ex)
            {
                _ui.DisplayError($"System error: {ex.Message}");
                _ui.TypeText("Restarting conversation...", 30);
                StartChat(); // Recursive restart
            }
        }

        /// <summary>
        /// Processes a single user input cycle.
        /// 
        /// Workflow:
        /// 1. Gets and validates input
        /// 2. Routes commands
        /// 3. Processes natural language
        /// 4. Generates responses
        /// </summary>
        private void ProcessUserInput()
        {
            string input = GetUserInput();

            if (string.IsNullOrWhiteSpace(input))
            {
                _ui.DisplayError("Please enter your question.");
                return;
            }

            if (IsExitCommand(input))
            {
                HandleExit();
                return;
            }

            if (IsHelpCommand(input))
            {
                DisplayHelp();
                return;
            }

            if (IsFavoritesCommand(input))
            {
                HandleFavoritesCommand(input);
                return;
            }

            ProcessNaturalLanguage(input);
        }

        /// <summary>
        /// Gets and formats user input with proper prompting.
        /// 
        /// UI Features:
        /// - Color-coded prompt
        /// - Input trimming
        /// - Null safety
        /// </summary>
        private string GetUserInput()
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write($"{_memory.UserName}: ");
            Console.ResetColor();
            return Console.ReadLine()?.Trim() ?? string.Empty;
        }

        /// <summary>
        /// Determines if input is an exit command.
        /// 
        /// Comparison:
        /// - Case-insensitive
        /// - Whole-word matching
        /// </summary>
        private bool IsExitCommand(string input) =>
            _exitCommands.Contains(input, StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// Handles graceful application termination.
        /// 
        /// Cleanup:
        /// - Ensures console reset
        /// - Provides friendly exit
        /// - Uses Environment.Exit
        /// </summary>
        private void HandleExit()
        {
            _ui.TypeText("Stay safe online! Goodbye.", 30);
            Environment.Exit(0);
        }

        /// <summary>
        /// Determines if input is a help request.
        /// 
        /// Comparison:
        /// - Case-insensitive
        /// - Whole-word matching
        /// </summary>
        private bool IsHelpCommand(string input) =>
            _helpCommands.Contains(input, StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// Displays comprehensive help information.
        /// 
        /// Content:
        /// - Available topics
        /// - Usage examples
        /// - Command reference
        /// </summary>
        private void DisplayHelp()
        {
            _ui.TypeText("I can help with these cybersecurity topics:", 20);

            foreach (string topic in _knowledgeBase.GetAllTopics())
            {
                if (!string.IsNullOrEmpty(topic))
                {
                    Console.WriteLine($"- {topic}");
                }
            }

            _ui.TypeText("\nTry saying:", 20);
            Console.WriteLine("• \"Tell me about password safety\"");
            Console.WriteLine("• \"How do I recognize phishing emails?\"");
            Console.WriteLine("• \"I'm interested in privacy\"");

            _ui.TypeText("\nCommands:", 20);
            Console.WriteLine("• help - Show this help");
            Console.WriteLine("• favorites - View your favorites");
            Console.WriteLine("• favorites add - Add a new favorite");
            Console.WriteLine("• exit - End the conversation");
        }

        /// <summary>
        /// Determines if input is a favorites command.
        /// 
        /// Comparison:
        /// - Case-insensitive
        /// - Prefix matching
        /// </summary>
        private bool IsFavoritesCommand(string input) =>
            _favoritesCommands.Any(cmd => input.StartsWith(cmd, StringComparison.OrdinalIgnoreCase));

        /// <summary>
        /// Routes favorites-related commands to appropriate handlers.
        /// 
        /// Supported Commands:
        /// - "favorites" - View all
        /// - "favorites add" - Add new
        /// </summary>
        private void HandleFavoritesCommand(string input)
        {
            if (input.Equals("favorites", StringComparison.OrdinalIgnoreCase))
            {
                _memory.DisplayFavorites();
            }
            else if (input.StartsWith("favorites add", StringComparison.OrdinalIgnoreCase))
            {
                _memory.StoreUserFavorite();
            }
            else
            {
                _ui.DisplayError("Unknown favorites command. Try 'favorites' or 'favorites add'");
            }
        }

        /// <summary>
        /// Processes natural language input with full NLP pipeline.
        /// 
        /// Pipeline Stages:
        /// 1. Sentiment analysis
        /// 2. Keyword extraction
        /// 3. Interest detection
        /// 4. Response generation
        /// </summary>
        private void ProcessNaturalLanguage(string input)
        {
            string sentiment = DetectSentiment(input);
            List<string> keywords = ExtractKeywords(input);

            if (TryHandleInterestExpression(input, keywords))
                return;

            if (keywords.Count == 0 && _memory.HasInterest())
            {
                HandleRememberedInterest();
                return;
            }

            GenerateResponse(input, sentiment, keywords);
        }

        /// <summary>
        /// Analyzes text to determine emotional sentiment.
        /// 
        /// Algorithm:
        /// - Keyword-based pattern matching
        /// - Priority-ordered sentiment categories
        /// - Default neutral sentiment
        /// </summary>
        private string DetectSentiment(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return "neutral";

            string lowerInput = input.ToLower();

            foreach (var sentiment in _sentimentKeywords)
            {
                if (sentiment.Value.Any(keyword => lowerInput.Contains(keyword)))
                    return sentiment.Key;
            }

            return "neutral";
        }

        /// <summary>
        /// Extracts meaningful keywords from input text.
        /// 
        /// Processing:
        /// - Tokenization
        /// - Stop word removal
        /// - Minimum length filtering
        /// </summary>
        private List<string> ExtractKeywords(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return new List<string>();

            return input.Split(new[] { ' ', ',', '.', '?', '!' },
                      StringSplitOptions.RemoveEmptyEntries)
                  .Select(word => word.ToLower().Trim())
                  .Where(word => word.Length > 2 && !_knowledgeBase.ShouldIgnoreWord(word))
                  .ToList();
        }

        /// <summary>
        /// Handles expressions of interest in specific topics.
        /// 
        /// Pattern:
        /// - Matches "interested in [topic]" phrases
        /// - Updates user memory
        /// - Provides immediate feedback
        /// </summary>
        private bool TryHandleInterestExpression(string input, List<string> keywords)
        {
            if (!input.ToLower().Contains("interested in"))
                return false;

            foreach (string topic in _knowledgeBase.GetAllTopics())
            {
                if (input.ToLower().Contains(topic.ToLower()))
                {
                    _memory.RememberInterest(topic);
                    _ui.TypeText($"I'll remember you're interested in {topic}. ", 20);

                    string response = _knowledgeBase.GetResponse(topic);
                    if (!string.IsNullOrEmpty(response))
                    {
                        _ui.TypeText(response, 20);
                    }
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Handles conversation when no keywords but remembered interest exists.
        /// 
        /// Personalization:
        /// - Uses stored interest
        /// - Provides contextual follow-up
        /// </summary>
        private void HandleRememberedInterest()
        {
            string response = _knowledgeBase.GetResponse(_memory.UserInterest);
            if (!string.IsNullOrEmpty(response))
            {
                _ui.TypeText($"Since you're interested in {_memory.UserInterest}, here's more: ", 20);
                _ui.TypeText(response, 20);
            }
        }

        /// <summary>
        /// Generates and displays response to user input.
        /// 
        /// Composition:
        /// - Sentiment-appropriate opening
        /// - Topic-matched content
        /// - Fallback guidance
        /// </summary>
        private void GenerateResponse(string input, string sentiment, List<string> keywords)
        {
            string sentimentResponse = GetSentimentResponse(sentiment);
            var matchedTopics = FindMatchingTopics(keywords);

            if (matchedTopics.Count > 0)
            {
                DisplayMatchedTopics(sentimentResponse, matchedTopics);
            }
            else
            {
                _ui.DisplayError(sentimentResponse + "I'm not sure about that. Try 'help' for options.");
            }
        }

        /// <summary>
        /// Generates sentiment-appropriate response openings.
        /// 
        /// Tone Matching:
        /// - Empathetic for negative
        /// - Enthusiastic for positive
        /// - Neutral otherwise
        /// </summary>
        private string GetSentimentResponse(string sentiment)
        {
            switch (sentiment)
            {
                case "worried":
                    return "I understand this can be concerning. ";
                case "positive":
                    return "Great! ";
                case "negative":
                    return "I'm sorry you're feeling frustrated. ";
                case "curious":
                    return "That's a great question! ";
                default:
                    return "";
            }
        }

        /// <summary>
        /// Finds knowledge base topics matching keywords.
        /// 
        /// Matching:
        /// - Case-insensitive
        /// - Exact match
        /// - Returns dictionary of matches
        /// </summary>
        private Dictionary<string, string> FindMatchingTopics(List<string> keywords)
        {
            var matches = new Dictionary<string, string>();

            foreach (string keyword in keywords)
            {
                string response = _knowledgeBase.GetResponse(keyword);
                if (!string.IsNullOrEmpty(response))
                {
                    matches[keyword] = response;
                }
            }

            return matches;
        }

        /// <summary>
        /// Displays matched topics with proper formatting.
        /// 
        /// Presentation:
        /// - Color-coded
        /// - Typing animation
        /// - Clear topic separation
        /// </summary>
        private void DisplayMatchedTopics(string sentimentResponse, Dictionary<string, string> matchedTopics)
        {
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.Write("ChatBot: ");
            Console.ForegroundColor = ConsoleColor.Magenta;

            if (!string.IsNullOrEmpty(sentimentResponse))
            {
                _ui.TypeText(sentimentResponse, 20);
            }

            foreach (var topic in matchedTopics)
            {
                _ui.TypeText($"{topic.Key.ToUpper()} >> {topic.Value}", 20);
            }

            Console.ResetColor();
        }
    }
}