using System;
using System.Collections.Generic;
using System.Linq;

namespace cybersecurity_chatbot_csharp
{
    /// <summary>
    /// Manages all conversation logic with enhanced sentiment integration
    /// and topic awareness
    /// </summary>
    public class ConversationManager
    {
        private readonly KnowledgeBase _knowledgeBase;
        private readonly MemoryManager _memory;
        private readonly UserInterface _ui;

        private readonly Func<string, bool> _isExitCommand = input =>
            new[] { "exit", "quit", "bye" }.Contains(input, StringComparer.OrdinalIgnoreCase);

        private readonly Func<string, bool> _isHelpCommand = input =>
            new[] { "help", "options", "topics" }.Contains(input, StringComparer.OrdinalIgnoreCase);

        private readonly Func<string, bool> _isNameQuery = input =>
            input.IndexOf("what is my name", StringComparison.OrdinalIgnoreCase) >= 0 ||
            input.IndexOf("who am i", StringComparison.OrdinalIgnoreCase) >= 0;

        private delegate string ResponseGenerator(string input, string sentiment, List<string> keywords);
        private readonly Dictionary<string, ResponseGenerator> _responseHandlers;

        public ConversationManager(KnowledgeBase knowledgeBase, MemoryManager memory, UserInterface ui)
        {
            _knowledgeBase = knowledgeBase ?? throw new ArgumentNullException(nameof(knowledgeBase));
            _memory = memory ?? throw new ArgumentNullException(nameof(memory));
            _ui = ui ?? throw new ArgumentNullException(nameof(ui));

            _responseHandlers = new Dictionary<string, ResponseGenerator>
            {
                ["default"] = GenerateDefaultResponse,
                ["interest"] = GenerateInterestBasedResponse,
                ["name"] = GenerateNameResponse
            };
        }

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
            _ui.TypeText("I can help with these cybersecurity topics:", 20);
            foreach (string topic in _knowledgeBase.GetAllTopics())
            {
                Console.WriteLine($"- {topic}");
            }
        }

        private void ProcessNaturalLanguage(string input)
        {
            string sentiment = DetectSentiment(input);
            List<string> keywords = ExtractKeywords(input);

            if (TryHandleInterestExpression(input, keywords)) return;

            var responseType = DetermineResponseType(input, keywords);
            string response = _responseHandlers[responseType](input, sentiment, keywords);

            // Format final response with sentiment and topic awareness
            response = FormatFinalResponse(response, sentiment, keywords);

            DisplayResponse(response);
        }

        private void DisplayResponse(string response)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("ChatBot: ");
            Console.ForegroundColor = ConsoleColor.Magenta;
            _ui.TypeText(response, 20);
            Console.ResetColor();
        }

        private string FormatFinalResponse(string baseResponse, string sentiment, List<string> keywords)
        {
            string finalResponse = baseResponse;

            // Add sentiment prefix if detected
            if (sentiment != "neutral")
            {
                string sentimentPrefix = GetSentimentResponse(sentiment);
                finalResponse = $"{sentimentPrefix}{finalResponse}";
            }

            // Check if this is a repeated topic
            foreach (string keyword in keywords)
            {
                if (_memory.GetTopicCount(keyword) > 1)
                {
                    string topicResponse = _knowledgeBase.GetResponse(keyword);
                    if (!string.IsNullOrEmpty(topicResponse))
                    {
                        finalResponse = _memory.GetInterestResponse(keyword, topicResponse);
                        break;
                    }
                }
            }

            return finalResponse;
        }

        private string DetectSentiment(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return "neutral";

            var sentimentMap = new Dictionary<string, string[]>
            {
                ["worried"] = new[] { "worried", "concerned", "scared", "nervous", "anxious" },
                ["positive"] = new[] { "happy", "excited", "great", "good", "awesome" },
                ["negative"] = new[] { "angry", "frustrated", "upset", "mad", "annoyed" },
                ["curious"] = new[] { "what", "how", "explain", "?", "tell me", "why" }
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
                    _memory.RememberInterest(topic);
                    string response = _knowledgeBase.GetResponse(topic);

                    DisplayResponse(response);
                    return true;
                }
            }
            return false;
        }

        private string DetermineResponseType(string input, List<string> keywords)
        {
            foreach (string keyword in keywords)
            {
                string response = _knowledgeBase.GetResponse(keyword);
                if (!string.IsNullOrEmpty(response) && _memory.IsCurrentInterest(keyword))
                {
                    return "interest";
                }
            }
            return "default";
        }

        private string GenerateDefaultResponse(string input, string sentiment, List<string> keywords)
        {
            var matchedTopics = keywords
                .Select(k => new { Key = k, Value = _knowledgeBase.GetResponse(k) })
                .Where(x => !string.IsNullOrEmpty(x.Value))
                .ToDictionary(x => x.Key, x => x.Value);

            if (matchedTopics.Count > 0)
            {
                return string.Join("\n", matchedTopics.Select(t => $"{t.Key.ToUpper()} >> {t.Value}"));
            }
            return "I'm not sure about that. Try 'help' for options.";
        }

        private string GenerateInterestBasedResponse(string input, string sentiment, List<string> keywords)
        {
            string baseResponse = keywords
                .Select(k => _knowledgeBase.GetResponse(k))
                .FirstOrDefault(r => !string.IsNullOrEmpty(r));

            return _memory.GetInterestResponse(_memory.UserInterest, baseResponse);
        }

        private string GenerateNameResponse(string input, string sentiment, List<string> keywords)
        {
            return _memory.OnNameRecall(_memory.UserName);
        }

        private string GetSentimentResponse(string sentiment)
        {
            switch (sentiment)
            {
                case "worried":
                    return "I understand this can be concerning. ";
                case "positive":
                    return "Great! I'm glad you're enthusiastic about security! ";
                case "negative":
                    return "I'm sorry you're feeling frustrated. Let me help. ";
                case "curious":
                    return "That's a great question! ";
                default:
                    return "";
            }
        }
    }
}