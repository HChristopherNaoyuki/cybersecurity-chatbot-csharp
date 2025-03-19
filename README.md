# Cybersecurity Awareness Chatbot

## Overview
This project is a **Cybersecurity Awareness Chatbot** designed to educate users about online safety practices. The chatbot is a console-based application that simulates real-life scenarios where users might encounter cyber threats, such as phishing, password safety, and safe browsing. It provides guidance on how to avoid common traps and stay safe online.

The chatbot is developed in **C#** and includes features such as:
- Voice greeting
- ASCII art display
- Personalized user interaction
- Basic response system for cybersecurity topics
- Input validation
- Enhanced console UI with color formatting and typing effects

---

## Features
- **Voice Greeting**: Plays a welcome message when the chatbot starts.
- **ASCII Art**: Displays a cybersecurity-themed logo or image.
- **User Interaction**: Asks for the user's name and personalizes responses.
- **Basic Response System**: Responds to user queries about cybersecurity topics like passwords, phishing, and safe browsing.
- **Input Validation**: Handles invalid or unexpected inputs gracefully.
- **Enhanced Console UI**: Uses colored text, borders, and typing effects to create a visually appealing interface.

---

## Prerequisites
Before running the chatbot, ensure you have the following installed:
- **.NET SDK** (version 6.0 or higher)
- **Visual Studio** (or any C# IDE)
- **Audio File**: A `.wav` file for the voice greeting (place it in the `Audio` folder).

---

## Setup Instructions

### 1. Clone the Repository
Clone this repository to your local machine using the following command:
```bash
git clone hhttps://github.com/HChristopherNaoyuki/cybersecurity-chatbot-csharp.git
```

### 2. Navigate to the Project Directory
Open the project in your preferred IDE (e.g., Visual Studio) or navigate to the project directory in the terminal:
```bash
cd cybersecurity-chatbot-csharp
```

### 3. Add the Audio File
- Create a folder named `Audio` in the project directory.
- Place the `welcome.wav` file inside the `Audio` folder. This file will be used for the voice greeting.

### 4. Build and Run the Project
- Open the project in Visual Studio and click **Build > Build Solution**.
- Run the project by pressing **F5** or clicking **Debug > Start Debugging**.
- Alternatively, you can run the project from the terminal using the following command:
  ```bash
  dotnet run
  ```

---

## Usage
1. **Launch the Chatbot**:
   - When the chatbot starts, it will play a voice greeting and display ASCII art.
   - You will be prompted to enter your name.

2. **Interact with the Chatbot**:
   - The chatbot will welcome you and start a conversation.
   - You can ask questions about cybersecurity topics such as:
     - "How do I create a strong password?"
     - "What is phishing?"
     - "How can I stay safe on public Wi-Fi?"
   - Type `exit` to end the chat.

3. **Example Interaction**:
   ```
   Enter your name: John
   =======================================================================
   Hello, John! Welcome to the Cybersecurity Awareness Bot.
   I'm here to help you stay safe online.
   =======================================================================
   John: What is phishing?
   ChatBot: Phishing is when attackers trick you into revealing sensitive information via fake emails or websites.
   John: exit
   ChatBot: Goodbye! Stay safe online!
   ```

---

## Project Structure
The project is organized as follows:
```
cybersecurity-chatbot-csharp/
│
├── Audio/                  # Folder containing the voice greeting audio file
│   └── welcome.wav         # Audio file for the voice greeting
│
├── Program.cs              # Main C# file containing the chatbot logic
├── README.md               # This file
└── cybersecurity-chatbot-csharp.csproj  # Project configuration file
```

---

## GitHub and Continuous Integration (CI)
- **GitHub Repository**: Ensure your project is hosted on GitHub with at least **3 meaningful commits**.
- **Continuous Integration (CI)**: Set up GitHub Actions to automatically check for syntax errors and code formatting. A sample workflow file (`.github/workflows/ci.yml`) is provided in the repository.

---

## License
This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.

---

Enjoy using the Cybersecurity Awareness Chatbot! Stay safe online! 🛡️

---
