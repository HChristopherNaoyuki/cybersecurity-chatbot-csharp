using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace cybersecurity_chatbot_csharp
{
    /// <summary>
    /// Enhanced knowledge base with more response variations
    /// and improved topic coverage
    /// </summary>
    public class KnowledgeBase
    {
        private readonly ArrayList _knowledgeBase;
        private readonly ArrayList _passwordResponses;
        private readonly ArrayList _phishingResponses;
        private readonly ArrayList _privacyResponses;
        private readonly ArrayList _vpnResponses;
        private readonly ArrayList _wifiResponses;
        private readonly ArrayList _emailResponses;
        private readonly ArrayList _2faResponses;
        private readonly ArrayList _ignoreWords;
        private readonly Random _random;

        public KnowledgeBase()
        {
            _random = new Random();
            _knowledgeBase = new ArrayList();
            _passwordResponses = new ArrayList();
            _phishingResponses = new ArrayList();
            _privacyResponses = new ArrayList();
            _vpnResponses = new ArrayList();
            _wifiResponses = new ArrayList();
            _emailResponses = new ArrayList();
            _2faResponses = new ArrayList();
            _ignoreWords = new ArrayList();

            InitializeKnowledgeBase();
        }

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
                    if (topic.Equals("vpn", StringComparison.OrdinalIgnoreCase))
                        return GetRandomVPNResponse();
                    if (topic.Equals("wifi", StringComparison.OrdinalIgnoreCase))
                        return GetRandomWifiResponse();
                    if (topic.Equals("email", StringComparison.OrdinalIgnoreCase))
                        return GetRandomEmailResponse();
                    if (topic.Equals("2fa", StringComparison.OrdinalIgnoreCase))
                        return GetRandom2FAResponse();

                    return entry[1];
                }
            }
            return null;
        }

        public bool ShouldIgnoreWord(string word)
        {
            return !string.IsNullOrEmpty(word) && _ignoreWords.Contains(word);
        }

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
            AddKnowledgeEntry("2fa", GetRandom2FAResponse());
            AddKnowledgeEntry("phishing", GetRandomPhishingResponse());
            AddKnowledgeEntry("vpn", GetRandomVPNResponse());
            AddKnowledgeEntry("wifi", GetRandomWifiResponse());
            AddKnowledgeEntry("email", GetRandomEmailResponse());
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
            // Password responses
            AddResponse(_passwordResponses, "Strong passwords should be at least 12 characters long and include a mix of uppercase, lowercase, numbers and symbols.");
            AddResponse(_passwordResponses, "Consider using a passphrase instead of a password - something like 'PurpleElephant$JumpedOver42Clouds!'");
            AddResponse(_passwordResponses, "Never reuse passwords across different accounts. Use a password manager to keep track of them all securely.");
            AddResponse(_passwordResponses, "Change your passwords immediately if a service you use reports a data breach.");
            AddResponse(_passwordResponses, "The strongest passwords are long, random, and unique to each account.");

            // 2FA responses
            AddResponse(_2faResponses, "Two-factor authentication adds security by requiring:\n1. Something you know (password)\n2. Something you have (phone/device)\nUse authenticator apps instead of SMS when possible");
            AddResponse(_2faResponses, "2FA protects you even if your password is compromised. Always enable it for important accounts!");
            AddResponse(_2faResponses, "Authenticator apps like Google Authenticator or Authy provide more secure 2FA than SMS codes.");
            AddResponse(_2faResponses, "Security keys (like YubiKey) provide the strongest form of two-factor authentication.");

            // Phishing responses
            AddResponse(_phishingResponses, "Phishing emails often create a sense of urgency. Always verify unusual requests through another channel.");
            AddResponse(_phishingResponses, "Check the sender's email address carefully - phishing attempts often use addresses that look similar to legitimate ones.");
            AddResponse(_phishingResponses, "Hover over links before clicking to see the actual URL. If it looks suspicious, don't click!");
            AddResponse(_phishingResponses, "Legitimate organizations will never ask for your password or sensitive information via email.");

            // Privacy responses
            AddResponse(_privacyResponses, "Review privacy settings on all your social media accounts regularly - they often change their policies.");
            AddResponse(_privacyResponses, "Be careful what personal information you share online - it can be used for social engineering attacks.");
            AddResponse(_privacyResponses, "Consider using privacy-focused browsers and search engines that don't track your activity.");
            AddResponse(_privacyResponses, "Use private browsing mode when accessing sensitive accounts on shared computers.");

            // VPN responses
            AddResponse(_vpnResponses, "VPN benefits:\n- Encrypts all internet traffic\n- Essential on public Wi-Fi\n- Choose no-log providers\n- Doesn't provide complete anonymity");
            AddResponse(_vpnResponses, "A good VPN should have:\n- Strong encryption\n- No-logs policy\n- Fast servers\n- Reliable connection");
            AddResponse(_vpnResponses, "While VPNs enhance privacy, they don't make you completely anonymous online.");

            // WiFi responses
            AddResponse(_wifiResponses, "Public Wi-Fi usage tips:\n- Avoid sensitive activities\n- Use a VPN\n- Disable file sharing\n- Turn off auto-connect");
            AddResponse(_wifiResponses, "Home WiFi security tips:\n- Use WPA3 encryption\n- Change default admin password\n- Disable WPS\n- Create guest network");
            AddResponse(_wifiResponses, "Never conduct banking or shopping on public WiFi without a VPN.");

            // Email responses
            AddResponse(_emailResponses, "Email safety tips:\n- Enable spam filters\n- Verify unusual requests\n- Don't open unexpected attachments");
            AddResponse(_emailResponses, "Watch for these email red flags:\n- Urgent requests for action\n- Poor grammar/spelling\n- Suspicious attachments\n- Requests for credentials");
            AddResponse(_emailResponses, "Use separate email accounts for important services like banking versus casual signups.");
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
                "tell", "me", "about", "a", "the", "an",
                "do", "explain", "can",
                "what", "is", "does", "could", "would",
                "should", "will", "please", "thanks", "thank"
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

        private string GetRandomVPNResponse()
        {
            return GetRandomResponse(_vpnResponses);
        }

        private string GetRandomWifiResponse()
        {
            return GetRandomResponse(_wifiResponses);
        }

        private string GetRandomEmailResponse()
        {
            return GetRandomResponse(_emailResponses);
        }

        private string GetRandom2FAResponse()
        {
            return GetRandomResponse(_2faResponses);
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