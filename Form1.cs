using System.Data;
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
        };
        #endregion

        #region Button Layout
        private readonly List<List<string>> keys =
        [
            ["AC", "n²", "⌫", "÷"],
            ["7", "8", "9", "×"],
            ["4", "5", "6", "-"],
            ["1", "2", "3", "+"],
            ["0", ".", "Ans", "="],
        ];
        #endregion

        #region Button Categories
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

        List<Panel> numKeys = [];

        #region UI Components
        private readonly Label stmt_label = new();
        private readonly Label answer_label = new();
        #endregion

        #region State
        private string fullExpression = "0";
        private double lastResult = 0.0;
        private bool justCalculated = false;
        #endregion

        #region Initialization
        public Window()
        {
            InitializeComponent();

            ClientSize = new Size(540, 910);

            InitializeLabel(stmt_label, Statement);
            InitializeLabel(answer_label, Answer);
        }

        private void InitializeLabel(Label label, Panel parent)
        {
            label.Text = label == stmt_label ? "0" : "";
            label.Font = new Font("Inter", LABEL_FONT_SIZE, FontStyle.Regular);
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
            if (sender is Panel panel)
                panel.Controls.Add(stmt_label);
        }

        private void Answer_Paint(object sender, PaintEventArgs e)
        {
            if (sender is Panel panel)
                panel.Controls.Add(answer_label);
        }

        private void Keyboard_Paint(object sender, PaintEventArgs e)
        {
            if (sender is not Panel kb)
                return;

            CreateKeyboardButtons(kb); // ❌ This runs EVERY paint! Will create duplicate buttons!
        }
        #endregion

        #region Button Creation
        private void CreateKeyboardButtons(Panel keyboard)
        {
            int workWidth = keyboard.Width - PADDING * 2;
            int workHeight = keyboard.Height - PADDING * 2;

            for (int row = 0; row < keys.Count; row++)
            {
                for (int col = 0; col < keys[row].Count; col++)
                {
                    int posX = PADDING + workWidth / 4 * col + PADDING;
                    int posY = PADDING + workHeight / 5 * row + PADDING;

                    keyboard.Controls.Add(CreateButton(keys[row][col], posX, posY));
                }
            }
        }

        private ImageButton CreateButton(string text, int x, int y)
        {
            ImageButton btn = new();
            btn.Text = text;
            btn.Location = new Point(x - 8, y - 8);

            Image? img = GetButtonImage(text);
            if (img != null)
            {
                btn.NormalImage = img;
                btn.Size = new Size(img.Width, img.Height); // Explicit size from image
                btn.ShowText = false;
            }

            btn.Click += Button_Click!;
            return btn;
        }

        private const string IMAGE_FOLDER = "keyboard-keys";

        public static Image? GetButtonImage(string buttonText)
        {
            if (
                imagePathMap.TryGetValue(buttonText, out string? path)
                && !string.IsNullOrEmpty(path)
            )
            {
                string fullPath = Path.Combine(IMAGE_FOLDER, path);
                if (File.Exists(fullPath))
                {
                    return Image.FromFile(fullPath);
                }
                else
                {
                    Console.WriteLine($"Image not found: {fullPath}");
                }
            }
            return null;
        }

        #endregion

        #region Display Management
        private void UpdateDisplay()
        {
            stmt_label.Text =
                fullExpression.Length > MAX_DISPLAY_LENGTH
                    ? fullExpression.Substring(fullExpression.Length - MAX_DISPLAY_LENGTH)
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
            if (text == "0")
                return true;

            if (HasConsecutiveOperators(text))
                return false;

            return ValidateTerms(text);
        }

        private static bool HasConsecutiveOperators(string text)
        {
            return Regex.IsMatch(text, @"[+\-×÷]{2,}");
        }

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

            while (text.Length > 1 && text.EndsWith('0'))
            {
                text = text[..^1];
            }

            if (text.Length > 1 && text.EndsWith('.'))
                text = text[..^1];

            return text;
        }
        #endregion

        #region Event Handlers - Button Click
        private void Button_Click(object sender, EventArgs e)
        {
            if (sender is not ImageButton btn)
                return;

            RouteButtonAction(btn.Text);
        }

        private void RouteButtonAction(string buttonText)
        {
            if (buttonText != "=" && !operators.Contains(buttonText))
            {
                answer_label.Text = "";
            }

            switch (buttonText)
            {
                case "AC":
                    HandleClear();
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

        #region Input Handlers]
        private void HandleDisplayableInput(string input)
        {
            if (input == "-" && stmt_label.Text == "0")
            {
                stmt_label.Text = "-";
                fullExpression = "-";
                return;
            }

            // If just calculated and user types a number or decimal, start fresh
            if (
                justCalculated
                && (numbers.Contains(input) || input == "." || operators.Contains(input))
            )
            {
                fullExpression = answer_label.Text;
                answer_label.Text = "";
                justCalculated = false;

                if (input == ".")
                {
                    fullExpression += "0";
                }
            }

            if (input == "." && !numbers.Contains(fullExpression.Last().ToString()))
            {
                fullExpression += "0";
            }

            string newText = BuildNewExpression(input);

            if (ShouldTrimBeforeOperator(input))
            {
                string trimmed = TrimTrailingZeros(fullExpression);
                newText = trimmed + input;
            }

            if (
                IsValidStatement(newText)
                || (newText.Last().ToString() == "-")
                || (newText[^2].ToString() == "-")
            )
            {
                fullExpression = newText;

                // Only reset justCalculated for non-operators
                if (!operators.Contains(input))
                {
                    justCalculated = false;
                }

                UpdateDisplay();
            }
        }

        private string BuildNewExpression(string input)
        {
            if (input == "n²")
                return fullExpression + "²";

            if (ShouldReplaceExpression(input))
                return input;

            return fullExpression + input;
        }

        private bool ShouldReplaceExpression(string input)
        {
            return fullExpression == "0" && !operators.Contains(input) && input != ".";
        }

        private bool ShouldTrimBeforeOperator(string input)
        {
            return operators.Contains(input) && fullExpression.EndsWith('0');
        }

        private void HandleClear()
        {
            ClearDisplay();
        }

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

                // Check for invalid results (NaN, Infinity)
                if (double.IsNaN(lastResult) || double.IsInfinity(lastResult))
                {
                    answer_label.Text = "Math Error";
                    justCalculated = false;
                    lastResult = 0.0;
                }
                else
                {
                    answer_label.Text = FormatResult(lastResult);
                    justCalculated = true;
                }
            }
            catch (DivideByZeroException)
            {
                answer_label.Text = "Cannot divide by zero";
                justCalculated = false;
                lastResult = 0.0;
            }
            catch (Exception)
            {
                answer_label.Text = "Error";
                justCalculated = false;
                lastResult = 0.0;
            }
        }

        private void HandleAnswer()
        {
            if (justCalculated)
            {
                ReplaceWithLastResult();
            }
            else
            {
                AppendLastResult();
            }

            UpdateDisplay();
        }

        private void ReplaceWithLastResult()
        {
            fullExpression = lastResult.ToString();
            answer_label.Text = "";
            justCalculated = false;
        }

        private void AppendLastResult()
        {
            if (fullExpression == "0")
                fullExpression = lastResult.ToString();
            else
                fullExpression += lastResult.ToString();
        }
        #endregion

        #region Expression Evaluation
        private static double EvaluateExpression(string expression)
        {
            expression = ProcessSquares(expression);
            expression = NormalizeOperators(expression);

            //// Remove trailing operators before evaluation
            // expression = Regex.Replace(expression, @"[+\-*/]+$", "");

            object result = new DataTable().Compute(expression, null);
            return Convert.ToDouble(result);
        }

        private static string ProcessSquares(string expression)
        {
            return Regex.Replace(
                expression,
                @"(\d+(?:\.\d+)?)²",
                m =>
                {
                    double num = double.Parse(m.Groups[1].Value);
                    return (num * num).ToString();
                }
            );
        }

        private static string NormalizeOperators(string expression)
        {
            return expression.Replace("×", "*").Replace("÷", "/");
        }

        private static string FormatResult(double result)
        {
            return result.ToString();
        }
        #endregion
    }
}
