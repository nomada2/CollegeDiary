﻿using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;
using CD.Views.Login;
using CD.Helper;
using Xamarin.Forms;
using System;
using System.Text.RegularExpressions;
using com.sun.tools.javac.util;

namespace CD.Views.SignUp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SignUpPage: ContentPage
    {
        IFirebaseRegister auth;
        readonly FireBaseHelperStudent firebaseStudent = new FireBaseHelperStudent();
        public SignUpPage()
        {
            InitializeComponent();
            auth = DependencyService.Get<IFirebaseRegister>();
        }

        private void LoginPage(object sender, System.EventArgs e)
        {
            Navigation.PushAsync(new LogIn());
        }

        private async void RegiterNewUser(object sender, EventArgs e)
        {
            bool validate = true;
            string pattern = null;
            pattern = "^([0-9a-zA-Z]([-\\.\\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\\w]*[0-9a-zA-Z]\\.)+[a-zA-Z]{2,9})$";
            if (!passwordMatch(PasswordEntry.Text, ConfirmPasswordEntry.Text))
            {
                await DisplayAlert("Error", "Passwords don't match", "Ok");
                validate = false;
            }
            if (validate && PasswordEntry.Text.Length < 6)
            {
                await DisplayAlert("Error", "The password needs to have at least 6 characters", "ok");
            }
            // cheking if  the email is valid
            if (Regex.IsMatch(this.SignUpEmailEntry.Text, pattern) && !string.IsNullOrEmpty(this.SignUpEmailEntry.Text) && validate)
            {
                validate = true;
            }
            else
            {
                await DisplayAlert("Subject not added", "Invalid email address entered", "OK");
                validate = false;
            }
            if (validate)
            {
                //System.Console.WriteLine("=====================================" + SignUpEmailEntry.Text + " " + PasswordEntry.Text);
                string Token = await auth.RegisterWithEmailAndPassword(SignUpEmailEntry.Text, PasswordEntry.Text);
                if (!string.IsNullOrEmpty(Token))
                {
                    await DisplayAlert("Account created", "Please verify your email", "ok");
                    //App.UserUID = auth.UserUID();
                    AddUserDetails(NameEntry.Text, College_University.Text, SignUpEmailEntry.Text);
                    await Navigation.PushAsync(new LogIn());
                }
                else
                {
                    await DisplayAlert("Error", "Please try again", "ok");
                }
            }
        }

        private bool passwordMatch(string password1, string password2)
        {
            return password1 == password2;
        }

        private async void AddUserDetails(string Name, string UC, string Email)
        {
            await firebaseStudent.AddStudent(App.UserUID, Name, UC, Email);
        }
    }
}