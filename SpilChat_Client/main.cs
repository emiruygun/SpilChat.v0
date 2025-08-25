using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChatApplication
{
    // ---------------------- Login Form ----------------------
    public partial class LoginForm : Form
    {
        private Panel leftPanel;
        private Panel rightPanel;
        private Panel loginPanel;
        private Label titleLabel;
        private Label welcomeLabel;
        private Label usernameLabel;
        private Label passwordLabel;
        private FlatTextBox usernameTextBox;
        private FlatTextBox passwordTextBox;
        private Button loginButton;
        private Button cancelButton;
        private Label forgotLabel;

        public string LoggedInUsername { get; private set; }

        public LoginForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            // === FORM ===
            this.Text = "SpilChat - Giriş";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.White;
            this.FormBorderStyle = FormBorderStyle.None;
            this.ClientSize = new Size(900, 600);
            this.Font = new Font("Segoe UI", 9f);

            // === SOL PANEL (Gradient Background) ===
            leftPanel = new GradientPanel
            {
                Size = new Size(500, 600),
                Location = new Point(0, 0),
                StartColor = Color.FromArgb(67, 56, 202),
                EndColor = Color.FromArgb(139, 92, 246)
            };

            // Hoş geldin mesajı
            var welcomeTitle = new Label
            {
                Text = "SpilChat'e\nHoş Geldiniz!",
                Font = new Font("Segoe UI", 32f, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = true,
                Location = new Point(60, 180),
                BackColor = Color.Transparent
            };

            var welcomeSubtitle = new Label
            {
                Text = "Arkadaşlarınızla sohbet etmeye başlayın.\nGüvenli, hızlı ve kolay mesajlaşma deneyimi.",
                Font = new Font("Segoe UI", 14f, FontStyle.Regular),
                ForeColor = Color.FromArgb(200, 200, 255),
                AutoSize = true,
                Location = new Point(60, 290),
                BackColor = Color.Transparent
            };

            // Dekoratif elementler
            var circle1 = new CirclePanel
            {
                Size = new Size(120, 120),
                Location = new Point(350, 80),
                BackColor = Color.FromArgb(50, 255, 255, 255)
            };

            var circle2 = new CirclePanel
            {
                Size = new Size(80, 80),
                Location = new Point(380, 450),
                BackColor = Color.FromArgb(30, 255, 255, 255)
            };

            leftPanel.Controls.AddRange(new Control[] { welcomeTitle, welcomeSubtitle, circle1, circle2 });

            // === SAĞ PANEL (Form Area) ===
            rightPanel = new Panel
            {
                Size = new Size(400, 600),
                Location = new Point(500, 0),
                BackColor = Color.White
            };

            // === LOGIN PANEL ===
            loginPanel = new Panel
            {
                Size = new Size(320, 450),
                Location = new Point(40, 75),
                BackColor = Color.Transparent
            };

            // Başlık
            titleLabel = new Label
            {
                Text = "Giriş Yap",
                Font = new Font("Segoe UI", 28f, FontStyle.Bold),
                ForeColor = Color.FromArgb(31, 41, 55),
                AutoSize = true,
                Location = new Point(0, 0)
            };

            welcomeLabel = new Label
            {
                Text = "Hesabınıza giriş yaparak devam edin",
                Font = new Font("Segoe UI", 12f, FontStyle.Regular),
                ForeColor = Color.FromArgb(107, 114, 128),
                AutoSize = true,
                Location = new Point(0, 45)
            };

            // Kullanıcı adı
            usernameLabel = new Label
            {
                Text = "Kullanıcı Adı",
                Font = new Font("Segoe UI", 11f, FontStyle.Regular),
                ForeColor = Color.FromArgb(55, 65, 81),
                AutoSize = true,
                Location = new Point(0, 100)
            };

            usernameTextBox = new FlatTextBox
            {
                Font = new Font("Segoe UI", 12f),
                Location = new Point(0, 125),
                Size = new Size(320, 50),
                PlaceholderText = "Kullanıcı adınızı girin"
            };

            // Şifre
            passwordLabel = new Label
            {
                Text = "Şifre",
                Font = new Font("Segoe UI", 11f, FontStyle.Regular),
                ForeColor = Color.FromArgb(55, 65, 81),
                AutoSize = true,
                Location = new Point(0, 190)
            };

            passwordTextBox = new FlatTextBox
            {
                Font = new Font("Segoe UI", 12f),
                Location = new Point(0, 215),
                Size = new Size(320, 50),
                UseSystemPasswordChar = true,
                PlaceholderText = "Şifrenizi girin"
            };

            /* Şifremi unuttum
            forgotLabel = new Label
            {
                Text = "Şifremi unuttum?",
                Font = new Font("Segoe UI", 10f, FontStyle.Regular),
                ForeColor = Color.FromArgb(67, 56, 202),
                AutoSize = true,
                Location = new Point(220, 280),
                Cursor = Cursors.Hand
            };
            forgotLabel.MouseEnter += (s, e) => forgotLabel.Font = new Font("Segoe UI", 10f, FontStyle.Underline);
            forgotLabel.MouseLeave += (s, e) => forgotLabel.Font = new Font("Segoe UI", 10f, FontStyle.Regular);*/

            // Giriş butonu
            loginButton = new GradientButton
            {
                Text = "Giriş Yap",
                Font = new Font("Segoe UI", 13f, FontStyle.Bold),
                Size = new Size(320, 52),
                Location = new Point(0, 310),
                StartColor = Color.FromArgb(67, 56, 202),
                EndColor = Color.FromArgb(139, 92, 246),
                ForeColor = Color.White
            };
            loginButton.Click += LoginButton_Click;

            // İptal butonu
            cancelButton = new OutlineButton
            {
                Text = "İptal",
                Font = new Font("Segoe UI", 12f, FontStyle.Regular),
                Size = new Size(320, 48),
                Location = new Point(0, 375),
                BorderColor = Color.FromArgb(209, 213, 219),
                ForeColor = Color.FromArgb(107, 114, 128)
            };
            cancelButton.Click += (s, e) => this.Close();

            // Kısayollar
            this.AcceptButton = loginButton;
            this.CancelButton = cancelButton;

            loginPanel.Controls.AddRange(new Control[] {
                titleLabel, welcomeLabel,
                usernameLabel, usernameTextBox,
                passwordLabel, passwordTextBox,
                forgotLabel, loginButton, cancelButton
            });

            rightPanel.Controls.Add(loginPanel);

            // Ana kontroller
            this.Controls.AddRange(new Control[] { leftPanel, rightPanel });

            // Kapatma butonu
            var closeButton = new Button
            {
                Text = "✕",
                Font = new Font("Segoe UI", 14f, FontStyle.Regular),
                Size = new Size(40, 40),
                Location = new Point(850, 10),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.Transparent,
                ForeColor = Color.FromArgb(107, 114, 128),
                Cursor = Cursors.Hand
            };
            closeButton.FlatAppearance.BorderSize = 0;
            closeButton.FlatAppearance.MouseOverBackColor = Color.FromArgb(240, 240, 240);
            closeButton.Click += (s, e) => this.Close();
            this.Controls.Add(closeButton);

            // Form taşıma
            this.MouseDown += Form_MouseDown;
            this.MouseMove += Form_MouseMove;
            this.MouseUp += Form_MouseUp;
            leftPanel.MouseDown += Form_MouseDown;
            leftPanel.MouseMove += Form_MouseMove;
            leftPanel.MouseUp += Form_MouseUp;
            rightPanel.MouseDown += Form_MouseDown;
            rightPanel.MouseMove += Form_MouseMove;
            rightPanel.MouseUp += Form_MouseUp;
        }

        // Form taşıma
        private bool isDragging = false;
        private Point lastCursor;
        private Point lastForm;

        private void Form_MouseDown(object sender, MouseEventArgs e)
        {
            isDragging = true;
            lastCursor = Cursor.Position;
            lastForm = this.Location;
        }

        private void Form_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging)
            {
                var delta = Point.Subtract(Cursor.Position, new Size(lastCursor));
                this.Location = Point.Add(lastForm, new Size(delta));
            }
        }

        private void Form_MouseUp(object sender, MouseEventArgs e)
        {
            isDragging = false;
        }

        private void LoginButton_Click(object sender, EventArgs e)
        {
            var user = usernameTextBox.Text.Trim();
            var pass = passwordTextBox.Text;

            if (string.IsNullOrWhiteSpace(user) || string.IsNullOrWhiteSpace(pass))
            {
                MessageBox.Show("Lütfen kullanıcı adı ve şifreyi girin.", "Eksik Bilgi",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            LoggedInUsername = user;
            DialogResult = DialogResult.OK;
        }
    }

    // Gradient Panel
    public class GradientPanel : Panel
    {
        public Color StartColor { get; set; } = Color.Blue;
        public Color EndColor { get; set; } = Color.Purple;

        protected override void OnPaint(PaintEventArgs e)
        {
            using (var brush = new LinearGradientBrush(ClientRectangle, StartColor, EndColor, 45f))
            {
                e.Graphics.FillRectangle(brush, ClientRectangle);
            }
            base.OnPaint(e);
        }
    }

    // Yuvarlak Panel
    public class CirclePanel : Panel
    {
        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            using (var brush = new SolidBrush(BackColor))
            {
                e.Graphics.FillEllipse(brush, 0, 0, Width - 1, Height - 1);
            }
        }
    }

    // Flat TextBox (arama ikonu destekli)
    public class FlatTextBox : Panel
    {
        private readonly TextBox textBox;
        private string _placeholderText = "";
        private bool _useSystemPasswordChar;

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, string lParam);
        private const int EM_SETCUEBANNER = 0x1501;

        // ---- Arama ikonu ayarları ----
        public bool ShowSearchIcon { get; set; } = false;
        public int IconSize { get; set; } = 12;
        public int IconPaddingLeft { get; set; } = 12;
        public Color IconColor { get; set; } = Color.FromArgb(156, 163, 175);

        private int TextLeft => ShowSearchIcon ? (IconPaddingLeft + IconSize + 8) : 15;

        public FlatTextBox()
        {
            SetStyle(ControlStyles.UserPaint |
                     ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw, true);

            BackColor = Color.FromArgb(249, 250, 251);

            textBox = new TextBox
            {
                BorderStyle = BorderStyle.None,
                BackColor = Color.FromArgb(249, 250, 251),
                Location = new Point(15, 0),
                Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top
            };
            textBox.Font = base.Font; // ilk senkron

            textBox.GotFocus += (s, e) => { BackColor = Color.White; Invalidate(); };
            textBox.LostFocus += (s, e) => { BackColor = Color.FromArgb(249, 250, 251); Invalidate(); };
            textBox.TextChanged += (s, e) => { Invalidate(); OnTextChanged(e); };

            // Placeholder'ı odakta bile göster
            textBox.HandleCreated += (s, e) => UpdateCueBanner();

            Controls.Add(textBox);
            Click += (s, e) => textBox.Focus();

            Resize += (s, e) => LayoutInnerTextBox();
            LayoutInnerTextBox();
        }

        private void UpdateCueBanner()
        {
            if (textBox != null && textBox.IsHandleCreated)
                SendMessage(textBox.Handle, EM_SETCUEBANNER, (IntPtr)1, _placeholderText ?? "");
        }

        public override string Text
        {
            get => textBox?.Text ?? "";
            set { if (textBox != null) textBox.Text = value; }
        }

        public override Font Font
        {
            get => base.Font;
            set
            {
                base.Font = value;
                if (textBox != null) textBox.Font = value;
                Invalidate();
            }
        }

        public string PlaceholderText
        {
            get => _placeholderText;
            set { _placeholderText = value ?? ""; UpdateCueBanner(); Invalidate(); }
        }

        public bool UseSystemPasswordChar
        {
            get => _useSystemPasswordChar;
            set
            {
                _useSystemPasswordChar = value;
                if (textBox != null) textBox.UseSystemPasswordChar = value;
                Invalidate();
            }
        }

        protected override void OnFontChanged(EventArgs e)
        {
            base.OnFontChanged(e);
            if (textBox != null) textBox.Font = base.Font;
            Invalidate();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            LayoutInnerTextBox();
        }

        private void LayoutInnerTextBox()
        {
            int h = textBox?.PreferredHeight ?? 24;
            int y = Math.Max((Height - h) / 2, 8);
            textBox.Bounds = new Rectangle(TextLeft, y, Math.Max(Width - TextLeft - 15, 10), h);
            textBox.UseSystemPasswordChar = _useSystemPasswordChar;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var rect = new Rectangle(0, 0, Width - 1, Height - 1);
            var borderColor = textBox.Focused ? Color.FromArgb(67, 56, 202) : Color.FromArgb(209, 213, 219);

            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            using (var brush = new SolidBrush(BackColor))
                e.Graphics.FillRoundedRectangle(brush, rect, 8);

            using (var pen = new Pen(borderColor, textBox.Focused ? 2 : 1))
                e.Graphics.DrawRoundedRectangle(pen, rect, 8);

            // Büyüteç ikonu
            if (ShowSearchIcon)
            {
                float cx = IconPaddingLeft + IconSize / 2f;
                float cy = Height / 2f;
                float r = IconSize * 0.38f;

                using (var p = new Pen(IconColor, 1.6f))
                {
                    e.Graphics.DrawEllipse(p, cx - r, cy - r, r * 2f, r * 2f);
                    float hx1 = cx + r * 0.7f;
                    float hy1 = cy + r * 0.7f;
                    float hx2 = hx1 + IconSize * 0.55f;
                    float hy2 = hy1 + IconSize * 0.55f;
                    e.Graphics.DrawLine(p, hx1, hy1, hx2, hy2);
                }
            }

            // (Panel üzerine placeholder çizimi istersen kalsın; CueBanner zaten var)
            if (string.IsNullOrEmpty(textBox.Text) &&
                !string.IsNullOrEmpty(_placeholderText) &&
                !textBox.Focused)
            {
                using (var brush = new SolidBrush(Color.FromArgb(156, 163, 175)))
                    e.Graphics.DrawString(_placeholderText, base.Font, brush,
                                          (float)TextLeft, (Height - base.Font.Height) / 2f);
            }
        }
    }

    // Gradient Button
    public class GradientButton : Button
    {
        public Color StartColor { get; set; } = Color.Blue;
        public Color EndColor { get; set; } = Color.Purple;
        private bool isHovered = false;

        public GradientButton()
        {
            SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);
            FlatStyle = FlatStyle.Flat;
            FlatAppearance.BorderSize = 0;
            Cursor = Cursors.Hand;

            MouseEnter += (s, e) => { isHovered = true; Invalidate(); };
            MouseLeave += (s, e) => { isHovered = false; Invalidate(); };
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            var rect = new Rectangle(0, 0, Width - 1, Height - 1);
            var startColor = isHovered ? Color.FromArgb(55, 48, 163) : StartColor;
            var endColor = isHovered ? Color.FromArgb(124, 58, 237) : EndColor;

            using (var brush = new LinearGradientBrush(rect, startColor, endColor, 45f))
            {
                e.Graphics.FillRoundedRectangle(brush, rect, 8);
            }

            var textRect = new Rectangle(0, 0, Width, Height);
            using (var brush = new SolidBrush(ForeColor))
            {
                var sf = new StringFormat
                {
                    Alignment = StringAlignment.Center,
                    LineAlignment = StringAlignment.Center
                };
                e.Graphics.DrawString(Text, Font, brush, textRect, sf);
            }
        }
    }

    // Outline Button
    public class OutlineButton : Button
    {
        public Color BorderColor { get; set; } = Color.Gray;
        private bool isHovered = false;

        public OutlineButton()
        {
            SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);
            FlatStyle = FlatStyle.Flat;
            FlatAppearance.BorderSize = 0;
            BackColor = Color.White;
            Cursor = Cursors.Hand;

            MouseEnter += (s, e) => { isHovered = true; Invalidate(); };
            MouseLeave += (s, e) => { isHovered = false; Invalidate(); };
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            var rect = new Rectangle(0, 0, Width - 1, Height - 1);
            var bgColor = isHovered ? Color.FromArgb(249, 250, 251) : Color.White;

            using (var brush = new SolidBrush(bgColor))
            {
                e.Graphics.FillRoundedRectangle(brush, rect, 8);
            }

            using (var pen = new Pen(BorderColor, 1))
            {
                e.Graphics.DrawRoundedRectangle(pen, rect, 8);
            }

            var textRect = new Rectangle(0, 0, Width, Height);
            using (var brush = new SolidBrush(ForeColor))
            {
                var sf = new StringFormat
                {
                    Alignment = StringAlignment.Center,
                    LineAlignment = StringAlignment.Center
                };
                e.Graphics.DrawString(Text, Font, brush, textRect, sf);
            }
        }
    }

    // Graphics Extensions (yüksek kaliteli, sınır içe hizalı)
    public static class GraphicsExtensions
    {
        public static void FillRoundedRectangle(this Graphics g, Brush brush, Rectangle rect, int radius)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            g.CompositingQuality = CompositingQuality.HighQuality;

            using (var path = GetRoundedRect(new RectangleF(rect.X, rect.Y, rect.Width, rect.Height), radius))
                g.FillPath(brush, path);
        }

        public static void DrawRoundedRectangle(this Graphics g, Pen pen, Rectangle rect, int radius)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            g.CompositingQuality = CompositingQuality.HighQuality;

            var r = new RectangleF(
                rect.X + pen.Width / 2f,
                rect.Y + pen.Width / 2f,
                rect.Width - pen.Width,
                rect.Height - pen.Width
            );

            float rad = Math.Max(0.1f, radius - pen.Width / 2f);

            try { pen.Alignment = PenAlignment.Inset; } catch { }

            using (var path = GetRoundedRect(r, rad))
                g.DrawPath(pen, path);
        }

        private static GraphicsPath GetRoundedRect(RectangleF rect, float radius)
        {
            float d = radius * 2f;
            var path = new GraphicsPath();
            if (radius <= 0f) { path.AddRectangle(rect); path.CloseFigure(); return path; }

            path.AddArc(rect.X, rect.Y, d, d, 180, 90);
            path.AddArc(rect.Right - d, rect.Y, d, d, 270, 90);
            path.AddArc(rect.Right - d, rect.Bottom - d, d, d, 0, 90);
            path.AddArc(rect.X, rect.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }
    }

    // ---------------------- Yuvarlak Panel (baloncuk için) ----------------------
    public class RoundedPanel : Panel
    {
        public int CornerRadius { get; set; } = 18;
        public Color FillColor { get; set; } = Color.White;
        public Color BorderColor { get; set; } = Color.Transparent;
        public float BorderWidth { get; set; } = 0f;

        public RoundedPanel()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.UserPaint |
                     ControlStyles.OptimizedDoubleBuffer, true);
            BackColor = Color.Transparent;
            Padding = new Padding(12, 8, 12, 8);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var rect = new Rectangle(0, 0, Width - 1, Height - 1);

            using (var b = new SolidBrush(FillColor))
                e.Graphics.FillRoundedRectangle(b, rect, CornerRadius);

            if (BorderWidth > 0f && BorderColor.A > 0)
            {
                using (var p = new Pen(BorderColor, BorderWidth))
                    e.Graphics.DrawRoundedRectangle(p, rect, CornerRadius);
            }

            base.OnPaint(e);
        }
    }

    // ---------------------- Chat Bubble ----------------------
    public class ChatBubble : UserControl
    {
        private Panel bubble;
        private RichTextBox rtb;
        private Label lblTime;

        public bool Outgoing { get; }
        public DateTime Time { get; }

        // --- Yeni: güvenli property'ler + init bayrağı ---
        private bool _built = false;

        private int _maxBubbleWidth = 420;

        public int MaxBubbleWidth
        {
            get => _maxBubbleWidth;
            set
            {
                if (_maxBubbleWidth != value)
                {
                    _maxBubbleWidth = value;
                    if (_built) Relayout();   // rtb hazırsa yeniden yerleş
                }
            }
        }

        private int _minBubbleWidth = 80;
        public int MinBubbleWidth
        {
            get => _minBubbleWidth;
            set
            {
                if (_minBubbleWidth != value)
                {
                    _minBubbleWidth = value;
                    if (_built) Relayout();
                }
            }
        }

        public string MessageText
        {
            get => rtb.Text;
            set { rtb.Text = value ?? ""; if (_built) Relayout(); }
        }

        public ChatBubble(bool outgoing, string text, DateTime time, int maxWidth)
        {
            DoubleBuffered = true;
            BackColor = Color.Transparent;

            Outgoing = outgoing;
            MaxBubbleWidth = maxWidth;

            var bubbleColor = outgoing
                ? Color.FromArgb(114, 88, 242)   // mor
                : Color.FromArgb(245, 246, 250); // açık gri

            // 1) ÖNCE kontrolleri oluştur
            rtb = new RichTextBox
            {
                BorderStyle = BorderStyle.None,
                ReadOnly = true,
                DetectUrls = true,
                ScrollBars = RichTextBoxScrollBars.None,
                ShortcutsEnabled = true,
                WordWrap = true,
                Font = new Font("Segoe UI", 11.5f, FontStyle.Regular),
                BackColor = bubbleColor,
                ForeColor = outgoing ? Color.White : Color.Black
            };
            rtb.LinkClicked += (s, e) => OpenUrl(e.LinkText);

            lblTime = new Label
            {
                AutoSize = true,
                Text = time.ToString("HH:mm"),
                BackColor = bubbleColor,
                ForeColor = outgoing ? Color.White : Color.Black,
                Font = new Font(SystemFonts.MessageBoxFont, FontStyle.Regular)
            };

            // 2) Sonra paneli oluştur
            bubble = new BubblePanel
            {
                Padding = new Padding(12, 8, 12, 8),
                FillColor = bubbleColor,
                BorderColor = bubbleColor,
                CornerRadius = 14,
                BorderWidth = 1.25f,
                BackColor = Color.Transparent
            };

            // 3) Hiyerarşi
            bubble.Controls.Add(rtb);
            bubble.Controls.Add(lblTime);
            Controls.Add(bubble);

            // 4) -- EKLE --
            _built = true;                 // << ilk kez burada true yap
            MessageText = text;            // text’i ver
            Relayout();                    // ve yerleşimi çalıştır

        }


        private void Relayout()
        {
            if (rtb == null || bubble == null) return; // KORUMA

            int w = ComputeDesiredWidth(rtb.Text, rtb.Font, MaxBubbleWidth, MinBubbleWidth);

            rtb.Location = new Point(12, 8);
            rtb.Width = w;
            rtb.Height = MeasureRtbHeight(rtb, rtb.Width);

            lblTime.Location = new Point(12, rtb.Bottom + 2);

            bubble.Size = new Size(rtb.Width + bubble.Padding.Horizontal,
                                   lblTime.Bottom + bubble.Padding.Bottom);

            Width = bubble.Width;
            Height = bubble.Height;
            AlignInParent();
        }

        private int ComputeDesiredWidth(string text, Font font, int maxWidth, int minWidth)
        {
            if (string.IsNullOrEmpty(text)) return minWidth;
            using (var g = CreateGraphics())
            {
                var sf = new StringFormat(StringFormatFlags.LineLimit) { Trimming = StringTrimming.Word };
                SizeF sz = g.MeasureString(text, font, maxWidth, sf);
                int desired = (int)Math.Ceiling(sz.Width) + 2;
                return Math.Max(minWidth, Math.Min(desired, maxWidth));
            }
        }

        public void AlignInParent()
        {
            if (Parent == null) return;
            int x = Outgoing ? Parent.ClientSize.Width - Width - 10 : 10;
            Location = new Point(x, Location.Y);
        }
        protected override void OnParentChanged(EventArgs e)
        {
            base.OnParentChanged(e);
            if (Parent != null) Parent.Resize += (s, ev) => AlignInParent();
        }

        private static int MeasureRtbHeight(RichTextBox box, int width)
        {
            box.Width = width;
            box.Height = 5;
            if (box.TextLength == 0) return box.Font.Height + 6;

            int last = Math.Max(0, box.TextLength - 1);
            var pos = box.GetPositionFromCharIndex(last);
            return Math.Max(pos.Y + box.Font.Height + 2, box.Font.Height + 6);
        }
        private static GraphicsPath RoundedRect(Rectangle r, int radius)
        {
            int d = Math.Max(0, radius) * 2;
            var path = new GraphicsPath();
            if (radius <= 0)
            {
                path.AddRectangle(r);
                path.CloseFigure();
                return path;
            }

            path.AddArc(r.X, r.Y, d, d, 180, 90);
            path.AddArc(r.Right - d, r.Y, d, d, 270, 90);
            path.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
            path.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }
        private static void OpenUrl(string raw)
        {
            string url = raw;
            if (!raw.StartsWith("http://", StringComparison.OrdinalIgnoreCase) &&
                !raw.StartsWith("https://", StringComparison.OrdinalIgnoreCase) &&
                !raw.StartsWith("mailto:", StringComparison.OrdinalIgnoreCase))
                url = "http://" + raw;

            try
            {
                Process.Start(new ProcessStartInfo { FileName = url, UseShellExecute = true });
            }
            catch (Exception ex)
            {
                MessageBox.Show("Bağlantı açılamadı:\n" + ex.Message, "Uyarı",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
    public class BubblePanel : Panel
    {
        public int CornerRadius { get; set; } = 14;
        public Color FillColor { get; set; } = Color.FromArgb(114, 88, 242);
        public Color BorderColor { get; set; } = Color.FromArgb(114, 88, 242);
        public float BorderWidth { get; set; } = 1.25f;

        public BubblePanel()
        {
            SetStyle(ControlStyles.UserPaint |
                     ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.OptimizedDoubleBuffer, true);

            // ÖNEMLİ: transparan değil, ebeveyne eşit bir düz renk
            BackColor = Color.White; // chatFlow.BackColor ile aynı renk
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            // halo oluşmasın diye arka planı ebeveyn rengiyle temizle
            using (var sb = new SolidBrush(Parent?.BackColor ?? Color.White))
                e.Graphics.FillRectangle(sb, ClientRectangle);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
            e.Graphics.CompositingQuality = CompositingQuality.HighQuality;

            var rect = new Rectangle(0, 0, Width - 1, Height - 1);

            using (var b = new SolidBrush(FillColor))
                e.Graphics.FillRoundedRectangle(b, rect, CornerRadius);

            using (var p = new Pen(BorderColor, BorderWidth))
            {
                try { p.Alignment = PenAlignment.Inset; } catch { }
                e.Graphics.DrawRoundedRectangle(p, rect, CornerRadius);
            }
        }
    }
    // ---------------------- Main Chat Form ----------------------
    internal static class Native
    {
        [DllImport("user32.dll")]
        public static extern bool HideCaret(IntPtr hWnd);
    }

    public partial class MainForm : Form
    {
        private Panel leftPanel;
        private Panel rightPanel;
        private Panel topPanel;
        private Panel bottomPanel;
        private TabControl leftTabControl;
        private ListView contactsListView;
        private ListView chatsListView;

        // BALONCUKLU GÖRÜNÜM: RichTextBox yerine FlowLayoutPanel
        private FlowLayoutPanel chatFlow;
        private TextBox messageInput;
        private Button sendButton;
        private Button exitButton;
        private Label chatTitleLabel;
        private Label userLabel;

        // Yeni: arama kutuları
        private FlatTextBox searchContactsBox;
        private FlatTextBox searchChatsBox;

        // Yeni: master listeler (filtreleme için)
        private List<string> usersMaster = new List<string>();
        private List<ConversationItemDto> chatsMaster = new List<ConversationItemDto>();

        private readonly HttpClient httpClient = new HttpClient { Timeout = TimeSpan.FromSeconds(15) };
        private string apiBaseUrl = "http://192.168.1.205:5101";
        private readonly string currentUsername;
        private string selectedChatUser = "";
        private readonly JsonSerializerOptions jsonOpts = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        // Okunmamış mesaj sayıları (isim -> sayı)
private readonly Dictionary<string, int> _unread =
    new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);


        public MainForm(string username)
        {
            currentUsername = username;
            InitializeComponent();

            jsonOpts.Converters.Add(new DateTimeOffsetAssumeLocalConverter());

            Shown += async (_, __) =>
            {
                await LoadContactsAsync();
                await LoadChatsAsync();
            };
        }

        private void InitializeComponent()
        {
            Text = $"SpilChat - {currentUsername}";
            Size = new Size(1000, 700);
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = Color.FromArgb(242, 242, 242);
            MinimumSize = new Size(800, 600);

            CreateLayout();

            this.Icon = new Icon(
                Path.Combine(Application.StartupPath, "Assets", "Icons", "mbb.ico")
            );
        }

        private static string FormatClock(DateTimeOffset t)
        {
            if (t.Offset == TimeSpan.Zero)
                return t.ToLocalTime().ToString("HH:mm");
            return t.ToString("HH:mm");
        }

        private static bool ContainsCI(string source, string term)
        {
            if (string.IsNullOrWhiteSpace(term)) return true;
            return CultureInfo.CurrentCulture.CompareInfo.IndexOf(source ?? "",
                term, CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace) >= 0;
        }

        private void StyleListViewAsCards(
            ListView lv,
            int rowHeight = 44,
            Action<DrawListViewSubItemEventArgs, Rectangle> contentDrawer = null)
        {
            lv.OwnerDraw = true;
            lv.Font = new Font("Segoe UI", 11f, FontStyle.Bold);

            var img = new ImageList { ImageSize = new Size(1, rowHeight), ColorDepth = ColorDepth.Depth32Bit };
            lv.SmallImageList = img;

            lv.Resize += (_, __) =>
            {
                if (lv.Columns.Count > 0)
                    lv.Columns[0].Width = lv.ClientSize.Width - 8;
            };

            lv.DrawColumnHeader += (s, e) => e.DrawBackground();

            lv.DrawSubItem += (s, e) =>
            {
                var g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;

                var rect = new Rectangle(e.Bounds.X + 6, e.Bounds.Y + 3, e.Bounds.Width - 12, e.Bounds.Height - 6);
                bool selected = e.Item.Selected;

                var fill = selected ? Color.FromArgb(232, 240, 254) : Color.FromArgb(245, 243, 241);
                var border = selected ? Color.FromArgb(99, 102, 241) : Color.FromArgb(220, 220, 220);

                using (var b = new SolidBrush(fill)) g.FillRoundedRectangle(b, rect, 8);
                using (var p = new Pen(border, selected ? 2 : 1)) g.DrawRoundedRectangle(p, rect, 8);

                if (contentDrawer != null)
                {
                    contentDrawer(e, rect);
                }
                else
                {
                    var textRect = Rectangle.Inflate(rect, -10, -6);
                    TextRenderer.DrawText(
                        g, e.SubItem.Text, lv.Font, textRect,
                        Color.FromArgb(32, 31, 30),
                        TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis
                    );
                }
            };

            try
            {
                typeof(Control).GetProperty("DoubleBuffered", BindingFlags.NonPublic | BindingFlags.Instance)
                               ?.SetValue(lv, true, null);
            }
            catch { }
        }

        // chatsListView kart içeriği
        private void DrawChatCardContent(DrawListViewSubItemEventArgs e, Rectangle cardRect)
        {
            var g = e.Graphics;
            var dto = e.Item.Tag as ConversationItemDto;

            string name = dto?.Peer ?? e.SubItem.Text;
            string snippet = dto?.Text ?? "";
            string timeStr = dto != null ? FormatClock(dto.LastTime) : "";

            var inner = Rectangle.Inflate(cardRect, -10, -8);
            int lineH = inner.Height / 2;

            var nameRect = new Rectangle(inner.X, inner.Y, inner.Width, lineH);
            var snippetRect = new Rectangle(inner.X, inner.Y + lineH - 2, inner.Width, lineH + 2);

            using (var nameFont = new Font("Segoe UI", 11f, FontStyle.Bold))
            using (var timeFont = new Font("Segoe UI", 9f, FontStyle.Regular))
            using (var nameBrush = new SolidBrush(Color.FromArgb(32, 31, 30)))
            using (var timeBrush = new SolidBrush(Color.FromArgb(120, 120, 120)))
            {
                TextRenderer.DrawText(g, name, nameFont, nameRect, nameBrush.Color,
                    TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis);

                TextRenderer.DrawText(g, timeStr, timeFont, nameRect, timeBrush.Color,
                    TextFormatFlags.Right | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis);
            }

            using (var snippetFont = new Font("Segoe UI", 9f, FontStyle.Regular))
            using (var snBrush = new SolidBrush(Color.FromArgb(100, 100, 100)))
            {
                TextRenderer.DrawText(g, snippet, snippetFont, snippetRect, snBrush.Color,
                    TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis);
            }
        }

        private void CreateLayout()
        {
            leftPanel = new Panel { Width = 300, Dock = DockStyle.Left, BackColor = Color.FromArgb(64, 64, 64), Padding = new Padding(10) };
            rightPanel = new Panel { Dock = DockStyle.Fill, BackColor = Color.White, Padding = new Padding(10) };
            topPanel = new Panel { Height = 60, Dock = DockStyle.Top, BackColor = Color.FromArgb(237, 235, 233), Padding = new Padding(15, 10, 15, 10) };
            bottomPanel = new Panel { Height = 80, Dock = DockStyle.Bottom, BackColor = Color.White, Padding = new Padding(15, 10, 15, 10) };

            chatTitleLabel = new Label
            {
                Text = "Sohbet seçin",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.FromArgb(32, 31, 30),
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft
            };

            leftTabControl = new TabControl { Dock = DockStyle.Fill, Font = new Font("Segoe UI", 10), Margin = new Padding(0, 0, 0, 110) };

            // Kişiler
            var contactsTab = new TabPage("Kişiler");
            var contactsSearchPanel = new Panel { Dock = DockStyle.Top, Height = 44, Padding = new Padding(8, 8, 8, 4), BackColor = Color.FromArgb(250, 249, 248) };
            searchContactsBox = new FlatTextBox
            {
                Dock = DockStyle.Fill,
                PlaceholderText = "Arayın",
                Font = new Font("Segoe UI", 10f),
                ShowSearchIcon = true
            };
            searchContactsBox.TextChanged += (_, __) => FilterContacts(searchContactsBox.Text);
            contactsSearchPanel.Controls.Add(searchContactsBox);

            contactsListView = new ListView
            {
                View = View.Details,
                FullRowSelect = true,
                HeaderStyle = ColumnHeaderStyle.None,
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(250, 249, 248),
                ForeColor = Color.FromArgb(32, 31, 30),
                Font = new Font("Segoe UI", 10),
                BorderStyle = BorderStyle.None
            };
            contactsListView.Columns.Add("", 250);
            contactsListView.ItemSelectionChanged += ContactsListView_ItemSelectionChanged;

            contactsTab.Controls.Add(contactsSearchPanel);
            contactsTab.Controls.Add(contactsListView);

            // Sohbetler
            var chatsTab = new TabPage("Sohbetler");
            var chatsSearchPanel = new Panel { Dock = DockStyle.Top, Height = 44, Padding = new Padding(8, 8, 8, 4), BackColor = Color.FromArgb(250, 249, 248) };
            searchChatsBox = new FlatTextBox
            {
                Dock = DockStyle.Fill,
                PlaceholderText = "Arayın",
                Font = new Font("Segoe UI", 10f),
                ShowSearchIcon = true
            };
            searchChatsBox.TextChanged += (_, __) => FilterChats(searchChatsBox.Text);
            chatsSearchPanel.Controls.Add(searchChatsBox);

            chatsListView = new ListView
            {
                View = View.Details,
                FullRowSelect = true,
                HeaderStyle = ColumnHeaderStyle.None,
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(250, 249, 248),
                ForeColor = Color.FromArgb(32, 31, 30),
                Font = new Font("Segoe UI", 10),
                BorderStyle = BorderStyle.None
            };
            chatsListView.Columns.Add("", 250);
            chatsListView.ItemSelectionChanged += ChatsListView_ItemSelectionChanged;

            chatsTab.Controls.Add(chatsSearchPanel);
            chatsTab.Controls.Add(chatsListView);

            leftTabControl.TabPages.Add(contactsTab);
            leftTabControl.TabPages.Add(chatsTab);

            contactsSearchPanel.SendToBack();
            chatsSearchPanel.SendToBack();

            // Kart görünümleri
            StyleListViewAsCards(contactsListView, 44);
            StyleListViewAsCards(chatsListView, 56, DrawChatCardContent);

            // Kullanıcı paneli
            var userPanel = new Panel { Height = 50, Dock = DockStyle.Bottom, BackColor = Color.FromArgb(50, 50, 50), Padding = new Padding(10, 5, 10, 5) };
            userLabel = new Label { Text = $"👤 {currentUsername}", Font = new Font("Segoe UI", 10, FontStyle.Bold), ForeColor = Color.White, Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleLeft };

            exitButton = new Button
            {
                Text = "Çıkış",
                Width = 70,
                Height = 30,
                BackColor = Color.FromArgb(196, 43, 28),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Cursor = Cursors.Hand,
                Dock = DockStyle.Right,
                Margin = new Padding(0, 10, 10, 10)
            };
            exitButton.FlatAppearance.BorderSize = 0;
            exitButton.Click += ExitButton_Click;

            userPanel.Controls.Add(exitButton);
            userPanel.Controls.Add(userLabel);

            leftPanel.Controls.Add(leftTabControl);
            leftPanel.Controls.Add(userPanel);

            topPanel.Controls.Add(chatTitleLabel);

            // ==== Yeni mesaj alanı: FlowLayoutPanel + Bubble ====
            chatFlow = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                BackColor = Color.White,
                Padding = new Padding(10)
            };
            // Flicker azalt
            typeof(Panel).GetProperty("DoubleBuffered",
                BindingFlags.NonPublic | BindingFlags.Instance)?.SetValue(chatFlow, true, null);

            // Eklendiğinde ve pencere boyu değişince yeniden hizala
            chatFlow.ControlAdded += (_, __) =>
            {
                RealignBubbles();
                // Kullanıcı zaten dipteyse, yeni mesaj eklenince otomatik alta in
                if (IsUserNearBottom()) ScrollChatToBottom();
            };
            this.ResizeEnd += (_, __) => RealignBubbles();
            this.SizeChanged += (_, __) => RealignBubbles(); // istersen bunu bırakmayabilirsin

            chatFlow.Resize += (_, __) => RealignBubbles();

            messageInput = new TextBox
            {
                Height = 35,
                Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top,
                Font = new Font("Segoe UI", 11),
                BorderStyle = BorderStyle.FixedSingle,
                Multiline = true
            };
            messageInput.KeyDown += MessageInput_KeyDown;

            sendButton = new Button
            {
                Text = "Gönder",
                Width = 80,
                Height = 35,
                Anchor = AnchorStyles.Right | AnchorStyles.Top,
                BackColor = Color.FromArgb(0, 120, 212),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            sendButton.FlatAppearance.BorderSize = 0;
            sendButton.Click += SendButton_Click;

            bottomPanel.Controls.Add(messageInput);
            bottomPanel.Controls.Add(sendButton);

            bottomPanel.Resize += (_, __) =>
            {
                sendButton.Location = new Point(bottomPanel.Width - sendButton.Width - 15, (bottomPanel.Height - sendButton.Height) / 2);
                messageInput.Location = new Point(10, (bottomPanel.Height - messageInput.Height) / 2);
                messageInput.Width = bottomPanel.Width - sendButton.Width - 30;
            };

            rightPanel.Controls.Add(chatFlow);
            rightPanel.Controls.Add(bottomPanel);
            rightPanel.Controls.Add(topPanel);

            Controls.Add(rightPanel);
            Controls.Add(leftPanel);

            ShowWelcomeMessage();
        }

        private void ShowWelcomeMessage()
        {
            chatFlow.Controls.Clear();
            AddSystemText($"SpilChat'e Hoş Geldiniz, {currentUsername}! Sol taraftan bir sohbet seçin.");
        }

        // MainForm içinde
        private void AddSystemText(string text)
        {
            // tek satırlık gri sistem mesajı satırı
            var row = new Panel
            {
                Width = chatFlow.ClientSize.Width - 30,
                Height = 40,                      // <-- sabit yükseklik; istersen ayarlayabilirsin
                Margin = new Padding(0, 6, 0, 6),
                BackColor = chatFlow.BackColor       // halo/gölgeyi önler
            };

            var lbl = new Label
            {
                Dock = DockStyle.Fill,
                Text = text,
                ForeColor = Color.Gray,
                Font = new Font("Segoe UI", 10, FontStyle.Regular),
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = chatFlow.BackColor
            };

            row.Controls.Add(lbl);
            chatFlow.Controls.Add(row);
            chatFlow.ScrollControlIntoView(row);
        }

        // --- Kaydırma yardımcıları ---
        private bool IsUserNearBottom()
        {
            var vs = chatFlow.VerticalScroll;
            // En alttan ~20px yukarıdaysa "dipte" say
            return (vs.Maximum - (vs.Value + vs.LargeChange)) <= 20;
        }

        private void ScrollChatToBottom()
        {
            if (chatFlow.Controls.Count == 0) return;

            // Layout tamamen bitsin, sonra kaydır
            this.BeginInvoke((Action)(() =>
            {
                try
                {
                    var last = chatFlow.Controls[chatFlow.Controls.Count - 1];
                    chatFlow.ScrollControlIntoView(last);

                    // Emniyet: çubuğu en dibe al
                    chatFlow.VerticalScroll.Value = chatFlow.VerticalScroll.Maximum;
                    chatFlow.PerformLayout();
                }
                catch { /* görmezden gel */ }
            }));
        }


        private void RealignBubbles()
        {
            foreach (Control ctrl in chatFlow.Controls)
            {
                if (ctrl is Panel row)
                {
                    row.Width = chatFlow.ClientSize.Width - 30;

                    if (row.Controls.Count > 0 && row.Controls[0] is ChatBubble b)
                    {
                        b.MaxBubbleWidth = (int)(chatFlow.ClientSize.Width * 0.60);
                        b.AlignInParent();
                        row.Height = b.Height;
                    }
                    else
                    {
                        // sistem mesajı satırı
                        row.Height = 40;
                    }
                }
            }
            // Pencere/alan yeniden hizalandıysa ve kullanıcı dipteyse, dibi koru
            if (IsUserNearBottom()) ScrollChatToBottom();

        }



        private void ExitButton_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Uygulamadan çıkmak istediğinizden emin misiniz?", "Çıkış",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                Application.Exit();
        }

        // --------- Kişiler ---------
        private async Task LoadContactsAsync()
        {
            try
            {
                var res = await httpClient.GetAsync($"{apiBaseUrl}/api/user/all");
                var body = await res.Content.ReadAsStringAsync();

                if (!res.IsSuccessStatusCode)
                {
                    MessageBox.Show($"Kişiler yüklenemedi.\nKod: {(int)res.StatusCode}\n{body}", "Hata",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var users = JsonSerializer.Deserialize<List<string>>(body, jsonOpts) ?? new List<string>();
                usersMaster = users
                    .Where(u => !string.Equals(u, currentUsername, StringComparison.OrdinalIgnoreCase))
                    .ToList();

                FilterContacts(searchContactsBox?.Text ?? string.Empty);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Kişiler yüklenemedi: " + ex.Message, "Hata",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FilterContacts(string query)
        {
            var filtered = string.IsNullOrWhiteSpace(query)
                ? usersMaster
                : usersMaster.Where(u => ContainsCI(u, query)).ToList();

            contactsListView.BeginUpdate();
            try
            {
                contactsListView.Items.Clear();
                foreach (var u in filtered)
                    contactsListView.Items.Add(new ListViewItem($"👤 {u}"));
            }
            finally
            {
                contactsListView.EndUpdate();
                contactsListView.Invalidate();
            }
        }

        // --------- Sohbetler (son konuşmalar) ---------
        private async Task LoadChatsAsync()
        {
            try
            {
                var url = $"{apiBaseUrl}/api/chat/recent/{Uri.EscapeDataString(currentUsername)}";

                using (var res = await httpClient.GetAsync(url))
                {
                    var body = await res.Content.ReadAsStringAsync();

                    if (!res.IsSuccessStatusCode)
                    {
                        MessageBox.Show($"Sohbet listesi yüklenemedi.\nKod: {(int)res.StatusCode}\n{body}",
                                        "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    chatsMaster = JsonSerializer.Deserialize<List<ConversationItemDto>>(body, jsonOpts)
                                  ?? new List<ConversationItemDto>();

                    FilterChats(searchChatsBox?.Text ?? string.Empty);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Sohbet listesi yüklenemedi: " + ex.Message, "Hata",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FilterChats(string query)
        {
            var filtered = string.IsNullOrWhiteSpace(query)
                ? chatsMaster
                : chatsMaster.Where(c => ContainsCI(c.Peer, query) || ContainsCI(c.Text, query)).ToList();

            chatsListView.BeginUpdate();
            try
            {
                chatsListView.Items.Clear();
                foreach (var c in filtered)
                {
                    var li = new ListViewItem(c.Peer) { Tag = c };
                    chatsListView.Items.Add(li);
                }
            }
            finally
            {
                chatsListView.EndUpdate();
                chatsListView.Invalidate();
            }
        }

        private void ContactsListView_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (!e.IsSelected) return;
            string contactName = e.Item.Text.Replace("👤 ", "");
            chatTitleLabel.Text = contactName;
            selectedChatUser = contactName;
            LoadChatHistory(contactName);
        }

        private void ChatsListView_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (!e.IsSelected) return;

            var dto = e.Item.Tag as ConversationItemDto;
            var peer = dto?.Peer ?? e.Item.Text;

            chatTitleLabel.Text = peer;
            selectedChatUser = peer;
            LoadChatHistory(peer);
        }

        // --------- Geçmiş ---------
        private async void LoadChatHistory(string chatName)
        {
            chatFlow.SuspendLayout();
            chatFlow.Controls.Clear();
            selectedChatUser = chatName;

            AddSystemText($"{chatName} ile sohbet");

            try
            {
                string from = Uri.EscapeDataString(currentUsername);
                string to = Uri.EscapeDataString(chatName);

                var response = await httpClient.GetAsync($"{apiBaseUrl}/api/chat/history?from={from}&to={to}");
                Console.WriteLine($"HISTORY URL: {response.RequestMessage?.RequestUri}");

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var messages = JsonSerializer.Deserialize<List<MessageDto>>(json, jsonOpts) ?? new List<MessageDto>();

                    foreach (var msg in messages)
                    {
                        bool outgoing = msg.FromUser == currentUsername;
                        AddBubble(msg.Message, outgoing, msg.Timestamp.LocalDateTime);
                    }
                }
                else
                {
                    var body = await response.Content.ReadAsStringAsync();
                    AddSystemText($"Sohbet geçmişi yüklenemedi. Kod: {(int)response.StatusCode}\n{body}");
                }
            }
            catch (Exception ex)
            {
                AddSystemText($"Sunucu hatası: {ex.Message}");
            }

            chatFlow.ResumeLayout();
            ScrollChatToBottom();
        }

        private void AddBubble(string text, bool outgoing, DateTime time)
        {
            int maxWidth = (int)(chatFlow.ClientSize.Width * 0.60);

            // 1) Önce balonu oluştur
            var bubble = new ChatBubble(outgoing, text, time, maxWidth);

            // 2) Sonra satırı (row) oluştur – artık bubble.Height kullanabiliriz
            var row = new Panel
            {
                Width = chatFlow.ClientSize.Width - 30,
                Height = bubble.Height,
                Margin = new Padding(0, 6, 0, 6),
                BackColor = chatFlow.BackColor   // halo/gölgeyi önler
            };
            row.BackColor = chatFlow.BackColor;

            // 3) Arka planları eşitle
            bubble.BackColor = row.BackColor;

            // 4) Balonu hizala
            bubble.Location = outgoing
                ? new Point(row.Width - bubble.Width - 5, 0)
                : new Point(5, 0);

            // 5) Ekle ve kaydır
            row.Controls.Add(bubble);
            chatFlow.Controls.Add(row);
            chatFlow.ScrollControlIntoView(row);

            // 6) Pencere yeniden boyutlanınca hizalamayı güncelle
            row.Resize += (_, __) =>
            {
                bubble.MaxBubbleWidth = (int)(chatFlow.ClientSize.Width * 0.60);
                bubble.AlignInParent();
                row.Height = bubble.Height;
            };
        }


        private void SendButton_Click(object sender, EventArgs e) => SendMessage();

        private void MessageInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && !e.Shift)
            {
                e.SuppressKeyPress = true;
                SendMessage();
            }
            ScrollChatToBottom();

        }

        private async void SendMessage()
        {
            string message = messageInput.Text.Trim();
            if (string.IsNullOrEmpty(message) || string.IsNullOrEmpty(selectedChatUser)) return;

            var now = DateTime.Now;

            AddBubble(message, true, now);

            messageInput.Clear();
            messageInput.Focus();

            await SendMessageToApi(currentUsername, selectedChatUser, message);
            await LoadChatsAsync(); // sol "Sohbetler" kartlarını tazele
        }

        private async Task SendMessageToApi(string fromUser, string toUser, string message)
        {
            var dto = new SendMessageDto { FromUser = fromUser, ToUser = toUser, Message = message };
            var json = JsonSerializer.Serialize(dto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                var response = await httpClient.PostAsync($"{apiBaseUrl}/api/chat/send", content);
                var body = await response.Content.ReadAsStringAsync();

                Console.WriteLine($"SEND URL: {response.RequestMessage?.RequestUri}");
                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Mesaj gönderme başarısız. Status={(int)response.StatusCode} {response.ReasonPhrase}");
                    Console.WriteLine($"Sunucu cevabı: {body}");
                    MessageBox.Show($"Mesaj gönderilemedi.\nKod: {(int)response.StatusCode}\n{body}", "Hata",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Sunucuya bağlanılamadı: " + ex.Message, "Bağlantı Hatası",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

    // ---------------------- DTO'lar ----------------------
    public class LoginDto { public string Username { get; set; } public string Password { get; set; } }

    public class SendMessageDto
    {
        public string FromUser { get; set; }
        public string ToUser { get; set; }
        public string Message { get; set; }
    }

    public class MessageDto
    {
        public int Id { get; set; }
        public string FromUser { get; set; }
        public string ToUser { get; set; }
        public string Message { get; set; }
        public DateTimeOffset Timestamp { get; set; }
    }

    public class ConversationItemDto
    {
        public string Peer { get; set; }
        public DateTimeOffset LastTime { get; set; }
        public string Text { get; set; }
    }

    // Offset YOKSA -> LOCAL varsayan converter
    internal sealed class DateTimeOffsetAssumeLocalConverter : JsonConverter<DateTimeOffset>
    {
        public override DateTimeOffset Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                var s = reader.GetString();
                if (string.IsNullOrWhiteSpace(s)) return default;

                if (DateTimeOffset.TryParse(s, CultureInfo.InvariantCulture, DateTimeStyles.None, out var dto))
                    return dto;

                if (DateTime.TryParse(s, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out var utcDt))
                    return new DateTimeOffset(utcDt, TimeSpan.Zero).ToLocalTime();
            }

            if (reader.TokenType == JsonTokenType.Number && reader.TryGetInt64(out long unixMs))
                return DateTimeOffset.FromUnixTimeMilliseconds(unixMs);

            return default;
        }

        public override void Write(Utf8JsonWriter writer, DateTimeOffset value, JsonSerializerOptions options)
            => writer.WriteStringValue(value.ToUniversalTime().ToString("o"));
    }

    // ---------------------- Program ----------------------
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            using (var loginForm = new LoginForm())
            {
                if (loginForm.ShowDialog() == DialogResult.OK)
                    Application.Run(new MainForm(loginForm.LoggedInUsername));
                else
                    Application.Exit();
            }
        }
    }
}
