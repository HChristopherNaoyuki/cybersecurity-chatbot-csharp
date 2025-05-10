using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace cybersecurity_chatbot_csharp
{
    /// <summary>
    /// Manages all user-specific data persistence and recall functionality
    /// Now includes topic frequency tracking and enhanced name recall
    /// </summary>
    public class MemoryManager
    {
        private const string StorageFileName = "user_memory.txt";
        private string _userName;
        private string _userInterest;
        private Dictionary<string, int> _topicCounts = new Dictionary<string, int>();

        public delegate string NameRecallResponse(string name);

        public MemoryManager()
        {
            _userName = null;
            _userInterest = null;
            InitializeStorage();
            LoadPersistedData();
        }

        /// <summary>
        /// Updated name recall response with more personality
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

        public string UserInterest => _userInterest;

        public bool HasInterest() => !string.IsNullOrEmpty(_userInterest);

        public bool IsCurrentInterest(string topic)
        {
            return !string.IsNullOrEmpty(topic) &&
                   !string.IsNullOrEmpty(_userInterest) &&
                   _userInterest.Equals(topic, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Records interest and increments topic count
        /// </summary>
        public void RememberInterest(string topic)
        {
            if (string.IsNullOrWhiteSpace(topic))
                throw new ArgumentException("Topic cannot be empty");

            _userInterest = topic.Trim();
            IncrementTopicCount(topic);
            PersistData();
        }

        /// <summary>
        /// Tracks how many times each topic has been discussed
        /// </summary>
        public void IncrementTopicCount(string topic)
        {
            if (string.IsNullOrWhiteSpace(topic)) return;

            string normalizedTopic = topic.ToLower().Trim();

            if (_topicCounts.ContainsKey(normalizedTopic))
            {
                _topicCounts[normalizedTopic]++;
            }
            else
            {
                _topicCounts[normalizedTopic] = 1;
            }
        }

        /// <summary>
        /// Gets the count for a specific topic
        /// </summary>
        public int GetTopicCount(string topic)
        {
            if (string.IsNullOrWhiteSpace(topic)) return 0;
            string normalizedTopic = topic.ToLower().Trim();
            return _topicCounts.TryGetValue(normalizedTopic, out int count) ? count : 0;
        }

        /// <summary>
        /// Gets all topic counts for reporting
        /// </summary>
        public Dictionary<string, int> GetAllTopicCounts()
        {
            return new Dictionary<string, int>(_topicCounts);
        }

        /// <summary>
        /// Updated to include contextual responses based on interest history
        /// </summary>
        public Func<string, string, string> GetInterestResponse = (interest, response) =>
        {
            string[] possibleResponses = {
                $"As someone interested in {interest}, {response}",
                $"Since you've asked about {interest} before, {response}",
                $"Given your interest in {interest}, {response}",
                $"I remember you're curious about {interest}. {response}"
            };
            return possibleResponses[new Random().Next(possibleResponses.Length)];
        };

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

                var lines = File.ReadAllLines(StorageFileName);
                if (lines.Length > 0)
                {
                    _userInterest = lines[0]?.Trim();
                }
                if (lines.Length > 1)
                {
                    // Load topic counts from remaining lines
                    foreach (var line in lines.Skip(1))
                    {
                        var parts = line.Split(':');
                        if (parts.Length == 2 && int.TryParse(parts[1], out int count))
                        {
                            _topicCounts[parts[0].ToLower()] = count;
                        }
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
                var lines = new List<string>();
                lines.Add(_userInterest ?? string.Empty);

                // Save topic counts in format "topic:count"
                foreach (var kvp in _topicCounts)
                {
                    lines.Add($"{kvp.Key}:{kvp.Value}");
                }

                File.WriteAllLines(StorageFileName, lines);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Memory Error] Save failed: {ex.Message}");
            }
        }
    }
}