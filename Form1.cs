using CefSharp;
using Keyauth;
using Newtonsoft.Json;
using System;
using System.Threading;
using System.Windows.Forms;

namespace KeyAuthExample
{
    public partial class LoaderInit : Form
    {
        public LoaderInit() => InitializeComponent();

        static string name = "Citrus";
        static string ownerid = "7pr3DBb5Id";
        static string secret = "12e93942aeed7be8bcaf9f74709612ba54b5230f4f5951a85ef77b0c34d71737";
        static string version = "1.0";

        public static api KeyAuthApp = new api(name, ownerid, secret, version);
        public class UserInformation
        {
            public string username { get; set; }
            public string password { get; set; }
            public string key { get; set; }
        }
        public void HandleInformation(string info)
        {
            UserInformation json = JsonConvert.DeserializeObject<UserInformation>(info);

            string username = json.username;
            string password = json.password;
            string key = json.key;

            if (json.key != null) { Register(username, password, key); }
            else { Login(username, password); }
        }
        public void Login(string username, string password) 
        { 
            if (KeyAuthApp.login(username, password))
            { 
                //Optional Success Message / Form Open
                MessageBox.Show($"Welcome back, {KeyAuthApp.user_data.username}!");
            } 
        }
        public void Register(string username, string password, string key) 
        { 
            if (KeyAuthApp.register(username, password, key)) 
            { 
                MessageBox.Show($"Thanks for registering, {KeyAuthApp.user_data.username}!"); 
            } 
        }
        private void Init(object sender, EventArgs e) 
        { 
            browser.JavascriptMessageReceived += MessageRecieved; 
            KeyAuthApp.init(); browser.LoadHtml(Properties.Resources.index); 
        }
        private void MessageRecieved(object sender, JavascriptMessageReceivedEventArgs e) => 
        new Thread(() => { Thread.CurrentThread.IsBackground = true; HandleInformation((string)e.Message.ToString()); }).Start();
    }
}
