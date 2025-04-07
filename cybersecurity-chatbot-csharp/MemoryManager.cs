using System;
using System.Collections;

namespace cybersecurity_chatbot_csharp
{
    /// <summary>
    /// Handles memory and recall functionality for the cybersecurity chatbot
    /// Specifically implements requirements from page 10 of the specification
    /// </summary>
    public class MemoryManager
    {
        // Stores user-provided information
        private string userName;
        private string primaryInterest;
        private DateTime interestTimestamp;

        // Stores conversation context
        private readonly ArrayList conversationContext;

        public MemoryManager()
        {
            conversationContext = new ArrayList();
        }

        /// <summary>
        /// Stores the user's name with validation
        /// </summary>
        public string UserName
        {
            get => !string.IsNullOrEmpty(userName) ? userName : "User";
            set => userName = ValidateName(value);
        }

        /// <summary>
        /// Records a user's cybersecurity interest based on specification example
        /// </summary>
        public void RecordInterest(string topic)
        {
            if (!string.IsNullOrWhiteSpace(topic))
            {
                primaryInterest = topic.ToLower().Trim();
                interestTimestamp = DateTime.Now;
                AddContext($"User expressed interest in {primaryInterest}");
            }
        }

        /// <summary>
        /// Gets the user's primary interest if recently mentioned
        /// </summary>
        public string GetActiveInterest()
        {
            // Interest expires after 30 minutes of conversation
            if (primaryInterest != null &&
                (DateTime.Now - interestTimestamp).TotalMinutes <= 30)
            {
                return primaryInterest;
            }
            return null;
        }

        /// <summary>
        /// Generates a personalized response incorporating remembered information
        /// </summary>
        public string GetPersonalizedResponse(string baseResponse)
        {
            string interest = GetActiveInterest();

            if (interest != null)
            {
                return $"As someone interested in {interest}, {baseResponse}";
            }
            return baseResponse;
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