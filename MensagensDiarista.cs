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
    [Activity(Label = "MensagensDiarista")]
    public class MensagensDiarista : Activity
    {
        TextView tMsg;
        ListView listagemMsgs;
        List<string> listaIdMsg = new List<string>();
        List<string> msgs = new List<string>();
        List<string> lidas = new List<string>();
        string id_d, id_mensagem;
        Conexao c = new Conexao();
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.mensagensDiarista);

            id_d = Intent.GetStringExtra("id_d");
            tMsg = FindViewById<TextView>(Resource.Id.txtMsg);
            listagemMsgs = FindViewById<ListView>(Resource.Id.listViewMensagens);
            
            ListarMsgs(id_d);
            listagemMsgs.ItemClick += Msg_ListView_Click;
        }

        private void Msg_ListView_Click(object sender, AdapterView.ItemClickEventArgs e)
        {
            id_mensagem = listaIdMsg[e.Position];
            var chamaTelaDetalheMsg = new Intent(this, typeof(DetalhesMsg) );
            chamaTelaDetalheMsg.PutExtra("id_m",id_mensagem);
            chamaTelaDetalheMsg.PutExtra("id_diarista",id_d);
            StartActivity(chamaTelaDetalheMsg);
        }

        private void ListarMsgs(string id_d)
        {
            string sql;
            try
            {
                c.AbrirCon();
                MySqlCommand cmd;
                MySqlDataReader lerMensagens;
                sql = "SELECT idmensagem,mensagem,idpreservico,idlogin_diarista, DATE_FORMAT(data_hora,'%d/%m/%Y %H:%i:%S') AS data_hora FROM mensagens WHERE idcliente IS NULL AND idlogin_diarista = @id_d" ;
                cmd = new MySqlCommand(sql, c.conn);
                cmd.Parameters.AddWithValue("@id_d",id_d);
                lerMensagens = cmd.ExecuteReader();
                
                if ( lerMensagens.HasRows )  {
                    while( lerMensagens.Read() )   {
                        listaIdMsg.Add(lerMensagens["idmensagem"].ToString() );
                        msgs.Add(lerMensagens["mensagem"].ToString() );      
                    }

                    ArrayAdapter<string> a = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, msgs);
                    listagemMsgs.Adapter = a;
                }
                else{
                    Toast.MakeText(Application.Context, "Não há mensagens.",ToastLength.Long).Show();
                }
            }
            catch (Exception ex)
            {
                Toast.MakeText(Application.Context, "erro ao listar mensagens: "+ex, ToastLength.Short).Show();
            }
        }
    }
}