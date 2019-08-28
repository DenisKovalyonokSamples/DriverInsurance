using Android.Graphics;
using Android.OS;
using Android.Support.V4.Content;
using Android.Views;
using Android.Widget;
using DI.Droid.Fragments.Base;
using DI.Localization;
using DI.Shared.DataAccess;
using DI.Shared.Enums;
using DI.Shared.Managers;
using System;

namespace DI.Droid.Fragments
{
    public class TrackDataFragment : BaseFragment
    {
        UserInfoFragment userInfoFragment;
        DynamicsFragment dynamicsFragment;
        DemoModeFragment demoModeFragment;

        LinearLayout selectorRatingsTab;
        LinearLayout selectorDynamicsTab;
        LinearLayout layoutTabSelectors;

        Button buttonCurrentRatingTab;
        Button buttonDynamicsTab;

        Color greenColor;
        Color blueColor;

        LinearLayout layoutTabs;
        ProgressBar progressBarLoading;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View partial = base.OnCreateView(inflater, container, savedInstanceState);

            layoutTabSelectors = partial.FindViewById<LinearLayout>(Resource.Id.layoutTabSelectors);
            selectorRatingsTab = partial.FindViewById<LinearLayout>(Resource.Id.selectorRatingsTab);
            selectorDynamicsTab = partial.FindViewById<LinearLayout>(Resource.Id.selectorDynamicsTab);

            layoutTabs = partial.FindViewById<LinearLayout>(Resource.Id.layoutTabs);
            buttonCurrentRatingTab = partial.FindViewById<Button>(Resource.Id.buttonCurrentRatingTab);
            buttonDynamicsTab = partial.FindViewById<Button>(Resource.Id.buttonDynamicsTab);
            buttonDynamicsTab = partial.FindViewById<Button>(Resource.Id.buttonDynamicsTab);
            progressBarLoading = partial.FindViewById<ProgressBar>(Resource.Id.progressBarLoading);

            greenColor = new Color(ContextCompat.GetColor(this.Activity, Resource.Color.content_green));
            blueColor = new Color(ContextCompat.GetColor(this.Activity, Resource.Color.actionbar_blue));

            userInfoFragment = new UserInfoFragment();
            dynamicsFragment = new DynamicsFragment();
            demoModeFragment = new DemoModeFragment();

            InitControls();

            return partial;
        }

        protected void InitControls()
        {
            layoutTabs.Visibility = ViewStates.Gone;
            layoutTabSelectors.Visibility = ViewStates.Gone;
            progressBarLoading.Visibility = ViewStates.Visible;

            CheckContract();
        }


        async void CheckContract()
        {
            try
            {
                if (SessionManager.СontractData == null)
                {
                    if (SessionManager.СontractorData != null)
                    {
                        SessionManager.СontractData = await APIDataManager.GetCurrentContract(SessionManager.СontractorData.Id.ToString());
                    }
                }

                if (SessionManager.СontractData != null)
                {
                    if (!this.Activity.IsFinishing)
                    {
                        var partialSetup = this.Activity.SupportFragmentManager.BeginTransaction();
                        partialSetup.Add(Resource.Id.fragmentInsideContainer, dynamicsFragment, "DynamicsFragment");
                        partialSetup.Hide(dynamicsFragment);
                        partialSetup.Add(Resource.Id.fragmentInsideContainer, userInfoFragment, "UserInfoFragment");
                        partialSetup.CommitAllowingStateLoss();
                    }

                    progressBarLoading.Visibility = ViewStates.Gone;
                    layoutTabSelectors.Visibility = ViewStates.Visible;
                    layoutTabs.Visibility = ViewStates.Visible;
                }
                else
                {
                    if (!this.Activity.IsFinishing)
                    {
                        var partialSetup = this.Activity.SupportFragmentManager.BeginTransaction();
                        partialSetup.Add(Resource.Id.fragmentInsideContainer, demoModeFragment, "DemoModeFragment");
                        partialSetup.CommitAllowingStateLoss();
                    }

                    layoutTabs.Visibility = ViewStates.Gone;
                    layoutTabSelectors.Visibility = ViewStates.Gone;
                    progressBarLoading.Visibility = ViewStates.Gone;
                }

                if (SessionManager.СontractData != null)
                {
                    buttonCurrentRatingTab.Text = AppResources.CurrentRating.ToUpper();
                    buttonDynamicsTab.Text = AppResources.Dynamics.ToUpper();
                }
            }
            catch (Exception ex)
            {
                HockeyApp.Android.Metrics.MetricsManager.TrackEvent("CheckContract ERROR: " + ex.ToString());
            }

            SetupGestures();
        }

        void SetupGestures()
        {
            buttonCurrentRatingTab.Click += delegate
            {
                selectorRatingsTab.Visibility = ViewStates.Visible;
                selectorDynamicsTab.Visibility = ViewStates.Invisible;

                ShowFragment(PartialType.UserInfo);
            };

            buttonDynamicsTab.Click += delegate
            {
                selectorRatingsTab.Visibility = ViewStates.Invisible;
                selectorDynamicsTab.Visibility = ViewStates.Visible;

                ShowFragment(PartialType.Dynamics);
            };
        }

        private void ShowFragment(PartialType type)
        {
            var partialSetup = this.Activity.SupportFragmentManager.BeginTransaction();

            if (type == PartialType.UserInfo)
            {
                partialSetup.Hide(dynamicsFragment);
                partialSetup.AddToBackStack(null);

                userInfoFragment = new UserInfoFragment();
                partialSetup.Replace(Resource.Id.fragmentInsideContainer, userInfoFragment, "UserInfoFragment");
                partialSetup.Show(userInfoFragment);
            }
            if (type == PartialType.Dynamics)
            {
                partialSetup.Hide(userInfoFragment);
                partialSetup.AddToBackStack(null);

                dynamicsFragment = new DynamicsFragment();
                partialSetup.Replace(Resource.Id.fragmentInsideContainer, dynamicsFragment, "DynamicsFragment");
                partialSetup.Show(dynamicsFragment);
            }

            if (!this.Activity.IsFinishing)
            {
                partialSetup.CommitAllowingStateLoss();
            }
        }

        public override void OnResume()
        {
            base.OnResume();

            if (this.Activity != null && !(this.Activity as MainActivity).isInit)
            {
                if ((this.Activity as MainActivity).ActiveTab != PartialType.CurrentRating)
                {
                    (this.Activity as MainActivity).SetMenuSelection(PartialType.CurrentRating);
                }
            }
        }

        #region abstract

        protected override int GetLayoutId()
        {
            return Resource.Layout.TrackData;
        }

        #endregion
    }
}
