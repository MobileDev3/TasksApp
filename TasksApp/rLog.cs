using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar")]
    class rLog: AppCompatActivity
    {
        string username = "Daniel";
        string password = "123456";
        TextView topText;
        EditText nameText;
        EditText passText;
        Button submit;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.rLogin);
            topText = FindViewById<TextView>(Resource.Id.topText);
            nameText = FindViewById<EditText>(Resource.Id.EditText1);
            passText = FindViewById<EditText>(Resource.Id.EditText2);
            submit = FindViewById<Button>(Resource.Id.button1);

            submit.Click += validateUser;
        }

        private void validateUser(object sender, EventArgs e)
        {
            if (nameText.Text.Equals(username) && passText.Text.Equals(password))
            {

                var intent = new Intent(this, typeof(tasksClass));

                intent.PutExtra("username", nameText.Text);
                intent.PutExtra("password", passText.Text);
                StartActivity(intent);
            }
        }

    }
}