using DI.Localization;
using DI.Shared.Managers;
using DI.iOS.Fragments.Base;
using Foundation;
using System;
using UIKit;

namespace DI.iOS
{
    public partial class MoreFragment : BaseFragment
    {
        public MoreFragment (IntPtr handle) : base (handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            InitControls();
        }

        void InitControls()
        {
            LBUserName.Text = string.Empty;
            if (SessionManager.СontractorData != null)
            {
                LBUserName.Text = SessionManager.СontractorData.FirstName + " " + SessionManager.СontractorData.LastName;
            }

            LBChat.Text = AppResources.Chat;
            LBProfile.Text = AppResources.Profile;
            LBContacts.Text = AppResources.Contacts;
            LBEvents.Text = AppResources.Events;

            SetupGestures();
        }

        void SetupGestures()
        {
            var vChatClick = new UITapGestureRecognizer(OpenSupportPage);
            VChat.UserInteractionEnabled = true;
            VChat.AddGestureRecognizer(vChatClick);

            var vProfileClick = new UITapGestureRecognizer(OpenProfileDetailsPage);
            VProfile.UserInteractionEnabled = true;
            VProfile.AddGestureRecognizer(vProfileClick);

            var vContactsClick = new UITapGestureRecognizer(OpenContactsPage);
            VContacts.UserInteractionEnabled = true;
            VContacts.AddGestureRecognizer(vContactsClick);

            var vEventsClick = new UITapGestureRecognizer(OpenEventsPage);
            VEvents.UserInteractionEnabled = true;
            VEvents.AddGestureRecognizer(vEventsClick);
        }

        void OpenSupportPage()
        {
            SupportController controller = Storyboard.InstantiateViewController("SupportController") as SupportController;
            if (controller != null)
            {
                PresentViewController(controller, true, null);
            }
        }

        void OpenProfileDetailsPage()
        {
            ProfileDetailsController controller = Storyboard.InstantiateViewController("ProfileDetailsController") as ProfileDetailsController;
            if (controller != null)
            {
                PresentViewController(controller, true, null);
            }
        }

        void OpenContactsPage()
        {
            ContactsController controller = Storyboard.InstantiateViewController("ContactsController") as ContactsController;
            if (controller != null)
            {
                PresentViewController(controller, true, null);
            }
        }

        void OpenEventsPage()
        {
            EventsController controller = Storyboard.InstantiateViewController("EventsController") as EventsController;
            if (controller != null)
            {
                PresentViewController(controller, true, null);
            }
        }
    }
}