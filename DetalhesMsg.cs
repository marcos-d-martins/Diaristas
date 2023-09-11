using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.Nio.Channels;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace diaria
{
    [Activity(Label = "DetalhesMsg")]
    public class DetalhesMsg : Activity
    {
        string id_m, id_d;
        TextView nome_cliente, logradouro, complemento, cidade,
            qtdComodos, desc_comodo, desc_regiao, desc_servico, data_servico, hor_inicio, hor_fim, conteudomsg;

        Conexao c = new Conexao();
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.detalhesMsg);
            id_m = Intent.GetStringExtra("id_m");
            id_d = Intent.GetStringExtra("id_diarista");

            Toast.MakeText(Application.Context, "id diarista: "+id_d, ToastLength.Short).Show();
            Toast.MakeText(Application.Context, "id mensagem: "+id_m, ToastLength.Short).Show();

            nome_cliente = FindViewById<TextView>(Resource.Id.txtNomeCliente);
            data_servico = FindViewById<TextView>(Resource.Id.txtData);
            /*hor_inicio = FindViewById<TextView>(Resource.Id.txtHoraInicio);
            hor_fim = FindViewById<TextView>(Resource.Id.txtHoraFim);
            logradouro = FindViewById<TextView>(Resource.Id.txtLogradouro);
            complemento = FindViewById<TextView>(Resource.Id.txtComplemento);
            desc_regiao = FindViewById<TextView>(Resource.Id.txtCidade);
            qtdComodos = FindViewById<TextView>(Resource.Id.txtQtdComodos);
            desc_comodo = FindViewById<TextView>(Resource.Id.txtComodo);
            desc_servico = FindViewById<TextView>(Resource.Id.txtServico);*/
            

            DetalharMsgE_Responder();
        }

        private void DetalharMsgE_Responder()
        {
            string sql;
            try
            {
                c.AbrirCon();
                MySqlCommand cmd;
                MySqlDataReader lerMsg;
                sql = "SELECT cl.nome AS nome_cliente, d.nome AS nome_diarista, logradouro,complemento,cidade,desc_regiao, qtdComodos, descricaoComodo, desc_servico, DATE_FORMAT(data_do_servico, '%d / %m / %Y') AS data_servico, horario_inicio, horario_fim, mensagem FROM preservico ps INNER JOIN mensagens ms ON ms.idpreservico = ps.idpreservico INNER JOIN diarista d ON d.iddiarista = ms.idlogin_diarista INNER JOIN cliente cl ON cl.idcliente = ps.fkcliente INNER JOIN endereco e ON e.idendereco = cl.fkendereco INNER JOIN regiao r ON r.id = e.fkregiao INNER JOIN comodos cm ON cm.idcomodos = ps.fkcomodos INNER JOIN rl_comodos_servico rlcs ON rlcs.id_comodo = cm.idcomodos INNER JOIN servico s ON s.idservico = rlcs.id_servico INNER JOIN itens_servico its ON its.id_servico = s.idservico WHERE idmensagem = @idmsg";
                cmd = new MySqlCommand(sql, c.conn);
                cmd.Parameters.AddWithValue("@idmsg", id_m);
                lerMsg = cmd.ExecuteReader();

                if ( lerMsg.HasRows )
                {
                    while( lerMsg.Read() )
                    {
                        nome_cliente.Text = lerMsg["nome_cliente"].ToString();
                        logradouro.Text += lerMsg["logradouro"].ToString();
                        complemento.Text += lerMsg["complemento"].ToString();
                        cidade.Text += lerMsg["cidade"].ToString();
                        desc_regiao.Text += lerMsg["desc_regiao"].ToString();
                        qtdComodos.Text += lerMsg["qtdComodos"].ToString();
                        desc_comodo.Text += lerMsg["descricaoComodo"].ToString();
                        desc_servico.Text += lerMsg["desc_servico"].ToString();
                        data_servico.Text += lerMsg["data_servico"].ToString();
                        hor_inicio.Text += lerMsg["horario_inicio"].ToString();
                        hor_fim.Text += lerMsg["horario_fim"].ToString();
                        conteudomsg.Text += lerMsg["mensagem"].ToString();
                        
                        Toast.MakeText(Application.Context, nome_cliente.Text,ToastLength.Short).Show();
                        Toast.MakeText(Application.Context, logradouro.Text,ToastLength.Short).Show();
                        Toast.MakeText(Application.Context, "complemento:"+complemento.Text,ToastLength.Short).Show();
                        Toast.MakeText(Application.Context, cidade.Text,ToastLength.Short).Show();
                        Toast.MakeText(Application.Context, desc_regiao.Text,ToastLength.Short).Show();
                        Toast.MakeText(Application.Context, "quantidade comodos:"+qtdComodos.Text,ToastLength.Short).Show();
                        Toast.MakeText(Application.Context, desc_comodo.Text,ToastLength.Short).Show();
                        Toast.MakeText(Application.Context, "hora início:"+hor_inicio.Text,ToastLength.Short).Show();
                        Toast.MakeText(Application.Context, hor_fim.Text,ToastLength.Short).Show();
                        Toast.MakeText(Application.Context, conteudomsg.Text,ToastLength.Short).Show();
                        Toast.MakeText(Application.Context, data_servico.Text,ToastLength.Short).Show();
                    }
                }
            }
            catch (Exception e)
            {
                Toast.MakeText(Application.Context, "erro ao listar mensagem: "+e, ToastLength.Short).Show();
            }
        }
    }
}