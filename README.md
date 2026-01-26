# Calculator

A modern, feature-rich Windows Forms calculator application built with .NET 10.0 and C#.

![Calculator Screenshot](https://img.shields.io/badge/.NET-10.0-512BD4?style=flat-square&logo=dotnet) ![Windows Forms](https://img.shields.io/badge/Windows-Forms-0078D4?style=flat-square&logo=windows)

## Features

### Core Functionality

- **Basic Arithmetic Operations**: Addition, subtraction, multiplication, and division
- **Square Function (n²)**: Instantly square any number
- **Answer Memory (Ans)**: Recall and reuse your last calculation result
- **Backspace (⌫)**: Easily correct input mistakes
- **Clear (AC)**: Reset the calculator to start fresh
- **Decimal Support**: Full support for decimal number calculations

### Smart Input Handling

- **Input Validation**: Prevents invalid expressions like consecutive operators
- **Decimal Control**: Only one decimal point allowed per number
- **Trailing Zero Trimming**: Automatically removes unnecessary trailing zeros
- **Expression Truncation**: Displays the last 18 characters for long expressions
- **Smart Expression Building**: Intelligent handling of operators and numbers

### User Experience

- **Visual Feedback**: Hover and click effects on all buttons
- **Error Handling**: Clear error messages for division by zero and invalid operations
- **Intuitive Layout**: Standard calculator layout for familiar operation
- **Responsive Design**: Clean, modern interface with color-coded buttons

## Button Layout

```
┌────┬────┬────┬────┐
│ AC │ n² │ ⌫  │ ÷  │
├────┼────┼────┼────┤
│ 7  │ 8  │ 9  │ ×  │
├────┼────┼────┼────┤
│ 4  │ 5  │ 6  │ -  │
├────┼────┼────┼────┤
│ 1  │ 2  │ 3  │ +  │
├────┼────┼────┼────┤
│ 0  │ .  │Ans │ =  │
└────┴────┴────┴────┘
```

## Color Coding

- **Numbers & Decimal** (0-9, .): Dark gray - for primary input
- **Operators** (+, -, ×, ÷): Orange - for mathematical operations
- **Clear (AC)**: Red - for resetting
- **Backspace (⌫)**: Light orange - for corrections
- **Equals (=)**: Blue - for calculating results
- **Special Functions** (n², Ans): Medium gray - for advanced features

## Requirements

- **Operating System**: Windows 10 or later
- **.NET Runtime**: .NET 10.0 or higher
- **Framework**: Windows Forms

## Installation

### Option 1: Run Pre-built Executable

1. Navigate to `bin/Debug/net10.0-windows/`
2. Double-click `calculator.exe` to launch the application

### Option 2: Build from Source

1. Ensure you have .NET 10.0 SDK installed
2. Open the solution file `calculator.sln` in Visual Studio 2022 or later
3. Build the solution (Ctrl + Shift + B)
4. Run the application (F5)

### Option 3: Command Line Build

```bash
# Navigate to the project directory
cd calculator

# Restore dependencies
dotnet restore

# Build the project
dotnet build

# Run the application
dotnet run
```

## Usage

### Basic Calculations

1. Click number buttons to input values
2. Click an operator (+, -, ×, ÷)
3. Enter the second number
4. Press **=** to see the result

### Using Special Functions

**Square Function (n²)**

- Enter a number
- Click **n²** to square it
- Continue building your expression or press **=**

**Answer Memory (Ans)**

- After calculating a result with **=**
- Click **Ans** in a new expression to reuse the last result
- Example: Calculate `5 + 5 = 10`, then `Ans × 2 = 20`

**Backspace (⌫)**

- Remove the last character from your expression
- Useful for quick corrections

**Clear (AC)**

- Clears the entire expression
- Resets the calculator to initial state

## Project Structure

```
calculator/
├── Program.cs              # Application entry point
├── Form1.cs                # Main calculator logic and functionality
├── Form1.Designer.cs       # UI designer-generated code
├── Form1.resx              # Form resources
├── calculator.csproj       # Project configuration
├── calculator.sln          # Solution file
├── bin/                    # Compiled binaries
│   └── Debug/
│       └── net10.0-windows/
│           └── calculator.exe
└── obj/                    # Build intermediates
```

## Technical Details

### Architecture

- **Pattern**: Windows Forms with event-driven architecture
- **UI Components**: Custom button generation with dynamic layouts
- **Expression Evaluation**: Uses `DataTable.Compute()` for safe expression parsing
- **State Management**: Tracks current expression, last result, and calculation state

### Key Components

**Window Class** (Form1.cs)

- Main calculator logic
- Input validation
- Expression evaluation
- UI event handling

**Constants**

- `BUTTON_SIZE`: 70px
- `PADDING`: 15px
- `MAX_DISPLAY_LENGTH`: 18 characters
- `LABEL_FONT_SIZE`: 28pt
- `BUTTON_FONT_SIZE`: 16pt

### Expression Processing

1. **Input Validation**: Checks for valid operator placement and decimal usage
2. **Square Processing**: Regex-based pattern matching to evaluate n² operations
3. **Operator Normalization**: Converts × and ÷ to \* and / for evaluation
4. **Result Formatting**: Handles infinity, NaN, and division by zero errors

## Error Handling

The calculator gracefully handles:

- **Division by Zero**: Displays "Cannot divide by zero"
- **Math Errors**: Shows "Math Error" for invalid operations (NaN, Infinity)
- **General Errors**: Shows "Error" for unexpected exceptions
- **Invalid Input**: Silently prevents invalid expressions from being entered

## Development

### Building the Project

```bash
dotnet build --configuration Release
```

### Running Tests

```bash
dotnet test
```

### Modifying Button Layout

Edit the `keys` list in `Form1.cs`:

```csharp
private readonly List<List<string>> keys =
[
    ["AC", "n²", "⌫", "÷"],
    ["7", "8", "9", "×"],
    ["4", "5", "6", "-"],
    ["1", "2", "3", "+"],
    ["0", ".", "Ans", "="],
];
```

### Customizing Colors

Modify the `GetButtonColor` method in `Form1.cs` to change button colors.

## Contributing

Contributions are welcome! Please follow these guidelines:

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## License

This project is open source and available under the MIT License.

## Acknowledgments

- Built with .NET 10.0 and Windows Forms
- Inspired by modern calculator design principles
- Uses `DataTable.Compute()` for expression evaluation

## Support

For issues, questions, or suggestions, please open an issue in the repository.

---

**Made with ❤️ using .NET 10.0 and C#**
