# Cybersecurity Awareness Chatbot

## Overview

A console-based chatbot designed to educate users about cybersecurity best practices. This application provides interactive guidance on topics like:

- Password security
- Phishing detection
- Privacy protection
- Secure browsing
- Two-factor authentication

## Features

### Core Functionality
- Interactive Q&A about cybersecurity topics
- Personalized responses using your name
- ASCII art welcome screen
- Voice greeting on startup

### Advanced Features
- Sentiment-aware responses
- Conversation memory (remembers your interests)
- Contextual follow-ups
- Multiple response variations

## Getting Started

### Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- Windows machine (for voice functionality)

### Installation
1. Clone the repository:
   ```bash
   git clone https://github.com/HChristopherNaoyuki/cybersecurity-chatbot-csharp.git
   ```
2. Navigate to project directory:
   ```bash
   cd cybersecurity-chatbot/src
   ```

### Running the Application
```bash
dotnet run
```

## Usage Examples

**Basic interaction:**
```
User: How can I create a strong password?
Chatbot: Strong passwords should be at least 12 characters long...
```

**Interest-based follow-up:**
```
User: I'm interested in privacy
Chatbot: I'll remember you're interested in privacy...
[Later in conversation]
Chatbot: Since you're interested in privacy, you should review...
```

## Development

### Project Structure
```
/src
  /CybersecurityChatbot      # Main application
  /CybersecurityChatbot.Tests # Unit tests
```

### Building and Testing
Run all tests:
```bash
dotnet test
```

### CI Pipeline
The GitHub Actions workflow automatically runs tests on:
- Push to main/master
- Pull requests to main/master

## Contributing

1. Fork the project
2. Create your feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some amazing feature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## License

Distributed under the MIT License. See `LICENSE` for more information.

---
