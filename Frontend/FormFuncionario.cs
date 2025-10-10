using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace CadastroFuncionarios
{
    public class FormFuncionarios : Form
    {
        private FuncionarioService service = new FuncionarioService();
        private TabControl tabControlMain;
        private TabPage tabCadastro, tabLista;

        private TextBox txtNome, txtAgencia, txtConta;
        private MaskedTextBox txtCPF;
        private DateTimePicker dtpDataNascimento, dtpDataAdmissao;
        private ComboBox cbRaca, cbDeficiencia, cbFormaPagamento, cbBanco;
        private Button btnConcluido, btnEditar, btnDemissao, btnExcluir;
        private ListView lvFuncionarios;

        private int editingId = -1;

        public FormFuncionarios()
        {
            this.Text = "Arco Tecnologia - Funcionários";
            this.Size = new Size(1000, 700);
            this.StartPosition = FormStartPosition.CenterScreen;

            this.BackColor = Color.FromArgb(45, 45, 48);
            this.ForeColor = Color.White;

            InicializarTabs();
            InicializarCadastro();
            InicializarLista();
            AtualizarListView();
        }

        private Label CriarLabel(string texto, int left, int top, int width = 100)
        {
            return new Label()
            {
                Text = texto,
                Left = left,
                Top = top,
                Width = width,
                ForeColor = Color.White,
                BackColor = Color.Transparent
            };
        }

        private TextBox CriarTextBox(int left, int top, int width = 150)
        {
            return new TextBox()
            {
                Left = left,
                Top = top,
                Width = width,
                BackColor = Color.FromArgb(64, 64, 64),
                ForeColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };
        }

        private ComboBox CriarComboBox(int left, int top, int width = 150)
        {
            return new ComboBox()
            {
                Left = left,
                Top = top,
                Width = width,
                BackColor = Color.FromArgb(64, 64, 64),
                ForeColor = Color.White,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
        }

        private Button CriarButton(string texto, int left, int top, int width = 100)
        {
            return new Button()
            {
                Text = texto,
                Left = left,
                Top = top,
                Width = width,
                BackColor = Color.FromArgb(70, 70, 70),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
        }

        private void InicializarTabs()
        {
            tabControlMain = new TabControl()
            {
                Dock = DockStyle.Fill,
                BackColor = this.BackColor,
                ForeColor = this.ForeColor
            };

            // Aba Cadastro com gradiente
            tabCadastro = new TabPage("Cadastro");
            tabCadastro.Paint += (s, e) =>
            {
                using (LinearGradientBrush brush = new LinearGradientBrush(
                    tabCadastro.ClientRectangle,
                    Color.Gray,          // de cima
                    Color.Yellow,        // para baixo
                    LinearGradientMode.Vertical))
                {
                    e.Graphics.FillRectangle(brush, tabCadastro.ClientRectangle);
                }
            };

            // Aba Lista fundo branco
            tabLista = new TabPage("Lista de Funcionários")
            {
                BackColor = Color.White,
                ForeColor = Color.Black
            };

            tabControlMain.TabPages.Add(tabCadastro);
            tabControlMain.TabPages.Add(tabLista);
            this.Controls.Add(tabControlMain);
        }

        private void InicializarCadastro()
        {
            Label lblNome = CriarLabel("Nome:", 20, 20);
            txtNome = CriarTextBox(130, 20, 400);

            Label lblCPF = CriarLabel("CPF:", 20, 60);
            txtCPF = new MaskedTextBox("000.000.000-00")
            {
                Left = 130,
                Top = 60,
                Width = 150,
                BackColor = Color.FromArgb(64, 64, 64),
                ForeColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            Label lblDataNascimento = CriarLabel("Data de Nascimento:", 20, 100, 150);
            dtpDataNascimento = new DateTimePicker()
            {
                Left = 170,
                Top = 95,
                Width = 150,
                Format = DateTimePickerFormat.Short
            };

            Label lblDataAdmissao = CriarLabel("Data de Admissão:", 20, 140, 150);
            dtpDataAdmissao = new DateTimePicker()
            {
                Left = 170,
                Top = 135,
                Width = 150,
                Format = DateTimePickerFormat.Short
            };

            Label lblRaca = CriarLabel("Raça/Cor:", 20, 220);
            cbRaca = CriarComboBox(140, 220);
            cbRaca.Items.AddRange(new string[] { "Branca", "Negra", "Parda", "Amarela", "Indígena" });

            Label lblPCD = CriarLabel("PCD:", 320, 220);
            cbDeficiencia = CriarComboBox(430, 220);
            cbDeficiencia.Items.AddRange(new string[] { "Não", "Auditiva", "Física", "Tetraplegia", "Paraplegia", "Nanismo", "Paralisia Cerebral" });

            Label lblFormaPagamento = CriarLabel("Forma Pagamento:", 20, 260, 120);
            cbFormaPagamento = CriarComboBox(140, 260);
            cbFormaPagamento.Items.AddRange(new string[] { "Débito em Conta", "Dinheiro" });
            cbFormaPagamento.SelectedIndexChanged += (s, e) =>
            {
                bool isDinheiro = cbFormaPagamento.Text == "Dinheiro";
                cbBanco.Enabled = !isDinheiro;
                txtAgencia.Enabled = !isDinheiro;
                txtConta.Enabled = !isDinheiro;
            };

            Label lblBanco = CriarLabel("Banco:", 320, 260);
            cbBanco = CriarComboBox(430, 260);
            cbBanco.Items.AddRange(new string[] { "Banco do Brasil", "Bradesco", "Caixa", "Nubank", "Santander" });

            Label lblAgencia = CriarLabel("Agência:", 20, 300);
            txtAgencia = CriarTextBox(140, 300, 100);

            Label lblConta = CriarLabel("Número Conta:", 320, 300, 100);
            txtConta = CriarTextBox(430, 300, 100);

            btnConcluido = CriarButton("Concluído", 20, 340, 120);
            btnConcluido.Click += BtnConcluido_Click;

            tabCadastro.Controls.AddRange(new Control[]
            {
                lblNome, txtNome, lblCPF, txtCPF,
                lblDataNascimento, dtpDataNascimento,
                lblDataAdmissao, dtpDataAdmissao,
                lblRaca, cbRaca, lblPCD, cbDeficiencia,
                lblFormaPagamento, cbFormaPagamento, lblBanco, cbBanco,
                lblAgencia, txtAgencia, lblConta, txtConta,
                btnConcluido
            });
        }

        private void InicializarLista()
        {
            lvFuncionarios = new ListView()
            {
                View = View.Details,
                FullRowSelect = true,
                GridLines = true,
                Dock = DockStyle.Top,
                Height = 500,
                BackColor = Color.White,
                ForeColor = Color.Black
            };

            lvFuncionarios.Columns.Add("Nome", 200);
            lvFuncionarios.Columns.Add("CPF", 120);
            lvFuncionarios.Columns.Add("Nascimento", 100);
            lvFuncionarios.Columns.Add("Data Admissão", 100);
            lvFuncionarios.Columns.Add("Data Demissão", 100);
            lvFuncionarios.Columns.Add("Raça", 100);
            lvFuncionarios.Columns.Add("PCD", 100);
            lvFuncionarios.Columns.Add("Forma Pagamento", 150);
            lvFuncionarios.Columns.Add("Banco", 100);
            lvFuncionarios.Columns.Add("Agência", 80);
            lvFuncionarios.Columns.Add("Conta", 80);

            btnEditar = CriarButton("Editar", 20, 510, 100);
            btnEditar.Click += BtnEditar_Click;

            btnDemissao = CriarButton("Registrar Demissão", 140, 510, 150);
            btnDemissao.Click += BtnDemissao_Click;

            btnExcluir = CriarButton("Excluir", 320, 510, 100);
            btnExcluir.Click += BtnExcluir_Click;

            tabLista.Controls.Add(lvFuncionarios);
            tabLista.Controls.Add(btnEditar);
            tabLista.Controls.Add(btnDemissao);
            tabLista.Controls.Add(btnExcluir);
        }

        private void BtnConcluido_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNome.Text))
            {
                MessageBox.Show("O campo Nome é obrigatório.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtCPF.Text) || !txtCPF.MaskFull)
            {
                MessageBox.Show("O campo CPF é obrigatório e deve estar completo.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Funcionario f;
            if (editingId != -1)
            {
                f = service.GetAll().Find(x => x.Id == editingId);
                if (f == null)
                {
                    MessageBox.Show("Funcionário não encontrado para edição.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    editingId = -1;
                    return;
                }
            }
            else
            {
                f = new Funcionario();
            }

            f.Nome = txtNome.Text.Trim();
            f.CPF = txtCPF.Text.Trim();
            f.DataNascimento = dtpDataNascimento.Value.Date;
            f.DataAdmissao = dtpDataAdmissao.Value.Date;
            f.Raca = string.IsNullOrWhiteSpace(cbRaca.Text) ? null : cbRaca.Text;
            f.PCD = string.IsNullOrWhiteSpace(cbDeficiencia.Text) ? null : cbDeficiencia.Text;
            f.FormaPagamento = string.IsNullOrWhiteSpace(cbFormaPagamento.Text) ? null : cbFormaPagamento.Text;
            f.Banco = string.IsNullOrWhiteSpace(cbBanco.Text) ? null : cbBanco.Text;
            f.Agencia = string.IsNullOrWhiteSpace(txtAgencia.Text) ? null : txtAgencia.Text;
            f.Conta = string.IsNullOrWhiteSpace(txtConta.Text) ? null : txtConta.Text;

            try
            {
                if (editingId != -1)
                {
                    f.Id = editingId;
                    service.Update(f);
                }
                else
                {
                    service.Add(f);
                }

                AtualizarListView();
                LimparCampos();
                editingId = -1;
                tabControlMain.SelectedTab = tabLista;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnEditar_Click(object sender, EventArgs e)
        {
            if (lvFuncionarios.SelectedItems.Count == 0)
            {
                MessageBox.Show("Selecione um funcionário para editar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int id = (int)lvFuncionarios.SelectedItems[0].Tag;
            Funcionario f = service.GetAll().Find(x => x.Id == id);

            if (f == null)
            {
                MessageBox.Show("Funcionário não encontrado.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            editingId = id;

            txtNome.Text = f.Nome;
            txtCPF.Text = f.CPF;
            dtpDataNascimento.Value = f.DataNascimento == DateTime.MinValue ? DateTime.Today : f.DataNascimento;
            dtpDataAdmissao.Value = f.DataAdmissao == DateTime.MinValue ? DateTime.Today : f.DataAdmissao;
            cbRaca.Text = f.Raca ?? "";
            cbDeficiencia.Text = f.PCD ?? "";
            cbFormaPagamento.Text = f.FormaPagamento ?? "";
            cbBanco.Text = f.Banco ?? "";
            txtAgencia.Text = f.Agencia ?? "";
            txtConta.Text = f.Conta ?? "";

            tabControlMain.SelectedTab = tabCadastro;
        }

        private void LimparCampos()
        {
            txtNome.Text = "";
            txtCPF.Text = "";
            dtpDataNascimento.Value = DateTime.Now.Date;
            dtpDataAdmissao.Value = DateTime.Now.Date;
            cbRaca.SelectedIndex = -1;
            cbDeficiencia.SelectedIndex = -1;
            cbFormaPagamento.SelectedIndex = -1;
            cbBanco.SelectedIndex = -1;
            txtAgencia.Text = "";
            txtConta.Text = "";
        }

        private void BtnDemissao_Click(object sender, EventArgs e)
        {
            if (lvFuncionarios.SelectedItems.Count == 0)
            {
                MessageBox.Show("Selecione um funcionário.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int id = (int)lvFuncionarios.SelectedItems[0].Tag;
            Funcionario f = service.GetAll().Find(x => x.Id == id);
            if (f == null)
            {
                MessageBox.Show("Funcionário não encontrado.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            using (Form formData = new Form() { Width = 260, Height = 160, Text = "Data de Demissão", StartPosition = FormStartPosition.CenterParent })
            {
                DateTimePicker dtp = new DateTimePicker() { Left = 20, Top = 20, Width = 200, Format = DateTimePickerFormat.Short };
                if (f.DataDemissao.HasValue) dtp.Value = f.DataDemissao.Value;
                Button btnOK = new Button() { Text = "OK", Left = 70, Top = 60, Width = 80 };
                btnOK.Click += (s, ev) => { formData.DialogResult = DialogResult.OK; formData.Close(); };
                formData.Controls.Add(dtp);
                formData.Controls.Add(btnOK);

                if (formData.ShowDialog() == DialogResult.OK)
                {
                    f.DataDemissao = dtp.Value.Date;
                    try
                    {
                        service.Update(f);
                        AtualizarListView();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void BtnExcluir_Click(object sender, EventArgs e)
        {
            if (lvFuncionarios.SelectedItems.Count == 0)
            {
                MessageBox.Show("Selecione um funcionário para excluir.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int id = (int)lvFuncionarios.SelectedItems[0].Tag;

            if (MessageBox.Show("Deseja excluir este funcionário?", "Confirmação", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    service.Delete(id);
                    AtualizarListView();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void AtualizarListView()
        {
            lvFuncionarios.Items.Clear();
            foreach (var f in service.GetAll())
            {
                string[] row = {
                    f.Nome ?? "",
                    f.CPF ?? "",
                    (f.DataNascimento == DateTime.MinValue) ? "" : f.DataNascimento.ToShortDateString(),
                    (f.DataAdmissao == DateTime.MinValue) ? "" : f.DataAdmissao.ToShortDateString(),
                    f.DataDemissao.HasValue ? f.DataDemissao.Value.ToShortDateString() : "",
                    f.Raca ?? "",
                    f.PCD ?? "",
                    f.FormaPagamento ?? "",
                    f.Banco ?? "",
                    f.Agencia ?? "",
                    f.Conta ?? ""
                };

                ListViewItem item = new ListViewItem(row)
                {
                    BackColor = f.DataDemissao.HasValue ? Color.LightCoral : Color.LightGreen,
                    Tag = f.Id
                };
                lvFuncionarios.Items.Add(item);
            }
        }
    }
}
