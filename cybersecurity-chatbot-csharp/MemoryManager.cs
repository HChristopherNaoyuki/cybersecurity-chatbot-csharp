using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace cybersecurity_chatbot_csharp
{
    /// <summary>
    /// Manages persistent keyword memory and contextual responses
    /// </summary>
    public class MemoryManager
    {
        private const string StorageFileName = "user_keywords.txt";
        private string _userName;
        private Dictionary<string, int> _keywordCounts = new Dictionary<string, int>();

        public delegate string NameRecallResponse(string name);

        public MemoryManager()
        {
            _userName = null;
            InitializeStorage();
            LoadPersistedData();
        }

        /// <summary>
        /// Personalized name recall response
        /// </summary>
        public NameRecallResponse OnNameRecall = (name) =>
            $"Your name is {name}. Have you forgotten?";

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
        /// Records a keyword and increments its count
        /// </summary>
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
        public int GetKeywordCount(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword)) return 0;
            return _keywordCounts.TryGetValue(NormalizeKeyword(keyword), out int count) ? count : 0;
        }

        /// <summary>
        /// Generates contextual response based on keyword history
        /// </summary>
        public string GetContextualResponse(string keyword, string baseResponse, int count)
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
        }

        private string NormalizeKeyword(string keyword)
        {
            return keyword.ToLower().Trim();
        }

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