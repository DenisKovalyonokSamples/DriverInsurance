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
    public class UsefullFragment : BaseFragment
    {
        SecurityModeFragment securityModeFragment;
        FindCarLocationFragment findCarLocationFragment;
        TracksFragment tracksFragment;

        LinearLayout layoutTabs;
        LinearLayout selectorSecurityModeTab;
        LinearLayout selectorMapTab;
        LinearLayout selectorTracksTab;
        LinearLayout layoutTabSelectors;

        Button buttonSecurityModeTab;
        Button buttonFindCarLocationTab;
        Button buttonTracksTab;

        LinearLayout layoutWithoutPolicyTabs;
        Button buttonWithoutTracksTab;
        Button buttonWithoutFindCarLocationTab;
        LinearLayout layoutWithoutTabSelectors;
        LinearLayout selectorWithoutTracksTab;
        LinearLayout selectorWithoutFindCarLocationTab;

        Color greenColor;
        Color blueColor;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View partial = base.OnCreateView(inflater, container, savedInstanceState);

            layoutTabs = partial.FindViewById<LinearLayout>(Resource.Id.layoutTabs);

            buttonTracksTab = partial.FindViewById<Button>(Resource.Id.buttonTracksTab);
            buttonSecurityModeTab = partial.FindViewById<Button>(Resource.Id.buttonSecurityModeTab);
            buttonFindCarLocationTab = partial.FindViewById<Button>(Resource.Id.buttonFindCarLocationTab);

            layoutTabSelectors = partial.FindViewById<LinearLayout>(Resource.Id.layoutTabSelectors);
            selectorSecurityModeTab = partial.FindViewById<LinearLayout>(Resource.Id.selectorSecurityModeTab);
            selectorMapTab = partial.FindViewById<LinearLayout>(Resource.Id.selectorMapTab);
            selectorTracksTab = partial.FindViewById<LinearLayout>(Resource.Id.selectorTracksTab);

            layoutWithoutPolicyTabs = partial.FindViewById<LinearLayout>(Resource.Id.layoutWithoutPolicyTabs);
            buttonWithoutTracksTab = partial.FindViewById<Button>(Resource.Id.buttonWithoutTracksTab);
            buttonWithoutFindCarLocationTab = partial.FindViewById<Button>(Resource.Id.buttonWithoutFindCarLocationTab);

            layoutWithoutTabSelectors = partial.FindViewById<LinearLayout>(Resource.Id.layoutWithoutTabSelectors);
            selectorWithoutTracksTab = partial.FindViewById<LinearLayout>(Resource.Id.selectorWithoutTracksTab);
            selectorWithoutFindCarLocationTab = partial.FindViewById<LinearLayout>(Resource.Id.selectorWithoutFindCarLocationTab);

            greenColor = new Color(ContextCompat.GetColor(this.Activity, Resource.Color.content_green));
            blueColor = new Color(ContextCompat.GetColor(this.Activity, Resource.Color.actionbar_blue));

            InitControls();

            return partial;
        }

        public override void OnResume()
        {
            base.OnResume();

            if (this.Activity != null && !(this.Activity as MainActivity).isInit)
            {
                if ((this.Activity as MainActivity).ActiveTab != PartialType.Usefull)
                {
                    (this.Activity as MainActivity).SetMenuSelection(PartialType.Usefull);
                }
            }
        }

        protected void InitControls()
        {
            buttonTracksTab.Text = AppResources.Tracks.ToUpper();
            buttonSecurityModeTab.Text = AppResources.SecurityMode.ToUpper();
            buttonFindCarLocationTab.Text = AppResources.FindCarLocation.ToUpper();

            buttonWithoutTracksTab.Text = AppResources.Tracks.ToUpper();
            buttonWithoutFindCarLocationTab.Text = AppResources.FindCarLocation.ToUpper();

            if (!(this.Activity as MainActivity).isInit)
            {
                if (SessionManager.СontractData != null)
                {
                    layoutWithoutPolicyTabs.Visibility = ViewStates.Visible;
                    layoutWithoutTabSelectors.Visibility = ViewStates.Visible;

                    findCarLocationFragment = new FindCarLocationFragment();
                    tracksFragment = new TracksFragment();

                    if (!this.Activity.IsFinishing)
                    {
                        var partialSetup = this.Activity.SupportFragmentManager.BeginTransaction();
                        partialSetup.Add(Resource.Id.fragmentInsideContainer, findCarLocationFragment, "FindCarLocationFragment");
                        partialSetup.Hide(findCarLocationFragment);
                        partialSetup.Add(Resource.Id.fragmentInsideContainer, tracksFragment, "TracksFragment");
                        partialSetup.CommitAllowingStateLoss();
                    }
                }
                else
                {
                    layoutTabs.Visibility = ViewStates.Visible;
                    layoutTabSelectors.Visibility = ViewStates.Visible;

                    securityModeFragment = new SecurityModeFragment();
                    findCarLocationFragment = new FindCarLocationFragment();
                    tracksFragment = new TracksFragment();

                    if (!this.Activity.IsFinishing)
                    {
                        var partialSetup = this.Activity.SupportFragmentManager.BeginTransaction();
                        partialSetup.Add(Resource.Id.fragmentInsideContainer, findCarLocationFragment, "FindCarLocationFragment");
                        partialSetup.Hide(findCarLocationFragment);
                        partialSetup.Add(Resource.Id.fragmentInsideContainer, securityModeFragment, "SecurityModeFragment");
                        partialSetup.Hide(securityModeFragment);
                        partialSetup.Add(Resource.Id.fragmentInsideContainer, tracksFragment, "TracksFragment");
                        partialSetup.CommitAllowingStateLoss();
                    }
                }
            }

            SetupGestures();
        }

        void SetupGestures()
        {
            buttonSecurityModeTab.Click += delegate
            {
                selectorSecurityModeTab.Visibility = ViewStates.Visible;
                selectorMapTab.Visibility = ViewStates.Invisible;
                selectorTracksTab.Visibility = ViewStates.Invisible;

                ShowFragment(PartialType.SecurityMode);
            };

            buttonFindCarLocationTab.Click += delegate
            {
                selectorSecurityModeTab.Visibility = ViewStates.Invisible;
                selectorMapTab.Visibility = ViewStates.Visible;
                selectorTracksTab.Visibility = ViewStates.Invisible;

                ShowFragment(PartialType.FindCarLocation);
            };

            buttonTracksTab.Click += delegate
            {
                selectorSecurityModeTab.Visibility = ViewStates.Invisible;
                selectorMapTab.Visibility = ViewStates.Invisible;
                selectorTracksTab.Visibility = ViewStates.Visible;

                ShowFragment(PartialType.Tracks);
            };

            buttonWithoutFindCarLocationTab.Click += delegate
            {
                selectorWithoutFindCarLocationTab.Visibility = ViewStates.Visible;
                selectorWithoutTracksTab.Visibility = ViewStates.Invisible;

                ShowFragment(PartialType.FindCarLocation, false);
            };

            buttonWithoutTracksTab.Click += delegate
            {
                selectorWithoutFindCarLocationTab.Visibility = ViewStates.Invisible;
                selectorWithoutTracksTab.Visibility = ViewStates.Visible;

                ShowFragment(PartialType.Tracks, false);
            };
        }

        private void ShowFragment(PartialType type, bool withSecurityMode = true)
        {
            var partialSetup = this.Activity.SupportFragmentManager.BeginTransaction();

            if (type == PartialType.SecurityMode)
            {
                partialSetup.Hide(findCarLocationFragment);
                partialSetup.Hide(tracksFragment);
                partialSetup.AddToBackStack(null);

                securityModeFragment = new SecurityModeFragment();
                partialSetup.Replace(Resource.Id.fragmentInsideContainer, securityModeFragment, "SecurityModeFragment");
                partialSetup.Show(securityModeFragment);
            }

            if (type == PartialType.FindCarLocation)
            {
                if (withSecurityMode)
                {
                    partialSetup.Hide(securityModeFragment);
                }
                partialSetup.Hide(tracksFragment);
                partialSetup.AddToBackStack(null);

                findCarLocationFragment = new FindCarLocationFragment();
                partialSetup.Replace(Resource.Id.fragmentInsideContainer, findCarLocationFragment, "FindCarLocationFragment");
                partialSetup.Show(findCarLocationFragment);
            }

            if (type == PartialType.Tracks)
            {
                if (withSecurityMode)
                {
                    partialSetup.Hide(securityModeFragment);
                }
                partialSetup.Hide(findCarLocationFragment);
                partialSetup.AddToBackStack(null);

                tracksFragment = new TracksFragment();
                partialSetup.Replace(Resource.Id.fragmentInsideContainer, tracksFragment, "TracksFragment");
                partialSetup.Show(tracksFragment);
            }

            if (!this.Activity.IsFinishing)
            {
                partialSetup.CommitAllowingStateLoss();
            }
        }

        #region abstract

        protected override int GetLayoutId()
        {
            return Resource.Layout.Usefull;
        }

        #endregion
    }
}
