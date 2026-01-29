using System.Data;
using System.Diagnostics;
using System.Text.RegularExpressions;
using CustomControls;

namespace calculator
{
    public partial class Window : Form
    {
        #region Constants
        private const int BUTTON_SIZE = 70;
        private const int PADDING = 15;
        private const int MAX_DISPLAY_LENGTH = 13;
        private const int LABEL_FONT_SIZE = 28;
        private const int BUTTON_FONT_SIZE = 16;
        private const int WINDOW_WIDTH = 536;
        private const int WINDOW_HEIGHT = 940;
        private const int KEYBOARD_Y_POSITION = 270;
        private const int KEYBOARD_HEIGHT = 662;
        private const int ARROW_BUTTON_OFFSET = 60;
        private const int MAX_HISTORY_SIZE = 5;
        private const string IMAGE_FOLDER = "keyboard-keys";
        #endregion

        #region State Variables
        private readonly List<string> stmt_history = [];
        private int stmt_index = -1;
        private string fullExpression = "0";
        private double lastResult = 0.0;
        private bool justCalculated = false;
        private bool keyboardInitialized = false; // Prevent duplicate button creation
        #endregion

        #region UI Components
        private readonly Label stmt_label = new();
        private readonly Label answer_label = new();
        #endregion

        #region Static Data
        private static readonly Dictionary<string, string> imagePathMap = new()
        {
            { "1", "1.png" },
            { "2", "2.png" },
            { "3", "3.png" },
            { "4", "4.png" },
            { "5", "5.png" },
            { "6", "6.png" },
            { "7", "7.png" },
            { "8", "8.png" },
            { "9", "9.png" },
            { "0", "0.png" },
            { "+", "plus.png" },
            { "-", "minus.png" },
            { "×", "multiply.png" },
            { "÷", "divide.png" },
            { "AC", "AC.png" },
            { "Ans", "Ans.png" },
            { "n²", "n².png" },
            { "⌫", "⌫.png" },
            { "=", "=.png" },
            { ".", "dot.png" },
            { "^", "up_button.png" },
            { "v", "down_button.png" },
        };

        private readonly List<List<string>> keys =
        [
            ["AC", "n²", "⌫", "÷"],
            ["7", "8", "9", "×"],
            ["4", "5", "6", "-"],
            ["1", "2", "3", "+"],
            ["0", ".", "Ans", "="],
        ];

        private readonly HashSet<string> displayable =
        [
            "0",
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "+",
            "-",
            "×",
            "÷",
            ".",
            "n²",
        ];
        private readonly HashSet<string> operators = ["+", "-", "×", "÷"];
        private readonly HashSet<string> numbers =
        [
            "0",
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
        ];
        #endregion

        #region Initialization
        public Window()
        {
            InitializeComponent();
            ClientSize = new Size(WINDOW_WIDTH, WINDOW_HEIGHT);
            InitializeLabel(stmt_label, Statement, "0");
            InitializeLabel(answer_label, Answer, "");
        }

        private void InitializeLabel(Label label, Panel parent, string initialText)
        {
            label.Text = initialText;
            label.Font = new Font("Jetbrains Mono", LABEL_FONT_SIZE, FontStyle.Regular);
            label.ForeColor = Color.White;
            label.AutoSize = false;
            label.TextAlign = ContentAlignment.MiddleRight;
            label.Size = new Size(parent.Width - 20, parent.Height - 20);
            label.Location = new Point(10, (parent.Height - label.Height) / 2 - 8);
        }
        #endregion

        #region Event Handlers - Paint
        private void Form1_Load(object sender, EventArgs e) { }

        private void Statement_Paint(object sender, PaintEventArgs e)
        {
            if (sender is Panel panel && !panel.Controls.Contains(stmt_label))
                panel.Controls.Add(stmt_label);
        }

        private void Answer_Paint(object sender, PaintEventArgs e)
        {
            if (sender is Panel panel && !panel.Controls.Contains(answer_label))
                panel.Controls.Add(answer_label);
        }

        private void Keyboard_Paint(object sender, PaintEventArgs e)
        {
            if (sender is not Panel kb || keyboardInitialized)
                return;

            Keyboard.Location = new Point(Keyboard.Location.X, KEYBOARD_Y_POSITION);
            Keyboard.Size = new Size(Screen.Size.Width, KEYBOARD_HEIGHT);
            CreateKeyboardButtons(kb);
            keyboardInitialized = true;
        }
        #endregion

        #region Button Creation
        private void CreateKeyboardButtons(Panel keyboard)
        {
            int workWidth = Keyboard.Size.Width - PADDING * 2;
            int workHeight = Keyboard.Size.Height - PADDING * 2 - ARROW_BUTTON_OFFSET;

            // Create main keyboard buttons
            for (int row = 0; row < keys.Count; row++)
            {
                for (int col = 0; col < keys[row].Count; col++)
                {
                    int posX = PADDING + workWidth / 4 * col + PADDING;
                    int posY = PADDING + workHeight / 5 * row + PADDING + ARROW_BUTTON_OFFSET;
                    keyboard.Controls.Add(CreateButton(keys[row][col], posX, posY));
                }
            }

            // Create arrow buttons
            keyboard.Controls.Add(CreateButton("^", 138, 16));
            keyboard.Controls.Add(CreateButton("v", 253, 16));
        }

        private ImageButton CreateButton(string text, int x, int y)
        {
            ImageButton btn = new() { Text = text, Location = new Point(x - 8, y - 8) };

            Image? img = GetButtonImage(text);
            if (img != null)
            {
                btn.NormalImage = img;
                btn.Size = new Size(img.Width, img.Height);
                btn.ShowText = false;
            }

            btn.Click += Button_Click!;
            return btn;
        }

        private static Image? GetButtonImage(string buttonText)
        {
            if (
                !imagePathMap.TryGetValue(buttonText, out string? path)
                || string.IsNullOrEmpty(path)
            )
                return null;

            string fullPath = Path.Combine(IMAGE_FOLDER, path);
            if (File.Exists(fullPath))
                return Image.FromFile(fullPath);

            Console.WriteLine($"Image not found: {fullPath}");
            return null;
        }
        #endregion

        #region Display Management
        private void UpdateDisplay()
        {
            stmt_label.Text =
                fullExpression.Length > MAX_DISPLAY_LENGTH
                    ? fullExpression[^MAX_DISPLAY_LENGTH..]
                    : fullExpression;
        }

        private void ClearDisplay()
        {
            fullExpression = "0";
            answer_label.Text = "";
            justCalculated = false;
            UpdateDisplay();
        }
        #endregion

        #region Validation
        private bool IsValidStatement(string text)
        {
            return text == "0" || (!HasConsecutiveOperators(text) && ValidateTerms(text));
        }

        private static bool HasConsecutiveOperators(string text) =>
            Regex.IsMatch(text, @"[+\-×÷]{2,}");

        private static bool ValidateTerms(string text)
        {
            string[] terms = Regex.Split(text, @"[+\-×÷]");

            foreach (string term in terms)
            {
                if (string.IsNullOrEmpty(term))
                    continue;
                if (term.Count(c => c == '.') > 1)
                    return false;
                if (term.Count(c => c == '²') > 1)
                    return false;
                if (!IsSquareAtEnd(term))
                    return false;
            }

            return true;
        }

        private static bool IsSquareAtEnd(string term)
        {
            int squareIndex = term.IndexOf('²');
            return squareIndex == -1 || squareIndex == term.Length - 1;
        }
        #endregion

        #region String Manipulation
        private static string TrimTrailingZeros(string text)
        {
            if (text == "0" || text.Length <= 1)
                return text;

            text = text.TrimEnd('0');
            return text.EndsWith('.') && text.Length > 1 ? text[..^1] : text;
        }
        #endregion

        #region Event Handlers - Button Click
        private void Button_Click(object sender, EventArgs e)
        {
            if (sender is ImageButton btn)
                RouteButtonAction(btn.Text);
        }

        private void RouteButtonAction(string buttonText)
        {
            // Clear answer label for most inputs
            if (buttonText != "=" && !operators.Contains(buttonText))
                answer_label.Text = "";

            // Handle history navigation
            if (buttonText == "^")
            {
                HandleUpArrow();
                return;
            }

            if (buttonText == "v")
            {
                HandleDownArrow();
                return;
            }

            // Reset history index for new input
            stmt_index = stmt_history.Count;

            // Block Ans button after n²
            if (buttonText == "Ans" && fullExpression.EndsWith("²"))
            {
                // Do nothing - block the Ans button
                return;
            }

            // Route to appropriate handler
            switch (buttonText)
            {
                case "AC":
                    ClearDisplay();
                    break;
                case "⌫":
                    HandleBackspace();
                    break;
                case "=":
                    HandleEquals();
                    break;
                case "Ans":
                    HandleAnswer();
                    break;
                default:
                    if (displayable.Contains(buttonText))
                        HandleDisplayableInput(buttonText);
                    break;
            }
        }
        #endregion

        #region Input Handlers
        private void HandleUpArrow()
        {
            if (stmt_index > 0)
            {
                stmt_index--;
                fullExpression = stmt_history[stmt_index];
                UpdateDisplay();
            }
        }

        private void HandleDownArrow()
        {
            if (stmt_index < stmt_history.Count - 1)
            {
                stmt_index++;
                fullExpression = stmt_history[stmt_index];
                UpdateDisplay();
            }
        }

        private void HandleDisplayableInput(string input)
        {
            // Block invalid input after square
            // After n², only allow operators (not numbers, not another n², not decimal)
            if (fullExpression.EndsWith("²"))
            {
                // Only operators are allowed after square
                if (!operators.Contains(input))
                {
                    // Block numbers, decimal, another n², Ans, etc.
                    return;
                }
            }

            // Handle negative numbers at start
            if (input == "-" && fullExpression == "0")
            {
                fullExpression = "-";
                UpdateDisplay();
                return;
            }

            // Check if we already have (operator + minus) pattern FIRST
            // If so, block ALL operator inputs including another minus
            if (operators.Contains(input) && fullExpression.Length >= 2)
            {
                string lastChar = fullExpression[^1].ToString();
                string secondLastChar = fullExpression[^2].ToString();

                if (operators.Contains(secondLastChar) && lastChar == "-")
                {
                    // Pattern like +- or ×- exists, don't allow ANY more operators (including -)
                    return;
                }
            }

            // Handle double minus (convert to positive)
            // This only runs if we DON'T have the operator+minus pattern from above
            if (input == "-" && fullExpression.Length >= 1 && fullExpression.EndsWith("-"))
            {
                // Check what comes before the minus
                if (fullExpression.Length == 1)
                {
                    // Just a single minus, reset to 0
                    fullExpression = "0";
                }
                else
                {
                    // Replace the minus with a plus
                    fullExpression = fullExpression[..^1] + "+";
                }
                UpdateDisplay();
                return;
            }

            // Block consecutive operators - ONLY allow +-, ×-, ÷-
            if (operators.Contains(input))
            {
                if (fullExpression.Length >= 1)
                {
                    string lastChar = fullExpression[^1].ToString();

                    // If last character is an operator
                    if (operators.Contains(lastChar))
                    {
                        // Only allow minus after +, ×, or ÷
                        if (
                            !(
                                input == "-"
                                && (lastChar == "+" || lastChar == "×" || lastChar == "÷")
                            )
                        )
                        {
                            // Block everything else
                            return;
                        }
                    }
                }
            }

            // Handle post-calculation input
            if (
                justCalculated
                && (numbers.Contains(input) || input == "." || operators.Contains(input))
            )
            {
                fullExpression = answer_label.Text;
                answer_label.Text = "";
                justCalculated = false;

                if (input == ".")
                    fullExpression += "0";
            }

            // Auto-add zero before decimal after operator
            if (input == "." && !numbers.Contains(fullExpression.Last().ToString()))
                fullExpression += "0";

            // Build new expression
            string newText = BuildNewExpression(input);

            // Trim zeros before operators
            if (ShouldTrimBeforeOperator(input))
                newText = TrimTrailingZeros(fullExpression) + input;

            // Update the expression
            fullExpression = newText;
            if (!operators.Contains(input))
                justCalculated = false;
            UpdateDisplay();
        }

        private string BuildNewExpression(string input)
        {
            if (input == "n²")
                return fullExpression + "²";

            if (fullExpression == "0" && !operators.Contains(input) && input != ".")
                return input;

            return fullExpression + input;
        }

        private bool ShouldTrimBeforeOperator(string input) =>
            operators.Contains(input)
            && fullExpression.Contains('.')
            && fullExpression.EndsWith('0');

        private void HandleBackspace()
        {
            fullExpression = fullExpression.Length <= 1 ? "0" : fullExpression[..^1];
            justCalculated = false;
            answer_label.Text = "";
            UpdateDisplay();
        }

        private void HandleEquals()
        {
            try
            {
                lastResult = EvaluateExpression(fullExpression);

                if (double.IsNaN(lastResult) || double.IsInfinity(lastResult))
                {
                    answer_label.Text = "Math Error";
                    justCalculated = false;
                    lastResult = 0.0;
                    return;
                }

                answer_label.Text = FormatResult(result: lastResult);
                justCalculated = true;
            }
            catch (DivideByZeroException e)
            {
                answer_label.Text = "Cannot divide by zero";
                ResetCalculationState();

                Debug.WriteLine(e);
            }
            catch (Exception e)
            {
                answer_label.Text = $"Error";
                ResetCalculationState();
                Debug.WriteLine(e);
            }

            AddToHistory(expression: fullExpression);
        }

        private void AddToHistory(string expression)
        {
            // Don't add duplicates
            if (stmt_history.Count > 0 && stmt_history[^1] == expression)
                return;

            stmt_history.Add(expression);
            stmt_index = stmt_history.Count - 1;

            // Maintain max history size
            if (stmt_history.Count > MAX_HISTORY_SIZE)
            {
                stmt_history.RemoveAt(0);
                stmt_index--;
            }

            Console.WriteLine($"History: [{string.Join(", ", stmt_history)}]");
        }

        private void ResetCalculationState()
        {
            justCalculated = false;
            lastResult = 0.0;
        }

        private void HandleAnswer()
        {
            fullExpression = justCalculated
                ? lastResult.ToString()
                : (fullExpression == "0" ? lastResult.ToString() : fullExpression + lastResult);

            if (justCalculated)
            {
                answer_label.Text = "";
                justCalculated = false;
            }

            UpdateDisplay();
        }
        #endregion

        #region Expression Evaluation
        private static double EvaluateExpression(string expression)
        {
            expression = ProcessSquares(expression);
            expression = NormalizeOperators(expression);
            expression = EnsureDoubleOperands(expression);
            object result = new DataTable().Compute(expression, null);
            return Convert.ToDouble(result);
        }

        private static string EnsureDoubleOperands(string expression)
        {
            // Match integers that are NOT part of a decimal number or scientific notation
            // Negative lookbehind: (?<![.\deE+\-]) - not after decimal, digit, e/E, or +/- (for exponents)
            // Negative lookahead: (?![.\deE]) - not before decimal, digit, or e/E
            return Regex.Replace(expression, @"(?<![.\deE+\-])(\d+)(?![.\deE])", "$1.0");
        }

        private static string ProcessSquares(string expression) =>
            Regex.Replace(
                expression,
                @"(\d+(?:\.\d+)?)²",
                m =>
                {
                    if (!double.TryParse(m.Groups[1].Value, out double num))
                        return m.Value; // Return original if parsing fails
                    double result = num * num;
                    // Check for overflow/infinity
                    if (double.IsInfinity(result) || double.IsNaN(result))
                        throw new OverflowException("Square operation resulted in overflow");
                    return result.ToString("G17"); // Use full precision
                }
            );

        private static string NormalizeOperators(string expression) =>
            expression.Replace("×", "*").Replace("÷", "/");

        private static string FormatResult(double result)
        {
            Debug.WriteLine($"FormatResult: input={result}");

            // Handle -0 case (make it 0)
            if (Math.Abs(result) < 0.000000001)
            {
                return "0";
            }

            // Check if the result is effectively a whole number
            if (Math.Abs(result - Math.Round(result)) < 0.000000001)
            {
                // It's a whole number, display without decimal
                return Math.Round(result).ToString("F0");
            }
            else
            {
                // It has decimals, show them but clean up trailing zeros
                string formatted = result.ToString("G15"); // Use G15 for good precision
                Debug.WriteLine($"  Formatted as G15: {formatted}");
                return formatted;
            }
        }
        #endregion
    }
}
