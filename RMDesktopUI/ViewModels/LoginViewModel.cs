using System;
using System.Threading.Tasks;
using System.Windows;
using Caliburn.Micro;
using RMDesktopUI.Helpers;

namespace RMDesktopUI.ViewModels
{
    public class LoginViewModel : Screen
    {
        private string _userName;
        private string _password;
        private bool _isErrorVisible;
        private string _errorMessage;
        private readonly IAPIHelper _apiHelper;

        public LoginViewModel(IAPIHelper apiHelper)
        {
            _apiHelper = apiHelper;
        }
        public string UserName
        {
            get => _userName;
            set
            {
                _userName = value;
                NotifyOfPropertyChange(() => UserName);
                NotifyOfPropertyChange(() => CanLogIn);
            }
        }
        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                NotifyOfPropertyChange(() => Password);
                NotifyOfPropertyChange(() => CanLogIn);
            }
        }
        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                _errorMessage = value;
                NotifyOfPropertyChange(() => ErrorMessage);
                NotifyOfPropertyChange(() => IsErrorVisible);
            }
        }
        public bool IsErrorVisible
        {
            get
            {
                bool output = ErrorMessage?.Length > 0;
                return output;
            }
        }


        public bool CanLogIn
        {
            get
            {
                bool output = false;
                if (UserName?.Length > 0 && Password?.Length > 0)
                {
                    output = true;
                }
                return output;
            }
        }

        public async Task LogIn()
        {
            try
            {
                ErrorMessage = string.Empty;
                var result = await _apiHelper.Authenticate(UserName, Password);
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
        }
    }
}