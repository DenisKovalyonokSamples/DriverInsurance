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
using Android.Support.Design.Widget;
using Android.Views.InputMethods;

namespace DI.Droid
{
    public delegate void ResendSMSTimeoutHandler();

    [Activity(Label = "DI", ScreenOrientation = ScreenOrientation.Portrait, LaunchMode = LaunchMode.SingleTask, Theme = "@style/MasterLayoutTheme", ConfigurationChanges = (ConfigChanges.KeyboardHidden | ConfigChanges.Orientation | ConfigChanges.ScreenSize | ConfigChanges.Locale))]
    public class PasswordRestoreActivity : BaseFormActivity
    {
        string phone;

        TextView textRestoreCodeSentTitle;
        TextView textEnterPasswordTitle;

        TextView codeVM;
        TextView passwordVM;
        TextView passwordConfirmVM;

        TextView textEnterSMSCodeTitle;
        EditText editSMSCodeOne;
        EditText editSMSCodeTwo;
        EditText editSMSCodeThree;
        EditText editSMSCodeFour;

        TextInputLayout editPassword;
        TextInputLayout editConfirmPassword;

        public static event ResendSMSTimeoutHandler ResendSMSTimeout;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.PasswordRestore);
            SetTitleBack();

            phone = Intent.GetStringExtra(Constants.PHONE);

            textEnterSMSCodeTitle = FindViewById<TextView>(Resource.Id.textEnterSMSCodeTitle);
            editSMSCodeOne = FindViewById<EditText>(Resource.Id.editSMSCodeOne);
            editSMSCodeTwo = FindViewById<EditText>(Resource.Id.editSMSCodeTwo);
            editSMSCodeThree = FindViewById<EditText>(Resource.Id.editSMSCodeThree);
            editSMSCodeFour = FindViewById<EditText>(Resource.Id.editSMSCodeFour);

            editPassword = FindViewById<TextInputLayout>(Resource.Id.editPassword);
            editConfirmPassword = FindViewById<TextInputLayout>(Resource.Id.editConfirmPassword);

            textRestoreCodeSentTitle = FindViewById<TextView>(Resource.Id.textRestoreCodeSentTitle);
            textEnterPasswordTitle = FindViewById<TextView>(Resource.Id.textEnterPasswordTitle);

            codeVM = FindViewById<TextView>(Resource.Id.codeVM);
            passwordVM = FindViewById<TextView>(Resource.Id.passwordVM);
            passwordConfirmVM = FindViewById<TextView>(Resource.Id.passwordConfirmVM);

            InitControls();
        }

        void InitControls()
        {
            base.InitControls();

            textEnterSMSCodeTitle.Text = AppResources.EnterConfirmationSMSCode;
            editPassword.Hint = AppResources.Password.ToUpper();
            editConfirmPassword.Hint = AppResources.Confirmation.ToUpper();

            textRestoreCodeSentTitle.Text = AppResources.RestoreCodeSent.ToUpper();
            textEnterPasswordTitle.Text = AppResources.CreateNewPassword.ToUpper();

            GetSubmitButton().Text = AppResources.ChangePassword.ToUpper();

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
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    ResendSMSTimeout?.Invoke();
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
            return FindViewById<Button>(Resource.Id.buttonSubmit);
        }

        protected override void OnDataNotValid()
        {
        }

        protected override async Task OnCancelAsync()
        {
        }

        protected override async Task OnSubmitAsync()
        {
            string code = editSMSCodeOne.Text + editSMSCodeTwo.Text + editSMSCodeThree.Text + editSMSCodeFour.Text;
            var data = await APIDataManager.ResetPassword(phone.Trim(), code, editPassword.EditText.Text);
            if (data != null && data.Success == true)
            {
                codeVM.Visibility = ViewStates.Invisible;

                TokenResponseModel token = await APIDataManager.Login("user" + phone.Replace("+", "").Trim(), editPassword.EditText.Text);
                if (token != null)
                {
                    sqliteManager.Register(phone, token.Token, "user" + phone.Replace("+", "").Trim());

                    SessionManager.UserData = await APIDataManager.GetUserByName("user" + phone.Replace("+", "").Trim());
                    if (SessionManager.UserData != null && SessionManager.UserData.CompanyId.HasValue)
                    {
                        SessionManager.СontractorData = await APIDataManager.GetCompany(SessionManager.UserData.CompanyId.Value);
                        if (SessionManager.СontractorData != null)
                        {
                            SessionManager.СontractData = await APIDataManager.GetCurrentContract(SessionManager.СontractorData.Id.ToString());
                        }
                    }

                    var activity = new Intent(this, typeof(MainActivity));
                    StartActivity(activity);

                    Finish();
                }
                else
                {
                    HideProgressBar();
                    GetSubmitButton().Visibility = ViewStates.Visible;

                    Toast.MakeText(this, AppResources.ErrorMessage, ToastLength.Long).Show();
                }
            }
            else
            {
                codeVM.Text = AppResources.WrongSMSCode.ToUpper();
                codeVM.Visibility = ViewStates.Visible;
                HideProgressBar();
                GetSubmitButton().Visibility = ViewStates.Visible;
            }
        }

        protected override async Task<bool> IsDataValidAsync()
        {
            bool hasErrors = false;

            if (editSMSCodeOne.Text == string.Empty || editSMSCodeTwo.Text == string.Empty
                || editSMSCodeThree.Text == string.Empty || editSMSCodeFour.Text == string.Empty)
            {
                codeVM.Text = AppResources.CodeRequired.ToUpper();
                codeVM.Visibility = ViewStates.Visible;

                hasErrors = true;
            }
            else
            {
                codeVM.Visibility = ViewStates.Invisible;
            }

            if (editPassword.EditText.Text == string.Empty)
            {
                passwordVM.Text = AppResources.PasswordRequired.ToUpper();
                passwordVM.Visibility = ViewStates.Visible;

                hasErrors = true;
            }
            else
            {
                passwordVM.Visibility = ViewStates.Invisible;
            }

            if (editConfirmPassword.EditText.Text == string.Empty)
            {
                passwordConfirmVM.Text = AppResources.ConfirmationRequired.ToUpper();
                passwordConfirmVM.Visibility = ViewStates.Visible;

                hasErrors = true;
            }
            else
            {
                passwordConfirmVM.Visibility = ViewStates.Invisible;
            }

            if (hasErrors)
                return false;

            if (editConfirmPassword.EditText.Text != editPassword.EditText.Text)
            {
                passwordConfirmVM.Text = AppResources.ConfirmationMustMatch.ToUpper();
                passwordConfirmVM.Visibility = ViewStates.Visible;

                hasErrors = true;
            }
            else
            {
                passwordConfirmVM.Visibility = ViewStates.Invisible;
            }

            if (hasErrors)
                return false;

            return true;
        }

        #endregion
    }
}
