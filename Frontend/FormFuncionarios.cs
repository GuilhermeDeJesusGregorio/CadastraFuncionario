using System;
using System.Drawing;
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

        // usa-se editingId para identificar o funcionário em edição (-1 = novo)
        private int editingId = -1;

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // FormFuncionarios
            // 
            this.ClientSize = new System.Drawing.Size(282, 253);
            this.Name = "FormFuncionarios";
            this.Load += new System.EventHandler(this.FormFuncionarios_Load);
            this.ResumeLayout(false);

        }

        private void FormFuncionarios_Load(object sender, EventArgs e)
        {

        }

        public FormFuncionarios()
        {
            this.Text = "Arco Tecnologia - Funcionários";
            this.Size = new Size(1000, 700);
            this.StartPosition = FormStartPosition.CenterScreen;

            InicializarTabs();
            InicializarCadastro();
            InicializarLista();
            AtualizarListView();
        }

        private void InicializarTabs()
        {
            tabControlMain = new TabControl() { Dock = DockStyle.Fill };
            tabCadastro = new TabPage("Cadastro");
            tabLista = new TabPage("Lista de Funcionários");
            tabControlMain.TabPages.Add(tabCadastro);
            tabControlMain.TabPages.Add(tabLista);
            this.Controls.Add(tabControlMain);
        }

        private void InicializarCadastro()
        {
            Label lblNome = new Label() { Text = "Nome:", Left = 20, Top = 20, Width = 100 };
            txtNome = new TextBox() { Left = 130, Top = 20, Width = 400 };

            Label lblCPF = new Label() { Text = "CPF:", Left = 20, Top = 60, Width = 100 };
            txtCPF = new MaskedTextBox("000.000.000-00") { Left = 130, Top = 60, Width = 150 };

            Label lblDataNascimento = new Label() { Text = "Data de Nascimento:", Left = 20, Top = 100, Width = 150 };
            dtpDataNascimento = new DateTimePicker() { Left = 170, Top = 95, Width = 150, Format = DateTimePickerFormat.Short };

            Label lblDataAdmissao = new Label() { Text = "Data de Admissão:", Left = 20, Top = 140, Width = 150 };
            dtpDataAdmissao = new DateTimePicker() { Left = 170, Top = 135, Width = 150, Format = DateTimePickerFormat.Short };

            Label lblRaca = new Label() { Text = "Raça/Cor:", Left = 20, Top = 220 };
            cbRaca = new ComboBox() { Left = 140, Top = 220, Width = 150 };
            cbRaca.Items.AddRange(new string[] { "Branca", "Negra", "Parda", "Amarela", "Indígena" });

            Label lblPCD = new Label() { Text = "PCD:", Left = 320, Top = 220 };
            cbDeficiencia = new ComboBox() { Left = 430, Top = 220, Width = 150 };
            cbDeficiencia.Items.AddRange(new string[] { "Não", "Auditiva", "Física", "Tetraplegia", "Paraplegia", "Nanismo", "Paralisia Cerebral" });

            Label lblFormaPagamento = new Label() { Text = "Forma Pagamento:", Left = 20, Top = 260 };
            cbFormaPagamento = new ComboBox() { Left = 140, Top = 260, Width = 150 };
            cbFormaPagamento.Items.AddRange(new string[] { "Débito em Conta", "Dinheiro" });
            cbFormaPagamento.SelectedIndexChanged += (s, e) =>
            {
                bool isDinheiro = cbFormaPagamento.Text == "Dinheiro";
                cbBanco.Enabled = !isDinheiro;
                txtAgencia.Enabled = !isDinheiro;
                txtConta.Enabled = !isDinheiro;
            };

            Label lblBanco = new Label() { Text = "Banco:", Left = 320, Top = 260 };
            cbBanco = new ComboBox() { Left = 430, Top = 260, Width = 150 };
            cbBanco.Items.AddRange(new string[] { "Banco do Brasil", "Bradesco", "Caixa", "Nubank", "Santander" });

            Label lblAgencia = new Label() { Text = "Agência:", Left = 20, Top = 300 };
            txtAgencia = new TextBox() { Left = 140, Top = 300, Width = 100 };

            Label lblConta = new Label() { Text = "Número Conta:", Left = 320, Top = 300 };
            txtConta = new TextBox() { Left = 430, Top = 300, Width = 100 };

            btnConcluido = new Button() { Text = "Concluído", Left = 20, Top = 340, Width = 100 };
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
                Height = 500
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

            btnEditar = new Button() { Text = "Editar", Top = 510, Left = 20, Width = 100 };
            btnEditar.Click += BtnEditar_Click;

            btnDemissao = new Button() { Text = "Registrar Demissão", Top = 510, Left = 140, Width = 150 };
            btnDemissao.Click += BtnDemissao_Click;

            btnExcluir = new Button() { Text = "Excluir", Top = 510, Left = 320, Width = 100 };
            btnExcluir.Click += BtnExcluir_Click;

            tabLista.Controls.Add(lvFuncionarios);
            tabLista.Controls.Add(btnEditar);
            tabLista.Controls.Add(btnDemissao);
            tabLista.Controls.Add(btnExcluir);
        }

        private void BtnConcluido_Click(object sender, EventArgs e)
        {
            // validações básicas
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

            // monta o objeto Funcionario (se estiver editando, busca pelo id)
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

            // popula campos
            f.Nome = txtNome.Text.Trim();
            f.CPF = txtCPF.Text.Trim();
            f.DataNascimento = dtpDataNascimento.Value.Date;
            f.DataAdmissao = dtpDataAdmissao.Value.Date;
            // campos opcionais: envia null se vazio
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
                    // garante que o id está presente no objeto
                    f.Id = editingId;
                    service.Update(f);
                }
                else
                {
                    service.Add(f);
                }

                // limpa e atualiza UI
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

            // define editingId para usar no salvar
            editingId = id;

            // preenche os campos
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
            txtCPF.Text = ""; // MaskedTextBox: use Text ou ResetText()
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

            // abre janela para escolher a data de demissão
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
                    Tag = f.Id // guarda o ID do funcionário
                };
                lvFuncionarios.Items.Add(item);
            }
        }
    }
}
