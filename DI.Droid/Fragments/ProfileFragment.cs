using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using DI.Droid.Fragments.Base;
using DI.Localization;
using DI.Shared.Enums;
using DI.Shared.Managers;

namespace DI.Droid.Fragments
{
    public class ProfileFragment : BaseFragment
    {
        RelativeLayout chatLayout;
        RelativeLayout profileLayout;
        RelativeLayout contactsLayout;
        RelativeLayout eventsLayout;

        TextView fioText;
        TextView chatText;
        TextView profileText;
        TextView contactsText;
        TextView eventsText;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View partial = base.OnCreateView(inflater, container, savedInstanceState);

            chatLayout = partial.FindViewById<RelativeLayout>(Resource.Id.layoutChat);
            profileLayout = partial.FindViewById<RelativeLayout>(Resource.Id.layoutProfile);
            contactsLayout = partial.FindViewById<RelativeLayout>(Resource.Id.layoutContacts);
            eventsLayout = partial.FindViewById<RelativeLayout>(Resource.Id.layoutEvents);

            chatText = partial.FindViewById<TextView>(Resource.Id.textChat);
            profileText = partial.FindViewById<TextView>(Resource.Id.textProfile);
            contactsText = partial.FindViewById<TextView>(Resource.Id.textContacts);
            eventsText = partial.FindViewById<TextView>(Resource.Id.textEvents);
            fioText = partial.FindViewById<TextView>(Resource.Id.textFIO);

            InitControls();

            return partial;
        }

        protected void InitControls()
        {
            fioText.Text = string.Empty;
            if (SessionManager.СontractorData != null)
            {
                fioText.Text = SessionManager.СontractorData.FirstName + " " + SessionManager.СontractorData.LastName;
            }

            chatText.Text = AppResources.Chat;
            profileText.Text = AppResources.Profile;
            contactsText.Text = AppResources.Contacts;
            eventsText.Text = AppResources.Events;

            SetupGestures();
        }

        void SetupGestures()
        {
            chatLayout.Click += delegate
            {
                var activity = new Intent(this.Activity, typeof(SupportActivity));
                StartActivity(activity);
            };
            profileLayout.Click += delegate
            {
                var activity = new Intent(this.Activity, typeof(ProfileDetailsActivity));
                StartActivity(activity);
            };
            contactsLayout.Click += delegate {
                var activity = new Intent(this.Activity, typeof(ContactsActivity));
                StartActivity(activity);
            };
            eventsLayout.Click += delegate
            {
                var activity = new Intent(this.Activity, typeof(EventsActivity));
                StartActivity(activity);
            };
        }

        public override void OnResume()
        {
            base.OnResume();

            if (this.Activity != null && !(this.Activity as MainActivity).isInit)
            {
                if ((this.Activity as MainActivity).ActiveTab != PartialType.Profile)
                {
                    (this.Activity as MainActivity).SetMenuSelection(PartialType.Profile);
                }
            }
        }

        #region abstract

        protected override int GetLayoutId()
        {
            return Resource.Layout.Profile;
        }

        #endregion
    }
}
