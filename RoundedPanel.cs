using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace CustomControls
{
    public class RoundedPanel : Panel
    {
        private int _cornerRadius = 20;
        private int _borderSize = 2;
        private Color _borderColor = Color.DodgerBlue;

        // --- PROPERTIES ---

        // 2. Add this attribute to EVERY public property you want to appear in the Properties window
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public int CornerRadius
        {
            get { return _cornerRadius; }
            set
            {
                _cornerRadius = value;
                Invalidate();
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public int BorderSize
        {
            get { return _borderSize; }
            set
            {
                _borderSize = value;
                Invalidate();
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Color BorderColor
        {
            get { return _borderColor; }
            set
            {
                _borderColor = value;
                Invalidate();
            }
        }

        // --- CONSTRUCTOR & METHODS ---

        public RoundedPanel()
        {
            DoubleBuffered = true;
            ResizeRedraw = true;
            BackColor = Color.White;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            Rectangle rectSurface = ClientRectangle;
            Rectangle rectBorder = Rectangle.Inflate(rectSurface, -_borderSize, -_borderSize);
            int smoothSize = 2;

            if (_borderSize > 0)
                smoothSize = _borderSize;

            if (_cornerRadius > 2)
            {
                using (GraphicsPath pathSurface = GetFigurePath(rectSurface, _cornerRadius))
                using (
                    GraphicsPath pathBorder = GetFigurePath(rectBorder, _cornerRadius - _borderSize)
                )
                using (Pen penSurface = new(Parent?.BackColor ?? Color.Transparent, smoothSize))
                using (Pen penBorder = new(_borderColor, _borderSize))
                {
                    Region = new Region(pathSurface);
                    e.Graphics.DrawPath(penSurface, pathSurface);
                    if (_borderSize >= 1)
                        e.Graphics.DrawPath(penBorder, pathBorder);
                }
            }
            else
            {
                Region = new Region(rectSurface);
                if (_borderSize >= 1)
                {
                    using (Pen penBorder = new(_borderColor, _borderSize))
                    {
                        penBorder.Alignment = PenAlignment.Inset;
                        e.Graphics.DrawRectangle(penBorder, 0, 0, Width - 1, Height - 1);
                    }
                }
            }
        }

        private GraphicsPath GetFigurePath(Rectangle rect, int radius)
        {
            GraphicsPath path = new();
            float curveSize = radius * 2F;

            path.StartFigure();
            path.AddArc(rect.X, rect.Y, curveSize, curveSize, 180, 90);
            path.AddArc(rect.Right - curveSize, rect.Y, curveSize, curveSize, 270, 90);
            path.AddArc(
                rect.Right - curveSize,
                rect.Bottom - curveSize,
                curveSize,
                curveSize,
                0,
                90
            );
            path.AddArc(rect.X, rect.Bottom - curveSize, curveSize, curveSize, 90, 90);
            path.CloseFigure();
            return path;
        }

        protected override void OnResize(EventArgs eventargs)
        {
            base.OnResize(eventargs);
            Invalidate();
        }
    }
}
