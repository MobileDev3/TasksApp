using System;
using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Android.Gms.Common.Apis;
using Android.Gms.Common;
using Android.Gms.Plus;
using Android.Util;
using Android.Graphics;
using System.Net;
using Android.Content;
using System.IO;

namespace TasksApp
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar")]
    class Glog : Activity, GoogleApiClient.IConnectionCallbacks, GoogleApiClient.IOnConnectionFailedListener

    {
        GoogleApiClient mGoogleApiClient;
        private ConnectionResult mConnectionResult;
        SignInButton mGsignBtn;
        TextView TxtName, TxtGender;
        ImageView ImgProfile;
        private bool mIntentInProgress;
        private bool mSignInClicked;
        private bool mInfoPopulated;
        public string TAG
        {
            get;
            private set;
        }
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            // Set our view from the "main" layout resource  
            SetContentView(Resource.Layout.google);
            
            mGsignBtn = FindViewById<SignInButton>(Resource.Id.sign_in_button);
            TxtGender = FindViewById<TextView>(Resource.Id.TxtGender);
            TxtName = FindViewById<TextView>(Resource.Id.TxtName);
            ImgProfile = FindViewById<ImageView>(Resource.Id.ImgProfile);
            Button button1 = FindViewById<Button>(Resource.Id.buttonTasks);
            button1.Click += Button_Click;
            mGsignBtn.Click += MGsignBtn_Click;
            GoogleApiClient.Builder builder = new GoogleApiClient.Builder(this);
            builder.AddConnectionCallbacks(this);
            builder.AddOnConnectionFailedListener(this);
            builder.AddApi(PlusClass.API);
            builder.AddScope(PlusClass.ScopePlusProfile);
            builder.AddScope(PlusClass.ScopePlusLogin);
            //Build our IGoogleApiClient  
            mGoogleApiClient = builder.Build();
        }
        protected override void OnStart()
        {
            base.OnStart();
            mGoogleApiClient.Connect();
        }
        protected override void OnStop()
        {
            base.OnStop();
            if (mGoogleApiClient.IsConnected)
            {
                mGoogleApiClient.Disconnect();
            }
        }
        public void OnConnected(Bundle connectionHint)
        {
            var person = PlusClass.PeopleApi.GetCurrentPerson(mGoogleApiClient);
            var name = string.Empty;
            if (person != null)
            {
                TxtName.Text = person.DisplayName;
                TxtGender.Text = person.Nickname;
                var Img = person.Image.Url;
                var imageBitmap = GetImageBitmapFromUrl(Img.Remove(Img.Length - 5));
                if (imageBitmap != null) ImgProfile.SetImageBitmap(imageBitmap);

            }
        }
        private void MGsignBtn_Click(object sender, EventArgs e)
        {
            if (!mGoogleApiClient.IsConnecting)
            {
                mSignInClicked = true;
                ResolveSignInError();
            }
            else if (mGoogleApiClient.IsConnected)
            {
                PlusClass.AccountApi.ClearDefaultAccount(mGoogleApiClient);
                mGoogleApiClient.Disconnect();
            }
        }
        private void Button_Click(object sender, EventArgs e)
        {
            var person = PlusClass.PeopleApi.GetCurrentPerson(mGoogleApiClient);
            if (person != null)
            {
                Intent intent = new Intent(this, typeof(tasksClass));
                StartActivity(intent);
            }          
        }
        private void ResolveSignInError()
        {
            if (mGoogleApiClient.IsConnecting)
            {
                return;
            }
            if (mConnectionResult.HasResolution)
            {
                try
                {
                    mIntentInProgress = true;
                    StartIntentSenderForResult(mConnectionResult.Resolution.IntentSender, 0, null, 0, 0, 0);
                }
                catch (Android.Content.IntentSender.SendIntentException io)
                {
                    mIntentInProgress = false;
                    mGoogleApiClient.Connect();
                }
            }
        }
        private Bitmap GetImageBitmapFromUrl(String url)
        {
            Bitmap imageBitmap = null;
            try
            {
                using (var webClient = new WebClient())
                {
                    var imageBytes = webClient.DownloadData(url);
                    if (imageBytes != null && imageBytes.Length > 0)
                    {
                        imageBitmap = BitmapFactory.DecodeByteArray(imageBytes, 0, imageBytes.Length);
                    }
                }
                return imageBitmap;
            }
            catch (IOException e) { }
            return null;
        }
        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            Log.Debug(TAG, "onActivityResult:" + requestCode + ":" + resultCode + ":" + data);
            if (requestCode == 0)
            {
                if (resultCode != Result.Ok)
                {
                    mSignInClicked = false;
                }
                mIntentInProgress = false;
                if (!mGoogleApiClient.IsConnecting)
                {
                    mGoogleApiClient.Connect();
                }
            }
        }
        public void OnConnectionFailed(ConnectionResult result)
        {
            if (!mIntentInProgress)
            {
                mConnectionResult = result;
                if (mSignInClicked)
                {
                    ResolveSignInError();
                }
            }
        }
        public void OnConnectionSuspended(int cause) { }
    }

}
