using DI.iOS.Controllers.Base;
using Foundation;
using System;
using UIKit;
using DI.iOS.Extensions;
using DI.iOS.Managers;
using DI.Localization;
using System.Threading.Tasks;
using DI.Shared.DataAccess;
using DI.Shared.Entities.API;
using DI.Shared.Managers;

namespace DI.iOS
{
    public partial class ValidateSMSCodeController : BaseFormController
    {
        public string Phone = string.Empty;
        Company userCompany;

        public ValidateSMSCodeController (IntPtr handle) : base (handle)
        {
        }

        public override void ViewDidLayoutSubviews()
        {
            TFSMSCodeOne.SetBottomBorder(ColorManager.control_default_color);
            TFSMSCodeTwo.SetBottomBorder(ColorManager.control_default_color);
            TFSMSCodeThree.SetBottomBorder(ColorManager.control_default_color);
            TFSMSCodeFour.SetBottomBorder(ColorManager.control_default_color);
            TFBonusCode.SetBottomBorder(ColorManager.control_default_color);
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            InitControls();
        }

        void InitControls()
        {
            LBToolbarTitle.Text = AppResources.AppName;

            LBCodeTitle.Text = AppResources.EnterConfirmationSMSCode;

            TFSMSCodeOne.Text = string.Empty;
            TFSMSCodeTwo.Text = string.Empty;
            TFSMSCodeThree.Text = string.Empty;
            TFSMSCodeFour.Text = string.Empty;

            TFSMSCodeOne.ShouldChangeCharacters = (fld, rng, str) => LimitTextLenth(fld, rng, str, 1, true, true);
            TFSMSCodeTwo.ShouldChangeCharacters = (fld, rng, str) => LimitTextLenth(fld, rng, str, 1, true, true);
            TFSMSCodeThree.ShouldChangeCharacters = (fld, rng, str) => LimitTextLenth(fld, rng, str, 1, true, true);
            TFSMSCodeFour.ShouldChangeCharacters = (fld, rng, str) => LimitTextLenth(fld, rng, str, 1, true, true);

            LBResendSMS.Text = AppResources.ResendSMS.ToUpper();
            LBDescription.Text = AppResources.SMSCodeConfirmationDescription;
            LBBonusCodeTitle.Text = AppResources.EnterBonusCodeDescription;

            TFBonusCode.Text = string.Empty;           

            GetSubmitButton().SetTitle(AppResources.Confirm.ToUpper(), UIControlState.Normal);

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

            TFBonusCode.TouchDown += delegate
            {
                TFBonusCode.SetBottomBorder(ColorManager.bgr_white);
            };
            TFBonusCode.EditingDidEnd += delegate
            {
                TFBonusCode.SetBottomBorder(ColorManager.control_default_color);
            };
            TFBonusCode.ShouldReturn += (textField) =>
            {
                textField.ResignFirstResponder();
                return true;
            };

            var lbResendSMSClick = new UITapGestureRecognizer(ResendSMS);
            LBResendSMS.UserInteractionEnabled = true;
            LBResendSMS.AddGestureRecognizer(lbResendSMSClick);
        }

        void ResendSMS()
        {
            var data = APIDataManager.SendSMSCode(Phone.Trim());
            new UIAlertView(null, AppResources.SMSCodeSent, null, "OK", null).Show();
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
            if (userCompany == null)
            {
                var company = new Company();
                company.Type = "ct_fiz";
                company.CompanyActivity = "Собственник ТС";
                company.Phone = Phone;

                userCompany = await APIDataManager.CreateCompany(company);
            }
            if (userCompany != null)
            {
                var user = new UserForRegistration();
                user.UserName = "user" + Phone.Replace("+", "").Trim();
                user.Password = DataManager.GeneratePassword();
                user.Phone = Phone.Trim();
                user.CompanyId = userCompany.Id;
                user.Groups = new System.Collections.Generic.List<string>() { "drivers" };
                user.ReferCode = TFBonusCode.Text;

                DI.Shared.Entities.API.User userResult = await APIDataManager.CreateUser(user);
                if (userResult != null)
                {
                    LBBonusCodeVM.Hidden = true;

                    TokenResponseModel token = await APIDataManager.Login(user.UserName, user.Password);
                    if (token != null)
                    {
                        sqliteManager.Register(user.Phone, token.Token, user.UserName);

                        SessionManager.UserData = userResult;
                        if (SessionManager.UserData != null && SessionManager.UserData.CompanyId.HasValue)
                        {
                            SessionManager.СontractorData = userCompany;
                        }

                        MainController controller = Storyboard.InstantiateViewController("MainController") as MainController;
                        if (controller != null)
                        {
                            PresentViewController(controller, true, null);
                        }
                    }
                }
                else
                {
                    if (TFBonusCode.Text != string.Empty)
                    {
                        LBBonusCodeVM.Text = AppResources.WrongBonusCode.ToUpper();
                        LBBonusCodeVM.Hidden = false;
                    }
                    else
                    {
                        new UIAlertView(null, AppResources.ErrorMessage, null, "OK", null).Show();
                    }

                    HideProgressBar();
                    GetSubmitButton().Hidden = false;
                }
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
                LBCodeVM.Text = AppResources.SMSCodeRequired.ToUpper();
                LBCodeVM.Hidden = false;

                hasErrors = true;
            }
            else
            {
                LBCodeVM.Hidden = true;
            }

            if (hasErrors)
                return false;

            string code = TFSMSCodeOne.Text + TFSMSCodeTwo.Text + TFSMSCodeThree.Text + TFSMSCodeFour.Text;

            var data = await APIDataManager.VerifySMSCode(Phone.Trim(), code);
            if (data != null && data.Success == true)
            {
                LBCodeVM.Hidden = true;
            }
            else
            {
                LBCodeVM.Text = AppResources.WrongSMSCode.ToUpper();
                LBCodeVM.Hidden = false;

                hasErrors = true;
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