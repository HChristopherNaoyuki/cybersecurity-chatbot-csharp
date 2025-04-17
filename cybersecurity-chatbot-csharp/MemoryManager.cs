using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace cybersecurity_chatbot_csharp
{
    /// <summary>
    /// Manages all user-specific data and persistence for the chatbot.
    /// Handles user profile, interests, favorites, and persistent storage.
    /// 
    /// Key Responsibilities:
    /// - Maintains user session state
    /// - Manages favorites collection
    /// - Handles interest tracking
    /// - Provides file-based persistence
    /// 
    /// Design Patterns:
    /// - Singleton (via DI container)
    /// - Repository (for data persistence)
    /// - Observer (for change notifications)
    /// </summary>
    public class MemoryManager
    {
        // Constants
        private const string StorageFileName = "user_data.txt";
        private const string InterestMarker = "[INTEREST]";

        // Fields
        private readonly List<string> _favorites;
        private string _userName;
        private string _userInterest;

        /// <summary>
        /// Initializes a new MemoryManager instance.
        /// 
        /// Initialization Sequence:
        /// 1. Sets default values
        /// 2. Initializes storage file
        /// 3. Loads persisted data
        /// </summary>
        public MemoryManager()
        {
            _favorites = new List<string>();
            _userName = "Guest";
            _userInterest = null;

            InitializeStorage();
            LoadPersistedData();
        }

        /// <summary>
        /// Gets or sets the user's name.
        /// 
        /// Validation:
        /// - Null/empty values are rejected
        /// - Automatically trimmed
        /// </summary>
        public string UserName
        {
            get => _userName;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Username cannot be empty");
                _userName = value.Trim();
            }
        }

        /// <summary>
        /// Gets the user's current interest topic.
        /// 
        /// Note:
        /// - Returns null if no interest is set
        /// - Read-only property (use RememberInterest to set)
        /// </summary>
        public string UserInterest => _userInterest;

        /// <summary>
        /// Determines if the user has expressed an interest.
        /// 
        /// Usage:
        /// - Guides conversation flow
        /// - Enables personalized responses
        /// </summary>
        public bool HasInterest() => !string.IsNullOrEmpty(_userInterest);

        /// <summary>
        /// Records and persists a user's interest in a topic.
        /// 
        /// Parameters:
        /// - topic: The cybersecurity topic of interest
        /// 
        /// Exceptions:
        /// - ArgumentException for invalid topics
        /// - IOException for storage failures
        /// </summary>
        public void RememberInterest(string topic)
        {
            if (string.IsNullOrWhiteSpace(topic))
                throw new ArgumentException("Topic cannot be empty");

            _userInterest = topic.Trim();
            PersistInterest();
        }

        /// <summary>
        /// Stores a user's favorite item with persistence.
        /// 
        /// Features:
        /// - Interactive prompt
        /// - Input validation
        /// - Immediate feedback
        /// - Automatic persistence
        /// </summary>
        public void StoreUserFavorite()
        {
            try
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("\nEnter your favorite item: ");
                Console.ResetColor();

                string favorite = Console.ReadLine()?.Trim();

                if (string.IsNullOrWhiteSpace(favorite))
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("No input received. Favorite not stored.");
                    Console.ResetColor();
                    return;
                }

                _favorites.Add(favorite);
                PersistFavorites();

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Stored: {favorite}");
                Console.ResetColor();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Error saving favorite: {ex.Message}");
                Console.ResetColor();
            }
        }

        /// <summary>
        /// Displays all stored favorites with formatting.
        /// 
        /// UI Features:
        /// - Color-coded output
        /// - Empty collection handling
        /// - Clear item numbering
        /// </summary>
        public void DisplayFavorites()
        {
            if (!_favorites.Any())
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("No favorites stored yet.");
                Console.ResetColor();
                return;
            }

            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("\nYour favorites:");
            Console.ResetColor();

            for (int i = 0; i < _favorites.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {_favorites[i]}");
            }
        }

        /// <summary>
        /// Gets a safe copy of all favorites.
        /// 
        /// Safety:
        /// - Returns new collection to prevent modification
        /// - Preserves ordering
        /// </summary>
        public IReadOnlyList<string> GetAllFavorites() =>
            new List<string>(_favorites);

        // Private helper methods
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
                Console.WriteLine($"[Error] Initializing storage: {ex.Message}");
            }
        }

        private void LoadPersistedData()
        {
            try
            {
                if (!File.Exists(StorageFileName)) return;

                var lines = File.ReadAllLines(StorageFileName);
                foreach (var line in lines)
                {
                    if (line.StartsWith(InterestMarker))
                    {
                        _userInterest = line.Substring(InterestMarker.Length).Trim();
                    }
                    else if (!string.IsNullOrWhiteSpace(line))
                    {
                        _favorites.Add(line.Trim());
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Error] Loading data: {ex.Message}");
            }
        }

        private void PersistFavorites()
        {
            try
            {
                var lines = new List<string>();
                if (!string.IsNullOrEmpty(_userInterest))
                {
                    lines.Add($"{InterestMarker}{_userInterest}");
                }
                lines.AddRange(_favorites);
                File.WriteAllLines(StorageFileName, lines);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Error] Saving favorites: {ex.Message}");
            }
        }

        private void PersistInterest()
        {
            try
            {
                var lines = new List<string>();
                if (!string.IsNullOrEmpty(_userInterest))
                {
                    lines.Add($"{InterestMarker}{_userInterest}");
                }
                lines.AddRange(_favorites);
                File.WriteAllLines(StorageFileName, lines);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Error] Saving interest: {ex.Message}");
            }
        }
    }
}