using DI.Localization;
using DI.iOS.Controllers.Base;
using Foundation;
using System;
using System.Threading.Tasks;
using UIKit;
using DI.iOS.Extensions;
using DI.iOS.Managers;
using DI.Shared.DataAccess;
using DI.Shared.Entities.API;
using DI.Shared.Managers;

namespace DI.iOS
{
    public delegate void ResendSMSTimeoutHandler();

    public partial class PasswordRestoreController : BaseFormController
    {
        public string Phone = string.Empty;

        public static event ResendSMSTimeoutHandler ResendSMSTimeout;

        public PasswordRestoreController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLayoutSubviews()
        {
            TFSMSCodeOne.SetBottomBorder(ColorManager.control_default_color);
            TFSMSCodeTwo.SetBottomBorder(ColorManager.control_default_color);
            TFSMSCodeThree.SetBottomBorder(ColorManager.control_default_color);
            TFSMSCodeFour.SetBottomBorder(ColorManager.control_default_color);
            TFPassword.SetBottomBorder(ColorManager.control_default_color);
            TFConfirmPassword.SetBottomBorder(ColorManager.control_default_color);
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            InitControls();
        }

        void InitControls()
        {
            LBToolbarTitle.Text = AppResources.AppName;
            LBPageTitle.Text = AppResources.RestoreCodeSent.ToUpper();

            LBCodeTitle.Text = AppResources.EnterConfirmationSMSCode;

            TFSMSCodeOne.Text = string.Empty;
            TFSMSCodeTwo.Text = string.Empty;
            TFSMSCodeThree.Text = string.Empty;
            TFSMSCodeFour.Text = string.Empty;

            TFSMSCodeOne.ShouldChangeCharacters = (fld, rng, str) => LimitTextLenth(fld, rng, str, 1, true, true);
            TFSMSCodeTwo.ShouldChangeCharacters = (fld, rng, str) => LimitTextLenth(fld, rng, str, 1, true, true);
            TFSMSCodeThree.ShouldChangeCharacters = (fld, rng, str) => LimitTextLenth(fld, rng, str, 1, true, true);
            TFSMSCodeFour.ShouldChangeCharacters = (fld, rng, str) => LimitTextLenth(fld, rng, str, 1, true, true);

            LBNewPasswordTitle.Text = AppResources.CreateNewPassword.ToUpper();

            LBPasswordTitle.Text = AppResources.Password;
            TFPassword.Text = string.Empty;
            TFPassword.AttributedPlaceholder = new NSAttributedString(AppResources.Password, null, ColorManager.control_default_color);

            LBConfirmPasswordTitle.Text = AppResources.Confirmation;
            TFConfirmPassword.Text = string.Empty;
            TFConfirmPassword.AttributedPlaceholder = new NSAttributedString(AppResources.Confirmation, null, ColorManager.control_default_color);

            GetSubmitButton().SetTitle(AppResources.ChangePassword.ToUpper(), UIControlState.Normal);

            SetupGestures();
        }

        void SetupGestures()
        {
            var ivBackClick = new UITapGestureRecognizer(ReturnToPreviousController);
            IVBack.UserInteractionEnabled = true;
            IVBack.AddGestureRecognizer(ivBackClick);

            TFSMSCodeOne.TouchDown += delegate
            {
                TFSMSCodeOne.SetBottomBorder(ColorManager.bgr_white);
            };
            TFSMSCodeOne.EditingDidEnd += delegate
            {
                TFSMSCodeOne.SetBottomBorder(ColorManager.control_default_color);
            };
            TFSMSCodeOne.ShouldReturn += (textField) =>
            {
                textField.ResignFirstResponder();
                return true;
            };

            TFSMSCodeTwo.TouchDown += delegate
            {
                TFSMSCodeTwo.SetBottomBorder(ColorManager.bgr_white);
            };
            TFSMSCodeTwo.EditingDidEnd += delegate
            {
                TFSMSCodeTwo.SetBottomBorder(ColorManager.control_default_color);
            };
            TFSMSCodeTwo.ShouldReturn += (textField) =>
            {
                textField.ResignFirstResponder();
                return true;
            };

            TFSMSCodeThree.TouchDown += delegate
            {
                TFSMSCodeThree.SetBottomBorder(ColorManager.bgr_white);
            };
            TFSMSCodeThree.EditingDidEnd += delegate
            {
                TFSMSCodeThree.SetBottomBorder(ColorManager.control_default_color);
            };
            TFSMSCodeThree.ShouldReturn += (textField) =>
            {
                textField.ResignFirstResponder();
                return true;
            };

            TFSMSCodeFour.TouchDown += delegate
            {
                TFSMSCodeFour.SetBottomBorder(ColorManager.bgr_white);
            };
            TFSMSCodeFour.EditingDidEnd += delegate
            {
                TFSMSCodeFour.SetBottomBorder(ColorManager.control_default_color);
            };
            TFSMSCodeFour.ShouldReturn += (textField) =>
            {
                textField.ResignFirstResponder();
                return true;
            };

            UITextField.Notifications.ObserveTextFieldTextDidChange(TF_ValueChange);

            TFPassword.TouchDown += delegate
            {
                TFPassword.SetBottomBorder(ColorManager.bgr_white);
                LBPasswordTitle.Hidden = false;
                TFPassword.Placeholder = string.Empty;
            };
            TFPassword.EditingDidEnd += delegate
            {
                TFPassword.SetBottomBorder(ColorManager.control_default_color);

                if (TFPassword.Text == string.Empty)
                {
                    LBPasswordTitle.Hidden = true;
                    TFPassword.AttributedPlaceholder = new NSAttributedString(AppResources.Password, null, ColorManager.control_default_color);
                }
            };
            TFPassword.ShouldReturn += (textField) =>
            {
                textField.ResignFirstResponder();
                return true;
            };

            TFConfirmPassword.TouchDown += delegate
            {
                TFConfirmPassword.SetBottomBorder(ColorManager.bgr_white);
                LBConfirmPasswordTitle.Hidden = false;
                TFConfirmPassword.Placeholder = string.Empty;
            };
            TFConfirmPassword.EditingDidEnd += delegate
            {
                TFConfirmPassword.SetBottomBorder(ColorManager.control_default_color);

                if (TFConfirmPassword.Text == string.Empty)
                {
                    LBConfirmPasswordTitle.Hidden = true;
                    TFConfirmPassword.AttributedPlaceholder = new NSAttributedString(AppResources.Confirmation, null, ColorManager.control_default_color);
                }
            };
            TFConfirmPassword.ShouldReturn += (textField) =>
            {
                textField.ResignFirstResponder();
                return true;
            };
        }

        void TF_ValueChange(object sender, NSNotificationEventArgs nsNotificationEventArgs)
        {
            UITextField textField = (UITextField)nsNotificationEventArgs.Notification.Object;

            if (textField == TFSMSCodeOne)
            {
                if (TFSMSCodeOne.Text != string.Empty)
                {
                    TFSMSCodeTwo.BecomeFirstResponder();
                }
            }

            if (textField == TFSMSCodeTwo)
            {
                if (TFSMSCodeTwo.Text != string.Empty)
                {
                    TFSMSCodeThree.BecomeFirstResponder();
                }
            }

            if (textField == TFSMSCodeThree)
            {
                if (TFSMSCodeThree.Text != string.Empty)
                {
                    TFSMSCodeFour.BecomeFirstResponder();
                }
            }
        }

        void ReturnToPreviousController()
        {
            ResendSMSTimeout?.Invoke();

            PresentingViewController.DismissViewController(true, null);
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
            string code = TFSMSCodeOne.Text + TFSMSCodeTwo.Text + TFSMSCodeThree.Text + TFSMSCodeFour.Text;
            var data = await APIDataManager.ResetPassword(Phone.Trim(), code, TFPassword.Text);
            if (data != null && data.Success == true)
            {
                LBCodeVM.Hidden = true;

                TokenResponseModel token = await APIDataManager.Login("user" + Phone.Replace("+", "").Trim(), TFPassword.Text);
                if (token != null)
                {
                    sqliteManager.Register(Phone, token.Token, "user" + Phone.Replace("+", "").Trim());

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
                else
                {
                    HideProgressBar();
                    GetSubmitButton().Hidden = false;

                    new UIAlertView(null, AppResources.ErrorMessage, null, "OK", null).Show();
                }
            }
            else
            {
                LBCodeVM.Text = AppResources.WrongSMSCode.ToUpper();
                LBCodeVM.Hidden = false;
                HideProgressBar();
                GetSubmitButton().Hidden = false;
            }
        }

        protected override Task OnCancelAsync()
        {
            return null;
        }

        protected override async Task<bool> IsDataValidAsync()
        {
            bool hasErrors = false;

            if (TFSMSCodeOne.Text == string.Empty || TFSMSCodeTwo.Text == string.Empty
                || TFSMSCodeThree.Text == string.Empty || TFSMSCodeFour.Text == string.Empty)
            {
                LBCodeVM.Text = AppResources.CodeRequired.ToUpper();
                LBCodeVM.Hidden = false;

                hasErrors = true;
            }
            else
            {
                LBCodeVM.Hidden = true;
            }

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

            if (TFConfirmPassword.Text == string.Empty)
            {
                LBConfirmPasswordVM.Text = AppResources.ConfirmationRequired.ToUpper();
                LBConfirmPasswordVM.Hidden = false;

                hasErrors = true;
            }
            else
            {
                LBConfirmPasswordVM.Hidden = true;
            }

            if (hasErrors)
                return false;

            if (TFConfirmPassword.Text != TFPassword.Text)
            {
                LBConfirmPasswordVM.Text = AppResources.ConfirmationMustMatch.ToUpper();
                LBConfirmPasswordVM.Hidden = false;

                hasErrors = true;
            }
            else
            {
                LBConfirmPasswordVM.Hidden = true;
            }

            if (hasErrors)
                return false;

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