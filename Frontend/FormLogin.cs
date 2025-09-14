using System;
using System.Drawing;
using System.Windows.Forms;

namespace CadastroFuncionarios
{
    public class FormLogin : Form
    {
        private TextBox txtUsuario, txtSenha;
        private Button btnLogin;
        private AuthService auth = new AuthService();

        public FormLogin()
        {
            this.Text = "Login - Arco Tecnologia";
            this.Size = new Size(450, 300);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.None;

            // Gradiente de fundo
            this.Paint += (s, e) =>
            {
                using (var brush = new System.Drawing.Drawing2D.LinearGradientBrush(
                    this.ClientRectangle,
                    Color.FromArgb(40, 40, 40),
                    Color.FromArgb(70, 70, 70),
                    90F))
                {
                    e.Graphics.FillRectangle(brush, this.ClientRectangle);
                }
            };

            // Painel central
            Panel pnlCard = new Panel()
            {
                Size = new Size(350, 220),
                BackColor = Color.FromArgb(30, 30, 30),
                Location = new Point((this.ClientSize.Width - 350) / 2, (this.ClientSize.Height - 220) / 2),
            };
            pnlCard.Anchor = AnchorStyles.None;

            // Título
            Label lblTitulo = new Label()
            {
                Text = "Arco Tecnologia",
                ForeColor = Color.Gold,
                Font = new Font("Arial", 16, FontStyle.Bold),
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Top,
                Height = 50
            };

            // Usuário
            Label lblUsuario = new Label() { Text = "Usuário:", Left = 20, Top = 77, Width = 80, ForeColor = Color.Gold };
            txtUsuario = new TextBox()
            {
                Left = 110,
                Top = 70,
                Width = 200,
                BackColor = Color.FromArgb(50, 50, 50),
                ForeColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            // Senha
            Label lblSenha = new Label() { Text = "Senha:", Left = 20, Top = 110, Width = 80, ForeColor = Color.Gold };
            txtSenha = new TextBox()
            {
                Left = 110,
                Top = 105,
                Width = 200,
                PasswordChar = '*',
                BackColor = Color.FromArgb(50, 50, 50),
                ForeColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            // Botão Entrar
            btnLogin = new Button()
            {
                Text = "Entrar",
                Left = 110,
                Top = 150,
                Width = 200,
                Height = 35,
                BackColor = Color.Gold,
                ForeColor = Color.Black,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Arial", 10, FontStyle.Bold)
            };
            btnLogin.FlatAppearance.BorderSize = 0;
            btnLogin.Click += BtnLogin_Click;

            pnlCard.Controls.Add(lblTitulo);
            pnlCard.Controls.Add(lblUsuario);
            pnlCard.Controls.Add(txtUsuario);
            pnlCard.Controls.Add(lblSenha);
            pnlCard.Controls.Add(txtSenha);
            pnlCard.Controls.Add(btnLogin);
            this.Controls.Add(pnlCard);

            // Botão de fechar
            Button btnClose = new Button()
            {
                Text = "X",
                ForeColor = Color.White,
                BackColor = Color.Transparent,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Arial", 10, FontStyle.Bold),
                Size = new Size(30, 30),
                Location = new Point(this.ClientSize.Width - 35, 5),
                TabStop = false
            };
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.Click += (s, e) => this.Close();
            this.Controls.Add(btnClose);
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // FormLogin
            // 
            this.ClientSize = new System.Drawing.Size(282, 253);
            this.Name = "FormLogin";
            this.Load += new System.EventHandler(this.FormLogin_Load);
            this.ResumeLayout(false);

        }

        private void FormLogin_Load(object sender, EventArgs e)
        {

        }

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            if (auth.Login(txtUsuario.Text, txtSenha.Text))
            {
                this.Hide();
                FormFuncionarios form1 = new FormFuncionarios();
                form1.FormClosed += (s, args) => this.Close();
                form1.Show();
            }
            else
            {
                MessageBox.Show("Usuário ou senha incorretos.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtSenha.Clear();
                txtUsuario.Focus();
            }
        }
    }
}
