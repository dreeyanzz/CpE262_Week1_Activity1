using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace CustomControls
{
    public class ImageButton : UserControl
    {
        private Image normalImage;
        private Image hoverImage;
        private Image pressedImage;
        private bool isHovering = false;
        private bool isPressed = false;
        private string buttonText = "";
        private string text = ""; // Data holder for Text property
        private Font textFont = new Font("Arial", 14, FontStyle.Bold);
        private Color textColor = Color.White;
        private bool showText = true;
        private bool autoSizeToImage = false;

        // Events
        public event EventHandler Click;

        [Category("Appearance")]
        [Description("The image to display in normal state")]
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Image NormalImage
        {
            get { return normalImage; }
            set
            {
                normalImage = value;
                if (autoSizeToImage && normalImage != null)
                {
                    Size = normalImage.Size;
                }
                Invalidate();
            }
        }

        [Category("Appearance")]
        [Description("The image to display when hovering")]
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Image HoverImage
        {
            get { return hoverImage; }
            set
            {
                hoverImage = value;
                Invalidate();
            }
        }

        [Category("Appearance")]
        [Description("The image to display when pressed")]
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Image PressedImage
        {
            get { return pressedImage; }
            set
            {
                pressedImage = value;
                Invalidate();
            }
        }

        [Category("Appearance")]
        [Description("The text to display on the button")]
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public string ButtonText
        {
            get { return buttonText; }
            set
            {
                buttonText = value;
                Invalidate();
            }
        }

        [Category("Appearance")]
        [Description("The font for the button text")]
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Font TextFont
        {
            get { return textFont; }
            set
            {
                textFont = value;
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
                Invalidate();
            }
        }

        [Category("Appearance")]
        [Description("Show or hide text overlay")]
        [Browsable(true)]
        [DefaultValue(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public bool ShowText
        {
            get { return showText; }
            set
            {
                showText = value;
                Invalidate();
            }
        }

        [Category("Data")]
        [Description("Text data associated with this button (not displayed)")]
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public string Text
        {
            get { return text; }
            set { text = value; }
        }

        [Category("Behavior")]
        [Description("Automatically resize control to match the image size")]
        [Browsable(true)]
        [DefaultValue(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public bool AutoSizeToImage
        {
            get { return autoSizeToImage; }
            set
            {
                autoSizeToImage = value;
                if (autoSizeToImage && normalImage != null)
                {
                    Size = normalImage.Size;
                }
                Invalidate();
            }
        }

        public ImageButton()
        {
            Size = new Size(100, 100);
            BackColor = Color.Transparent; // ✅ Make background transparent!

            // Enable double buffering
            SetStyle(
                ControlStyles.UserPaint
                    | ControlStyles.AllPaintingInWmPaint
                    | ControlStyles.OptimizedDoubleBuffer
                    | ControlStyles.ResizeRedraw
                    | ControlStyles.SupportsTransparentBackColor
                    | // ✅ Support transparency
                    ControlStyles.Selectable,
                true
            );

            DoubleBuffered = true;
            UpdateStyles();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;

            // Determine which image to draw
            Image imageToDraw = normalImage;

            if (isPressed && pressedImage != null)
                imageToDraw = pressedImage;
            else if (isHovering && hoverImage != null)
                imageToDraw = hoverImage;

            // Draw the image at its actual size (no stretching!)
            if (imageToDraw != null)
            {
                g.DrawImage(imageToDraw, 0, 0, imageToDraw.Width, imageToDraw.Height);
            }

            // Draw text if enabled
            if (showText && !string.IsNullOrEmpty(buttonText))
            {
                using (SolidBrush textBrush = new SolidBrush(textColor))
                {
                    StringFormat sf = new StringFormat();
                    sf.Alignment = StringAlignment.Center;
                    sf.LineAlignment = StringAlignment.Center;

                    g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
                    g.DrawString(
                        buttonText,
                        textFont,
                        textBrush,
                        new RectangleF(0, 0, Width, Height),
                        sf
                    );
                }
            }
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            isHovering = true;
            Cursor = Cursors.Hand;
            Invalidate();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            isHovering = false;
            isPressed = false;
            Invalidate();
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Button == MouseButtons.Left)
            {
                isPressed = true;
                Invalidate();
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            if (e.Button == MouseButtons.Left)
            {
                isPressed = false;
                Invalidate();

                // Trigger click event if mouse is still over the button
                if (ClientRectangle.Contains(e.Location))
                {
                    Click?.Invoke(this, EventArgs.Empty);
                }
            }
        }
    }
}
