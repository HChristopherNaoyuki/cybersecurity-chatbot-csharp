using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace cybersecurity_chatbot_csharp
{
    /// <summary>
    /// Manages the chatbot's knowledge base and response generation system.
    /// Implements the Repository pattern for knowledge storage and retrieval.
    /// 
    /// Key Responsibilities:
    /// - Stores and retrieves topic-response pairs
    /// - Generates random responses for common queries
    /// - Filters ignored words during input processing
    /// - Provides topic discovery for help system
    /// 
    /// Design Patterns:
    /// - Repository: Centralized knowledge storage
    /// - Strategy: Different response generation approaches
    /// - Flyweight: Reuses common response objects
    /// </summary>
    public class KnowledgeBase
    {
        private readonly ArrayList _knowledgeBase;
        private readonly ArrayList _passwordResponses;
        private readonly ArrayList _phishingResponses;
        private readonly ArrayList _privacyResponses;
        private readonly ArrayList _ignoreWords;
        private readonly Random _random;

        /// <summary>
        /// Initializes a new KnowledgeBase with default cybersecurity content.
        /// Uses the Builder pattern internally for complex initialization.
        /// </summary>
        public KnowledgeBase()
        {
            _random = new Random();
            _knowledgeBase = new ArrayList();
            _passwordResponses = new ArrayList();
            _phishingResponses = new ArrayList();
            _privacyResponses = new ArrayList();
            _ignoreWords = new ArrayList();

            InitializeKnowledgeBase();
        }

        /// <summary>
        /// Gets a response for the specified topic.
        /// Implements the Strategy pattern for response generation.
        /// </summary>
        public string GetResponse(string topic)
        {
            if (string.IsNullOrEmpty(topic)) return null;

            foreach (string[] entry in _knowledgeBase)
            {
                if (entry != null && entry.Length >= 2 &&
                    entry[0].Equals(topic, StringComparison.OrdinalIgnoreCase))
                {
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
        /// Checks if a word should be ignored during input processing.
        /// </summary>
        public bool ShouldIgnoreWord(string word)
        {
            return !string.IsNullOrEmpty(word) && _ignoreWords.Contains(word);
        }

        /// <summary>
        /// Gets all available topics for help display.
        /// Filters out meta-topics like 'help' and 'purpose'.
        /// </summary>
        public ArrayList GetAllTopics()
        {
            ArrayList topics = new ArrayList();
            foreach (string[] entry in _knowledgeBase)
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

        private void InitializeKnowledgeBase()
        {
            InitializeRandomResponses();
            InitializeIgnoreWords();
            InitializeStaticResponses();
        }

        private void InitializeStaticResponses()
        {
            AddKnowledgeEntry("how are you", "I'm functioning optimally! Ready to discuss cybersecurity.");
            AddKnowledgeEntry("purpose", "I provide cybersecurity education to help you stay safe online.");
            AddKnowledgeEntry("help", "I can explain: Passwords, 2FA, phishing, VPNs, Wi-Fi security, email safety");

            AddKnowledgeEntry("password", GetRandomPasswordResponse());
            AddKnowledgeEntry("2fa", "Two-factor authentication adds security by requiring:\n1. Something you know (password)\n2. Something you have (phone/device)\nUse authenticator apps instead of SMS when possible");
            AddKnowledgeEntry("phishing", GetRandomPhishingResponse());
            AddKnowledgeEntry("vpn", "VPN benefits:\n- Encrypts all internet traffic\n- Essential on public Wi-Fi\n- Choose no-log providers\n- Doesn't provide complete anonymity");
            AddKnowledgeEntry("wifi", "Public Wi-Fi usage tips:\n- Avoid sensitive activities\n- Use a VPN\n- Disable file sharing\n- Turn off auto-connect");
            AddKnowledgeEntry("email", "Email safety tips:\n- Enable spam filters\n- Verify unusual requests\n- Don't open unexpected attachments");
            AddKnowledgeEntry("privacy", GetRandomPrivacyResponse());
        }

        private void AddKnowledgeEntry(string topic, string response)
        {
            if (!string.IsNullOrEmpty(topic) && !string.IsNullOrEmpty(response))
            {
                _knowledgeBase.Add(new string[] { topic, response });
            }
        }

        private void InitializeRandomResponses()
        {
            AddResponse(_passwordResponses, "Strong passwords should be at least 12 characters long and include a mix of uppercase, lowercase, numbers and symbols.");
            AddResponse(_passwordResponses, "Consider using a passphrase instead of a password - something like 'PurpleElephant$JumpedOver42Clouds!'");
            AddResponse(_passwordResponses, "Never reuse passwords across different accounts. Use a password manager to keep track of them all securely.");
            AddResponse(_passwordResponses, "Change your passwords immediately if a service you use reports a data breach.");

            AddResponse(_phishingResponses, "Phishing emails often create a sense of urgency. Always verify unusual requests through another channel.");
            AddResponse(_phishingResponses, "Check the sender's email address carefully - phishing attempts often use addresses that look similar to legitimate ones.");
            AddResponse(_phishingResponses, "Hover over links before clicking to see the actual URL. If it looks suspicious, don't click!");
            AddResponse(_phishingResponses, "Legitimate organizations will never ask for your password or sensitive information via email.");

            AddResponse(_privacyResponses, "Review privacy settings on all your social media accounts regularly - they often change their policies.");
            AddResponse(_privacyResponses, "Be careful what personal information you share online - it can be used for social engineering attacks.");
            AddResponse(_privacyResponses, "Consider using privacy-focused browsers and search engines that don't track your activity.");
            AddResponse(_privacyResponses, "Use private browsing mode when accessing sensitive accounts on shared computers.");
        }

        private void AddResponse(ArrayList list, string response)
        {
            if (list != null && !string.IsNullOrEmpty(response))
            {
                list.Add(response);
            }
        }

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
                    _ignoreWords.Add(word.ToLower());
                }
            }
        }

        private string GetRandomPasswordResponse()
        {
            return GetRandomResponse(_passwordResponses);
        }

        private string GetRandomPhishingResponse()
        {
            return GetRandomResponse(_phishingResponses);
        }

        private string GetRandomPrivacyResponse()
        {
            return GetRandomResponse(_privacyResponses);
        }

        private string GetRandomResponse(ArrayList responseList)
        {
            if (responseList == null || responseList.Count == 0)
                return "I don't have information about that topic.";

            int index = _random.Next(responseList.Count);
            return responseList[index]?.ToString() ?? "Please ask about another topic.";
        }
    }
}