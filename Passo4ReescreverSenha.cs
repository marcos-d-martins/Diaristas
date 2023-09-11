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
    [Activity(Label = "Passo4ReescreverSenha")]
    public class Passo4ReescreverSenha : Activity
    {
        Conexao c = new Conexao();
        EditText eNovaSenha, eNovaSenhaRepetir;
        Button bGravar;
        int verificadorDiarista = 0;
        string id_diarista, id_cliente;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.passo4ReescreverSenha);


            id_diarista = Intent.GetStringExtra("diarista_id");
            id_cliente = Intent.GetStringExtra("cliente_id");
            eNovaSenha = FindViewById<EditText>(Resource.Id.edtNovaSenha);
            eNovaSenhaRepetir = FindViewById<EditText>(Resource.Id.edtNovaSenhaRepetir);
            bGravar = FindViewById<Button>(Resource.Id.btnGravar);

            bGravar.Click += Gravar;
            // Create your application here
        }

        private void Gravar(object sender, EventArgs e)
        {
            string sql;
            try
            {
                c.AbrirCon();
                MySqlCommand alterarSenha;
                sql = "UPDATE diarista SET senha = @s WHERE iddiarista = @id_d";
                alterarSenha = new MySqlCommand(sql, c.conn);
                alterarSenha.Parameters.AddWithValue("@s", eNovaSenha.Text);
                alterarSenha.Parameters.AddWithValue("@id_d", id_diarista);

                if (eNovaSenha.Text != eNovaSenhaRepetir.Text)
                {
                    Toast.MakeText(Application.Context, "A senha e confirmação não são iguais! Digite novamente.", ToastLength.Long).Show();
                }
                else
                {
                    if (alterarSenha.ExecuteNonQuery() > 0)
                    {
                        Toast.MakeText(Application.Context, "Senha alterada com sucesso! Redirecionando ao Login..", ToastLength.Long).Show();
                        var SucessoLoginSenhaAlterada = new Intent(this, typeof(Login));
                        StartActivity(SucessoLoginSenhaAlterada);
                    }
                }
                verificadorDiarista++;
            }

            catch (Exception excep)
            {
                Toast.MakeText(Application.Context, "erro ao editar senha: " + excep, ToastLength.Long).Show();
            }

            if (verificadorDiarista == 0)
            {
                string sql2;
                try
                {
                    c.AbrirCon();
                    MySqlCommand alterarSenhaCliente;
                    sql2 = "UPDATE cliente SET senha = @s WHERE idcliente = @id_c";
                    alterarSenhaCliente = new MySqlCommand(sql2, c.conn);
                    alterarSenhaCliente.Parameters.AddWithValue("@s", eNovaSenha.Text);
                    alterarSenhaCliente.Parameters.AddWithValue("@id_c", id_cliente);

                    if (eNovaSenha.Text != eNovaSenhaRepetir.Text)
                    {
                        Toast.MakeText(Application.Context, "A senha e confirmação não são iguais! Digite novamente.", ToastLength.Long).Show();
                    }
                    else
                    {
                        if (alterarSenhaCliente.ExecuteNonQuery() > 0)
                        {
                            Toast.MakeText(Application.Context, "Senha alterada com sucesso! Redirecionando ao Login..", ToastLength.Long).Show();
                            var SucessoLoginSenhaAlterada = new Intent(this, typeof(Login));
                            StartActivity(SucessoLoginSenhaAlterada);
                        }
                    }
                    verificadorDiarista = 0;

                }
                catch (Exception excep)
                {
                    Toast.MakeText(Application.Context, "erro ao editar senha: " + excep, ToastLength.Long).Show();
                }
            }
        }
    }
}