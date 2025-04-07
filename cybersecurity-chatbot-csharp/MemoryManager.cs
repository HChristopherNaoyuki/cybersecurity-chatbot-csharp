using System;
using System.Collections;

namespace cybersecurity_chatbot_csharp
{
    /// <summary>
    /// Handles memory and recall functionality for the cybersecurity chatbot
    /// Implements all required methods from the specification
    /// </summary>
    public class MemoryManager
    {
        private string userName;
        private string userInterest;
        private DateTime interestTimestamp;
        private readonly ArrayList conversationContext;

        public MemoryManager()
        {
            conversationContext = new ArrayList();
        }

        /// <summary>
        /// Gets or sets the user's name with validation
        /// </summary>
        public string UserName
        {
            get => !string.IsNullOrEmpty(userName) ? userName : "User";
            set => userName = ValidateName(value);
        }

        /// <summary>
        /// Gets the user's current interest topic
        /// </summary>
        public string UserInterest => userInterest;

        /// <summary>
        /// Checks if the user has an active interest
        /// </summary>
        public bool HasInterest()
        {
            return !string.IsNullOrEmpty(userInterest) &&
                   (DateTime.Now - interestTimestamp).TotalMinutes <= 30;
        }

        /// <summary>
        /// Records a user's cybersecurity interest (implements specification requirement)
        /// </summary>
        public void RememberInterest(string topic)
        {
            if (!string.IsNullOrWhiteSpace(topic))
            {
                userInterest = topic.ToLower().Trim();
                interestTimestamp = DateTime.Now;
                AddContext($"User expressed interest in {userInterest}");
            }
        }

        /// <summary>
        /// Generates a personalized response incorporating remembered information
        /// </summary>
        public string GetPersonalizedResponse(string baseResponse)
        {
            return HasInterest()
                ? $"As someone interested in {userInterest}, {baseResponse}"
                : baseResponse;
        }

        /// <summary>
        /// Adds context to the current conversation
        /// </summary>
        public void AddContext(string contextNote)
        {
            if (!string.IsNullOrWhiteSpace(contextNote))
            {
                conversationContext.Add($"[{DateTime.Now:HH:mm}] {contextNote}");

                // Keep only the last 5 context items
                while (conversationContext.Count > 5)
                {
                    conversationContext.RemoveAt(0);
                }
            }
        }

        /// <summary>
        /// Gets recent conversation context
        /// </summary>
        public ArrayList GetRecentContext()
        {
            return new ArrayList(conversationContext); // Return copy
        }

        private string ValidateName(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return null;

            // Allow only letters and spaces
            char[] cleaned = Array.FindAll(name.ToCharArray(),
                c => char.IsLetter(c) || char.IsWhiteSpace(c));
            return new string(cleaned).Trim();
        }
    }
}