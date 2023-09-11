using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Security;
using Android.Views;
using Android.Widget;
using Microsoft.SqlServer.Server;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace diaria
{
    [Activity(Label = "DetalhesVaga")]
    public class DetalhesVaga : Activity
    {
        string idregiao,descServico, idPre, id_d;
        List<string> listaidServico = new List<string>();
        List<string> listaidRegiao = new List<string>();
        Button btEnviaMsg;
        TextView txtNome,txtRegiao,txtData,txtServico,txtQtdComodos;
        EditText enviaMsg;
        
        Conexao c = new Conexao();
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.detalhesVaga);

            idPre = Intent.GetStringExtra("idpres");
            id_d = Intent.GetStringExtra("id_diarista");

            txtNome = FindViewById<TextView>(Resource.Id.tNome);
            txtServico = FindViewById<TextView>(Resource.Id.tServico);
            txtData = FindViewById<TextView>(Resource.Id.tData);
            txtRegiao = FindViewById<TextView>(Resource.Id.tRegiao);
            txtQtdComodos = FindViewById<TextView>(Resource.Id.tQtdComodos);
            enviaMsg = FindViewById<EditText>(Resource.Id.edtMsg);

            btEnviaMsg = FindViewById<Button>(Resource.Id.btnEnviaMensagem);

            descricaoVaga(idPre);

            btEnviaMsg.Click += BtEnviaMsg_Click;

        }

        private void BtEnviaMsg_Click(object sender, EventArgs e)
        {
            string sql;
            try
            {
                c.AbrirCon();
                sql = "INSERT INTO mensagens(idmensagem,mensagem,idpreservico,idlogin_diarista,data_hora,idcliente) VALUES(NULL,@m,@idpre,@idDiarista,NOW(),1 )";
                MySqlCommand cmd;

                cmd = new MySqlCommand(sql, c.conn);
                cmd.Parameters.AddWithValue("@m", enviaMsg.Text);
                cmd.Parameters.AddWithValue("@idpre", idPre);
                cmd.Parameters.AddWithValue("@idDiarista", id_d);
                //cmd.ExecuteNonQuery();

                if (cmd.ExecuteNonQuery() > 0)
                {
                    Toast.MakeText(Application.Context, "Mensagem enviada!", ToastLength.Short).Show();
                    var chamaIndex = new Intent(this, typeof(IndexDiarista));
                    StartActivity(chamaIndex);
                }
            }
            catch (Exception ee)
            {
                Toast.MakeText(Application.Context, "erro cadastrar msg: " + ee, ToastLength.Short).Show();
            }
        }

        // MÉTODO QUE DETALHA A VAGA EM QUE A DIARISTA ESCOLHERÁ OU NÃO.
        private void descricaoVaga(string idpre)
        {
            //Toast.MakeText(Application.Context, idpre.ToString(), ToastLength.Long).Show();

            c.AbrirCon();
            string sql;
            
            try
            {
                MySqlCommand cmd;
                MySqlDataReader lerVaga;
                sql = "SELECT idpreservico, DATE_FORMAT(data_do_servico,'%d/%m/%Y') AS data_servico , desc_servico,desc_regiao, qtdComodos, cl.nome AS nome_cliente FROM preservico, cliente cl, endereco, regiao r, rl_comodos_servico rl,servico,comodos co WHERE fkcliente = idcliente AND fkendereco = idendereco AND fkregiao = r.id AND fkcomodos = rl.id AND idservico = id_servico AND co.idcomodos = id_comodo AND idpreservico = @idpreservico";
                cmd = new MySqlCommand(sql, c.conn);
                cmd.Parameters.AddWithValue("@idpreservico", idpre);
                lerVaga = cmd.ExecuteReader();

                if (lerVaga.HasRows)
                {
                    while ( lerVaga.Read() )
                    {
                        txtNome.Text += lerVaga["nome_cliente"].ToString();
                        txtServico.Text += lerVaga["desc_servico"].ToString();
                        txtData.Text += lerVaga["data_servico"].ToString();
                        txtQtdComodos.Text += lerVaga["qtdComodos"].ToString();
                        txtRegiao.Text += lerVaga["desc_regiao"].ToString();
                    }
                }
                else {
                    Toast.MakeText(Application.Context, "Nada foi encontrado: ", ToastLength.Long).Show();
                }
            }
            catch (Exception)
            {
                
            }
        }

        private void GravaMensagem()
        {
            
        }

    }
}