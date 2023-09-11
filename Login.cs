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
    [Activity(Label = "Login")]


    public class Login : Activity
    {
        Conexao c = new Conexao();
        EditText EdUsuario, EdSenha;
        string idcliente, id_d, nomeUsuario, sql2, sql, emailRecuperarSenhaDiarista, emailRecuperarSenhaCliente, senha;
        int Cont = 0;
        Button BtLogin, BtEsqueciSenha;
        string caracteresComparativos = "abcdefghijklmnopqrstuvwxyz";
        string caracteresMAIUSCULOS = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        string caracteresEspeciais = "!@#$%¨&*()^^~~``´´.+_-[]{};:";
        string stringNumeros = "0123456789";
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.login);
            BtLogin = FindViewById<Button>(Resource.Id.btnLogin);
            BtEsqueciSenha = FindViewById<Button>(Resource.Id.btnEsqueciSenha);

            EdUsuario = FindViewById<EditText>(Resource.Id.btnUsuario);
            EdSenha = FindViewById<EditText>(Resource.Id.btnSenha);
            idcliente = Intent.GetStringExtra("idcliente");

            BtLogin.Click += delegate
            {
                fazerLogin();
            };

            BtEsqueciSenha.Click += BtEsqueciSenha_Click;

        }

        // PERGUNTA SECRETA: QUAL O NOME DA SUA PRIMEIRA PROFESSORA?
        private void BtEsqueciSenha_Click(object sender, EventArgs e)
        {
            var telaRecuperaSenha = new Intent(this, typeof(RecuperaSenha) );
            telaRecuperaSenha.PutExtra("emailCliente", emailRecuperarSenhaCliente);
            telaRecuperaSenha.PutExtra("emailDiarista", emailRecuperarSenhaDiarista);
            StartActivity(telaRecuperaSenha);
        }

        private void fazerLogin()
        {
            c.AbrirCon();

            try
            {
                sql = "SELECT iddiarista, nome, email, senha FROM diarista WHERE email = @e AND senha = @s";
                MySqlCommand command;
                MySqlDataReader ler;
                command = new MySqlCommand(sql, c.conn);
                command.Parameters.AddWithValue("@e", EdUsuario.Text);
                command.Parameters.AddWithValue("@s", EdSenha.Text);

                ler = command.ExecuteReader();

                if (ler.HasRows)
                {
                    while (ler.Read() )
                    {
                            StringComparison comparar;
                            nomeUsuario = ler["nome"].ToString();
                            id_d = ler["iddiarista"].ToString();
                            emailRecuperarSenhaDiarista = ler["email"].ToString();
                            senha = ler["senha"].ToString();
                        if ( senha.Contains(caracteresMAIUSCULOS.Substring()) )  {
                            Toast.MakeText(Application.Context, "Têm maiúsculo!",ToastLength.Short).Show();
                        }
                        else
                        {
                            Toast.MakeText(Application.Context, "Não contém caracteres maiúsculos.", ToastLength.Short).Show();
                        }
                    }
                    Cont++;
                    Toast.MakeText(Application.Context, "Diarista autenticada com sucesso. Vc Será redirecionado...", ToastLength.Long).Show();
                    var telaIndexDiarista = new Intent(this, typeof(IndexDiarista) );
                    telaIndexDiarista.PutExtra("nome", nomeUsuario);
                    telaIndexDiarista.PutExtra("id_d", id_d);
                    StartActivity(telaIndexDiarista);
                }
                else
                {
                    c.FecharCon();
                }
            }
            catch (Exception e)
            {
                Toast.MakeText(Application.Context, "Erro: " + e, ToastLength.Long).Show();
            }


            if (Cont == 0)
            {
                c.AbrirCon();
                try
                {
                    sql2 = "SELECT idcliente, nome, email, senha FROM cliente WHERE email = @em AND senha = @se";
                    MySqlCommand buscaCliente;
                    MySqlDataReader lerDados;
                    buscaCliente = new MySqlCommand(sql2, c.conn);
                    buscaCliente.Parameters.AddWithValue("@em", EdUsuario.Text);
                    buscaCliente.Parameters.AddWithValue("@se", EdSenha.Text);

                    lerDados = buscaCliente.ExecuteReader();

                    if (lerDados.HasRows)
                    {
                        while ( lerDados.Read() )
                        {
                            idcliente = lerDados["idcliente"].ToString();
                            nomeUsuario = lerDados["nome"].ToString();
                            emailRecuperarSenhaCliente = lerDados["email"].ToString();
                        }
                        Toast.MakeText(Application.Context, "Você está sendo redirecionado...", ToastLength.Long).Show();
                        var telaIndexCliente = new Intent(this, typeof(IndexCliente));
                        telaIndexCliente.PutExtra("nome", nomeUsuario);
                        telaIndexCliente.PutExtra("idcliente", idcliente);

                        StartActivity(telaIndexCliente);
                    }
                    else
                    {
                        Toast.MakeText(Application.Context, "Dados incorretos: ", ToastLength.Long).Show();
                        Cont = 0;
                    }
                }
                catch (Exception e)
                {
                    Toast.MakeText(Application.Context, "Erro: " + e, ToastLength.Long).Show();
                }
            }


        }

    }
}
