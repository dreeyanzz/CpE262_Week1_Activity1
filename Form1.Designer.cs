namespace calculator
{
    partial class Window
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Screen = new Panel();
            Answer = new Panel();
            Statement = new Panel();
            Keyboard = new Panel();
            Screen.SuspendLayout();
            SuspendLayout();
            // 
            // Screen
            // 
            Screen.BackColor = Color.FromArgb(172, 128, 128);
            Screen.Controls.Add(Answer);
            Screen.Controls.Add(Statement);
            Screen.Location = new Point(45, 33);
            Screen.Name = "Screen";
            Screen.Size = new Size(493, 220);
            Screen.TabIndex = 0;
            Screen.Paint += Screen_Paint;
            // 
            // Answer
            // 
            Answer.BackColor = Color.FromArgb(113, 119, 110);
            Answer.Location = new Point(17, 128);
            Answer.Name = "Answer";
            Answer.Size = new Size(458, 76);
            Answer.TabIndex = 1;
            Answer.Paint += Answer_Paint;
            // 
            // Statement
            // 
            Statement.BackColor = Color.FromArgb(113, 119, 110);
            Statement.Location = new Point(17, 14);
            Statement.Name = "Statement";
            Statement.Size = new Size(458, 108);
            Statement.TabIndex = 0;
            Statement.Paint += Statement_Paint;
            // 
            // Keyboard
            // 
            Keyboard.BackColor = Color.FromArgb(172, 128, 128);
            Keyboard.Location = new Point(45, 277);
            Keyboard.Name = "Keyboard";
            Keyboard.Size = new Size(493, 545);
            Keyboard.TabIndex = 2;
            Keyboard.Paint += Keyboard_Paint;
            // 
            // Window
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(116, 77, 77);
            ClientSize = new Size(582, 853);
            Controls.Add(Keyboard);
            Controls.Add(Screen);
            Name = "Window";
            Text = "Calculator";
            Load += Form1_Load;
            Screen.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel Screen;
        private Panel Answer;
        private Panel Statement;
        private Panel Keyboard;
    }
}
