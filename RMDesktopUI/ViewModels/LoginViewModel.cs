using System;
using System.Threading.Tasks;
using System.Windows;
using Caliburn.Micro;
using RMDesktopUI.EventModels;
using RMDesktopUI.Library.Api;

namespace RMDesktopUI.ViewModels
{
    public class LoginViewModel : Screen
    {
        private string _userName = "masurov@gmail.com";
        private string _password = "$04sqyz)(FSM";
        private string _errorMessage;
        private readonly IApiHelper _apiHelper;
        private readonly IEventAggregator _events;

        public LoginViewModel(IApiHelper apiHelper, IEventAggregator events)
        {
            _apiHelper = apiHelper;
            _events = events;
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
                bool output = UserName?.Length > 0 && Password?.Length > 0;
                return output;
            }
        }

        public async Task LogIn()
        {
            try
            {
                ErrorMessage = "";
                var result = await _apiHelper.Authenticate(UserName, Password);

                // Capture more information about user
                await _apiHelper.GetLoggedInUserInfo(result.Access_Token);

                _events.PublishOnUIThread(new LogOnEvent());
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
        }
    }
}