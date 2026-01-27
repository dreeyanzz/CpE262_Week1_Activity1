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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Window));
            Keyboard = new Panel();
            Screen = new Panel();
            Answer = new Panel();
            Statement = new Panel();
            Screen.SuspendLayout();
            SuspendLayout();
            // 
            // Keyboard
            // 
            Keyboard.BackgroundImage = (Image)resources.GetObject("Keyboard.BackgroundImage");
            Keyboard.Location = new Point(21, 277);
            Keyboard.Name = "Keyboard";
            Keyboard.Size = new Size(490, 602);
            Keyboard.TabIndex = 3;
            Keyboard.Paint += Keyboard_Paint;
            // 
            // Screen
            // 
            Screen.BackColor = Color.Transparent;
            Screen.BackgroundImage = (Image)resources.GetObject("Screen.BackgroundImage");
            Screen.Controls.Add(Answer);
            Screen.Controls.Add(Statement);
            Screen.Location = new Point(21, 39);
            Screen.Name = "Screen";
            Screen.Size = new Size(490, 230);
            Screen.TabIndex = 4;
            // 
            // Answer
            // 
            Answer.BackgroundImage = (Image)resources.GetObject("Answer.BackgroundImage");
            Answer.Location = new Point(18, 139);
            Answer.Name = "Answer";
            Answer.Size = new Size(454, 73);
            Answer.TabIndex = 1;
            Answer.Paint += Answer_Paint;
            // 
            // Statement
            // 
            Statement.BackgroundImage = (Image)resources.GetObject("Statement.BackgroundImage");
            Statement.Location = new Point(18, 18);
            Statement.Name = "Statement";
            Statement.Size = new Size(454, 120);
            Statement.TabIndex = 0;
            Statement.Paint += Statement_Paint;
            // 
            // Window
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(116, 77, 77);
            ClientSize = new Size(540, 910);
            Controls.Add(Screen);
            Controls.Add(Keyboard);
            Name = "Window";
            Text = "Calculator";
            Load += Form1_Load;
            Screen.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion
        private Panel Keyboard;
        private Panel Screen;
        private Panel Statement;
        private Panel Answer;
    }
}
