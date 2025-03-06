# Cybersecurity Awareness Chatbot

## Overview
This project is a **Cybersecurity Awareness Chatbot** developed in **C#**. The chatbot is designed to educate users about cybersecurity topics such as password safety, phishing, and safe browsing. It provides a conversational interface where users can ask questions and receive informative responses. The chatbot also includes features like a **voice greeting**, **ASCII art**, and a **typing effect** to simulate a natural conversation.

---

## Features
1. **Voice Greeting**: Plays a welcome message when the chatbot starts.
2. **ASCII Art**: Displays a cybersecurity-themed logo when the chatbot launches.
3. **User Interaction**: Asks for the user's name and personalizes responses.
4. **Basic Response System**: Responds to general questions and specific cybersecurity topics.
5. **Input Validation**: Handles invalid or unexpected inputs gracefully.
6. **Typing Effect**: Simulates a typing effect for chatbot responses to create a conversational feel.
7. **GitHub Version Control**: The project is version-controlled using Git, with meaningful commit messages.
8. **Continuous Integration (CI)**: GitHub Actions is set up to check for syntax errors and successful builds.

---

## Prerequisites
Before running the chatbot, ensure you have the following installed:
- **.NET SDK** (version 6.0 or later)
- **Visual Studio** (optional, for easier development)
- A `.wav` file named `welcome.wav` for the voice greeting (place it in the project directory).

---

## Setup Instructions
1. **Clone the Repository**:
   ```bash
   git clone https://github.com/HChristopherNaoyuki/cybersecurity-chatbot-csharp.git
   cd cybersecurity-chatbot-csharp
   ```

2. **Build the Project**:
   ```bash
   dotnet build
   ```

3. **Run the Chatbot**:
   ```bash
   dotnet run
   ```

4. **Interact with the Chatbot**:
   - Enter your name when prompted.
   - Ask questions about cybersecurity topics like password safety, phishing, or safe browsing.
   - Type `exit` to quit the chatbot.

---

## GitHub Version Control
### Commit Messages
Ensure you make **at least three meaningful commits** with descriptive messages. Example commit messages:
- `Initial commit: Set up project structure and main files.`
- `Added voice greeting and ASCII art.`
- `Implemented basic chatbot responses and input validation.`

### Continuous Integration (CI)
GitHub Actions is set up to run a CI workflow. The workflow checks for:
- Syntax errors.
- Code formatting.
- Successful builds.

To set up GitHub Actions:
1. Navigate to the **Actions** tab in your GitHub repository.
2. Choose a preconfigured workflow or set up a custom one.
3. Save the workflow file in the `.github/workflows` folder.

---

## Usage Examples
### General Questions
- **User**: "How are you?"
  - **Chatbot**: "I'm just a bot, but I'm here to help you stay safe online!"
- **User**: "What's your purpose?"
  - **Chatbot**: "My purpose is to educate you about cybersecurity and help you stay safe online."
- **User**: "What can I ask you about?"
  - **Chatbot**: "You can ask me about:\n- Password safety\n- Phishing\n- Safe browsing."

### Cybersecurity Topics
- **User**: "password"
  - **Chatbot**: "Use strong, unique passwords for each account. Avoid using personal details."
- **User**: "phishing"
  - **Chatbot**: "Be cautious of emails asking for personal information. Scammers often disguise themselves as trusted organizations."
- **User**: "safe browsing"
  - **Chatbot**: "Always check for 'https://' in the URL and avoid clicking on suspicious links."

---

## Project Structure
```
cybersecurity-chatbot-csharp/
│
├── Program.cs              # Main program file
├── welcome.wav             # Voice greeting audio file
├── README.md               # Project documentation
├── .github/workflows/      # GitHub Actions workflow
└── cybersecurity_chatbot_csharp.csproj  # Project configuration file
```

---

## Rubric Compliance
This project meets the following rubric criteria for **Question 1**:
- **Voice Greeting and Image Display**: Includes a voice greeting and ASCII art.
- **Text-Based Greeting and User Interaction**: Personalizes responses using the user's name.
- **Basic Response System**: Responds to general questions and specific cybersecurity topics.
- **Input Validation**: Handles invalid inputs gracefully.
- **Enhanced Console UI**: Uses colored text, ASCII art, and typing effects for a better user experience.
- **GitHub Version Control and CI**: Includes meaningful commits and a CI workflow.

---
