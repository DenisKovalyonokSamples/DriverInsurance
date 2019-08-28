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
    [Register ("ErrorInfoController")]
    partial class ErrorInfoController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView IVLogo { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel LBErrorDescription { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel LBToolbarTitle { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView VContainer { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView VToolbar { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (IVLogo != null) {
                IVLogo.Dispose ();
                IVLogo = null;
            }

            if (LBErrorDescription != null) {
                LBErrorDescription.Dispose ();
                LBErrorDescription = null;
            }

            if (LBToolbarTitle != null) {
                LBToolbarTitle.Dispose ();
                LBToolbarTitle = null;
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