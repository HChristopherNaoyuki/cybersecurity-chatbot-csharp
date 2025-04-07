using System;
using System.Collections;

namespace cybersecurity_chatbot_csharp
{
    /// <summary>
    /// Manages the chatbot's knowledge base including:
    /// - Topic-response pairs
    /// - Random responses for common queries
    /// - Ignored words for input processing
    /// </summary>
    public class KnowledgeBase
    {
        private readonly ArrayList knowledgeBase;
        private readonly ArrayList passwordResponses;
        private readonly ArrayList phishingResponses;
        private readonly ArrayList privacyResponses;
        private readonly ArrayList ignoreWords;
        private readonly Random random;

        public KnowledgeBase()
        {
            random = new Random();
            knowledgeBase = new ArrayList();
            passwordResponses = new ArrayList();
            phishingResponses = new ArrayList();
            privacyResponses = new ArrayList();
            ignoreWords = new ArrayList();

            // Initialize collections in safe order
            InitializeRandomResponses();  // Must be called first
            InitializeIgnoreWords();
            InitializeKnowledgeBase();
        }

        /// <summary>
        /// Gets a response for the given topic
        /// </summary>
        public string GetResponse(string topic)
        {
            if (string.IsNullOrEmpty(topic))
                return null;

            foreach (string[] entry in knowledgeBase)
            {
                if (entry != null && entry.Length >= 2 &&
                    entry[0].Equals(topic, StringComparison.OrdinalIgnoreCase))
                {
                    // Refresh random responses each time
                    if (topic.Equals("password", StringComparison.OrdinalIgnoreCase))
                        return GetRandomPasswordResponse();
                    if (topic.Equals("phishing", StringComparison.OrdinalIgnoreCase))
                        return GetRandomPhishingResponse();
                    if (topic.Equals("privacy", StringComparison.OrdinalIgnoreCase))
                        return GetRandomPrivacyResponse();

                    return entry[1];
                }
            }
            return null;
        }

        /// <summary>
        /// Checks if the word should be ignored during input processing
        /// </summary>
        public bool ShouldIgnoreWord(string word)
        {
            return !string.IsNullOrEmpty(word) && ignoreWords.Contains(word.ToLower());
        }

        /// <summary>
        /// Gets all available topics for help display
        /// </summary>
        public ArrayList GetAllTopics()
        {
            ArrayList topics = new ArrayList();
            foreach (string[] entry in knowledgeBase)
            {
                if (entry != null && entry.Length >= 1 &&
                    !entry[0].Equals("help", StringComparison.OrdinalIgnoreCase) &&
                    !entry[0].Equals("how are you", StringComparison.OrdinalIgnoreCase) &&
                    !entry[0].Equals("purpose", StringComparison.OrdinalIgnoreCase))
                {
                    topics.Add(entry[0]);
                }
            }
            return topics;
        }

        /// <summary>
        /// Initializes the main knowledge base with topic-response pairs
        /// </summary>
        private void InitializeKnowledgeBase()
        {
            // Add general responses
            AddKnowledgeEntry("how are you", "I'm functioning optimally! Ready to discuss cybersecurity.");
            AddKnowledgeEntry("purpose", "I provide cybersecurity education to help you stay safe online.");
            AddKnowledgeEntry("help", "I can explain: Passwords, 2FA, phishing, VPNs, Wi-Fi security, email safety");

            // Add topic responses with random variants
            AddKnowledgeEntry("password", GetRandomPasswordResponse());
            AddKnowledgeEntry("2fa", "Two-factor authentication adds security by requiring:\n1. Something you know (password)\n2. Something you have (phone/device)\nUse authenticator apps instead of SMS when possible");
            AddKnowledgeEntry("phishing", GetRandomPhishingResponse());
            AddKnowledgeEntry("vpn", "VPN benefits:\n- Encrypts all internet traffic\n- Essential on public Wi-Fi\n- Choose no-log providers\n- Doesn't provide complete anonymity");
            AddKnowledgeEntry("wifi", "Public Wi-Fi usage tips:\n- Avoid sensitive activities\n- Use a VPN\n- Disable file sharing\n- Turn off auto-connect");
            AddKnowledgeEntry("email", "Email safety tips:\n- Enable spam filters\n- Verify unusual requests\n- Don't open unexpected attachments");
            AddKnowledgeEntry("privacy", GetRandomPrivacyResponse());
        }

        /// <summary>
        /// Safely adds a knowledge entry with null checks
        /// </summary>
        private void AddKnowledgeEntry(string topic, string response)
        {
            if (!string.IsNullOrEmpty(topic) && !string.IsNullOrEmpty(response))
            {
                knowledgeBase.Add(new string[] { topic, response });
            }
        }

        /// <summary>
        /// Initializes multiple possible responses for common topics
        /// </summary>
        private void InitializeRandomResponses()
        {
            // Password responses
            AddResponse(passwordResponses, "Strong passwords should be at least 12 characters long and include a mix of uppercase, lowercase, numbers and symbols.");
            AddResponse(passwordResponses, "Consider using a passphrase instead of a password - something like 'PurpleElephant$JumpedOver42Clouds!'");
            AddResponse(passwordResponses, "Never reuse passwords across different accounts. Use a password manager to keep track of them all securely.");
            AddResponse(passwordResponses, "Change your passwords immediately if a service you use reports a data breach.");

            // Phishing responses
            AddResponse(phishingResponses, "Phishing emails often create a sense of urgency. Always verify unusual requests through another channel.");
            AddResponse(phishingResponses, "Check the sender's email address carefully - phishing attempts often use addresses that look similar to legitimate ones.");
            AddResponse(phishingResponses, "Hover over links before clicking to see the actual URL. If it looks suspicious, don't click!");
            AddResponse(phishingResponses, "Legitimate organizations will never ask for your password or sensitive information via email.");

            // Privacy responses
            AddResponse(privacyResponses, "Review privacy settings on all your social media accounts regularly - they often change their policies.");
            AddResponse(privacyResponses, "Be careful what personal information you share online - it can be used for social engineering attacks.");
            AddResponse(privacyResponses, "Consider using privacy-focused browsers and search engines that don't track your activity.");
            AddResponse(privacyResponses, "Use private browsing mode when accessing sensitive accounts on shared computers.");
        }

        /// <summary>
        /// Safely adds a response to a response list
        /// </summary>
        private void AddResponse(ArrayList list, string response)
        {
            if (list != null && !string.IsNullOrEmpty(response))
            {
                list.Add(response);
            }
        }

        /// <summary>
        /// Initializes the list of words to ignore during input processing
        /// </summary>
        private void InitializeIgnoreWords()
        {
            string[] wordsToIgnore = new string[]
            {
                "tell", "me", "about", "what", "is", "a", "the",
                "how", "do", "you", "explain", "your", "can"
            };

            foreach (string word in wordsToIgnore)
            {
                if (!string.IsNullOrEmpty(word))
                {
                    ignoreWords.Add(word.ToLower());
                }
            }
        }

        /// <summary>
        /// Gets a random password-related response with bounds checking
        /// </summary>
        private string GetRandomPasswordResponse()
        {
            return GetRandomResponse(passwordResponses);
        }

        /// <summary>
        /// Gets a random phishing-related response with bounds checking
        /// </summary>
        private string GetRandomPhishingResponse()
        {
            return GetRandomResponse(phishingResponses);
        }

        /// <summary>
        /// Gets a random privacy-related response with bounds checking
        /// </summary>
        private string GetRandomPrivacyResponse()
        {
            return GetRandomResponse(privacyResponses);
        }

        /// <summary>
        /// Generic method to get a random response from a list with safety checks
        /// </summary>
        private string GetRandomResponse(ArrayList responseList)
        {
            if (responseList == null || responseList.Count == 0)
                return "I don't have information about that topic.";

            int index = random.Next(responseList.Count);
            return responseList[index]?.ToString() ?? "Please ask about another topic.";
        }
    }
}