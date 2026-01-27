# Calculator Project - Complete Overview

## 📋 Project Summary

A modern, visually appealing Windows Forms calculator application built with .NET 10.0 and C#. The calculator features a custom UI with image-based buttons, smooth animations, and comprehensive arithmetic functionality.

---

## 🏗️ Architecture

### Technology Stack
- **Framework**: .NET 10.0
- **UI Framework**: Windows Forms
- **Language**: C#
- **Target Platform**: Windows 10+

### Project Structure
```
calculator/
├── 📄 Core Application Files
│   ├── Program.cs                    # Entry point
│   ├── Form1.cs                      # Main calculator logic
│   ├── Form1.Designer.cs             # UI designer code
│   └── Form1.resx                    # Form resources
│
├── 🎨 Custom Controls
│   ├── CircleButton.cs               # Custom circular button control
│   ├── ImageButton.cs                # Image-based button control (USED)
│   └── RoundedPanel.cs               # Rounded panel control
│
├── 🖼️ Assets
│   ├── keyboard-keys/                # Button images (20 PNG files)
│   │   ├── 0.png - 9.png            # Number buttons
│   │   ├── plus.png, minus.png      # Operator buttons
│   │   ├── multiply.png, divide.png
│   │   ├── =.png, AC.png, dot.png
│   │   ├── Ans.png, n².png, ⌫.png
│   └── codesnaps/                    # Screenshots
│
├── 🔧 Configuration
│   ├── calculator.csproj             # Project configuration
│   ├── calculator.sln                # Solution file
│   └── Properties/Resources.resx     # Embedded resources
│
├── 📦 Build Output
│   ├── bin/Debug/                    # Debug builds
│   ├── bin/Release/                  # Release builds
│   └── obj/                          # Build intermediates
│
└── 📚 Documentation
    ├── README.md                     # Comprehensive documentation
    ├── LICENSE                       # MIT License
    └── .gitignore                    # Git ignore rules
```

---

## 🎯 Features & Functionality

### Core Operations
1. **Basic Arithmetic**
   - Addition (+)
   - Subtraction (-)
   - Multiplication (×)
   - Division (÷)

2. **Special Functions**
   - Square (n²): Squares the current number
   - Answer (Ans): Recalls last calculation result
   - Backspace (⌫): Removes last character
   - Clear (AC): Resets calculator

3. **Input Features**
   - Decimal point support
   - Negative numbers
   - Expression building
   - Real-time display updates

### Smart Input Validation
```csharp
// Prevents:
✗ Consecutive operators (e.g., "5++3")
✗ Multiple decimals in one number (e.g., "5.3.2")
✗ Invalid square placement (e.g., "5²3")
✗ Misplaced operators

// Allows:
✓ Negative numbers (e.g., "-5 + 3")
✓ Complex expressions (e.g., "5² + 3 × 2")
✓ Chained calculations with Ans
```

### Expression Processing
```csharp
// Example flow:
User Input:  "5² + 3 × 2"
           ↓
Process:     "25 + 3 * 2"  (Square processed, operators normalized)
           ↓
Evaluate:    31             (DataTable.Compute)
           ↓
Display:     "31"
```

---

## 🎨 User Interface

### Layout Design
```
┌─────────────────────────────────────────┐
│  Screen Panel (490 × 230)              │
│  ┌─────────────────────────────────┐   │
│  │ Statement Label                  │   │
│  │ "5² + 3 × 2"                    │   │
│  └─────────────────────────────────┘   │
│  ┌─────────────────────────────────┐   │
│  │ Answer Label                     │   │
│  │ "31"                            │   │
│  └─────────────────────────────────┘   │
└─────────────────────────────────────────┘
┌─────────────────────────────────────────┐
│  Keyboard Panel (490 × 602)            │
│  ┌────┬────┬────┬────┐                 │
│  │ AC │ n² │ ⌫  │ ÷  │                 │
│  ├────┼────┼────┼────┤                 │
│  │ 7  │ 8  │ 9  │ ×  │                 │
│  ├────┼────┼────┼────┤                 │
│  │ 4  │ 5  │ 6  │ -  │                 │
│  ├────┼────┼────┼────┤                 │
│  │ 1  │ 2  │ 3  │ +  │                 │
│  ├────┼────┼────┼────┤                 │
│  │ 0  │ .  │Ans │ =  │                 │
│  └────┴────┴────┴────┘                 │
└─────────────────────────────────────────┘
```

### Visual Elements

**Constants**
```csharp
BUTTON_SIZE = 70px
PADDING = 15px
MAX_DISPLAY_LENGTH = 13 characters
LABEL_FONT_SIZE = 28pt
BUTTON_FONT_SIZE = 16pt
```

**Display Properties**
- Font: Inter (28pt for labels)
- Color: White text on dark background
- Alignment: Right-aligned
- Auto-truncation for long expressions

### Button Images
All buttons use PNG images from `keyboard-keys/` directory:
- High-quality graphics
- Hover effects
- Press states
- Transparent backgrounds

---

## 💻 Code Architecture

### Main Components

#### 1. Window Class (Form1.cs)
**Responsibilities**:
- Main calculator logic
- UI event handling
- Expression evaluation
- State management

**Key Properties**:
```csharp
private string fullExpression = "0";      // Current expression
private double lastResult = 0.0;          // Last calculation result
private bool justCalculated = false;      // Calculation state flag
```

**Key Methods**:
| Method | Purpose |
|--------|---------|
| `UpdateDisplay()` | Updates screen with current expression |
| `IsValidStatement()` | Validates expression before accepting |
| `EvaluateExpression()` | Calculates result using DataTable.Compute |
| `HandleEquals()` | Processes = button click |
| `HandleAnswer()` | Recalls last result |
| `ProcessSquares()` | Evaluates n² operations |

#### 2. ImageButton Control (CustomControls)
**Features**:
- Image-based rendering (normal, hover, pressed states)
- Transparent background
- Click event handling
- Text overlay support (optional)
- High-quality anti-aliasing

**Properties**:
```csharp
public Image NormalImage    // Default button appearance
public Image HoverImage     // Mouse hover state
public Image PressedImage   // Mouse down state
public string Text          // Associated data (button value)
public bool ShowText        // Toggle text overlay
```

#### 3. CircleButton Control (CustomControls)
**Status**: Not currently used in calculator
**Features**:
- Perfect circular shape
- Custom colors and borders
- Drop shadow effect
- Hover animations

#### 4. RoundedPanel Control (CustomControls)
**Status**: Not currently used in calculator
**Features**:
- Rounded corners
- Custom border
- Anti-aliased rendering

---

## 🔧 Technical Details

### Expression Evaluation Process

```csharp
// Step 1: Process Square Operations
Input:  "5² + 3"
Regex:  @"(\d+(?:\.\d+)?)²"
Output: "25 + 3"

// Step 2: Normalize Operators
Input:  "25 + 3 × 2 ÷ 4"
Output: "25 + 3 * 2 / 4"

// Step 3: Evaluate with DataTable.Compute
Input:  "25 + 3 * 2 / 4"
Result: 26.5
```

### Input Validation Rules

**Regular Expressions Used**:
```csharp
// Consecutive operators check
@"[+\-×÷]{2,}"

// Square pattern matching
@"(\d+(?:\.\d+)?)²"
```

**Validation Checks**:
1. No consecutive operators
2. Max one decimal per number
3. Max one square per number
4. Square must be at end of number

### Error Handling

```csharp
try {
    result = EvaluateExpression(expression);
    
    if (double.IsNaN(result) || double.IsInfinity(result))
        Display "Math Error"
} 
catch (DivideByZeroException)
    Display "Cannot divide by zero"
catch (Exception)
    Display "Error"
```

---

## 🎮 User Interaction Flow

### Example Calculation: "5² + 3 × 2"

```
Step 1: User clicks "5"
    fullExpression = "5"
    Display: "5"

Step 2: User clicks "n²"
    fullExpression = "5²"
    Display: "5²"

Step 3: User clicks "+"
    fullExpression = "5² + "
    Display: "5² + "

Step 4: User clicks "3"
    fullExpression = "5² + 3"
    Display: "5² + 3"

Step 5: User clicks "×"
    fullExpression = "5² + 3 × "
    Display: "5² + 3 × "

Step 6: User clicks "2"
    fullExpression = "5² + 3 × 2"
    Display: "5² + 3 × 2"

Step 7: User clicks "="
    Process: "5²" → "25"
    Process: "×" → "*"
    Compute: "25 + 3 * 2" = 31
    lastResult = 31
    justCalculated = true
    Display: "31"
```

### Answer (Ans) Feature

```
Scenario 1: After Calculation
Previous: "5 + 5 = 10"
User clicks: "Ans"
Result: fullExpression = "10"

Scenario 2: During Expression
Expression: "3 + "
User clicks: "Ans"
Result: fullExpression = "3 + 10"
```

---

## 🐛 Known Issues & Limitations

### Current Issues
1. **Keyboard Paint Event**
   ```csharp
   // ⚠️ CRITICAL BUG
   private void Keyboard_Paint(object sender, PaintEventArgs e)
   {
       CreateKeyboardButtons(kb); // Creates duplicate buttons on every paint!
   }
   ```
   **Impact**: Multiple button instances created
   **Solution**: Move button creation to Form_Load or use a flag

2. **Display Truncation**
   - Only shows last 13 characters of expression
   - No scrolling or full expression view

3. **No Keyboard Input**
   - Calculator only works with mouse clicks
   - No keyboard shortcuts

### Limitations
- Cannot edit middle of expression (only backspace from end)
- No memory functions (M+, M-, MR, MC)
- No percentage function
- No scientific functions (sin, cos, log, etc.)
- No history tracking
- Expression length limited to display width

---

## 🚀 Build & Deployment

### Build Commands

**Debug Build**:
```bash
dotnet build --configuration Debug
```

**Release Build**:
```bash
dotnet build --configuration Release
```

**Run Application**:
```bash
dotnet run
```

### Executable Location
- Debug: `bin/Debug/net10.0-windows/calculator.exe`
- Release: `bin/Release/net10.0-windows/calculator.exe`

### System Requirements
- OS: Windows 10 or later
- .NET: 10.0 Runtime or SDK
- DPI: System Aware (handles high DPI displays)

---

## 📝 Code Patterns & Best Practices

### Design Patterns Used
1. **Event-Driven Architecture**: All button clicks trigger event handlers
2. **State Machine**: Tracks calculator state (justCalculated flag)
3. **Strategy Pattern**: Different handlers for different button types
4. **Template Method**: Consistent button creation with customization

### Code Organization
```csharp
#region Constants        // Configuration values
#region Button Layout    // UI structure
#region Button Categories // Button classification
#region UI Components    // UI elements
#region State           // Application state
#region Initialization  // Setup code
#region Event Handlers  // UI events
#region Input Handlers  // Button logic
#region Expression Evaluation // Math processing
```

### Naming Conventions
- **Fields**: camelCase with descriptive names
- **Constants**: UPPER_SNAKE_CASE
- **Methods**: PascalCase with verb prefixes (Handle, Create, Update)
- **Properties**: PascalCase

---

## 🔮 Future Enhancement Ideas

### Potential Features
1. **Keyboard Support**
   - Number keys (0-9)
   - Operator keys (+, -, *, /)
   - Enter for equals
   - Backspace, Delete, Escape

2. **Advanced Functions**
   - Memory operations (M+, M-, MR, MC)
   - Percentage calculations
   - Square root
   - Power function (x^y)
   - Parentheses support

3. **UI Improvements**
   - History panel showing recent calculations
   - Copy/paste support
   - Theme switching (dark/light)
   - Responsive sizing
   - Expression editing (cursor movement)

4. **Error Handling**
   - More descriptive error messages
   - Input correction suggestions
   - Undo/redo functionality

5. **Performance**
   - Fix duplicate button creation bug
   - Optimize rendering
   - Add unit tests

---

## 📊 Statistics

### Code Metrics
- **Total Files**: 13 code files
- **Lines of Code**: ~1,500+ (excluding designer files)
- **Custom Controls**: 3 (ImageButton, CircleButton, RoundedPanel)
- **Image Assets**: 20 PNG files
- **Button Configuration**: 5×4 grid = 20 buttons

### Key Classes
| Class | Lines | Purpose |
|-------|-------|---------|
| Window (Form1.cs) | ~500 | Main calculator logic |
| ImageButton | ~250 | Custom image button control |
| CircleButton | ~300 | Custom circular button control |
| RoundedPanel | ~150 | Custom panel with rounded corners |

---

## 🎓 Learning Points

### Technical Concepts Demonstrated
1. **Windows Forms Development**: Custom controls, event handling
2. **Graphics Programming**: GDI+, anti-aliasing, image rendering
3. **Regular Expressions**: Pattern matching for validation
4. **Expression Parsing**: Using DataTable.Compute for safe evaluation
5. **State Management**: Tracking application state across interactions
6. **Custom Control Creation**: Extending base WinForms controls
7. **Resource Management**: Embedding and loading images

### C# Features Used
- Properties with getters/setters
- Events and delegates
- LINQ and collections
- String manipulation
- Exception handling
- Regex pattern matching
- Optional parameters
- Collection initializers (`[]` syntax)

---

## 📄 License

MIT License - Open source and free to use/modify

---

**Project Status**: ✅ Functional with minor bugs
**Last Updated**: Based on code snapshot
**Version**: .NET 10.0

---

This calculator serves as an excellent example of modern Windows Forms development with custom UI controls and comprehensive input validation!
