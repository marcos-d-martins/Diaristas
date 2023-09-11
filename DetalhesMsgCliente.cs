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
    [Activity(Label = "DetalhesMsgCliente")]
    public class DetalhesMsgCliente : Activity
    {
        Conexao c = new Conexao();
        TextView msgResposta;
        EditText eResponder;
        Button botaoResponder;
        string id_msg, id_cliente,id_diarista,id_pre,data_hora_da_msg, msg;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.detalhesMsgCliente);

            msgResposta = FindViewById<TextView>(Resource.Id.txtMsg);
            eResponder = FindViewById<EditText>(Resource.Id.edtResponder);
            botaoResponder = FindViewById<Button>(Resource.Id.btnResponder);

            id_pre = Intent.GetStringExtra("id_pre");
            id_diarista = Intent.GetStringExtra("id_d");
            id_msg = Intent.GetStringExtra("id_msg");
            id_cliente = Intent.GetStringExtra("id_c");
            Toast.MakeText(Application.Context, "id préserviço:: "+id_pre, ToastLength.Short).Show();
            Toast.MakeText(Application.Context, "id diarista: "+id_diarista, ToastLength.Short).Show();
            Toast.MakeText(Application.Context, "id mensagem: "+id_msg, ToastLength.Short).Show();
            Toast.MakeText(Application.Context, "id do cliente :"+id_cliente, ToastLength.Short).Show();


            VerDetalhesMsg();
            botaoResponder.Click += BotaoResposta_Click;
        }

        private void BotaoResposta_Click(object sender, EventArgs e)
        {
            string insercao;
            try
            {
                c.AbrirCon();
                try
                {
                    MySqlCommand insere;
                    insercao = "INSERT INTO mensagens VALUES(NULL,@msg,@idpre,@id_d,NOW(),NULL, NULL)";
                    insere = new MySqlCommand(insercao, c.conn);
                    insere.Parameters.AddWithValue("@msg",eResponder.Text);
                    insere.Parameters.AddWithValue("@idpre",id_pre);
                    insere.Parameters.AddWithValue("@id_d",id_diarista);
                    insere.Parameters.AddWithValue("@id_c", id_cliente);

                    if ( insere.ExecuteNonQuery() > 0 )
                    {
                        Toast.MakeText(Application.Context, "mensagem enviada!", ToastLength.Short).Show();
                        var voltarTelaInicio = new Intent(this, typeof(IndexCliente) );
                        StartActivity(voltarTelaInicio);
                    }
                }
                catch (Exception exc)
                {
                    Toast.MakeText(Application.Context, "erro ao enviar msg: "+exc , ToastLength.Short).Show();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void VerDetalhesMsg()
        {
            string sql;
            try
            {
                c.AbrirCon();
                MySqlCommand cmd;
                MySqlDataReader ler;
                sql = "SELECT mensagem, d.nome AS nome_diarista FROM mensagens ms INNER JOIN diarista d ON d.iddiarista = ms.idlogin_diarista INNER JOIN preservico ps ON ps.idpreservico = ms.idpreservico WHERE idcliente = @id_c AND idmensagem = @idM;";
                cmd = new MySqlCommand(sql, c.conn);
                cmd.Parameters.AddWithValue("@id_c",id_cliente);
                cmd.Parameters.AddWithValue("@idM",id_msg);
                ler = cmd.ExecuteReader();

                if( ler.HasRows) {
                    while( ler.Read() ) {
                        msgResposta.Text = ler["mensagem"].ToString();
                    }
                }
            }
            catch (Exception e)
            {
                Toast.MakeText(Application.Context,"Erro ao listar detalhes msg: "+e,ToastLength.Short).Show();
            }
        }
    }
}