# Cybersecurity Awareness Chatbot (C#)

This repository contains a C# console-based chatbot application that provides interactive cybersecurity education. The chatbot engages users in conversations about key topics such as password safety, phishing awareness, VPN usage, and online privacy. It offers personalized responses, remembers user preferences, and presents information in an accessible and dynamic manner.

---

## Project Structure

The project is organized into clearly defined directories and files to support maintainability, collaboration, and documentation.

### `.github/workflows`

Contains GitHub Actions workflow files used for automation tasks such as:

- Continuous Integration (CI)
- Code validation and formatting
- Automated builds or test runs

These YAML files define how the repository behaves when code is pushed, pull requests are opened, or scheduled events occur.

---

### `GitHub Link`

Includes metadata or supporting documentation pointing to the repository’s live URL on GitHub.

- `github-link-pdf`: A plain pdf with a link to the public GitHub repository. Useful for reference, submissions, or automated systems.

---

### `Presentation`

This directory contains the official presentation materials for the project.

- `presentation.pptx`: A Microsoft PowerPoint file that outlines the chatbot’s goals, system architecture, features, implementation details, and demonstration screenshots. It is intended for academic presentation.

---

### `cybersecurity-chatbot-csharp`

This is the main application source folder containing the C# classes that define the chatbot’s core functionality.

#### Key Components

- **`Program.cs`**  
  Entry point of the application. It initializes and launches the chatbot by invoking the main logic contained in other classes.

- **`ChatBot.cs`**  
  Controls the application flow. It handles user input, processes natural language prompts, and coordinates responses from various services including the knowledge base and memory management modules.

- **`KnowledgeBase.cs`**  
  Manages cybersecurity-related data, including:
  - Topic-response pairs
  - Randomized responses for common subjects such as passwords, phishing, and privacy
  - Filtering out non-essential words from user input to identify core queries

- **`MemoryManager.cs`**  
  Manages user-specific data such as:
  - Username
  - Expressed interests
  - Stored favorites  
  Data is persisted locally via file storage so that user preferences are remembered across sessions.

- **`UserInterface.cs`**  
  Manages all interactions with the user through the console, including:
  - Text input and validation
  - Typing animations for messages
  - Formatted error messages
  - ASCII image rendering
  - Audio playback of welcome messages (WAV format)

---

## Features

- Personalized conversations using stored user data
- Secure and validated user input handling
- Dynamic responses with randomized facts
- ASCII art generation from image assets
- Audio greeting for improved engagement
- File-based persistence of user interests and favorites
- Clear and readable console formatting using ANSI colors

---

## Getting Started

### Prerequisites

- [.NET SDK](https://dotnet.microsoft.com/download) (version 6.0 or later recommended)
- A console terminal or IDE that supports C# projects (e.g., Visual Studio, Rider, VS Code)

### Running the Application

1. Open a terminal in the root of the `cybersecurity-chatbot-csharp` directory.
2. Build and run the project using the .NET CLI:

```bash
dotnet run
```

3. Follow the console prompts to begin interacting with the chatbot.

---

## License

This project is licensed under the [MIT License](LICENSE). You are free to use, modify, and distribute this software in accordance with the license terms. Please include the original copyright and license.

---
