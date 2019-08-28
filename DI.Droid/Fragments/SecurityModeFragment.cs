using Android.Graphics;
using Android.OS;
using Android.Support.V4.Content;
using Android.Views;
using Android.Widget;
using DI.Droid.Fragments.Base;
using DI.Localization;

namespace DI.Droid.Fragments
{
    public class SecurityModeFragment : BaseFragment
    {
        TextView textModeTitle;
        ImageView imageButton;
        ImageView imageLock;
        TextView textCurrentState;
        ScrollView scrollMainContainer;

        bool isTurnedOn = false;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View partial = base.OnCreateView(inflater, container, savedInstanceState);

            textModeTitle = partial.FindViewById<TextView>(Resource.Id.textModeTitle);
            imageButton = partial.FindViewById<ImageView>(Resource.Id.imageButton);
            imageLock = partial.FindViewById<ImageView>(Resource.Id.imageLock);
            textCurrentState = partial.FindViewById<TextView>(Resource.Id.textCurrentState);
            scrollMainContainer = partial.FindViewById<ScrollView>(Resource.Id.scrollMainContainer);

            InitControls();

            return partial;
        }

        protected void InitControls()
        {
            textModeTitle.Text = AppResources.SecurityModeTitle;

            RefreshButtonState();

            SetupGestures();

            scrollMainContainer.Visibility = ViewStates.Visible;
        }

        void SetupGestures()
        {
            imageButton.Click += delegate
            {
                isTurnedOn = !isTurnedOn;
                RefreshButtonState();
            };
        }

        void RefreshButtonState()
        {
            if (!isTurnedOn)
            {
                imageButton.SetImageResource(Resource.Mipmap.round);
                imageLock.SetImageResource(Resource.Mipmap.ic_lock_open);
                textCurrentState.Text = AppResources.ModeOffTitle.ToUpper();
                textCurrentState.SetTextColor(new Color(ContextCompat.GetColor(this.Activity, Resource.Color.description_message_color)));
            }
            else
            {
                imageButton.SetImageResource(Resource.Mipmap.group_2);
                imageLock.SetImageResource(Resource.Mipmap.ic_lock_on);
                textCurrentState.Text = AppResources.ModeOnTitle.ToUpper();
                textCurrentState.SetTextColor(new Color(ContextCompat.GetColor(this.Activity, Resource.Color.bgr_white)));
            }
        }

        #region abstract

        protected override int GetLayoutId()
        {
            return Resource.Layout.SecurityMode;
        }

        #endregion
    }
}
