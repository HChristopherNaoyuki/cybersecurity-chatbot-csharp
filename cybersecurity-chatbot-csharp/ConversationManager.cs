using System;
using System.Collections.Generic;
using System.Linq;

namespace cybersecurity_chatbot_csharp
{
    /// <summary>
    /// Handles all conversation logic and user interaction flow
    /// 
    /// Responsibilities:
    /// - Processes user input through NLP pipeline
    /// - Detects commands and special queries
    /// - Manages conversation state and flow
    /// - Coordinates with KnowledgeBase and MemoryManager
    /// - Generates appropriate responses
    /// 
    /// Design Patterns:
    /// - Chain of Responsibility for command detection
    /// - Strategy pattern for response generation
    /// - Observer pattern for keyword tracking
    /// </summary>
    public class ConversationManager
    {
        // Delegate type definitions
        public delegate bool CommandDetector(string input);
        public delegate string ResponseGenerator(string keyword, string sentiment, int count);
        public delegate string SentimentDetector(string input);
        public delegate List<string> KeywordExtractor(string input);

        // Dependencies
        private readonly KnowledgeBase _knowledgeBase;
        private readonly MemoryManager _memory;
        private readonly UserInterface _ui;

        // Command detection delegates (lambda expressions)
        private readonly CommandDetector _isExitCommand = input =>
            new[] { "exit", "quit", "bye" }.Contains(input, StringComparer.OrdinalIgnoreCase);

        private readonly CommandDetector _isHelpCommand = input =>
            new[] { "help", "options", "topics" }.Contains(input, StringComparer.OrdinalIgnoreCase);

        private readonly CommandDetector _isNameQuery = input =>
            input.IndexOf("what is my name", StringComparison.OrdinalIgnoreCase) >= 0 ||
            input.IndexOf("who am i", StringComparison.OrdinalIgnoreCase) >= 0;

        private readonly CommandDetector _isFrequentQuestionQuery = input =>
            input.IndexOf("most frequently asked", StringComparison.OrdinalIgnoreCase) >= 0 ||
            input.IndexOf("faq", StringComparison.OrdinalIgnoreCase) >= 0 ||
            input.IndexOf("most common question", StringComparison.OrdinalIgnoreCase) >= 0 ||
            input.IndexOf("what do i ask most", StringComparison.OrdinalIgnoreCase) >= 0 ||
            input.IndexOf("frequent questions", StringComparison.OrdinalIgnoreCase) >= 0;

        // Response generation delegate
        private readonly ResponseGenerator _keywordResponseGenerator;

        // Sentiment detection delegate
        private readonly SentimentDetector _detectSentiment;

        // Keyword extraction delegate
        private readonly KeywordExtractor _extractKeywords;

        /// <summary>
        /// Initializes a new ConversationManager with dependencies
        /// </summary>
        /// <param name="knowledgeBase">KnowledgeBase instance</param>
        /// <param name="memory">MemoryManager instance</param>
        /// <param name="ui">UserInterface instance</param>
        /// <exception cref="ArgumentNullException">Thrown if any dependency is null</exception>
        public ConversationManager(KnowledgeBase knowledgeBase, MemoryManager memory, UserInterface ui)
        {
            _knowledgeBase = knowledgeBase ?? throw new ArgumentNullException(nameof(knowledgeBase));
            _memory = memory ?? throw new ArgumentNullException(nameof(memory));
            _ui = ui ?? throw new ArgumentNullException(nameof(ui));

            // Initialize response generator with lambda
            _keywordResponseGenerator = (keyword, sentiment, count) =>
            {
                string response = _knowledgeBase.GetResponse(keyword);
                if (string.IsNullOrEmpty(response)) return null;

                // Add contextual prefix if discussed before
                if (count > 1)
                {
                    response = _memory.GetContextualResponse(keyword, response, count);
                }

                // Add sentiment prefix if needed
                if (sentiment != "neutral")
                {
                    response = $"{GetSentimentResponse(sentiment)}{response}";
                }

                return $"{keyword.ToUpper()}: {response}";
            };

            // Initialize sentiment detector with lambda
            _detectSentiment = input =>
            {
                if (string.IsNullOrWhiteSpace(input)) return "neutral";

                var sentimentMap = new Dictionary<string, string[]>
                {
                    ["worried"] = new[] { "worried", "concerned", "scared" },
                    ["positive"] = new[] { "happy", "excited", "great" },
                    ["negative"] = new[] { "angry", "frustrated", "upset" },
                    ["curious"] = new[] { "what", "how", "explain", "?", "why" }
                };

                string lowerInput = input.ToLower();
                foreach (var sentiment in sentimentMap)
                {
                    if (sentiment.Value.Any(keyword => lowerInput.Contains(keyword)))
                    {
                        return sentiment.Key;
                    }
                }
                return "neutral";
            };

            // Initialize keyword extractor with lambda
            _extractKeywords = input =>
            {
                if (string.IsNullOrWhiteSpace(input)) return new List<string>();

                return input.Split(new[] { ' ', ',', '.', '?', '!' },
                          StringSplitOptions.RemoveEmptyEntries)
                      .Select(word => word.ToLower().Trim())
                      .Where(word => word.Length > 2 && !_knowledgeBase.ShouldIgnoreWord(word))
                      .ToList();
            };
        }

        /// <summary>
        /// Main chat loop that processes user input continuously
        /// </summary>
        public void StartChat()
        {
            try
            {
                while (true)
                {
                    ProcessUserInput();
                }
            }
            catch (Exception ex)
            {
                _ui.DisplayError($"Conversation error: {ex.Message}");
                StartChat(); // Restart conversation on error
            }
        }

        /// <summary>
        /// Processes a single user input through conversation pipeline
        /// </summary>
        private void ProcessUserInput()
        {
            string input = GetUserInput();

            if (string.IsNullOrWhiteSpace(input))
            {
                _ui.DisplayError("Please enter your question.");
                return;
            }

            // Check for special commands using delegate predicates
            if (_isExitCommand(input))
            {
                HandleExit();
                return;
            }

            if (_isHelpCommand(input))
            {
                DisplayHelp();
                return;
            }

            if (_isNameQuery(input))
            {
                _ui.TypeText(_memory.OnNameRecall(_memory.UserName), 20);
                return;
            }

            if (_isFrequentQuestionQuery(input))
            {
                _ui.TypeText(_memory.GetFrequentQuestionResponse(), 20);
                return;
            }

            ProcessNaturalLanguage(input);
        }

        /// <summary>
        /// Gets formatted user input with username prefix
        /// </summary>
        /// <returns>Trimmed user input string</returns>
        private string GetUserInput()
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write($"{_memory.UserName}: ");
            Console.ResetColor();
            return Console.ReadLine()?.Trim() ?? string.Empty;
        }

        /// <summary>
        /// Handles exit command with farewell message
        /// </summary>
        private void HandleExit()
        {
            _ui.TypeText("Stay safe online! Goodbye.", 30);
            Environment.Exit(0);
        }

        /// <summary>
        /// Displays help information with available topics
        /// </summary>
        private void DisplayHelp()
        {
            _ui.TypeText("Chatbot: ", 20);
            Console.ForegroundColor = ConsoleColor.Magenta;
            _ui.TypeText("I can help with these cybersecurity topics:", 20);
            foreach (string topic in _knowledgeBase.GetAllTopics())
            {
                Console.WriteLine($"- {topic}");
            }
            Console.ResetColor();
        }

        /// <summary>
        /// Processes natural language input through full NLP pipeline
        /// </summary>
        /// <param name="input">Raw user input</param>
        private void ProcessNaturalLanguage(string input)
        {
            // Use sentiment detection delegate
            string sentiment = _detectSentiment(input);

            // Use keyword extraction delegate
            List<string> keywords = _extractKeywords(input);

            // Save all keywords to memory
            keywords.ForEach(keyword => _memory.RememberKeyword(keyword));

            if (TryHandleInterestExpression(input, keywords)) return;

            DisplayMultiTopicResponses(keywords, sentiment);
        }

        /// <summary>
        /// Displays individual responses for each recognized keyword
        /// </summary>
        /// <param name="keywords">List of extracted keywords</param>
        /// <param name="sentiment">Detected user sentiment</param>
        private void DisplayMultiTopicResponses(List<string> keywords, string sentiment)
        {
            bool anyResponses = false;

            foreach (string keyword in keywords.Distinct())
            {
                int count = _memory.GetKeywordCount(keyword);
                // Use response generator delegate
                string response = _keywordResponseGenerator(keyword, sentiment, count);

                if (!string.IsNullOrEmpty(response))
                {
                    anyResponses = true;
                    DisplayResponse(response);
                }
            }

            if (!anyResponses)
            {
                DisplayResponse("I'm not sure about that. Try 'help' for options.");
            }
        }

        /// <summary>
        /// Handles explicit interest expressions ("I'm interested in...")
        /// </summary>
        /// <param name="input">User input</param>
        /// <param name="keywords">Extracted keywords</param>
        /// <returns>True if interest was expressed and handled</returns>
        private bool TryHandleInterestExpression(string input, List<string> keywords)
        {
            if (!input.ToLower().Contains("interested in")) return false;

            foreach (string topic in _knowledgeBase.GetAllTopics())
            {
                if (input.ToLower().Contains(topic.ToLower()))
                {
                    _memory.RememberKeyword(topic);
                    string response = _knowledgeBase.GetResponse(topic);
                    DisplayResponse(response);
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Displays a formatted response to the user
        /// </summary>
        /// <param name="response">Response text to display</param>
        private void DisplayResponse(string response)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("ChatBot: ");
            Console.ForegroundColor = ConsoleColor.Magenta;
            _ui.TypeText(response, 20);
            Console.ResetColor();
        }

        /// <summary>
        /// Gets an appropriate sentiment-based prefix for responses
        /// </summary>
        /// <param name="sentiment">Detected sentiment</param>
        /// <returns>Sentiment-specific prefix string</returns>
        private string GetSentimentResponse(string sentiment)
        {
            switch (sentiment)
            {
                case "worried": return "I understand this can be concerning. ";
                case "positive": return "Great! I'm glad you're enthusiastic! ";
                case "negative": return "I'm sorry you're feeling frustrated. ";
                case "curious": return "That's a great question! ";
                default: return "";
            }
        }
    }
}