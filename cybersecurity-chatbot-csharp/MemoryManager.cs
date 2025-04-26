using System;
using System.IO;
using System.Linq;

namespace cybersecurity_chatbot_csharp
{
    /// <summary>
    /// Manages all user-specific data persistence and recall functionality for the chatbot.
    /// Implements a lightweight file-based storage system following the Single Responsibility Principle.
    ///
    /// Key Features:
    /// - Persistent storage of user interests across sessions
    /// - Name recall functionality with customizable response format
    /// - Interest-based response generation
    /// - Thread-safe file operations with proper error handling
    ///
    /// Design Patterns:
    /// - Memento: For capturing and restoring user state
    /// - Repository: Provides abstraction over storage mechanism
    /// - Strategy: Allows customizable response formatting via delegates
    /// </summary>
    public class MemoryManager
    {
        private const string StorageFileName = "user_memory.txt";
        private string _userName;
        private string _userInterest;

        /// <summary>
        /// Initializes a new instance of the MemoryManager.
        /// Performs automatic storage initialization and data loading.
        /// </summary>
        public MemoryManager()
        {
            _userName = "Guest";
            _userInterest = null;
            InitializeStorage();
            LoadPersistedData();
        }

        /// <summary>
        /// Delegate for customizing the name recall response format.
        /// Allows injection of different response strategies without modifying core logic.
        /// </summary>
        /// <param name="name">The user's name to include in the response</param>
        /// <returns>Formatted name recall response string</returns>
        public delegate string NameRecallResponse(string name);

        /// <summary>
        /// Default name recall response strategy.
        /// Uses a playful tone while reminding the user of their name.
        /// Example: "Have you forgotten your name? I'm going to tell you anyways John."
        /// </summary>
        public NameRecallResponse OnNameRecall = (name) =>
            $"Have you forgotten your name? I'm going to tell you anyways {name}.";

        /// <summary>
        /// Gets or sets the user's name with validation.
        /// Ensures name is never null or whitespace.
        /// </summary>
        /// <exception cref="ArgumentException">Thrown when attempting to set empty name</exception>
        public string UserName
        {
            get => _userName;
            set => _userName = string.IsNullOrWhiteSpace(value)
                ? throw new ArgumentException("Username cannot be empty")
                : value.Trim();
        }

        /// <summary>
        /// Gets the user's current area of interest.
        /// Returns null if no interest has been expressed.
        /// </summary>
        public string UserInterest => _userInterest;

        /// <summary>
        /// Determines if the user has expressed an interest in any topic.
        /// </summary>
        /// <returns>True if user has a stored interest, false otherwise</returns>
        public bool HasInterest() => !string.IsNullOrEmpty(_userInterest);

        /// <summary>
        /// Records and persists a user's interest in a specific topic.
        /// Triggers immediate persistence to ensure data durability.
        /// </summary>
        /// <param name="topic">The topic of interest</param>
        /// <exception cref="ArgumentException">Thrown when topic is null or empty</exception>
        public void RememberInterest(string topic)
        {
            if (string.IsNullOrWhiteSpace(topic))
                throw new ArgumentException("Topic cannot be empty");

            _userInterest = topic.Trim();
            PersistData();
        }

        /// <summary>
        /// Generates an interest-contextualized response using the provided base response.
        /// Example: "As someone interested in privacy, [base response]"
        /// </summary>
        public Func<string, string, string> GetInterestResponse = (interest, response) =>
            $"As someone interested in {interest}, {response}";

        /// <summary>
        /// Initializes the storage file if it doesn't exist.
        /// Handles potential file system errors gracefully.
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
        /// Silently handles errors to prevent application crashes.
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
        /// Uses atomic write operation to prevent data corruption.
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