using Android.Graphics;
using Android.OS;
using Android.Support.V4.Content;
using Android.Views;
using Android.Widget;
using DI.Droid.Fragments.Base;
using DI.Localization;
using DI.Shared.Enums;
using DI.Shared.Managers;

namespace DI.Droid.Fragments
{
    public class MenuFragment : BaseFragment
    {
        RelativeLayout mainLayout;
        RelativeLayout policiesLayout;
        RelativeLayout bonusesLayout;
        RelativeLayout usefullLayout;
        RelativeLayout profileLayout;

        ImageView imageViewMain;
        ImageView imageViewPolicies;
        ImageView imageViewBonuses;
        ImageView imageViewUsefull;
        ImageView imageViewProfile;

        TextView mainTitle;
        TextView policiesTitle;
        TextView bonusesTitle;
        TextView usefullTitle;
        TextView profileTitle;

        LinearLayout selectorMain;
        LinearLayout selectorPolicies;
        LinearLayout selectorBonuses;
        LinearLayout selectorUsefull;
        LinearLayout selectorProfile;

        RelativeLayout layoutNotifications;
        ImageView imageViewNotification;
        TextView textNotificationsCounter;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View partial = base.OnCreateView(inflater, container, savedInstanceState);

            mainLayout = partial.FindViewById<RelativeLayout>(Resource.Id.layoutMain);
            policiesLayout = partial.FindViewById<RelativeLayout>(Resource.Id.layoutPolicies);
            bonusesLayout = partial.FindViewById<RelativeLayout>(Resource.Id.layoutBonuses);
            usefullLayout = partial.FindViewById<RelativeLayout>(Resource.Id.layoutUsefull);
            profileLayout = partial.FindViewById<RelativeLayout>(Resource.Id.layoutProfile);

            imageViewMain = partial.FindViewById<ImageView>(Resource.Id.imageViewMain);
            imageViewPolicies = partial.FindViewById<ImageView>(Resource.Id.imageViewPolicies);
            imageViewBonuses = partial.FindViewById<ImageView>(Resource.Id.imageViewBonuses);
            imageViewUsefull = partial.FindViewById<ImageView>(Resource.Id.imageViewUsefull);
            imageViewProfile = partial.FindViewById<ImageView>(Resource.Id.imageViewProfile);

            selectorMain = partial.FindViewById<LinearLayout>(Resource.Id.selectorMain);
            selectorPolicies = partial.FindViewById<LinearLayout>(Resource.Id.selectorPolicies);
            selectorBonuses = partial.FindViewById<LinearLayout>(Resource.Id.selectorBonuses);
            selectorUsefull = partial.FindViewById<LinearLayout>(Resource.Id.selectorUsefull);
            selectorProfile = partial.FindViewById<LinearLayout>(Resource.Id.selectorProfile);

            mainTitle = partial.FindViewById<TextView>(Resource.Id.textMain);
            policiesTitle = partial.FindViewById<TextView>(Resource.Id.textPolicies);
            bonusesTitle = partial.FindViewById<TextView>(Resource.Id.textBonuses);
            usefullTitle = partial.FindViewById<TextView>(Resource.Id.textUsefull);
            profileTitle = partial.FindViewById<TextView>(Resource.Id.textProfile);

            layoutNotifications = partial.FindViewById<RelativeLayout>(Resource.Id.layoutNotifications);
            imageViewNotification = partial.FindViewById<ImageView>(Resource.Id.imageViewNotification);
            textNotificationsCounter = partial.FindViewById<TextView>(Resource.Id.textNotificationsCounter);

            InitControls();

            return partial;
        }

        protected void InitControls()
        {
            if (this.Activity is MainActivity)
            {
                SwitchSelectedItem(PartialType.CurrentRating);
            }
            else
            {
                ResetSelections();
            }

            mainTitle.Text = AppResources.Main;
            policiesTitle.Text = AppResources.Policies;
            bonusesTitle.Text = AppResources.Bonuses;
            usefullTitle.Text = AppResources.Usefull;
            profileTitle.Text = AppResources.More;

            SetupGestures();

            InitNotificator();
        }

        void InitNotificator()
        {
            imageViewNotification.Visibility = ViewStates.Gone;
            textNotificationsCounter.Visibility = ViewStates.Gone;
        }

        void SetupGestures()
        {
            mainLayout.Click += delegate
            {
                if (this.Activity is MainActivity)
                {
                    SwitchSelectedItem(PartialType.CurrentRating);
                }
                else
                {
                    SessionManager.ShowPartialOnMain = PartialType.CurrentRating;
                    this.Activity.Finish();
                }
            };
            policiesLayout.Click += delegate
            {
                if (this.Activity is MainActivity)
                {
                    SwitchSelectedItem(PartialType.Policies);
                }
                else
                {
                    SessionManager.ShowPartialOnMain = PartialType.Policies;
                    this.Activity.Finish();
                }
            };
            bonusesLayout.Click += delegate
            {
                if (this.Activity is MainActivity)
                {
                    SwitchSelectedItem(PartialType.Bonuses);
                }
                else
                {
                    SessionManager.ShowPartialOnMain = PartialType.Bonuses;
                    this.Activity.Finish();
                }
            };
            usefullLayout.Click += delegate
            {
                if (this.Activity is MainActivity)
                {
                    SwitchSelectedItem(PartialType.Usefull);
                }
                else
                {
                    SessionManager.ShowPartialOnMain = PartialType.Usefull;
                    this.Activity.Finish();
                }
            };
            profileLayout.Click += delegate
            {
                if (this.Activity is MainActivity)
                {
                    SwitchSelectedItem(PartialType.Profile);
                }
                else
                {
                    SessionManager.ShowPartialOnMain = PartialType.Profile;
                    this.Activity.Finish();
                }
            };
        }

        public void SwitchSelectedItem(PartialType type, bool resetWithFragmentSetup = true)
        {
            if (type != PartialType.Support)
            {
                ResetSelections();
            }

            if ((this.Activity as MainActivity).isInit && type != PartialType.CurrentRating)
            {
                (this.Activity as MainActivity).isInit = false;
            }

            if (type == PartialType.CurrentRating)
            {
                (this.Activity as MainActivity).ActiveTab = PartialType.CurrentRating;
                selectorMain.Visibility = ViewStates.Visible;
                imageViewMain.SetColorFilter(new Color(ContextCompat.GetColor(this.Activity, Resource.Color.bgr_white)), PorterDuff.Mode.SrcAtop);
                mainTitle.SetTextColor(new Color(ContextCompat.GetColor(this.Activity, Resource.Color.bgr_white)));
            }
            if (type == PartialType.Policies)
            {
                (this.Activity as MainActivity).ActiveTab = PartialType.Policies;
                selectorPolicies.Visibility = ViewStates.Visible;
                imageViewPolicies.SetColorFilter(new Color(ContextCompat.GetColor(this.Activity, Resource.Color.bgr_white)), PorterDuff.Mode.SrcAtop);
                policiesTitle.SetTextColor(new Color(ContextCompat.GetColor(this.Activity, Resource.Color.bgr_white)));
            }
            if (type == PartialType.Bonuses)
            {
                (this.Activity as MainActivity).ActiveTab = PartialType.Bonuses;
                selectorBonuses.Visibility = ViewStates.Visible;
                imageViewBonuses.SetColorFilter(new Color(ContextCompat.GetColor(this.Activity, Resource.Color.bgr_white)), PorterDuff.Mode.SrcAtop);
                bonusesTitle.SetTextColor(new Color(ContextCompat.GetColor(this.Activity, Resource.Color.bgr_white)));
            }
            if (type == PartialType.Profile)
            {
                (this.Activity as MainActivity).ActiveTab = PartialType.Profile;
                selectorProfile.Visibility = ViewStates.Visible;
                imageViewProfile.SetColorFilter(new Color(ContextCompat.GetColor(this.Activity, Resource.Color.bgr_white)), PorterDuff.Mode.SrcAtop);
                profileTitle.SetTextColor(new Color(ContextCompat.GetColor(this.Activity, Resource.Color.bgr_white)));
            }
            if (type == PartialType.Usefull)
            {
                (this.Activity as MainActivity).ActiveTab = PartialType.Usefull;
                selectorUsefull.Visibility = ViewStates.Visible;
                imageViewUsefull.SetColorFilter(new Color(ContextCompat.GetColor(this.Activity, Resource.Color.bgr_white)), PorterDuff.Mode.SrcAtop);
                usefullTitle.SetTextColor(new Color(ContextCompat.GetColor(this.Activity, Resource.Color.bgr_white)));
            }

            if (resetWithFragmentSetup)
                (this.Activity as MainActivity).SetupFragment(type);
        }

        void ResetSelections()
        {
            selectorMain.Visibility = ViewStates.Gone;
            selectorPolicies.Visibility = ViewStates.Gone;
            selectorBonuses.Visibility = ViewStates.Gone;
            selectorProfile.Visibility = ViewStates.Gone;
            selectorUsefull.Visibility = ViewStates.Gone;

            imageViewMain.SetColorFilter(new Color(ContextCompat.GetColor(this.Activity, Resource.Color.description_message_color)), PorterDuff.Mode.SrcAtop);
            imageViewPolicies.SetColorFilter(new Color(ContextCompat.GetColor(this.Activity, Resource.Color.description_message_color)), PorterDuff.Mode.SrcAtop);
            imageViewBonuses.SetColorFilter(new Color(ContextCompat.GetColor(this.Activity, Resource.Color.description_message_color)), PorterDuff.Mode.SrcAtop);
            imageViewProfile.SetColorFilter(new Color(ContextCompat.GetColor(this.Activity, Resource.Color.description_message_color)), PorterDuff.Mode.SrcAtop);
            imageViewUsefull.SetColorFilter(new Color(ContextCompat.GetColor(this.Activity, Resource.Color.description_message_color)), PorterDuff.Mode.SrcAtop);

            mainTitle.SetTextColor(new Color(ContextCompat.GetColor(this.Activity, Resource.Color.description_message_color)));
            policiesTitle.SetTextColor(new Color(ContextCompat.GetColor(this.Activity, Resource.Color.description_message_color)));
            bonusesTitle.SetTextColor(new Color(ContextCompat.GetColor(this.Activity, Resource.Color.description_message_color)));
            profileTitle.SetTextColor(new Color(ContextCompat.GetColor(this.Activity, Resource.Color.description_message_color)));
            usefullTitle.SetTextColor(new Color(ContextCompat.GetColor(this.Activity, Resource.Color.description_message_color)));
        }

        #region abstract

        protected override int GetLayoutId()
        {
            return Resource.Layout.Menu;
        }

        #endregion
    }
}
