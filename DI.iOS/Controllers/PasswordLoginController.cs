using DI.iOS.Controllers.Base;
using Foundation;
using System;
using UIKit;
using DI.iOS.Extensions;
using DI.iOS.Managers;
using DI.Localization;
using System.Threading.Tasks;
using DI.Shared.Entities.API;
using DI.Shared.DataAccess;
using DI.Shared.Managers;
using System.Threading;

namespace DI.iOS
{
    public partial class PasswordLoginController : BaseFormController
    {
        Timer timeoutTimer;
        long timeoutDelay = (long)TimeSpan.FromSeconds(1).TotalMilliseconds;

        public string Phone = string.Empty;

        public PasswordLoginController(IntPtr handle) : base(handle)
        {

        }

        public override void ViewDidLayoutSubviews()
        {
            TFPassword.SetBottomBorder(ColorManager.control_default_color);
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            InitControls();
        }

        void InitControls()
        {
            LBToolbarTitle.Text = AppResources.AppName;
            LBPageTitle.Text = AppResources.SuchPhoneRegistered.ToUpper();

            LBPasswordTitle.Text = AppResources.EnterPassword;
            TFPassword.Text = string.Empty;
            TFPassword.AttributedPlaceholder = new NSAttributedString(AppResources.EnterPassword, null, ColorManager.control_default_color);


            LBRestorePassword.AttributedText = new NSAttributedString(AppResources.RestorePassword.ToUpper(),
                underlineStyle: NSUnderlineStyle.Single);

            GetSubmitButton().SetTitle(AppResources.Enter.ToUpper(), UIControlState.Normal);

            PasswordRestoreController.ResendSMSTimeout += ActivateTimeoutForSMSResend;

            SetupGestures();
        }

        void SetupGestures()
        {
            var ivBackClick = new UITapGestureRecognizer(ReturnToPreviousController);
            IVBack.UserInteractionEnabled = true;
            IVBack.AddGestureRecognizer(ivBackClick);

            TFPassword.TouchDown += delegate
            {
                TFPassword.SetBottomBorder(ColorManager.bgr_white);
                LBPasswordTitle.Hidden = false;
                TFPassword.Placeholder = string.Empty;
            };
            TFPassword.EditingDidEnd += delegate
            {
                TFPassword.SetBottomBorder(ColorManager.control_default_color);
            };
            TFPassword.ShouldReturn += (textField) =>
            {
                textField.ResignFirstResponder();
                return true;
            };

            var lbRestorePasswordClick = new UITapGestureRecognizer(RestorePassword);
            LBRestorePassword.UserInteractionEnabled = true;
            LBRestorePassword.AddGestureRecognizer(lbRestorePasswordClick);
        }

        int timeout = 0;
        void ActivateTimeoutForSMSResend()
        {
            LBRestorePassword.AttributedText = new NSAttributedString(AppResources.RestorePassword.ToUpper(),
                underlineStyle: NSUnderlineStyle.Single);

            LBRestorePassword.Text = AppResources.PasswordRecoveryTimeoutMessage + " 01:00";

            timeoutTimer = new System.Threading.Timer(new TimerCallback(TimeoutHandler), null, timeoutDelay, Timeout.Infinite);
        }

        public void TimeoutHandler(object o)
        {
            InvokeOnMainThread(() =>
            {
                if (LBRestorePassword.Text == AppResources.PasswordRecoveryTimeoutMessage + " 00:00")
                {
                    LBRestorePassword.AttributedText = new NSAttributedString(AppResources.RestorePassword.ToUpper(),
                    underlineStyle: NSUnderlineStyle.Single);

                    timeout = 0;
                    timeoutTimer.Dispose();
                }
                else
                {
                    timeout++;
                    int value = 60 - timeout;

                    if (value > 9)
                    {
                        LBRestorePassword.Text = AppResources.PasswordRecoveryTimeoutMessage + " 00:" + value.ToString();
                    }
                    else
                    {
                        LBRestorePassword.Text = AppResources.PasswordRecoveryTimeoutMessage + " 00:0" + value.ToString();
                    }
                }
            });

            timeoutTimer.Change(timeoutDelay, Timeout.Infinite);

            return;
        }

        void ReturnToPreviousController()
        {
            PresentingViewController.DismissViewController(true, null);
        }

        async void RestorePassword()
        {
            if (LBRestorePassword.Text == AppResources.RestorePassword.ToUpper())
            {
                LBRestorePassword.Hidden = true;
                GetSubmitButton().Hidden = true;
                ShowProgressBar();

                var data = await APIDataManager.SendSMSForPasswordReset(Phone.Trim());
                if (data != null && data.Success == true)
                {
                    PasswordRestoreController controller = Storyboard.InstantiateViewController("PasswordRestoreController") as PasswordRestoreController;
                    if (controller != null)
                    {
                        controller.Phone = Phone;
                        PresentViewController(controller, true, null);
                    }
                }
                else
                {
                    new UIAlertView(null, AppResources.ErrorMessage, null, "OK", null).Show();
                }

                HideProgressBar();
                LBRestorePassword.Hidden = false;
                GetSubmitButton().Hidden = false;
            }
        }

        #region Abstract

        protected override async Task LoadDataAsync()
        {
        }

        protected override void OnDataNotValid()
        {
        }

        protected override async Task OnSubmitAsync()
        {
            SessionManager.UserData = await APIDataManager.GetUserByName("user" + Phone.Replace("+", "").Trim());
            if (SessionManager.UserData != null && SessionManager.UserData.CompanyId.HasValue)
            {
                SessionManager.СontractorData = await APIDataManager.GetCompany(SessionManager.UserData.CompanyId.Value);
                if (SessionManager.СontractorData != null)
                {
                    SessionManager.СontractData = await APIDataManager.GetCurrentContract(SessionManager.СontractorData.Id.ToString());
                }
            }

            MainController controller = Storyboard.InstantiateViewController("MainController") as MainController;
            if (controller != null)
            {
                PresentViewController(controller, true, null);
            }
        }

        protected override Task OnCancelAsync()
        {
            return null;
        }

        protected override async Task<bool> IsDataValidAsync()
        {
            bool hasErrors = false;

            if (TFPassword.Text == string.Empty)
            {
                LBPasswordVM.Text = AppResources.PasswordRequired.ToUpper();
                LBPasswordVM.Hidden = false;

                hasErrors = true;
            }
            else
            {
                LBPasswordVM.Hidden = true;
            }

            if (hasErrors)
                return false;

            TokenResponseModel token = await APIDataManager.Login("user" + Phone.Replace("+", "").Trim(), TFPassword.Text);
            if (token != null)
            {
                LBPasswordVM.Hidden = true;

                sqliteManager.Register(Phone, token.Token, "user" + Phone.Replace("+", "").Trim());
            }
            else
            {
                LBPasswordVM.Text = AppResources.WrongPassword.ToUpper();
                LBPasswordVM.Hidden = false;

                hasErrors = true;
            }

            if (hasErrors)
                return false;

            GetSubmitButton().Hidden = true;
            ShowProgressBar();

            return true;
        }

        protected override UIButton GetSubmitButton()
        {
            return BTSubmit;
        }

        protected override UIButton GetCancelButton()
        {
            return null;
        }

        protected override UIActivityIndicatorView GetProgressBar()
        {
            return AIProgressBar;
        }

        protected override UIActivityIndicatorView GetProgressBarLoading()
        {
            return null;
        }

        #endregion
    }
}