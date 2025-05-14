using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace cybersecurity_chatbot_csharp
{
    /// <summary>
    /// Manages persistent keyword memory and contextual responses
    /// 
    /// Responsibilities:
    /// - Tracks keyword usage frequency with counts
    /// - Generates contextual responses based on discussion history
    /// - Persists keyword data to file for session-to-session memory
    /// - Handles personalized name recall
    /// - Identifies most frequently asked questions
    /// 
    /// Design Patterns:
    /// - Singleton pattern for memory persistence
    /// - Delegate pattern for response generation
    /// </summary>
    public class MemoryManager
    {
        // Constants
        private const string StorageFileName = "user_keywords.txt";

        // Delegate type definitions for response generation
        public delegate string ContextualResponseGenerator(string keyword, string baseResponse, int count);
        public delegate string NameRecallResponse(string name);
        public delegate string FrequentQuestionResponse(string keyword);

        // Fields
        private string _userName;
        private Dictionary<string, int> _keywordCounts = new Dictionary<string, int>();
        private readonly ContextualResponseGenerator _contextualResponseGenerator;

        /// <summary>
        /// Delegate for personalized name recall responses
        /// </summary>
        public NameRecallResponse OnNameRecall = (name) =>
            $"Your name is {name}. Have you forgotten?";

        /// <summary>
        /// Delegate for frequent question responses
        /// </summary>
        public FrequentQuestionResponse OnFrequentQuestion = (keyword) =>
            $"Your most frequently asked question (FAQ) is {keyword}.";

        /// <summary>
        /// Gets or sets the current user name with validation
        /// </summary>
        /// <exception cref="ArgumentException">Thrown when username is empty or whitespace</exception>
        public string UserName
        {
            get => _userName ?? throw new InvalidOperationException("Username not set");
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Username cannot be empty");
                _userName = value.Trim();
            }
        }

        /// <summary>
        /// Gets the most frequently asked keyword based on tracked counts
        /// </summary>
        /// <returns>The keyword with highest count, or null if no keywords tracked</returns>
        public string MostFrequentKeyword
        {
            get
            {
                if (_keywordCounts.Count == 0) return null;

                var maxPair = _keywordCounts.Aggregate((l, r) => l.Value > r.Value ? l : r);
                return maxPair.Key;
            }
        }

        /// <summary>
        /// Initializes a new MemoryManager instance
        /// - Sets up storage file
        /// - Loads persisted data
        /// - Initializes response generators
        /// </summary>
        public MemoryManager()
        {
            _userName = null;

            // Initialize contextual response generator with lambda
            _contextualResponseGenerator = (keyword, baseResponse, count) =>
            {
                var contextualPrefixes = new Dictionary<int, string[]>
                {
                    [2] = new[]
                    {
                        $"Since we discussed {keyword} before, ",
                        $"About {keyword} again, ",
                        $"Regarding {keyword}, "
                    },
                    [3] = new[]
                    {
                        $"As we've talked about {keyword} several times, ",
                        $"You seem interested in {keyword}, ",
                        $"Since you keep asking about {keyword}, "
                    },
                    [4] = new[]
                    {
                        $"You're really curious about {keyword}, ",
                        $"I notice you frequently ask about {keyword}, ",
                        $"You've asked about {keyword} {count} times now, "
                    }
                };

                // Find the closest matching tier
                int tier = contextualPrefixes.Keys
                    .Where(k => k <= count)
                    .DefaultIfEmpty(0)
                    .Max();

                if (tier > 0 && contextualPrefixes.TryGetValue(tier, out var prefixes))
                {
                    return prefixes[new Random().Next(prefixes.Length)] + baseResponse;
                }

                return baseResponse;
            };

            InitializeStorage();
            LoadPersistedData();
        }

        /// <summary>
        /// Records a keyword and increments its count
        /// </summary>
        /// <param name="keyword">The keyword to remember</param>
        public void RememberKeyword(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword)) return;

            string normalized = NormalizeKeyword(keyword);

            lock (_keywordCounts)
            {
                if (_keywordCounts.ContainsKey(normalized))
                {
                    _keywordCounts[normalized]++;
                }
                else
                {
                    _keywordCounts[normalized] = 1;
                }
            }

            PersistData();
        }

        /// <summary>
        /// Gets discussion count for a keyword
        /// </summary>
        /// <param name="keyword">The keyword to check</param>
        /// <returns>Number of times keyword was discussed</returns>
        public int GetKeywordCount(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword)) return 0;
            return _keywordCounts.TryGetValue(NormalizeKeyword(keyword), out int count) ? count : 0;
        }

        /// <summary>
        /// Generates contextual response based on keyword history
        /// </summary>
        /// <param name="keyword">Current keyword</param>
        /// <param name="baseResponse">Base response without context</param>
        /// <param name="count">Current discussion count</param>
        /// <returns>Contextualized response string</returns>
        public string GetContextualResponse(string keyword, string baseResponse, int count)
        {
            return _contextualResponseGenerator(keyword, baseResponse, count);
        }

        /// <summary>
        /// Gets the response for the most frequent question
        /// </summary>
        /// <returns>Formatted response about most frequent question</returns>
        public string GetFrequentQuestionResponse()
        {
            string keyword = MostFrequentKeyword;
            return keyword != null ? OnFrequentQuestion(keyword) :
                "You haven't asked enough questions yet to determine a frequent topic.";
        }

        /// <summary>
        /// Normalizes keywords to lowercase and trims whitespace
        /// </summary>
        /// <param name="keyword">Raw keyword</param>
        /// <returns>Normalized keyword</returns>
        private string NormalizeKeyword(string keyword)
        {
            return keyword.ToLower().Trim();
        }

        /// <summary>
        /// Initializes the storage file if it doesn't exist
        /// </summary>
        private void InitializeStorage()
        {
            try
            {
                if (!File.Exists(StorageFileName))
                {
                    File.Create(StorageFileName).Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Memory Error] Initialization failed: {ex.Message}");
            }
        }

        /// <summary>
        /// Loads persisted keyword data from file
        /// </summary>
        private void LoadPersistedData()
        {
            try
            {
                if (!File.Exists(StorageFileName)) return;

                foreach (string line in File.ReadAllLines(StorageFileName))
                {
                    var parts = line.Split(':');
                    if (parts.Length == 2 && int.TryParse(parts[1], out int count))
                    {
                        _keywordCounts[parts[0].ToLower()] = count;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Memory Error] Load failed: {ex.Message}");
            }
        }

        /// <summary>
        /// Saves current keyword counts to file
        /// </summary>
        private void PersistData()
        {
            try
            {
                var lines = _keywordCounts.Select(kvp => $"{kvp.Key}:{kvp.Value}");
                File.WriteAllLines(StorageFileName, lines);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Memory Error] Save failed: {ex.Message}");
            }
        }
    }
}