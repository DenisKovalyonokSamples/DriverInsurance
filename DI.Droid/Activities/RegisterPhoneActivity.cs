using Android.App;
using Android.Widget;
using Android.OS;
using DI.Droid.Base;
using DI.Shared.DataAccess;
using DI.Localization;
using Android.Content.PM;
using System.Threading.Tasks;
using Android.Content;
using DI.Shared;
using Android.Views;

namespace DI.Droid
{
    [Activity(Label = "DI", ScreenOrientation = ScreenOrientation.Portrait, LaunchMode = LaunchMode.SingleTask, 
        Theme = "@style/MasterLayoutTheme", ConfigurationChanges = (ConfigChanges.KeyboardHidden | ConfigChanges.Orientation | ConfigChanges.ScreenSize | ConfigChanges.Locale))]
    public class RegisterPhoneActivity : BaseFormActivity
    {
        EditText editPhone;
        TextView phoneVM;
        TextView textToolbarTitle;
        TextView textRegisterPhoneDescription;

        TextView textCodeDescription;
        EditText editBonusCode;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.RegisterPhone);

            editPhone = FindViewById<EditText>(Resource.Id.editPhone);
            editBonusCode = FindViewById<EditText>(Resource.Id.editBonusCode);
            textCodeDescription = FindViewById<TextView>(Resource.Id.textCodeDescription);
            phoneVM = FindViewById<TextView>(Resource.Id.phoneVM);
            textToolbarTitle = FindViewById<TextView>(Resource.Id.textToolbarTitle);
            textRegisterPhoneDescription = FindViewById<TextView>(Resource.Id.textRegisterPhoneDescription);

            InitControls();
        }

        void InitControls()
        {
            base.InitControls();

            this.SetSupportActionBar(_toolbar);
            this.SupportActionBar.SetDisplayHomeAsUpEnabled(false);
            this.SupportActionBar.SetDisplayShowTitleEnabled(false);
            textToolbarTitle.Text = AppResources.AppName;

            editPhone.Hint = AppResources.PhoneNumber;
            textCodeDescription.Text = AppResources.EnterBonusCodeDescription;
            textRegisterPhoneDescription.Text = AppResources.RegisterPhoneDescription;

            GetSubmitButton().Text = AppResources.Next.ToUpper();

            SetupGestures();
        }

        int lenth_before = 0;
        void SetupGestures()
        {
            editPhone.TextChanged += delegate
            {
                if (editPhone.Text == "+" || editPhone.Text == "7" || editPhone.Text == string.Empty)
                {
                    editPhone.Text = "+7";
                    editPhone.SetSelection(2);
                }
                //if "."
                if (editPhone.Text.Length == 3)
                {
                    string letter = editPhone.Text[2].ToString();

                    editPhone.Text = editPhone.Text.Replace(letter, " (" + letter);
                    editPhone.SetSelection(5);
                }

                if (editPhone.Text.Length == 4)
                {
                    editPhone.Text = "+7";
                    editPhone.SetSelection(2);
                }

                if (editPhone.Text.Length == 7)
                {
                    editPhone.Text += ") ";
                    editPhone.SetSelection(9);
                }

                if (editPhone.Text.Length == 8)
                {
                    editPhone.Text = editPhone.Text.Substring(0, 6);
                    editPhone.SetSelection(6);
                }

                if (editPhone.Text.Length == 12)
                {
                    string letter = editPhone.Text[11].ToString();
                    if (lenth_before == 13)
                    {
                        editPhone.Text = editPhone.Text.Substring(0, 11);
                        editPhone.SetSelection(11);
                        lenth_before = 0;
                    }
                    else
                    {
                        editPhone.Text += "-";
                        editPhone.SetSelection(13);
                    }
                }

                if (editPhone.Text.Length == 15)
                {
                    string letter = editPhone.Text[14].ToString();
                    if (lenth_before == 16)
                    {
                        editPhone.Text = editPhone.Text.Substring(0, 14);
                        editPhone.SetSelection(14);
                        lenth_before = 0;
                    }
                    else
                    {
                        editPhone.Text += "-";
                        editPhone.SetSelection(16);
                    }
                }

                if (editPhone.Text.Length >= 19)
                {
                    editPhone.Text = editPhone.Text.Substring(0, 18);
                    editPhone.SetSelection(18);
                }
            };

            editPhone.BeforeTextChanged += delegate
            {
                if (editPhone.Text.Length == 13)
                {
                    lenth_before = 13;
                }

                if (editPhone.Text.Length == 16)
                {
                    lenth_before = 16;
                }
            };

            editPhone.AfterTextChanged += delegate
            {
                if (editPhone.Text.Length == 3)
                {
                    string letter = editPhone.Text[2].ToString();

                    editPhone.Text.Replace(letter, " (" + letter);
                }
            };
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
            return AppResources.AppName.ToUpper();
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
            var token = await APIDataManager.Login(Constants.DEFAULT_USER_LOGIN, Constants.DEFAULT_USER_PASSWORD);
            if (token != null)
            {
                string phone = editPhone.Text.Replace(" ", string.Empty).Replace("(", string.Empty).Replace(")", string.Empty).Replace("-", string.Empty);

                var data = await APIDataManager.SendSMSCode(phone);
                if (data != null && data.Success == true)
                {
                    var activity = new Intent(this, typeof(ValidateSMSCodeActivity));
                    activity.PutExtra(Constants.PHONE, phone);
                    StartActivity(activity);
                }
                else
                {
                    if (data != null && data.Success == false && data.ErrorCode == 4)
                    {
                        var activity = new Intent(this, typeof(PasswordLoginActivity));
                        activity.PutExtra(Constants.PHONE, phone);
                        StartActivity(activity);
                    }
                    else
                    {
                        if (data != null && data.Success == false && data.ErrorCode == 2)
                        {
                            phoneVM.Text = AppResources.IncorrectDataEntered.ToUpper();
                            phoneVM.Visibility = ViewStates.Visible;
                        }
                        else
                        {
                            Toast.MakeText(this, AppResources.ErrorMessage, ToastLength.Long).Show();
                        }
                    }
                }
            }

            HideProgressBar();
            GetSubmitButton().Visibility = ViewStates.Visible;
        }

        protected override async Task<bool> IsDataValidAsync()
        {
            bool hasErrors = false;

            if (editPhone.Text == string.Empty || editPhone.Text == "+7" || editPhone.Text == "+")
            {
                phoneVM.Text = AppResources.PhoneRequired.ToUpper();
                phoneVM.Visibility = ViewStates.Visible;

                hasErrors = true;
            }
            else
            {
                phoneVM.Visibility = ViewStates.Invisible;
            }

            if (hasErrors)
                return false;

            GetSubmitButton().Visibility = ViewStates.Gone;
            ShowProgressBar();

            return true;
        }

        #endregion
    }
}
