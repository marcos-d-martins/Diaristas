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
    [Activity(Label = "CadastroCliente")]
    public class CadastroCliente : Activity
    {
        Conexao c = new Conexao();
        EditText eNomeCompleto, eEmail, ETelefone, eSenha, eCPF, eRespostaPerguntaSecreta;
        Button btCadastro;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.cadastroCliente);
            eNomeCompleto = FindViewById<EditText>(Resource.Id.edtNome);
            eEmail = FindViewById<EditText>(Resource.Id.edtEmail);
            eSenha = FindViewById<EditText>(Resource.Id.edtSenha);
            ETelefone = FindViewById<EditText>(Resource.Id.edtTelefone);
            eCPF = FindViewById<EditText>(Resource.Id.edtCpf);

            btCadastro = FindViewById<Button>(Resource.Id.btnSalvarCadastro);
            btCadastro.Click += btCadastro_Click;

        }

        private void btCadastro_Click(object sender, EventArgs e)
        {
     
            c.AbrirCon();
            string sql;

            try
            {
                sql = "INSERT INTO cliente (idcliente,nome,email,senha,cpf,telefone)" +
                    "VALUES(NULL,@n,@e,@sen,@c,@t)";
                MySqlCommand cmd;
                cmd = new MySqlCommand(sql, c.conn);
                cmd.Parameters.AddWithValue("@n", eNomeCompleto.Text);
                cmd.Parameters.AddWithValue("@e", eEmail.Text);
                cmd.Parameters.AddWithValue("@sen", eSenha.Text);
                cmd.Parameters.AddWithValue("@c", eCPF.Text);
                cmd.Parameters.AddWithValue("@t", ETelefone.Text);


                if (cmd.ExecuteNonQuery() > 0) {
                    Toast.MakeText(Application.Context, "cadastro efetuado com sucesso!", ToastLength.Short).Show();
                    Toast.MakeText(Application.Context, "Efetue o login para buscar diaristas. :)", ToastLength.Long).Show();
                    var direcionaParaLogin = new Intent(this, typeof(Login));
                    StartActivity(direcionaParaLogin);
                }
                else {
                    Toast.MakeText(Application.Context, "erro.", ToastLength.Long).Show();
                }
            }
            catch (Exception ee) {
                Toast.MakeText(Application.Context, "Erro ao cadastrar: " + ee, ToastLength.Long).Show();
            }
        }
    }
}