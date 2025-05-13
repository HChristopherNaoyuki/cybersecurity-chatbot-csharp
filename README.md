# Cybersecurity Awareness Chatbot

## Overview

A console-based chatbot designed to educate users about cybersecurity best practices through interactive conversations.

## Features

### Core Functionality
- **Interactive Conversations**: Natural dialogue flow with personalized responses
- **Cybersecurity Education**: Covers topics like passwords, phishing, VPNs, and privacy
- **User Memory**: Remembers user details and conversation history
- **Sentiment Analysis**: Adapts responses based on detected user emotions

### Technical Highlights
- **Delegate-Based Architecture**: Clean separation of concerns using C# delegates
- **Modular Design**: Easily extensible components
- **Persistent Memory**: Saves conversation history between sessions
- **Error Resilient**: Graceful handling of edge cases

## Getting Started

### Prerequisites
- .NET 6.0 SDK or later
- Windows OS (for voice greeting functionality)
- Text-to-speech libraries (optional)

### Installation
1. Clone the repository:
   ```bash
   git clone https://github.com/HChristopherNaoyuki/cybersecurity-chatbot-csharp.git
   ```
2. Navigate to project directory:
   ```bash
   cd cybersecurity-chatbot/src
   ```
3. Build the solution:
   ```bash
   dotnet build
   ```

### Usage
Run the chatbot:
```bash
dotnet run --project cybersecurity_chatbot_csharp.csproj
```

**Basic Commands:**
- `help` - Show available topics
- `exit` - Quit the chatbot
- "What's my name?" - Recall stored username

## Project Structure

```
/src
│
├── ChatBot.cs              - Main orchestrator class
├── ConversationManager.cs  - Handles conversation logic
├── KnowledgeBase.cs        - Contains cybersecurity responses
├── MemoryManager.cs        - Manages user memory
├── Program.cs              - Entry point
└── UserInterface.cs        - Handles all UI components
```

## Development

### Key Design Patterns
- **Facade Pattern**: Simplified UI operations via `UserInterface`
- **Strategy Pattern**: Delegate-based implementations for flexible behaviors
- **Observer Pattern**: Event-driven memory updates

### Continuous Integration
The project includes GitHub Actions for:
- Automated builds
- Code quality checks
- Test execution

## License

Distributed under the MIT License. See `LICENSE` for more information.

---
