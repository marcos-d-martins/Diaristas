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
    [Activity(Label = "RecuperaSenha")]
    public class RecuperaSenha : Activity
    {
        Conexao c = new Conexao();
        EditText eRespostaDaPergunta, eEmailRecuperacao;
        Button btRespostaRecuperaSenha;

        int contador = 0;
        string email_c, email_di;
        string cpf, dt_nascimento, id_diarista, id_cliente;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.recuperaSenha);


            eEmailRecuperacao = FindViewById<EditText>(Resource.Id.edtEmailRecuperacao);
            eRespostaDaPergunta = FindViewById<EditText>(Resource.Id.edtRespostaDaPergunta);
            btRespostaRecuperaSenha = FindViewById<Button>(Resource.Id.btnEnviarResposta1);

            btRespostaRecuperaSenha.Click += RecuperarSenha_Click;
        }

        // PERGUNTA SECRETA: QUAL O NOME DA SUA PRIMEIRA PROFESSORA?
        private void RecuperarSenha_Click(object sender, EventArgs e)
        {
            c.AbrirCon();

            try
            {
                string sql;
                sql = "SELECT iddiarista,email, respostaPerguntaSecreta,cpf,DATE_FORMAT(dt_nascimento,'%d/%m/%Y') AS dt_nascimento FROM diarista WHERE email = @e AND respostaPerguntaSecreta = @resp";
                MySqlCommand command;
                MySqlDataReader reader;
                command = new MySqlCommand(sql, c.conn);
                command.Parameters.AddWithValue("@e", eEmailRecuperacao.Text);
                command.Parameters.AddWithValue("@resp",eRespostaDaPergunta.Text);
                reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    //VERIFICAR SE A PERGUNTA SECRETA CORRESPONDE ao VALOR NO BANCO E REDIRECIONAR À TELA RecuperaSenhapasso2.cs 
                    while (reader.Read() )
                    {
                        cpf = reader["cpf"].ToString();
                        dt_nascimento = reader["dt_nascimento"].ToString();
                        id_diarista = reader["iddiarista"].ToString();
                    }
                    contador++;
                    Toast.MakeText(Application.Context, "Certa! Redirecionando para a verificação 2..", ToastLength.Long).Show();
                    var redefinirPasso2 = new Intent(this, typeof(RecuperaSenhaPasso2) );
                    redefinirPasso2.PutExtra("id_d", id_diarista);
                    redefinirPasso2.PutExtra("cpf",cpf);
                    redefinirPasso2.PutExtra("dt_nasc", dt_nascimento);
                    StartActivity(redefinirPasso2);
                }
                else
                {
                    Toast.MakeText(Application.Context, "A resposta secreta está errada. Tente redefinir novamente...", ToastLength.Long);
                    var respostaErradaVoltarLogin= new Intent(this, typeof(Login) );
                    StartActivity(respostaErradaVoltarLogin);

                    c.FecharCon();
                }
            }
            catch (Exception exc)
            {
                Toast.MakeText(Application.Context, "Erro: " + exc, ToastLength.Long).Show();
            }


            if (contador == 0)
            {
                c.AbrirCon();
                try
                {
                    string sql2;
                    sql2 = "SELECT idcliente, email, respostaPerguntaSecreta,cpf,DATE_FORMAT(dt_nascimento,'%d/%m/%Y') AS dt_nascimento FROM cliente WHERE email = @em AND respostaPerguntaSecreta = @resp";
                    MySqlCommand buscaCliente;
                    MySqlDataReader lerDados;
                    buscaCliente = new MySqlCommand(sql2, c.conn);
                    buscaCliente.Parameters.AddWithValue("@em", eEmailRecuperacao.Text) ;
                    buscaCliente.Parameters.AddWithValue("@resp", eRespostaDaPergunta.Text);


                    lerDados = buscaCliente.ExecuteReader();

                    if (lerDados.HasRows)
                    {
                        //VERIFICAR SE A PERGUNTA SECRETA CORRESPONDE ao VALOR NO BANCO E REDIRECIONAR À TELA RecuperaSenhapasso2.cs
                        while ( lerDados.Read() )
                        {
                            id_cliente = lerDados["idcliente"].ToString();
                            cpf = lerDados["cpf"].ToString();
                            dt_nascimento = lerDados["dt_nascimento"].ToString();
                        }
                        Toast.MakeText(Application.Context, "Certa! Você está sendo redirecionado para a 2ª etapa de recuperação...", ToastLength.Long).Show();
                        
                        var telaEtapa2 = new Intent(this, typeof(RecuperaSenhaPasso2) );
                        telaEtapa2.PutExtra("id_cl", id_cliente);
                        telaEtapa2.PutExtra("cpf", cpf);
                        telaEtapa2.PutExtra("dt_nasc", dt_nascimento);
                        
                        StartActivity(telaEtapa2);
                    }
                    else
                    {
                        Toast.MakeText(Application.Context, "A resposta secreta está errada. Voltando à tela de Login...", ToastLength.Long).Show();
                        contador = 0;

                        var respErradaVoltaLoginCliente = new Intent(this, typeof(Login) );
                        StartActivity(respErradaVoltaLoginCliente);
                    }
                }
                catch (Exception exc)
                {
                    Toast.MakeText(Application.Context, "Erro: " + exc, ToastLength.Long).Show();
                }
            }


        }
    }
}