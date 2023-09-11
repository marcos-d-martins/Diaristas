using Android.App;
using Android.Content;
using Android.OS;
using Android.Provider;
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
    [Activity(Label = "AvaliacaoDiarista")]
    public class AvaliacaoDiarista : Activity
    {
        Conexao c = new Conexao();
        TextView tNomeDiarista;
        EditText eEscreverComentario;
        RadioButton nota;
        Button gravar;
        string id_diarista, idcliente;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.avaliacaoDiarista);
            idcliente = Intent.GetStringExtra("id_cl");

            eEscreverComentario = FindViewById<EditText>(Resource.Id.edtEscreveComentario);
            nota = FindViewById<RadioButton>(Resource.Id.rdUmaEstrela);
            nota = FindViewById<RadioButton>(Resource.Id.rdDuasEstrelas);
            nota = FindViewById<RadioButton>(Resource.Id.rdTresEstrelas);
            nota = FindViewById<RadioButton>(Resource.Id.rdQuatroEstrelas);
            nota = FindViewById<RadioButton>(Resource.Id.rdCincoEstrelas);
            gravar = FindViewById<Button>(Resource.Id.btnEnviarAvaliacao);

            tNomeDiarista = FindViewById<TextView>(Resource.Id.txtNomeDiarista);
            Toast.MakeText(Application.Context, tNomeDiarista.ToString(), ToastLength.Short).Show();

            carregarInfoDiarista();

            nota.Click += nota1_Click;
            nota.Click += nota2_Click;
            nota.Click += nota3_Click;
            nota.Click += nota4_Click;
            nota.Click += nota5_Click;
            
            gravar.Click += Avaliar;
        }

        private void nota5_Click(object sender, EventArgs e)
        {
            nota.Text = "5";
        }

        private void nota4_Click(object sender, EventArgs e)
        {
            nota.Text = "4";
        }

        private void nota3_Click(object sender, EventArgs e)
        {
            nota.Text = "3";
        }

        private void nota2_Click(object sender, EventArgs e)
        {
            nota.Text = "2";
        }

        private void nota1_Click(object sender, EventArgs e)
        {
            nota.Text = "1";
        }

        private void carregarInfoDiarista()
        {
            string sql;
            try
            {
                c.AbrirCon();
                sql = "SELECT nome,fkdiarista,fkcliente,comentario,nota FROM avaliacao a INNER JOIN diarista d ON d.iddiarista = a.fkdiarista WHERE fkdiarista = @id_d";
                MySqlCommand cmd;
                MySqlDataReader ler;
                cmd = new MySqlCommand(sql, c.conn);
                cmd.Parameters.AddWithValue("@id_d",id_diarista);
                ler = cmd.ExecuteReader();

                if (ler.HasRows)
                {
                    while( ler.Read() )
                    {
                        tNomeDiarista.Text = ler["nome"].ToString();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("erro ao ler dados da diarista:"+e);
            }
        }

        private void Avaliar(object sender, EventArgs e)
        {
            string insercao;
            string seleciona_dados;
            try
            {
                c.AbrirCon();
                MySqlCommand insere;
                MySqlDataReader lerAvaliacoes;

                seleciona_dados = "SELECT idavaliacao,fkdiarista,fkcliente,comentario,nota FROM avaliacao";
                insercao = "INSERT INTO avaliacao (idavaliacao,fkdiarista,fkcliente,comentario,nota) VALUES(NULL,@id_d,@id_cl,@c,@nota)";
                insere = new MySqlCommand(insercao, c.conn);

                insere.Parameters.AddWithValue("@id_d",id_diarista);
                insere.Parameters.AddWithValue("@id_cl",idcliente);
                insere.Parameters.AddWithValue("@c",eEscreverComentario.Text);
                insere.Parameters.AddWithValue("@nota",nota.Text);
                

                if (insere.ExecuteNonQuery() > 0 )
                {
                    Toast.MakeText(Application.Context, "avaliação enviada! Obrigado.", ToastLength.Short).Show();
                    var redirecionaTelaInicio = new Intent(this, typeof(IndexCliente) );
                    StartActivity(redirecionaTelaInicio);
                }
            }
            catch (Exception exc)
            {
                Console.WriteLine("erro ao enviar avaliação: " + exc);
            }
        }
    }
}