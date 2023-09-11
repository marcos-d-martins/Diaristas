using Android.App;
using Android.Content;
using Android.Media.Audiofx;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace diaria
{
    [Activity(Label = "IndexDiarista")]
    public class IndexDiarista : Activity
    {

        string id_diarista, nomeDiarista, idregiao, idservico, idpre;
        Spinner spinnerRegiao, spinnerServicos;
        Button btListarVagas, LogoutBT, bVerMensagens;
        List<string> listaidRegiao = new List<string>();
        List<string> listaDescRegiao = new List<string>();
        List<string> listaidServico = new List<string>();
        List<string> listaDescServico = new List<string>();
        List<string> listaPreServico = new List<string>();
        List<string> listaDeMensagem = new List<string>();

        ListView lista;
        Conexao c = new Conexao();
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.indexDiarista);
            nomeDiarista = Intent.GetStringExtra("nome");
            id_diarista = Intent.GetStringExtra("id_d");
            Toast.MakeText(Application.Context, "bem-vindo(a), " + nomeDiarista + " !", ToastLength.Long).Show();

            btListarVagas = FindViewById<Button>(Resource.Id.btnDetalhesVaga);
            bVerMensagens = FindViewById<Button>(Resource.Id.btVerMsgs);
            spinnerRegiao = FindViewById<Spinner>(Resource.Id.carregaRegiao);
            spinnerServicos = FindViewById<Spinner>(Resource.Id.carregaServico);
            lista = FindViewById<ListView>(Resource.Id.listaVagas);


            LogoutBT = FindViewById<Button>(Resource.Id.logout);

            AlimentarSpinnerRegioes();
            AlimentaSpinnerServicos();


            btListarVagas.Click += btListar_Click;
            lista.ItemClick += Lista_ItemClick;

            LogoutBT.Click += LogOut_Click;
            bVerMensagens.Click += IrTelaMensagens;
        }

        private void Lista_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            idpre = listaPreServico[e.Position];
            //Toast.MakeText(Application.Context, idpre, ToastLength.Long).Show();
            var chamaTelaDetalhes = new Intent(this, typeof(DetalhesVaga) );
            chamaTelaDetalhes.PutExtra("idpres", idpre);
            chamaTelaDetalhes.PutExtra("id_diarista", id_diarista);

            //Toast.MakeText(Application.Context, id_diarista, ToastLength.Long).Show();
            StartActivity(chamaTelaDetalhes);
        }

        private void LogOut_Click(object sender, EventArgs e)
        {
            var voltaLogin = new Intent(this, typeof(Login) );
            StartActivity(voltaLogin);

            Toast.MakeText(Application.Context, "Você saiu. Volte sempre!", ToastLength.Short).Show();
        }

        private void IrTelaMensagens(object sender, EventArgs e)
        {
            var irTelaMsg = new Intent(this, typeof(MensagensDiarista) );
            irTelaMsg.PutExtra("id_d", id_diarista);
            StartActivity(irTelaMsg);
        }
        /*private void listaDescompactadaRegioes_Click(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            idservico = listaidServico[e.Position];
            idregiao = listaidRegiao[e.Position];

            var chamaTelaDetalhes = new Intent(this, typeof(DetalhesVaga));
            chamaTelaDetalhes.PutExtra("@idservico", idservico);
            chamaTelaDetalhes.PutExtra("@idregiao",  idregiao);
            StartActivity(chamaTelaDetalhes);
        }*/

        private void btListar_Click(object sender, EventArgs e)
        {
            string sql;
            try
            {
                c.AbrirCon();
                MySqlCommand cmd;
                MySqlDataReader lerVagas;

                sql = "SELECT idpreservico, CONCAT('Cliente ',nome,' solicita ', desc_servico, ' para o dia', DATE_FORMAT(data_do_servico,' %d /%m/%Y')) mensagem FROM preservico, cliente, endereco,regiao r, rl_comodos_servico rl, servico  WHERE fkcliente = idcliente AND fkendereco = idendereco AND fkregiao = r.id AND r.id = @idregiaoSpinner AND fkcomodos = rl.id AND idservico = id_servico AND idservico = @idServicoSPINNER";
                cmd = new MySqlCommand(sql, c.conn);
                cmd.Parameters.AddWithValue("@idregiaoSpinner", listaidRegiao[spinnerRegiao.SelectedItemPosition]);
                cmd.Parameters.AddWithValue("@idServicoSPINNER", listaidServico[spinnerServicos.SelectedItemPosition]);
                lerVagas = cmd.ExecuteReader();

                if (lerVagas.HasRows)
                {
                    while ( lerVagas.Read() )
                    {
                        listaPreServico.Add(lerVagas["idpreservico"].ToString() );
                        listaDeMensagem.Add(lerVagas["mensagem"].ToString() );
                    }
                    ArrayAdapter<string> a = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, listaDeMensagem);
                    lista.Adapter = a;
                }
                else {
                    Toast.MakeText(Application.Context, "Não foi encontrado esse serviço para essa localidade: ", ToastLength.Long).Show();
                }
            }
            catch (Exception ee)
            {
                Toast.MakeText(Application.Context, "erro ao listar vagas: " + ee, ToastLength.Long).Show();
            }
        }


        private void AlimentarSpinnerRegioes()
        {

            string regioes;
            c.AbrirCon();
            try
            {
                regioes = "SELECT id, desc_regiao FROM regiao";
                MySqlCommand comando;
                MySqlDataReader lerDados;
                comando = new MySqlCommand(regioes, c.conn);
                lerDados = comando.ExecuteReader();

                if (lerDados.HasRows)
                {
                    while (lerDados.Read())
                    {
                        listaidRegiao.Add(lerDados["id"].ToString());
                        listaDescRegiao.Add(lerDados["desc_regiao"].ToString());
                    }
                    ArrayAdapter<string> a = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, listaDescRegiao);
                    spinnerRegiao.Adapter = a;
                }

            }
            catch (Exception ee)
            {
                Toast.MakeText(Application.Context, "Erro ao ler:" + ee, ToastLength.Short).Show();
            }
        }

        private void AlimentaSpinnerServicos()
        {
            string servicos;

            try
            {
                c.AbrirCon();
                MySqlCommand cmd;
                MySqlDataReader lerServicos;
                servicos = "SELECT idservico, desc_servico FROM servico";
                cmd = new MySqlCommand(servicos, c.conn);
                lerServicos = cmd.ExecuteReader();

                if (lerServicos.HasRows)
                {
                    while (lerServicos.Read())
                    {
                        listaidServico.Add(lerServicos["idservico"].ToString());
                        listaDescServico.Add(lerServicos["desc_servico"].ToString());
                    }
                    ArrayAdapter<string> a = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleExpandableListItem1, listaDescServico);
                    spinnerServicos.Adapter = a;
                }
            }
            catch (Exception ee)
            {
                Toast.MakeText(Application.Context, "não foi possível mostrar a lista de serviços:" + ee, ToastLength.Short).Show();
            }
        }

    }
}