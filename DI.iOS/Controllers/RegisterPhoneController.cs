using DI.iOS.Controllers.Base;
using DI.iOS.Managers;
using DI.iOS.Extensions;
using Foundation;
using System;
using UIKit;
using DI.Localization;
using System.Threading.Tasks;
using DI.Shared.DataAccess;
using DI.Shared;

namespace DI.iOS
{
    public partial class RegisterPhoneController : BaseFormController
    {
        int lenth_before = 0;

        public RegisterPhoneController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLayoutSubviews()
        {
            TFPhone.SetBottomBorder(ColorManager.control_default_color);
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            InitControls();
        }

        void InitControls()
        {
            LBToolbarTitle.Text = AppResources.AppName;
            LBPhoneTitle.Text = AppResources.PhoneNumber;
            LBDescription.Text = AppResources.RegisterPhoneDescription;
            
            TFPhone.Text = "+7";

            GetSubmitButton().SetTitle(AppResources.Next.ToUpper(), UIControlState.Normal);

            SetupGestures();
        }

        void SetupGestures()
        {
            TFPhone.TouchDown += delegate
            {
                TFPhone.SetBottomBorder(ColorManager.bgr_white);
            };
            TFPhone.EditingDidEnd += delegate
            {
                TFPhone.SetBottomBorder(ColorManager.control_default_color);
            };
            TFPhone.ShouldReturn += (textField) =>
            {
                textField.ResignFirstResponder();
                return true;
            };

            UITextField.Notifications.ObserveTextFieldTextDidChange(TF_ValueChange);
        }

        #region Phone Mask

        void TF_ValueChange(object sender, NSNotificationEventArgs nsNotificationEventArgs)
        {
            UITextField textField = (UITextField)nsNotificationEventArgs.Notification.Object;

            if (textField == TFPhone)
            {
                if (textField.Text.Length == 13)
                {
                    lenth_before = 13;
                }

                if (textField.Text.Length == 16)
                {
                    lenth_before = 16;
                }

                if (textField.Text == "+" || textField.Text == "7" || textField.Text == string.Empty)
                {
                    textField.Text = "+7";
                    SetCursorPosition(textField, 2);
                }

                if (textField.Text.Length == 3)
                {
                    string letter = textField.Text[2].ToString();

                    textField.Text = textField.Text.Replace(letter, " (" + letter);
                    SetCursorPosition(textField, 5);
                }

                if (textField.Text.Length == 4)
                {
                    textField.Text = "+7";
                    SetCursorPosition(textField, 2);
                }

                if (textField.Text.Length == 7)
                {
                    textField.Text += ") ";
                    SetCursorPosition(textField, 9);
                }

                if (textField.Text.Length == 8)
                {
                    textField.Text = textField.Text.Substring(0, 6);
                    SetCursorPosition(textField, 6);
                }

                if (textField.Text.Length == 12)
                {
                    string letter = textField.Text[11].ToString();
                    if (lenth_before == 13)
                    {
                        textField.Text = textField.Text.Substring(0, 11);
                        SetCursorPosition(textField, 11);
                        lenth_before = 0;
                    }
                    else
                    {
                        textField.Text += "-";
                        SetCursorPosition(textField, 13);
                        lenth_before = 13;
                    }
                }

                if (textField.Text.Length == 15)
                {
                    string letter = textField.Text[14].ToString();
                    if (lenth_before == 16)
                    {
                        textField.Text = textField.Text.Substring(0, 14);
                        SetCursorPosition(textField, 14);
                        lenth_before = 0;
                    }
                    else
                    {
                        textField.Text += "-";
                        SetCursorPosition(textField, 16);
                        lenth_before = 16;
                    }
                }

                if (textField.Text.Length >= 19)
                {
                    textField.Text = textField.Text.Substring(0, 18);


                    SetCursorPosition(textField, 18);
                }
            }
        }

        void SetCursorPosition(UITextField textField, int position)
        {
            textField.BecomeFirstResponder();
            var indexToSet = position;
            var positionToSet = textField.GetPosition(textField.BeginningOfDocument, indexToSet);
            textField.SelectedTextRange = textField.GetTextRange(positionToSet, positionToSet);
        }

        #endregion

        #region Abstract

        protected override async Task LoadDataAsync()
        {
        }

        protected override void OnDataNotValid()
        {
        }

        protected override async Task OnSubmitAsync()
        {
            var token = await APIDataManager.Login(Constants.DEFAULT_USER_LOGIN, Constants.DEFAULT_USER_PASSWORD);
            if (token != null)
            {
                string phone = TFPhone.Text.Replace(" ", string.Empty).Replace("(", string.Empty).Replace(")", string.Empty).Replace("-", string.Empty);

                var data = await APIDataManager.SendSMSCode(phone);
                if (data != null && data.Success == true)
                {
                    ValidateSMSCodeController controller = Storyboard.InstantiateViewController("ValidateSMSCodeController") as ValidateSMSCodeController;
                    if (controller != null)
                    {
                        controller.Phone = phone;
                        PresentViewController(controller, true, null);
                    }
                }
                else
                {
                    if (data != null && data.Success == false && data.ErrorCode == 4)
                    {
                        PasswordLoginController controller = Storyboard.InstantiateViewController("PasswordLoginController") as PasswordLoginController;
                        if (controller != null)
                        {
                            controller.Phone = phone;
                            PresentViewController(controller, true, null);
                        }
                    }
                    else
                    {
                        if (data != null && data.Success == false && data.ErrorCode == 2)
                        {
                            LBPhoneVM.Text = AppResources.IncorrectDataEntered.ToUpper();
                            LBPhoneVM.Hidden = false;
                        }
                        else
                        {
                            new UIAlertView(null, AppResources.ErrorMessage, null, "OK", null).Show();
                        }
                    }
                }
            }

            HideProgressBar();
            GetSubmitButton().Hidden = false;
        }

        protected override Task OnCancelAsync()
        {
            return null;
        }

        protected override async Task<bool> IsDataValidAsync()
        {
            bool hasErrors = false;

            if (TFPhone.Text == string.Empty || TFPhone.Text == "+7" || TFPhone.Text == "+")
            {
                LBPhoneVM.Text = AppResources.PhoneRequired.ToUpper();
                LBPhoneVM.Hidden = false;

                hasErrors = true;
            }
            else
            {
                LBPhoneVM.Hidden = true;
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