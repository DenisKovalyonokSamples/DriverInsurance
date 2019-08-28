// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace DI.iOS
{
    [Register ("MoreFragment")]
    partial class MoreFragment
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView IVAvatar { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel LBChat { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel LBContacts { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel LBEvents { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel LBProfile { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel LBUserName { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView VChat { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView VContacts { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView VEvents { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView VProfile { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (IVAvatar != null) {
                IVAvatar.Dispose ();
                IVAvatar = null;
            }

            if (LBChat != null) {
                LBChat.Dispose ();
                LBChat = null;
            }

            if (LBContacts != null) {
                LBContacts.Dispose ();
                LBContacts = null;
            }

            if (LBEvents != null) {
                LBEvents.Dispose ();
                LBEvents = null;
            }

            if (LBProfile != null) {
                LBProfile.Dispose ();
                LBProfile = null;
            }

            if (LBUserName != null) {
                LBUserName.Dispose ();
                LBUserName = null;
            }

            if (VChat != null) {
                VChat.Dispose ();
                VChat = null;
            }

            if (VContacts != null) {
                VContacts.Dispose ();
                VContacts = null;
            }

            if (VEvents != null) {
                VEvents.Dispose ();
                VEvents = null;
            }

            if (VProfile != null) {
                VProfile.Dispose ();
                VProfile = null;
            }
        }
    }
}