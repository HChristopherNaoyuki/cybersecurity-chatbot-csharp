using System;
using System.Collections.Generic;
using System.Linq;

namespace cybersecurity_chatbot_csharp
{
    /// <summary>
    /// Handles all conversation logic with:
    /// - Multi-keyword response generation
    /// - Persistent keyword tracking
    /// - Contextual conversation flow
    /// - Natural language processing
    /// </summary>
    public class ConversationManager
    {
        // Dependencies
        private readonly KnowledgeBase _knowledgeBase;
        private readonly MemoryManager _memory;
        private readonly UserInterface _ui;

        // Command detection predicates
        private readonly Func<string, bool> _isExitCommand = input =>
            new[] { "exit", "quit", "bye" }.Contains(input, StringComparer.OrdinalIgnoreCase);

        private readonly Func<string, bool> _isHelpCommand = input =>
            new[] { "help", "options", "topics" }.Contains(input, StringComparer.OrdinalIgnoreCase);

        private readonly Func<string, bool> _isNameQuery = input =>
            input.IndexOf("what is my name", StringComparison.OrdinalIgnoreCase) >= 0 ||
            input.IndexOf("who am i", StringComparison.OrdinalIgnoreCase) >= 0;

        public ConversationManager(KnowledgeBase knowledgeBase, MemoryManager memory, UserInterface ui)
        {
            _knowledgeBase = knowledgeBase ?? throw new ArgumentNullException(nameof(knowledgeBase));
            _memory = memory ?? throw new ArgumentNullException(nameof(memory));
            _ui = ui ?? throw new ArgumentNullException(nameof(ui));
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
                StartChat();
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

            ProcessNaturalLanguage(input);
        }

        /// <summary>
        /// Gets formatted user input with username prefix
        /// </summary>
        private string GetUserInput()
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write($"{_memory.UserName}: ");
            Console.ResetColor();
            return Console.ReadLine()?.Trim() ?? string.Empty;
        }

        private void HandleExit()
        {
            _ui.TypeText("Stay safe online! Goodbye.", 30);
            Environment.Exit(0);
        }

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
        private void ProcessNaturalLanguage(string input)
        {
            string sentiment = DetectSentiment(input);
            List<string> keywords = ExtractKeywords(input);

            // Save all keywords to memory
            foreach (string keyword in keywords)
            {
                _memory.RememberKeyword(keyword);
            }

            if (TryHandleInterestExpression(input, keywords)) return;

            DisplayMultiTopicResponses(keywords, sentiment);
        }

        /// <summary>
        /// Displays individual responses for each recognized keyword
        /// </summary>
        private void DisplayMultiTopicResponses(List<string> keywords, string sentiment)
        {
            bool anyResponses = false;

            foreach (string keyword in keywords.Distinct())
            {
                string response = GetKeywordResponse(keyword, sentiment);
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
        /// Generates a contextual response for a single keyword
        /// </summary>
        private string GetKeywordResponse(string keyword, string sentiment)
        {
            string response = _knowledgeBase.GetResponse(keyword);
            if (string.IsNullOrEmpty(response)) return null;

            // Get discussion count for contextual response
            int discussionCount = _memory.GetKeywordCount(keyword);

            // Add contextual prefix if discussed before
            if (discussionCount > 1)
            {
                response = _memory.GetContextualResponse(keyword, response, discussionCount);
            }

            // Add sentiment prefix if needed
            if (sentiment != "neutral")
            {
                response = $"{GetSentimentResponse(sentiment)}{response}";
            }

            return $"{keyword.ToUpper()}: {response}";
        }

        private void DisplayResponse(string response)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("ChatBot: ");
            Console.ForegroundColor = ConsoleColor.Magenta;
            _ui.TypeText(response, 20);
            Console.ResetColor();
        }

        private string DetectSentiment(string input)
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
        }

        private List<string> ExtractKeywords(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return new List<string>();

            return input.Split(new[] { ' ', ',', '.', '?', '!' },
                      StringSplitOptions.RemoveEmptyEntries)
                  .Select(word => word.ToLower().Trim())
                  .Where(word => word.Length > 2 && !_knowledgeBase.ShouldIgnoreWord(word))
                  .ToList();
        }

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