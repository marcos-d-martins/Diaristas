using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace diaria
{
    [Activity(Label = "RecuperaSenhaPasso3")]
    public class RecuperaSenhaPasso3 : Activity
    {
        string dt_nasc, cpf, barra, id_d, id_cl;
        //Conexao c = new Conexao();
        Spinner spinnerCPFs;
        Button btRecuperaSenha;
        ProgressBar p3;
        int contador;
        List<string> listaCPFs = new List<string>{"474.393.020-01", "093.205.020-48", "377.692.750-08" , "355.850.970-67", "687.647.840-88", "799.723.380-61"};
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.recuperaSenhaPasso3);

            id_d = Intent.GetStringExtra("id_d");
            id_cl = Intent.GetStringExtra("id_cl");
            dt_nasc = Intent.GetStringExtra("dt_nasc");
            cpf = Intent.GetStringExtra("cpf");
            p3 = FindViewById<ProgressBar>(Resource.Id.progressBarPasso3);
            spinnerCPFs = FindViewById<Spinner>(Resource.Id.spCPFs);

            btRecuperaSenha = FindViewById<Button>(Resource.Id.btnRecuperaSenha);

            carregaSpinnerCPFs();

            btRecuperaSenha.Click += btRecuperaSenha_Click;
        }

        private void btRecuperaSenha_Click(object sender, EventArgs e)
        {
            var indices = 0;

            for( var i = 0; i <= listaCPFs[indices].Length; i++)
            {
                string posicaoCPFspinner = listaCPFs[spinnerCPFs.SelectedItemPosition];

                if( cpf == posicaoCPFspinner )
                {
                    Toast.MakeText(Application.Context,"Respostas corretas! Você irá criar uma nova senha agora.", ToastLength.Short).Show();
                    contador++;
                    var irTelaRedefinirSenha = new Intent(this, typeof(Passo4ReescreverSenha) );
                    irTelaRedefinirSenha.PutExtra("diarista_id", id_d);
                    StartActivity(irTelaRedefinirSenha);
                    if( contador == 0 )
                    {
                        Toast.MakeText(Application.Context, "Respostas corretas! Você irá criar uma nova senha agora.", ToastLength.Short).Show();
                        contador = 0;
                        var irTelaRedefinirSenhaCliente = new Intent(this, typeof(Passo4ReescreverSenha));
                        irTelaRedefinirSenhaCliente.PutExtra("cliente_id", id_cl);
                        StartActivity(irTelaRedefinirSenhaCliente);
                    }
                }
                else
                {
                    Toast.MakeText(Application.Context,"Seu CPF está errado. Recomeçando todo o processo...",ToastLength.Short).Show();
                    var respostaErrada3 = new Intent(this, typeof(Login));
                    StartActivity(respostaErrada3);
                            
                }
            }
        }

        private void carregaSpinnerCPFs()
        {
            listaCPFs.Add(cpf);
            ArrayAdapter<string> a = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, listaCPFs);
            spinnerCPFs.Adapter = a;
        }
    }
}