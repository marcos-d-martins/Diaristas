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
    [Activity(Label = "CadastroDiarista")]
    public class CadastroDiarista : Activity
    {
        EditText eNome, eEmail, eCpf, eTelefone, eSenha, eDescricao, eRespostaPerguntaSecreta;
        Spinner lsEstado, lsAvaliacao;
        Button botaoSalvar;
        string nomeCliente;
        Conexao c = new Conexao();

        protected override void OnCreate(Bundle savedInstanceState) {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.cadastroDiarista);


            eNome = FindViewById<EditText>(Resource.Id.edtNomeCompleto);
            eEmail = FindViewById<EditText>(Resource.Id.edtEmail);
            eCpf = FindViewById<EditText>(Resource.Id.edtCpf);
            eTelefone = FindViewById<EditText>(Resource.Id.edtTelefone);
            eSenha = FindViewById<EditText>(Resource.Id.edtSenha);
            eDescricao = FindViewById<EditText>(Resource.Id.edtDescricao);
            eRespostaPerguntaSecreta = FindViewById<EditText>(Resource.Id.edtRespostaDaPergunta);

            botaoSalvar = FindViewById<Button>(Resource.Id.btnSalvarCadastro);

            botaoSalvar.Click += CadastrarDiarista;
        }

        private void CadastrarDiarista(object sender, EventArgs e)
        {
            string sql;

            try
            {
                c.AbrirCon();
                MySqlCommand cmd;

                sql = "INSERT INTO diarista(iddiarista,nome,email,senha,telefone,respostaPerguntaSecreta, descricao,cpf,fk_endereco)" +
                    " VALUES(NULL,@n,@e,@sen,@tel,@resp, @desc,@cpf, @fk_End)";
                cmd = new MySqlCommand(sql, c.conn);

                cmd.Parameters.AddWithValue("@n",eNome.Text);
                cmd.Parameters.AddWithValue("@e", eEmail.Text);
                cmd.Parameters.AddWithValue("@sen",eSenha.Text);
                cmd.Parameters.AddWithValue("@tel",eTelefone.Text);
                cmd.Parameters.AddWithValue("@resp",eRespostaPerguntaSecreta.Text);
                cmd.Parameters.AddWithValue("@desc", eDescricao.Text);
                cmd.Parameters.AddWithValue("@cpf", eCpf.Text);
                cmd.Parameters.AddWithValue("@fk_end", 1);

                if (cmd.ExecuteNonQuery() > 0) {

                    Toast.MakeText(Application.Context, "Cadastro aprovado!",ToastLength.Short).Show();
                    Toast.MakeText(Application.Context, "Redirecionando você à página para buscar clientes.!",ToastLength.Short).Show();
                    
                    var varInicioDiarista = new Intent(this, typeof(IndexDiarista) );
                    StartActivity(varInicioDiarista);
                }


                //c.FecharCon();
            }
            catch (Exception ex)
            {
                Toast.MakeText(Application.Context, "erro ao cadastrar:" + ex, ToastLength.Long).Show();
            }
        }
    
        public static bool ValidaCPF(string vrCPF) {
            string valor = vrCPF.Replace(".", "");
            valor = valor.Replace("-", "");

            if (valor.Length != 11)
            {
                return false;
            }

            bool igual = true;


            for (int i = 1; i < 11 && igual; i++)

                if (valor[i] != valor[0])
                {
                    igual = false;
                }

            if (igual || valor == "12345678909") { 
                return false;
            }

            int[] numeros = new int[11];

            for (int i = 0; i < 11; i++)
            {
                numeros[i] = int.Parse( valor[i].ToString() );
            }


            int soma = 0;

            for (int i = 0; i < 9; i++)
            {
                soma += (10 - i) * numeros[i];
            }

            int resultado = soma % 11;

            if (resultado == 1 || resultado == 0) {

                if (numeros[9] != 0)

                    return false;
            }

            else if (numeros[9] != 11 - resultado)

                return false;

            soma = 0;

            for (int i = 0; i < 10; i++)

                soma += (11 - i) * numeros[i];

            resultado = soma % 11;
                
            if (resultado == 1 || resultado == 0) {
                if (numeros[10] != 0)

                    return false;
            }

            else

                if (numeros[10] != 11 - resultado)

                return false;
            return true;
        }

    }
}