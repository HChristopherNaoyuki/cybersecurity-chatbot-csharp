using System;
using System.Media;
using System.Threading;

namespace cybersecurity_chatbot_csharp
{
    class Program
    {
        static void Main(string[] args)
        {
            // Task 1: Voice Greeting
            PlayVoiceGreeting();

            // Task 2: Image Display
            DisplayAsciiArt();

            // Task 3: Text-Based Greeting and User Interaction
            string userName = GetUserName();
            DisplayWelcomeMessage(userName);

            // Task 4: Basic Response System
            StartChat(userName);

            // Task 5: Input Validation is handled within the StartChat method
        }

        // Method to play a voice greeting
        static void PlayVoiceGreeting()
        {
            try
            {
                // Load and play the WAV file
                SoundPlayer player = new SoundPlayer("Audio\\welcome.wav");
                player.PlaySync(); // Play synchronously
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error playing voice greeting: " + ex.Message);
                Console.ResetColor();
            }
        }

        // Method to display ASCII art
        static void DisplayAsciiArt()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(@"
 _____         _                                                _  _            _____  _             _    _             _   
/  __ \       | |                                              (_)| |          /  __ \| |           | |  | |           | |  
| /  \/ _   _ | |__    ___  _ __  ___   ___   ___  _   _  _ __  _ | |_  _   _  | /  \/| |__    __ _ | |_ | |__    ___  | |_ 
| |    | | | || '_ \  / _ \| '__|/ __| / _ \ / __|| | | || '__|| || __|| | | | | |    | '_ \  / _` || __|| '_ \  / _ \ | __|
| \__/\| |_| || |_) ||  __/| |   \__ \|  __/| (__ | |_| || |   | || |_ | |_| | | \__/\| | | || (_| || |_ | |_) || (_) || |_ 
 \____/ \__, ||_.__/  \___||_|   |___/ \___| \___| \__,_||_|   |_| \__| \__, |  \____/|_| |_| \__,_| \__||_.__/  \___/  \__|
         __/ |                                                           __/ |                                              
        |___/                                                           |___/                                               
            ");
            Console.ResetColor();
        }

        // Method to get the user's name
        static string GetUserName()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("Enter your name: ");
            Console.ResetColor();
            string userName = Console.ReadLine();

            // Input validation for empty name
            while (string.IsNullOrWhiteSpace(userName))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Name cannot be empty. Please enter your name:");
                Console.ResetColor();
                userName = Console.ReadLine();
            }

            return userName;
        }

        // Method to display a welcome message
        static void DisplayWelcomeMessage(string userName)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\nHello, {userName}! Welcome to the Cybersecurity Awareness Bot.");
            Console.WriteLine("I'm here to help you stay safe online.\n");
            Console.ResetColor();
        }

        // Method to simulate typing effect
        static void TypeText(string text, int delay = 50)
        {
            foreach (char c in text)
            {
                Console.Write(c);
                Thread.Sleep(delay); // Simulate typing delay
            }
            Console.WriteLine(); // Move to the next line after typing
        }

        // Method to start the chat
        static void StartChat(string userName)
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            TypeText("You can ask me about:\n- Passwords\n- Phishing\n- Safe browsing\n", 30);
            Console.ResetColor();

            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write($"{userName}, what would you like to know about? (Type 'exit' to quit): ");
                Console.ResetColor();
                string userInput = Console.ReadLine().ToLower();

                // Task 5: Input Validation
                if (string.IsNullOrWhiteSpace(userInput))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    TypeText("I didn’t quite understand that. Could you rephrase?", 30);
                    Console.ResetColor();
                    continue;
                }

                if (userInput == "exit")
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    TypeText("Goodbye! Stay safe online!", 30);
                    Console.ResetColor();
                    break;
                }

                // Task 4: Basic Response System
                switch (userInput)
                {
                    // General Questions
                    case "how are you?":
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        TypeText("I'm just a bot, but I'm here to help you stay safe online!", 30);
                        Console.ResetColor();
                        break;

                    case "what's your purpose?":
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        TypeText("My purpose is to educate you about cybersecurity and help you stay safe online.", 30);
                        Console.ResetColor();
                        break;

                    case "what can i ask you about?":
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        TypeText("You can ask me about:\n- Passwords\n- Phishing\n- Safe browsing\n- General questions", 30);
                        Console.ResetColor();
                        break;

                    // Passwords
                    case "how do i create a strong password?":
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        TypeText("A strong password should be at least 12 characters long, include a mix of letters, numbers, and symbols, and avoid common words or phrases.", 30);
                        Console.ResetColor();
                        break;

                    case "should i use the same password for multiple sites?":
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        TypeText("No, you should use unique passwords for each site. If one account is compromised, others will remain secure.", 30);
                        Console.ResetColor();
                        break;

                    case "what is two-factor authentication (2fa) and why should i use it?":
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        TypeText("Two-factor authentication adds an extra layer of security by requiring a second form of verification, such as a code sent to your phone. It helps protect your accounts even if your password is stolen.", 30);
                        Console.ResetColor();
                        break;

                    case "how can i securely store my passwords?":
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        TypeText("Use a password manager to securely store and generate strong passwords. Avoid writing them down or saving them in unencrypted files.", 30);
                        Console.ResetColor();
                        break;

                    case "how do i know if my password has been leaked?":
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        TypeText("You can check if your password has been leaked using websites like 'Have I Been Pwned'. If it has, change it immediately.", 30);
                        Console.ResetColor();
                        break;

                    case "what should i do if i forget my password?":
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        TypeText("Use the 'Forgot Password' feature on the website to reset your password. Make sure to create a strong, unique password.", 30);
                        Console.ResetColor();
                        break;

                    // Phishing
                    case "what is phishing?":
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        TypeText("Phishing is a type of cyberattack where attackers trick you into revealing sensitive information, such as passwords or credit card numbers, by pretending to be a trusted entity.", 30);
                        Console.ResetColor();
                        break;

                    case "how can i spot a phishing email?":
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        TypeText("Look for suspicious sender addresses, poor grammar, urgent requests for personal information, and links that don't match the sender's claimed identity.", 30);
                        Console.ResetColor();
                        break;

                    case "what should i do if i think i’ve received a phishing email?":
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        TypeText("Do not click on any links or download attachments. Report the email to your IT department or email provider.", 30);
                        Console.ResetColor();
                        break;

                    case "is it safe to click on links in an email from an unknown sender?":
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        TypeText("No, it is not safe. Links in emails from unknown senders could lead to malicious websites or trigger malware downloads.", 30);
                        Console.ResetColor();
                        break;

                    case "what is spear phishing?":
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        TypeText("Spear phishing is a targeted form of phishing where attackers customize their messages to trick a specific individual or organization.", 30);
                        Console.ResetColor();
                        break;

                    // Safe Browsing
                    case "how can i browse the internet safely?":
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        TypeText("Use a secure browser, enable HTTPS, avoid suspicious websites, and keep your software up to date.", 30);
                        Console.ResetColor();
                        break;

                    case "what is https and why is it important?":
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        TypeText("HTTPS encrypts data between your browser and the website, protecting it from being intercepted by attackers. Always look for 'https://' in the URL.", 30);
                        Console.ResetColor();
                        break;

                    case "what is a vpn and how does it help with safe browsing?":
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        TypeText("A VPN (Virtual Private Network) encrypts your internet connection, making it harder for attackers to intercept your data. It also hides your IP address.", 30);
                        Console.ResetColor();
                        break;

                    case "why should i avoid public wi-fi for sensitive transactions?":
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        TypeText("Public Wi-Fi networks are often unsecured, making it easy for attackers to intercept your data. Use a VPN if you must use public Wi-Fi.", 30);
                        Console.ResetColor();
                        break;

                    case "how do i recognize a fake website?":
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        TypeText("Check for misspellings in the URL, look for HTTPS, and verify the website's legitimacy by searching for reviews or contacting the organization directly.", 30);
                        Console.ResetColor();
                        break;

                    // Common Questions People Ask Chatbots
                    case "what can you do?":
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        TypeText("I can help you learn about cybersecurity topics like passwords, phishing, and safe browsing. Ask me anything!", 30);
                        Console.ResetColor();
                        break;

                    case "how do you know so much?":
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        TypeText("I'm programmed with a lot of information about cybersecurity to help you stay safe online.", 30);
                        Console.ResetColor();
                        break;

                    case "are you a human?":
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        TypeText("No, I'm a chatbot designed to assist you with cybersecurity questions.", 30);
                        Console.ResetColor();
                        break;

                    case "can you remember previous conversations?":
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        TypeText("No, I don't have memory of previous conversations. Each session is independent.", 30);
                        Console.ResetColor();
                        break;

                    case "how can i get the best answers from you?":
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        TypeText("Ask specific questions about cybersecurity topics like passwords, phishing, or safe browsing.", 30);
                        Console.ResetColor();
                        break;

                    case "can you help me with tasks like writing or programming?":
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        TypeText("I'm focused on cybersecurity topics, but I can provide general advice on writing or programming if it relates to online safety.", 30);
                        Console.ResetColor();
                        break;

                    case "how do you handle privacy?":
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        TypeText("I don't store any personal data or conversation history. Your privacy is important!", 30);
                        Console.ResetColor();
                        break;

                    case "can you provide medical or legal advice?":
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        TypeText("No, I can't provide medical or legal advice. Always consult a qualified professional for such matters.", 30);
                        Console.ResetColor();
                        break;

                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        TypeText("I didn’t quite understand that. Could you rephrase?", 30);
                        Console.ResetColor();
                        break;
                }
            }
        }
    }
}