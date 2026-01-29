using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace CustomControls
{
    public class CircleButton : Button
    {
        // Private fields
        private Color buttonColor = Color.DodgerBlue;
        private Color buttonBorder = Color.Black;
        private Color textColor = Color.White;
        private int borderWidth = 2;
        private bool isHovering = false;
        private bool enableShadow = true;
        private int shadowDepth = 8;
        private Color shadowColor = Color.FromArgb(80, 0, 0, 0);

        // Custom properties
        [Category("Appearance")]
        [Description("The background color of the button")]
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Color ButtonColor
        {
            get { return buttonColor; }
            set
            {
                buttonColor = value;
                Invalidate(); // Redraw the button
            }
        }

        [Category("Appearance")]
        [Description("The border color of the button")]
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Color ButtonBorder
        {
            get { return buttonBorder; }
            set
            {
                buttonBorder = value;
                Invalidate();
            }
        }

        [Category("Appearance")]
        [Description("The color of the button text")]
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Color TextColor
        {
            get { return textColor; }
            set
            {
                textColor = value;
                ForeColor = value; // Also update ForeColor for consistency
                Invalidate();
            }
        }

        [Category("Appearance")]
        [Description("The width of the button border")]
        [Browsable(true)]
        [DefaultValue(2)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public int BorderWidth
        {
            get { return borderWidth; }
            set
            {
                borderWidth = value;
                Invalidate();
            }
        }

        [Category("Appearance")]
        [Description("Enable or disable drop shadow")]
        [Browsable(true)]
        [DefaultValue(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public bool EnableShadow
        {
            get { return enableShadow; }
            set
            {
                enableShadow = value;
                Invalidate();
            }
        }

        [Category("Appearance")]
        [Description("The depth/offset of the shadow")]
        [Browsable(true)]
        [DefaultValue(8)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public int ShadowDepth
        {
            get { return shadowDepth; }
            set
            {
                shadowDepth = value;
                Invalidate();
            }
        }

        [Category("Appearance")]
        [Description("The color of the shadow")]
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Color ShadowColor
        {
            get { return shadowColor; }
            set
            {
                shadowColor = value;
                Invalidate();
            }
        }

        [Category("Appearance")]
        [Description("The text displayed on the button")]
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public string ButtonText
        {
            get { return Text; }
            set
            {
                Text = value;
                Invalidate();
            }
        }

        // Constructor
        public CircleButton()
        {
            // Set default properties
            Size = new Size(100, 100);
            FlatStyle = FlatStyle.Flat;
            FlatAppearance.BorderSize = 0;
            BackColor = Color.Transparent;
            textColor = Color.White;
            ForeColor = Color.White;
            Font = new Font("Inter", 24, FontStyle.Bold); // Larger default font like in Figma

            // Enable double buffering to reduce flickering
            SetStyle(
                ControlStyles.UserPaint
                    | ControlStyles.AllPaintingInWmPaint
                    | ControlStyles.OptimizedDoubleBuffer
                    | ControlStyles.ResizeRedraw
                    | ControlStyles.SupportsTransparentBackColor,
                true
            );

            UpdateStyles();
        }

        // Override OnPaint to draw the circular button
        protected override void OnPaint(PaintEventArgs pevent)
        {
            Graphics graphics = pevent.Graphics;
            graphics.SmoothingMode = SmoothingMode.AntiAlias;
            graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphics.CompositingQuality = CompositingQuality.HighQuality;

            // Calculate the drawing area (leave space for shadow)
            int shadowOffset = enableShadow ? shadowDepth : 0;
            int drawSize = Math.Min(Width, Height) - shadowOffset;
            Rectangle buttonRect = new Rectangle(0, 0, drawSize, drawSize);

            // Determine button color (lighter when hovering)
            Color drawColor = isHovering ? LightenColor(buttonColor, 30) : buttonColor;

            // Draw shadow if enabled
            if (enableShadow && shadowDepth > 0)
            {
                using (GraphicsPath shadowPath = new GraphicsPath())
                {
                    shadowPath.AddEllipse(shadowDepth, shadowDepth, drawSize, drawSize);
                    using (PathGradientBrush shadowBrush = new PathGradientBrush(shadowPath))
                    {
                        shadowBrush.CenterColor = shadowColor;
                        shadowBrush.SurroundColors = new Color[] { Color.Transparent };
                        shadowBrush.FocusScales = new PointF(0.8f, 0.8f);
                        graphics.FillPath(shadowBrush, shadowPath);
                    }
                }
            }

            // Create circular path for the button
            GraphicsPath path = new GraphicsPath();
            path.AddEllipse(buttonRect);
            Region = new Region(path);

            // Fill the circle
            using (SolidBrush brush = new SolidBrush(drawColor))
            {
                graphics.FillEllipse(brush, buttonRect);
            }

            // Draw the border
            if (borderWidth > 0)
            {
                using (Pen pen = new Pen(buttonBorder, borderWidth))
                {
                    Rectangle borderRect = new Rectangle(
                        borderWidth / 2,
                        borderWidth / 2,
                        drawSize - borderWidth,
                        drawSize - borderWidth
                    );
                    graphics.DrawEllipse(pen, borderRect);
                }
            }

            // Draw the text centered with better quality
            if (!string.IsNullOrEmpty(Text))
            {
                using (SolidBrush textBrush = new SolidBrush(textColor))
                {
                    StringFormat stringFormat = new StringFormat();
                    stringFormat.Alignment = StringAlignment.Center;
                    stringFormat.LineAlignment = StringAlignment.Center;
                    stringFormat.Trimming = StringTrimming.EllipsisCharacter;

                    // Draw text in the button area
                    RectangleF textRect = new RectangleF(0, 0, drawSize, drawSize);
                    Font fontToUse = Font ?? new Font("Inter", 24, FontStyle.Bold);
                    graphics.DrawString(Text, fontToUse, textBrush, textRect, stringFormat);
                }
            }
        }

        // Mouse enter event - for hover effect
        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            isHovering = true;
            Invalidate(); // Redraw the button
        }

        // Mouse leave event - for hover effect
        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            isHovering = false;
            Invalidate(); // Redraw the button
        }

        // Helper method to lighten a color (for hover effect)
        private Color LightenColor(Color color, int amount)
        {
            int r = Math.Min(255, color.R + amount);
            int g = Math.Min(255, color.G + amount);
            int b = Math.Min(255, color.B + amount);
            return Color.FromArgb(color.A, r, g, b);
        }

        // Override OnResize to maintain circular shape
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            // Keep the button circular by making width and height equal
            int size = Math.Min(Width, Height);
            Size = new Size(size, size);
        }
    }
}
