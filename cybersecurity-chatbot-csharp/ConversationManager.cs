using System;
using System.Collections.Generic;
using System.Linq;

namespace cybersecurity_chatbot_csharp
{
    /// <summary>
    /// Orchestrates all conversation logic and natural language processing for the chatbot.
    /// Implements a sophisticated pipeline for processing user input and generating contextual responses.
    ///
    /// Key Features:
    /// - Multi-stage input processing pipeline
    /// - Sentiment-aware response generation
    /// - Contextual memory integration
    /// - Customizable command handling
    /// - Rich text formatting with color coding
    ///
    /// Design Patterns:
    /// - Chain of Responsibility: For command processing pipeline
    /// - Strategy: Multiple response generation strategies
    /// - State: Manages conversation context
    /// - Observer: Notifies UI of responses
    /// </summary>
    public class ConversationManager
    {
        private readonly KnowledgeBase _knowledgeBase;
        private readonly MemoryManager _memory;
        private readonly UserInterface _ui;

        /// <summary>
        /// Lambda expression for detecting exit commands.
        /// Provides case-insensitive comparison against known exit phrases.
        /// </summary>
        private readonly Func<string, bool> _isExitCommand = input =>
            new[] { "exit", "quit", "bye" }.Contains(input, StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// Lambda expression for detecting help commands.
        /// Matches against various forms of help requests.
        /// </summary>
        private readonly Func<string, bool> _isHelpCommand = input =>
            new[] { "help", "options", "topics" }.Contains(input, StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// Lambda expression for detecting name queries.
        /// Uses substring matching for flexible phrasing.
        /// </summary>
        private readonly Func<string, bool> _isNameQuery = input =>
            input.IndexOf("what is my name", StringComparison.OrdinalIgnoreCase) >= 0;

        /// <summary>
        /// Delegate defining the signature for response generation strategies.
        /// Allows polymorphic response handling based on context.
        /// </summary>
        private delegate string ResponseGenerator(string input, string sentiment, List<string> keywords);

        /// <summary>
        /// Dictionary mapping response types to their generation strategies.
        /// Enables flexible addition of new response types without modifying core logic.
        /// </summary>
        private readonly Dictionary<string, ResponseGenerator> _responseHandlers;

        /// <summary>
        /// Initializes a new ConversationManager with required dependencies.
        /// Uses constructor injection for testability and loose coupling.
        /// </summary>
        /// <param name="knowledgeBase">The knowledge repository</param>
        /// <param name="memory">The memory persistence system</param>
        /// <param name="ui">The user interface handler</param>
        /// <exception cref="ArgumentNullException">Thrown if any dependency is null</exception>
        public ConversationManager(KnowledgeBase knowledgeBase, MemoryManager memory, UserInterface ui)
        {
            _knowledgeBase = knowledgeBase ?? throw new ArgumentNullException(nameof(knowledgeBase));
            _memory = memory ?? throw new ArgumentNullException(nameof(memory));
            _ui = ui ?? throw new ArgumentNullException(nameof(ui));

            // Initialize response handlers
            _responseHandlers = new Dictionary<string, ResponseGenerator>
            {
                ["default"] = GenerateDefaultResponse,
                ["interest"] = GenerateInterestBasedResponse,
                ["name"] = GenerateNameResponse
            };
        }

        /// <summary>
        /// Starts the main conversation loop.
        /// Implements self-healing error recovery through recursive restart.
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
                StartChat(); // Recursive restart
            }
        }

        /// <summary>
        /// Processes a single user input through the complete conversation pipeline.
        /// Implements the following workflow:
        /// 1. Input collection and validation
        /// 2. Command detection and handling
        /// 3. Natural language processing
        /// 4. Response generation and display
        /// </summary>
        private void ProcessUserInput()
        {
            string input = GetUserInput();

            if (string.IsNullOrWhiteSpace(input))
            {
                _ui.DisplayError("Please enter your question.");
                return;
            }

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
                DisplayNameResponse();
                return;
            }

            ProcessNaturalLanguage(input);
        }

        /// <summary>
        /// Collects user input with proper console formatting.
        /// Applies trimming and null protection.
        /// </summary>
        /// <returns>The cleaned user input string</returns>
        private string GetUserInput()
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write($"{_memory.UserName}: ");
            Console.ResetColor();
            return Console.ReadLine()?.Trim() ?? string.Empty;
        }

        /// <summary>
        /// Handles application exit with proper cleanup and user feedback.
        /// </summary>
        private void HandleExit()
        {
            _ui.TypeText("Stay safe online! Goodbye.", 30);
            Environment.Exit(0);
        }

        /// <summary>
        /// Displays help information including available topics and commands.
        /// Uses the knowledge base to dynamically generate topic list.
        /// </summary>
        private void DisplayHelp()
        {
            _ui.TypeText("I can help with these cybersecurity topics:", 20);
            foreach (string topic in _knowledgeBase.GetAllTopics())
            {
                Console.WriteLine($"- {topic}");
            }
        }

        /// <summary>
        /// Displays the name recall response with proper formatting.
        /// Uses the memory manager's configured response strategy.
        /// </summary>
        private void DisplayNameResponse()
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("ChatBot: ");
            Console.ForegroundColor = ConsoleColor.Magenta;
            _ui.TypeText(_memory.OnNameRecall(_memory.UserName), 20);
            Console.ResetColor();
        }

        /// <summary>
        /// Processes natural language input through the complete NLP pipeline:
        /// 1. Sentiment analysis
        /// 2. Keyword extraction
        /// 3. Interest detection
        /// 4. Contextual response generation
        /// </summary>
        /// <param name="input">The user's natural language input</param>
        private void ProcessNaturalLanguage(string input)
        {
            string sentiment = DetectSentiment(input);
            List<string> keywords = ExtractKeywords(input);

            if (TryHandleInterestExpression(input, keywords)) return;

            var responseType = DetermineResponseType(input, keywords);
            string response = _responseHandlers[responseType](input, sentiment, keywords);

            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("ChatBot: ");
            Console.ForegroundColor = ConsoleColor.Magenta;
            _ui.TypeText(response, 20);
            Console.ResetColor();
        }

        /// <summary>
        /// Analyzes text to determine emotional sentiment.
        /// Uses keyword matching against a sentiment lexicon.
        /// </summary>
        /// <param name="input">Text to analyze</param>
        /// <returns>Detected sentiment ("neutral" if none matched)</returns>
        private string DetectSentiment(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return "neutral";

            var sentimentMap = new Dictionary<string, string[]>
            {
                ["worried"] = new[] { "worried", "concerned", "scared" },
                ["positive"] = new[] { "happy", "excited", "great" },
                ["negative"] = new[] { "angry", "frustrated", "upset" },
                ["curious"] = new[] { "what", "how", "explain", "?" }
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
        }

        /// <summary>
        /// Extracts meaningful keywords from input text.
        /// Applies stopword filtering and minimum length requirements.
        /// </summary>
        /// <param name="input">Text to process</param>
        /// <returns>List of relevant keywords</returns>
        private List<string> ExtractKeywords(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return new List<string>();

            return input.Split(new[] { ' ', ',', '.', '?', '!' },
                      StringSplitOptions.RemoveEmptyEntries)
                  .Select(word => word.ToLower().Trim())
                  .Where(word => word.Length > 2 && !_knowledgeBase.ShouldIgnoreWord(word))
                  .ToList();
        }

        /// <summary>
        /// Handles expressions of interest in specific topics.
        /// Updates memory and provides immediate feedback.
        /// </summary>
        /// <param name="input">User input to analyze</param>
        /// <param name="keywords">Extracted keywords</param>
        /// <returns>True if interest was detected and handled</returns>
        private bool TryHandleInterestExpression(string input, List<string> keywords)
        {
            if (!input.ToLower().Contains("interested in")) return false;

            foreach (string topic in _knowledgeBase.GetAllTopics())
            {
                if (input.ToLower().Contains(topic.ToLower()))
                {
                    _memory.RememberInterest(topic);
                    string response = _knowledgeBase.GetResponse(topic);

                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write("ChatBot: ");
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    _ui.TypeText(response, 20);
                    Console.ResetColor();

                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Determines the appropriate response type based on context.
        /// Considers both current input and remembered interests.
        /// </summary>
        private string DetermineResponseType(string input, List<string> keywords)
        {
            if (_memory.HasInterest() && keywords.Contains(_memory.UserInterest.ToLower()))
                return "interest";
            return "default";
        }

        /// <summary>
        /// Generates a default response based on keywords and sentiment.
        /// Provides topic-matched responses when possible.
        /// </summary>
        private string GenerateDefaultResponse(string input, string sentiment, List<string> keywords)
        {
            string sentimentResponse = GetSentimentResponse(sentiment);
            var matchedTopics = keywords
                .Select(k => new { Key = k, Value = _knowledgeBase.GetResponse(k) })
                .Where(x => !string.IsNullOrEmpty(x.Value))
                .ToDictionary(x => x.Key, x => x.Value);

            if (matchedTopics.Count > 0)
            {
                return string.Join("\n", matchedTopics.Select(t => $"{t.Key.ToUpper()} >> {t.Value}"));
            }
            return sentimentResponse + "I'm not sure about that. Try 'help' for options.";
        }

        /// <summary>
        /// Generates an interest-contextualized response.
        /// Personalizes the response based on remembered interests.
        /// </summary>
        private string GenerateInterestBasedResponse(string input, string sentiment, List<string> keywords)
        {
            string baseResponse = _knowledgeBase.GetResponse(_memory.UserInterest);
            return _memory.GetInterestResponse(_memory.UserInterest, baseResponse);
        }

        /// <summary>
        /// Generates a name recall response.
        /// Uses the memory manager's configured response strategy.
        /// </summary>
        private string GenerateNameResponse(string input, string sentiment, List<string> keywords)
        {
            return _memory.OnNameRecall(_memory.UserName);
        }

        /// <summary>
        /// Generates sentiment-appropriate response openings.
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
    }
}