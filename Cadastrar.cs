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
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var listarUsu = new ListarUsuario();
            listarUsu.ShowDialog();

            //Verficicar se foi selecionado algum item
            if (listarUsu.UsuarioSelecionado == "")
                return;

            var conn = Login.ConnectOpen;
            //Buscar usuario selecionado

            StringBuilder sql = new StringBuilder();

            sql.Append(" select usu.Id_Usuario, usu.Usuario, usu.Senha, per.NomePerfil from Usuarios as usu");
            sql.Append(" inner join  ");
            sql.Append(" Perfil as per  ");
            sql.Append("on Id_Perfil = Perfil");
            sql.Append(" where ");

            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(sql.ToString(), conn);
            da.Fill(dt);

            //Linha 0, Coluna 0 Nome
            txtUsuario.Text = dt.Rows[0][0].ToString();

            //Linha 0, Coluna 1 Senha
            txtSenha.Text = dt.Rows[0][1].ToString();

            //Linha 0, Coluna 3 Perfil
            cmbPerfil.Text = dt.Rows[0][3].ToString();

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
    }
}
