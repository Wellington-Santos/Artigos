using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Artigos
{
    public partial class Dashboard : Form
    {
            //1 - Autores
            //2 - Revisores
            //3 - Gerente
        public Dashboard()
        {
            InitializeComponent();
        }

        private void Dashboard_Load(object sender, EventArgs e)
        {
            var frmLogin = new Login();
            frmLogin.ShowDialog();

            if (frmLogin.logado == false) {
                Close();

            }

            if(Login.perfilUsuario == 3)
            {
                button1.Enabled = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var cad = new Cadastrar();
            cad.ShowDialog();
        }
    }
}
