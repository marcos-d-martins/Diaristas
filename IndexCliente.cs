using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace diaria
{
    [Activity(Label = "IndexCliente")]
    public class IndexCliente : Activity
    {
        string idcliente, nome;
        Conexao c = new Conexao();
        CheckBox servicos;
        Button bPefil, btSair, bMensangens, bAvaliacoes;
        List<string> listaCheckBox_DescServicos = new List<string>();
        List<string> listaIdCheckBox = new List<string>();
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.indexCliente);

            nome = Intent.GetStringExtra("nome");
            idcliente = Intent.GetStringExtra("idcliente");
            servicos = FindViewById<CheckBox>(Resource.Id.chServicos);
            btSair = FindViewById<Button>(Resource.Id.btnSair);
            bMensangens = FindViewById<Button>(Resource.Id.btnMensagens);
            bAvaliacoes = FindViewById<Button>(Resource.Id.btnAvaliacoes);

            Toast.MakeText(Application.Context, "Bem-vindo(a)" + nome + " !", ToastLength.Long).Show();

            bMensangens.Click += RedirecionaTelaMensagensCliente;
            bAvaliacoes.Click += telaAvaliacoes;
            btSair.Click += LogOut;
        }

        private void exibirPerfil(object sender, EventArgs e)
        {
            var telaPerfilCliente = new Intent(this, typeof(PerfilCliente));
            telaPerfilCliente.PutExtra("idCliente", idcliente);
            StartActivity(telaPerfilCliente);
        }

        private void telaAvaliacoes(object sender, EventArgs e)
        {
            var testeIrAvaliacao = new Intent(this, typeof(AvaliacaoDiarista) );            
            testeIrAvaliacao.PutExtra("id_cl", idcliente);
            StartActivity(testeIrAvaliacao);
        }
        private void ExcluirConta()
        {
            string string_exclusao;

            try
            {

            }
            catch (Exception)
            {

                throw;
            }
        }
        private void RedirecionaTelaMensagensCliente(object sender,EventArgs e)
        {
            var irTelaMensagensCliente = new Intent(this, typeof(MensagensCliente) );
            irTelaMensagensCliente.PutExtra("id_c", idcliente);
            StartActivity(irTelaMensagensCliente);
        }

        private void LogOut(object sender, EventArgs e)
        {
            Finish();
        }

        /*private void listaServicos()
        {
            string sql;
            try
            {
                MySqlCommand cmd;
                MySqlDataReader lerDados;
                sql = "SELECT idservico, desc_servico FROM servico";
                cmd = new MySqlCommand(sql, c.conn);
                lerDados = cmd.ExecuteReader();

                if (lerDados.HasRows)
                {
                    while (lerDados.Read())
                    {
                        //servicos = (CheckBox)lerDados["desc_servico"].ToString();
                        listaCheckBox_DescServicos.Add(lerDados["idservico"].ToString());
                        listaIdCheckBox.Add(lerDados["desc_servico"].ToString());
                    }

                }
            }
            catch (Exception)
            {

            }
        }*/
    }
}