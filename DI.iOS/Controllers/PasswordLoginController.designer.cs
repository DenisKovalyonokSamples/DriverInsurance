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
    [Register ("PasswordLoginController")]
    partial class PasswordLoginController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIActivityIndicatorView AIProgressBar { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton BTSubmit { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView IVBack { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel LBPageTitle { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel LBPasswordTitle { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel LBPasswordVM { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel LBRestorePassword { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel LBToolbarTitle { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField TFPassword { get; set; }

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

            if (IVBack != null) {
                IVBack.Dispose ();
                IVBack = null;
            }

            if (LBPageTitle != null) {
                LBPageTitle.Dispose ();
                LBPageTitle = null;
            }

            if (LBPasswordTitle != null) {
                LBPasswordTitle.Dispose ();
                LBPasswordTitle = null;
            }

            if (LBPasswordVM != null) {
                LBPasswordVM.Dispose ();
                LBPasswordVM = null;
            }

            if (LBRestorePassword != null) {
                LBRestorePassword.Dispose ();
                LBRestorePassword = null;
            }

            if (LBToolbarTitle != null) {
                LBToolbarTitle.Dispose ();
                LBToolbarTitle = null;
            }

            if (TFPassword != null) {
                TFPassword.Dispose ();
                TFPassword = null;
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