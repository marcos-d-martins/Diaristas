using Android.App;
using Android.Content;
using Android.OS;
using Android.OS.Strictmode;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using diaria;
using Javax.Security.Auth;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace diaria
{
    [Activity(Label = "RecuperaSenhaPasso2")]
    public class RecuperaSenhaPasso2 : Activity
    {
        Conexao c = new Conexao();
        Spinner spinnerDatas;
        string sql, sql2, cpf, dt_nasc, idDiarista, idCliente;
        int contador = 0;
        

        List<string> listaDatasPreenchidas = new List<string>() {"19/09/1991","20/06/1998","01/05/1986","23/07/1977"};
        List<string> listaIdcliente = new List<string>();
        List<string> listaIddiarista = new List<string>();
                
        Button BtRegistraDataCorreta;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.recuperaSenhaPasso2);

            idDiarista = Intent.GetStringExtra("id_d");
            idCliente = Intent.GetStringExtra("id_cl");
            cpf = Intent.GetStringExtra("cpf");
            dt_nasc = Intent.GetStringExtra("dt_nasc");

            
            BtRegistraDataCorreta = FindViewById<Button>(Resource.Id.btnRegistrarDataCorreta);
            spinnerDatas = FindViewById<Spinner>(Resource.Id.spinnerEscolherDatas);
            AlimentaSpinnerDatas(dt_nasc);

            BtRegistraDataCorreta.Click += BtRegistraDataCorreta_Click;                        
        }


        /*
        private void embaralharArray( List<string>[] a)
        {
            int numeros = a.Length;
            Random aleatorio = new Random();
            for ( int i=0; i < numeros; i++ )
            {
                trocarIndices(a, i, i + aleatorio.Next(numeros - i) );
            }
        }


        private static void trocarIndices(List<string>[] arr[],int a, int b)
        {
            List<string>[] temp = arr[a];
            arr[a] = arr[b];
            arr[b] = temp;
        }
        */
        private void BtRegistraDataCorreta_Click(object sender, EventArgs e){
            //COMPARAR DATA COM DT_NASCIMENTO DIARISTA.
            int indexes = 0;
            for (int i = 0; i < listaDatasPreenchidas[indexes].Length; i++)
            {
                string dat;
                dat =  listaDatasPreenchidas[spinnerDatas.SelectedItemPosition];
                

                if ( dt_nasc == dat )
                {
                    Toast.MakeText(Application.Context, "Sucesso!", ToastLength.Long).Show();
                    Toast.MakeText(Application.Context, dt_nasc, ToastLength.Long).Show();
                    //Toast.MakeText(Application.Context, spinnerDatas.SelectedItemPosition, ToastLength.Long).Show();
                    contador++;
                    var etapa3Recupera = new Intent( this, typeof(RecuperaSenhaPasso3) );
                    etapa3Recupera.PutExtra("dt_nasc", dt_nasc);
                    etapa3Recupera.PutExtra("cpf", cpf);
                    etapa3Recupera.PutExtra("id_d", idDiarista);
                    StartActivity(etapa3Recupera);
                    
                    //COMPARAR DATA COM DT_NASCIMENTO CLIENTE.
                    if ( contador == 0)
                    {
                        Toast.MakeText(Application.Context, "Sucesso!", ToastLength.Long).Show();
                        Toast.MakeText(Application.Context, dt_nasc, ToastLength.Long).Show();
                        //Toast.MakeText(Application.Context, spinnerDatas.SelectedItemPosition, ToastLength.Long).Show();
                        contador = 0;
                        var etapa3RecuperaSenhaCliente = new Intent(this, typeof(RecuperaSenhaPasso3));
                        etapa3RecuperaSenhaCliente.PutExtra("dt_nasc", dt_nasc);
                        etapa3RecuperaSenhaCliente.PutExtra("cpf", cpf);
                        etapa3RecuperaSenhaCliente.PutExtra("id_cl", idCliente);
                        StartActivity(etapa3Recupera);
                    }
                }
                else
                {
                    Toast.MakeText(Application.Context, "Sua data de nascimento está errada. Recomece todo o processor...", ToastLength.Long).Show();
                    var voltaTelaLoginDtNascErrada = new Intent(this, typeof(Login) );
                    StartActivity(voltaTelaLoginDtNascErrada);

                }
            }
        }

        private void AlimentaSpinnerDatas(string dt)
        {
            //var embaralhado = listaDatasPreenchidas.OrderBy()
            
            listaDatasPreenchidas.Add(dt);            
            ArrayAdapter<string> a = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, listaDatasPreenchidas);
            spinnerDatas.Adapter = a;

        }
    }
}