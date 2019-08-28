using Android.App;
using Android.Widget;
using Android.OS;
using DI.Droid.Base;
using DI.Localization;
using Android.Content.PM;
using System.Threading.Tasks;
using Android.Content;
using DI.Shared;
using Android.Views;
using DI.Shared.Entities.API;
using DI.Shared.DataAccess;
using DI.Shared.Managers;
using Android.Views.InputMethods;

namespace DI.Droid
{
    [Activity(Label = "DI", ScreenOrientation = ScreenOrientation.Portrait, LaunchMode = LaunchMode.SingleTask, Theme = "@style/MasterLayoutTheme", ConfigurationChanges = (ConfigChanges.KeyboardHidden | ConfigChanges.Orientation | ConfigChanges.ScreenSize | ConfigChanges.Locale))]
    public class ValidateSMSCodeActivity : BaseFormActivity
    {
        Company userCompany;

        EditText editSMSCodeOne;
        EditText editSMSCodeTwo;
        EditText editSMSCodeThree;
        EditText editSMSCodeFour;

        TextView smsCodeVM;
        TextView bonusCodeVM;
        TextView textEnterSMSCodeTitle;
        TextView textResendSMSTitle;
        TextView textRegisterPhoneDescription;

        LinearLayout layoutResendSMS;

        TextView textCodeDescription;
        EditText editBonusCode;

        string phone;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.ValidateSMSCode);
            SetTitleBack();

            phone = Intent.GetStringExtra(Constants.PHONE);

            editSMSCodeOne = FindViewById<EditText>(Resource.Id.editSMSCodeOne);
            editSMSCodeTwo = FindViewById<EditText>(Resource.Id.editSMSCodeTwo);
            editSMSCodeThree = FindViewById<EditText>(Resource.Id.editSMSCodeThree);
            editSMSCodeFour = FindViewById<EditText>(Resource.Id.editSMSCodeFour);

            editBonusCode = FindViewById<EditText>(Resource.Id.editBonusCode);
            textCodeDescription = FindViewById<TextView>(Resource.Id.textCodeDescription);
            bonusCodeVM = FindViewById<TextView>(Resource.Id.bonusCodeVM);
            smsCodeVM = FindViewById<TextView>(Resource.Id.smsCodeVM);
            textEnterSMSCodeTitle = FindViewById<TextView>(Resource.Id.textEnterSMSCodeTitle);
            textResendSMSTitle = FindViewById<TextView>(Resource.Id.textResendSMSTitle);
            textRegisterPhoneDescription = FindViewById<TextView>(Resource.Id.textRegisterPhoneDescription);
            layoutResendSMS = FindViewById<LinearLayout>(Resource.Id.layoutResendSMS);

            InitControls();
        }

        void InitControls()
        {
            base.InitControls();

            textEnterSMSCodeTitle.Text = AppResources.EnterConfirmationSMSCode;
            textResendSMSTitle.Text = AppResources.ResendSMS.ToUpper();
            textRegisterPhoneDescription.Text = AppResources.SMSCodeConfirmationDescription;
            textCodeDescription.Text = AppResources.EnterBonusCodeDescription;

            GetSubmitButton().Text = AppResources.Confirm.ToUpper();

            SetupGestures();
        }

        void SetupGestures()
        {
            editSMSCodeOne.TextChanged += delegate
            {
                if (editSMSCodeOne.Text != string.Empty)
                {
                    editSMSCodeTwo.RequestFocus();
                }
            };
            editSMSCodeTwo.TextChanged += delegate
            {
                if (editSMSCodeTwo.Text != string.Empty)
                {
                    editSMSCodeThree.RequestFocus();
                }
            };
            editSMSCodeThree.TextChanged += delegate
            {
                if (editSMSCodeThree.Text != string.Empty)
                {
                    editSMSCodeFour.RequestFocus();
                }
            };
            editSMSCodeFour.TextChanged += delegate
            {
                if (editSMSCodeFour.Text != string.Empty)
                {
                    InputMethodManager imm = (InputMethodManager)GetSystemService(Context.InputMethodService);
                    imm.HideSoftInputFromWindow(editSMSCodeFour.WindowToken, 0);
                    editSMSCodeFour.ClearFocus();
                }
            };

            layoutResendSMS.Click += delegate
            {
                var data = APIDataManager.SendSMSCode(phone.Trim());
                Toast.MakeText(this, AppResources.SMSCodeSent, ToastLength.Long).Show();
            };
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    Finish();
                    return true;
            }

            return base.OnOptionsItemSelected(item);
        }

        #region abstract

        protected override int GetStatusBarColor()
        {
            return Resource.Color.statusbar_blue;
        }

        protected override int GetActiveBarColor()
        {
            return Resource.Color.actionbar_blue;
        }

        protected override string GetTitle()
        {
            return AppResources.AppName;
        }

        protected override async Task LoadDataAsync()
        {
        }

        protected override Button GetCancelButton()
        {
            return null;
        }

        protected override Button GetSubmitButton()
        {
            return FindViewById<Button>(Resource.Id.buttonSave);
        }

        protected override void OnDataNotValid()
        {
        }

        protected override async Task OnCancelAsync()
        {
        }

        protected override async Task OnSubmitAsync()
        {
            if (userCompany == null)
            {
                var company = new Company();
                company.Type = "ct_fiz";
                company.CompanyActivity = "Собственник ТС";
                company.Phone = phone;

                userCompany = await APIDataManager.CreateCompany(company);
            }
            if (userCompany != null)
            {
                var user = new UserForRegistration();
                user.UserName = "user" + phone.Replace("+", "").Trim();
                user.Password = DataManager.GeneratePassword();
                user.Phone = phone.Trim();
                user.CompanyId = userCompany.Id;
                user.Groups = new System.Collections.Generic.List<string>() { "drivers" };
                user.ReferCode = editBonusCode.Text;

                Shared.Entities.API.User userResult = await APIDataManager.CreateUser(user);
                if (userResult != null)
                {
                    bonusCodeVM.Visibility = ViewStates.Gone;

                    TokenResponseModel token = await APIDataManager.Login(user.UserName, user.Password);
                    if (token != null)
                    {
                        sqliteManager.Register(user.Phone, token.Token, user.UserName);

                        SessionManager.UserData = userResult;
                        if (SessionManager.UserData != null && SessionManager.UserData.CompanyId.HasValue)
                        {
                            SessionManager.СontractorData = userCompany;
                        }

                        var activity = new Intent(this, typeof(MainActivity));
                        StartActivity(activity);
                    }

                    Finish();
                }
                else
                {
                    if (editBonusCode.Text != string.Empty)
                    {
                        bonusCodeVM.Text = AppResources.WrongBonusCode.ToUpper();
                        bonusCodeVM.Visibility = ViewStates.Visible;
                    }
                    else
                    {
                        Toast.MakeText(this, AppResources.ErrorMessage, ToastLength.Long).Show();
                    }

                    HideProgressBar();
                    GetSubmitButton().Visibility = ViewStates.Visible;
                }
            }           
        }

        protected override async Task<bool> IsDataValidAsync()
        {
            bool hasErrors = false;

            if (editSMSCodeOne.Text == string.Empty || editSMSCodeTwo.Text == string.Empty
                || editSMSCodeThree.Text == string.Empty || editSMSCodeFour.Text == string.Empty) 
            {
                smsCodeVM.Text = AppResources.SMSCodeRequired.ToUpper();
                smsCodeVM.Visibility = ViewStates.Visible;

                hasErrors = true;
            }
            else
            {
                smsCodeVM.Visibility = ViewStates.Invisible;
            }

            if (hasErrors)
                return false;

            string code = editSMSCodeOne.Text + editSMSCodeTwo.Text + editSMSCodeThree.Text + editSMSCodeFour.Text;

            var data = await APIDataManager.VerifySMSCode(phone.Trim(), code);
            if (data != null && data.Success == true)
            {
                smsCodeVM.Visibility = ViewStates.Invisible;
            }
            else
            {
                smsCodeVM.Text = AppResources.WrongSMSCode.ToUpper();
                smsCodeVM.Visibility = ViewStates.Visible;

                hasErrors = true;
            }

            if (hasErrors)
                return false;

            return true;
        }

        #endregion
    }
}
