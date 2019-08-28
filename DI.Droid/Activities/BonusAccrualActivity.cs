using Android.App;
using Android.Widget;
using Android.OS;
using DI.Droid.Base;
using DI.Localization;
using Android.Content.PM;
using System.Threading.Tasks;
using Android.Content;
using Android.Views;

namespace DI.Droid
{
    [Activity(Label = "DI", ScreenOrientation = ScreenOrientation.Portrait, LaunchMode = LaunchMode.SingleTask, Theme = "@style/MasterLayoutTheme", ConfigurationChanges = (ConfigChanges.KeyboardHidden | ConfigChanges.Orientation | ConfigChanges.ScreenSize | ConfigChanges.Locale))]
    public class BonusAccrualActivity : BaseFormActivity
    {
        EditText editBonusCode;
        TextView bonusCodeVM;
        TextView textBonusAccrualTitle;
        TextView textRecomendationTitle;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.BonusAccrual);

            editBonusCode = FindViewById<EditText>(Resource.Id.editBonusCode);

            bonusCodeVM = FindViewById<TextView>(Resource.Id.bonusCodeVM);
            textRecomendationTitle = FindViewById<TextView>(Resource.Id.textRecomendationTitle);
            textBonusAccrualTitle = FindViewById<TextView>(Resource.Id.textBonusAccrualTitle);

            InitControls();
        }

        void InitControls()
        {
            base.InitControls();

            editBonusCode.Hint = AppResources.Code.ToUpper();

            textRecomendationTitle.Text = AppResources.IfYouComeByRecommendation.ToUpper();
            textBonusAccrualTitle.Text = AppResources.EnterCodeFromMessage.ToUpper();

            GetSubmitButton().Text = AppResources.GetBonusPoints.ToUpper();
            GetCancelButton().Text = AppResources.Skip.ToUpper();
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
            return AppResources.BonusAccrual.ToUpper();
        }

        protected override async Task LoadDataAsync()
        {
        }

        protected override Button GetCancelButton()
        {
            return FindViewById<Button>(Resource.Id.buttonCancel);
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
            var activity = new Intent(this, typeof(MainActivity));
            StartActivity(activity);
            Finish();
        }

        protected override async Task OnSubmitAsync()
        {
            var activity = new Intent(this, typeof(MainActivity));
            StartActivity(activity);
            Finish();
        }

        protected override async Task<bool> IsDataValidAsync()
        {
            bool hasErrors = false;

            if (editBonusCode.Text == string.Empty)
            {
                bonusCodeVM.Text = AppResources.CodeRequired.ToUpper();
                bonusCodeVM.Visibility = ViewStates.Visible;

                hasErrors = true;
            }
            else
            {
                bonusCodeVM.Visibility = ViewStates.Invisible;
            }

            if (hasErrors)
                return false;

            return true;
        }

        #endregion
    }
}
