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
using System.Text;

namespace diaria
{
    [Activity(Label = "MensagensCliente")]
    public class MensagensCliente : Activity
    {
        Button bAvaliacoes;
        ListView lvListaMensagens;
        List<string> idListaMensagens = new List<string>();
        List<string> listaMensagens = new List<string>();
        Conexao c = new Conexao();
        string idCliente, idMsg_string, id_preservico, id_diarista, data_hora_da_mensagem, msg;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.mensagensCliente);

            lvListaMensagens = FindViewById<ListView>(Resource.Id.listViewMensagens);
            idCliente = Intent.GetStringExtra("id_c");
            //            Toast.MakeText(Application.Context,idCliente,ToastLength.Short).Show();
            LerMGSsDoBanco();

            lvListaMensagens.ItemClick += DetalhesMSG_ItemClick;
        }

        private void DetalhesMSG_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
         // id tipo STRING recebe id tipo List<> QUE APONTA A POSIÇÃO À QUAL ESTÁ O id RECUPERADO NO MÉTODO DE SELECT.
            idMsg_string = idListaMensagens[e.Position];

            var irTelaDetalhesMsgCliente = new Intent(this, typeof(DetalhesMsgCliente) );
            irTelaDetalhesMsgCliente.PutExtra("id_msg", idMsg_string);
            irTelaDetalhesMsgCliente.PutExtra("id_c", idCliente);
            irTelaDetalhesMsgCliente.PutExtra("data_hora_da_msg", data_hora_da_mensagem);
            irTelaDetalhesMsgCliente.PutExtra("id_pre", id_preservico);
            irTelaDetalhesMsgCliente.PutExtra("id_d", id_diarista);
            StartActivity(irTelaDetalhesMsgCliente);
        }


        private void LerMGSsDoBanco()
        {
            string consulta;
            try
            {
                c.AbrirCon();
                MySqlCommand cmd;
                MySqlDataReader ler;
                consulta = "SELECT idmensagem,ps.idpreservico AS id_preservico,d.iddiarista AS id_d,data_hora, CONCAT('a diarista ',nome,' lhe enviou a mensagem... ', mensagem) AS conteudo_msg, d.nome AS nome_diarista,mensagem FROM mensagens ms INNER JOIN diarista d ON d.iddiarista = ms.idlogin_diarista INNER JOIN preservico ps ON ps.idpreservico = ms.idpreservico WHERE idcliente = @id_c";
                cmd = new MySqlCommand(consulta, c.conn);
                cmd.Parameters.AddWithValue("@id_c", idCliente);
                ler = cmd.ExecuteReader();

                if (  ler.HasRows  )
                {
                    while( ler.Read() )
                    {
                        idListaMensagens.Add(ler["idmensagem"].ToString() );
                        listaMensagens.Add(ler["conteudo_msg"].ToString() );
                        id_preservico = ler["id_preservico"].ToString();
                        id_diarista = ler["id_d"].ToString();
                        data_hora_da_mensagem = ler["data_hora"].ToString();
                        msg = ler["mensagem"].ToString();
                    }
                    ArrayAdapter<string> a = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, listaMensagens);
                    lvListaMensagens.Adapter = a;
                }
                else {
                    Toast.MakeText(Application.Context, "Não há mensagens.", ToastLength.Long).Show();
                }
            }
            catch (Exception exc)
            {
                Toast.MakeText(Application.Context, "erro ao listar msgs: "+exc, ToastLength.Short).Show();
            }
        }
    }
}