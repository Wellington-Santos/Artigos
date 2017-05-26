using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Windows.Forms;

namespace Artigos
{
    public partial class Cadastrar : Form
    {
        public bool logado = false;
        private Conexao conn;
        private SqlConnection ConnectOpen;

        public Cadastrar()
        {
            InitializeComponent();
            conn = new Conexao();
            ConnectOpen = conn.ConnectToDatabase();
        }



        private void button1_Click(object sender, EventArgs e)
        {
            if (button1.Text == "Alterar")
            {
                StringBuilder sql = new StringBuilder();
                sql.Append(" update usuarios set ");
                sql.Append(" senha = @senha, ");
                sql.Append(" perfil = @perfil "); //Não esqueçam de dar um espaço no final 
                sql.Append(" where usuario = @usuario");

                SqlCommand command = null;
                int perfilSeleted = 0;
                switch (cmbPerfil.Text)
                {
                    case "Autores":
                        perfilSeleted = 1;
                        break;
                    case "Revisores":
                        perfilSeleted = 2;
                        break;
                    case "Gerente":
                        perfilSeleted = 3;
                        break;
                    default:
                        perfilSeleted = 1;
                        break;
                }

                command = new SqlCommand(sql.ToString(), ConnectOpen);
                command.Parameters.Add(new SqlParameter("@senha", txtSenha.Text));
                command.Parameters.Add(new SqlParameter("@perfil", perfilSeleted));
                command.Parameters.Add(new SqlParameter("@usuario", txtUsuario.Text));
                command.ExecuteNonQuery();

                MessageBox.Show("Alterado com sucesso!");
                LimparTela();
            }
            else
            {
                //incluir o using System.Text
                StringBuilder sql = new StringBuilder();
                sql.Append("Insert into usuarios(Usuario, senha, perfil) ");
                sql.Append("Values (@usuario, @senha, @perfil)");
                SqlCommand command = null;

                int perfilSeleted = 0;
                switch (cmbPerfil.Text)
                {
                    case "Autores":
                        perfilSeleted = 1;
                        break;
                    case "Revisores":
                        perfilSeleted = 2;
                        break;
                    case "Gerente":
                        perfilSeleted = 3;
                        break;
                    default:
                        perfilSeleted = 1;
                        break;
                }

                try
                {
                    command = new SqlCommand(sql.ToString(), ConnectOpen);
                    command.Parameters.Add(new SqlParameter("@usuario", txtUsuario.Text));
                    command.Parameters.Add(new SqlParameter("@senha", txtSenha.Text));
                    command.Parameters.Add(new SqlParameter("@perfil", perfilSeleted));
                    command.ExecuteNonQuery();

                    MessageBox.Show("Cadastrado com sucesso!");
                    Hide();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao cadastrar" + ex);
                    throw;
                }
            }//Fim else 

        }


        private void LimparTela()
        {
            button1.Text = "Cadastrar";
            button3.Visible = false;
            txtSenha.Text = "";
            txtUsuario.Text = "";
            cmbPerfil.Text = "Selecione o perfil";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var listarUsu = new ListarUsuario();
            listarUsu.ShowDialog();

            //Verificar se foi selecionado algum item
            if (listarUsu.UsuarioSelecionado == "")
                return;

            var conn = Login.ConnectOpen;
            //Buscar usuário selecionado
            string sql = "Select * from usuarios where Usuario = '" + listarUsu.UsuarioSelecionado + "'";


            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(sql, conn);
            da.Fill(dt);

            //Linha 0, coluna 0
            txtUsuario.Text = dt.Rows[0][0].ToString();

            //Linha 0, coluna 1
            txtSenha.Text = dt.Rows[0][1].ToString();

            string PerfilSelecionado;

            switch (dt.Rows[0][2].ToString())
            {
                case "1":
                    PerfilSelecionado = "Autores";
                    break;
                case "2":
                    PerfilSelecionado = "Revisores";
                    break;
                case "3":
                    PerfilSelecionado = "Gerente";
                    break;
                default:
                    PerfilSelecionado = "Autores";
                    break;
            }

            cmbPerfil.Text = PerfilSelecionado;

            //Trocar o text do cadastra para alterar
            button1.Text = "Alterar";

            //Alterar a visualização do excluir
            button3.Visible = true;

        }

        private void Cadastrar_Load(object sender, EventArgs e)
        {
            if (Login.perfilUsuario == 3)
            {
                lblPerfil.Visible = true;
                cmbPerfil.Visible = true;
                btnListar.Visible = true;
            }
        }

        private void btnExcluir_Click(object sender, EventArgs e)
        {
            var conn = Login.ConnectOpen;

            //Confirmar exclusão
            DialogResult result = MessageBox.Show("Deseja REALMENTE excluir?", "Delete",
                MessageBoxButtons.OKCancel, MessageBoxIcon.Information);

            //Caso o usuário dê ok, a exclusão prossegue
            if (!result.Equals(DialogResult.OK))
                return; //caso cancele, o código abaixo não será excutado.

            //Buscar usuário selecionado
            string sql = "Delete from usuarios where Usuario = @usuario";

            SqlCommand command = null;
            command = new SqlCommand(sql.ToString(), ConnectOpen);
            command.Parameters.Add(new SqlParameter("@usuario", txtUsuario.Text));
            command.ExecuteNonQuery();
            LimparTela();
            MessageBox.Show("Excluído com sucesso!");

        }
    }
}
