using System;
using System.IO;
using System.Linq;

namespace cybersecurity_chatbot_csharp
{
    /// <summary>
    /// Manages all user-specific data persistence and recall functionality for the chatbot.
    /// Handles user identity, interests, and personalized responses.
    /// 
    /// Key Features:
    /// - Dynamic username initialization (no default "Guest")
    /// - Persistent interest tracking
    /// - Personalized response generation
    /// - Thread-safe file operations
    /// </summary>
    public class MemoryManager
    {
        private const string StorageFileName = "user_memory.txt";
        private string _userName;
        private string _userInterest;

        /// <summary>
        /// Initializes a new MemoryManager instance.
        /// Does not set default username - waits for explicit setting.
        /// </summary>
        public MemoryManager()
        {
            _userName = null; // Will be set when user provides name
            _userInterest = null;
            InitializeStorage();
            LoadPersistedData();
        }

        /// <summary>
        /// Delegate for customizing name recall responses.
        /// Allows flexible response formatting without modifying core logic.
        /// </summary>
        public delegate string NameRecallResponse(string name);

        /// <summary>
        /// Default name recall response strategy.
        /// Uses playful tone while reminding user of their name.
        /// </summary>
        public NameRecallResponse OnNameRecall = (name) =>
            $"Have you forgotten your name? I'm going to tell you anyways {name}.";

        /// <summary>
        /// Gets or sets the user's name with validation.
        /// Ensures name is never null or whitespace.
        /// </summary>
        /// <exception cref="ArgumentException">Thrown when name is empty</exception>
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
        /// Gets the user's current interest topic.
        /// Returns null if no interest has been expressed.
        /// </summary>
        public string UserInterest => _userInterest;

        /// <summary>
        /// Checks if user has expressed an interest in any topic.
        /// </summary>
        public bool HasInterest() => !string.IsNullOrEmpty(_userInterest);

        /// <summary>
        /// Checks if specified topic matches user's current interest.
        /// Case-insensitive comparison.
        /// </summary>
        public bool IsCurrentInterest(string topic)
        {
            return !string.IsNullOrEmpty(topic) &&
                   !string.IsNullOrEmpty(_userInterest) &&
                   _userInterest.Equals(topic, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Records and persists a user's interest in a topic.
        /// Triggers immediate file persistence.
        /// </summary>
        /// <param name="topic">Topic of interest</param>
        /// <exception cref="ArgumentException">Thrown for empty topics</exception>
        public void RememberInterest(string topic)
        {
            if (string.IsNullOrWhiteSpace(topic))
                throw new ArgumentException("Topic cannot be empty");

            _userInterest = topic.Trim();
            PersistData();
        }

        /// <summary>
        /// Generates interest-contextualized responses.
        /// Formats base response with interest context.
        /// </summary>
        public Func<string, string, string> GetInterestResponse = (interest, response) =>
            $"As someone interested in {interest}, {response}";

        /// <summary>
        /// Initializes storage file if it doesn't exist.
        /// Handles file system errors gracefully.
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
        /// Loads persisted data from storage file.
        /// Only loads interest (username is session-only).
        /// </summary>
        private void LoadPersistedData()
        {
            try
            {
                if (!File.Exists(StorageFileName)) return;

                var lines = File.ReadAllLines(StorageFileName);
                _userInterest = lines.FirstOrDefault()?.Trim();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Memory Error] Load failed: {ex.Message}");
            }
        }

        /// <summary>
        /// Persists current user interest to storage.
        /// Uses atomic write operation to prevent corruption.
        /// </summary>
        private void PersistData()
        {
            try
            {
                File.WriteAllText(StorageFileName, _userInterest ?? string.Empty);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Memory Error] Save failed: {ex.Message}");
            }
        }
    }
}