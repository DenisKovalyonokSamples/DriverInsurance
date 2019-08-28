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
    [Register ("RegisterPhoneController")]
    partial class RegisterPhoneController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIActivityIndicatorView AIProgressBar { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton BTSubmit { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView IVHelp { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel LBDescription { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel LBPhoneTitle { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel LBPhoneVM { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel LBToolbarTitle { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField TFPhone { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView VContainer { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView VToolbar { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (AIProgressBar != null) {
                AIProgressBar.Dispose ();
                AIProgressBar = null;
            }

            if (BTSubmit != null) {
                BTSubmit.Dispose ();
                BTSubmit = null;
            }

            if (IVHelp != null) {
                IVHelp.Dispose ();
                IVHelp = null;
            }

            if (LBDescription != null) {
                LBDescription.Dispose ();
                LBDescription = null;
            }

            if (LBPhoneTitle != null) {
                LBPhoneTitle.Dispose ();
                LBPhoneTitle = null;
            }

            if (LBPhoneVM != null) {
                LBPhoneVM.Dispose ();
                LBPhoneVM = null;
            }

            if (LBToolbarTitle != null) {
                LBToolbarTitle.Dispose ();
                LBToolbarTitle = null;
            }

            if (TFPhone != null) {
                TFPhone.Dispose ();
                TFPhone = null;
            }

            if (VContainer != null) {
                VContainer.Dispose ();
                VContainer = null;
            }

            if (VToolbar != null) {
                VToolbar.Dispose ();
                VToolbar = null;
            }
        }
    }
}