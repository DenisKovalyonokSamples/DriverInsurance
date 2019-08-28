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
    [Register ("ContactsController")]
    partial class ContactsController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView IVBack { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel LBToolbarTitle { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView VToolbar { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (IVBack != null) {
                IVBack.Dispose ();
                IVBack = null;
            }

            if (LBToolbarTitle != null) {
                LBToolbarTitle.Dispose ();
                LBToolbarTitle = null;
            }

            if (VToolbar != null) {
                VToolbar.Dispose ();
                VToolbar = null;
            }
        }
    }
}