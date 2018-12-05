using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;

namespace TasksApp
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource  
            SetContentView(Resource.Layout.activity_main);

            Button button1 = FindViewById<Button>(Resource.Id.Button1);
            button1.Click += Button_Click;
            Button button2 = FindViewById<Button>(Resource.Id.Button2);
            button2.Click += Button2_Click;
        }

        private void Button_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(Glog));
            StartActivity(intent);

        }
        private void Button2_Click(object sender, EventArgs e)
        {
           Intent intent = new Intent(this, typeof(rLog));
            StartActivity(intent);
        }

    }
}

