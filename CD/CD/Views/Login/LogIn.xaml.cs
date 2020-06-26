﻿
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System;
using Autofac;
using CD.ViewModel.Auth;
using CD.Views.SignUp;
using CD.Views.ForgotPassword;

namespace CD.Views.Login
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LogIn : ContentPage
	{
		public LogIn()
		{
			InitializeComponent();
			this.BindingContext = (Application.Current as App).Container.Resolve<LoginViewModel>();
		}

		private void Login(object sender, EventArgs e)
		{
			Loading.IsRunning = true;
			Loading.Color = Color.Red;
		}

		private void SignUpPage(object sender, EventArgs e)
		{
			Navigation.PushAsync(new SignUpPage());
		}

		private void ForgotPasswordPage(object sender, EventArgs e)
		{
			Navigation.PushAsync(new SimpleForgotPasswordPage());
		}
	}
}