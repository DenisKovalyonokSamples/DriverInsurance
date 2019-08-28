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
    [Register ("ValidateSMSCodeController")]
    partial class ValidateSMSCodeController
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
        UIKit.UIImageView IVInfoIcon { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel LBBonusCodeTitle { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel LBBonusCodeVM { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel LBCodeTitle { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel LBCodeVM { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel LBDescription { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel LBPageTitle { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel LBResendSMS { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel LBToolbarTitle { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField TFBonusCode { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField TFSMSCodeFour { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField TFSMSCodeOne { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField TFSMSCodeThree { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField TFSMSCodeTwo { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView VCodeContainer { get; set; }

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

            if (IVInfoIcon != null) {
                IVInfoIcon.Dispose ();
                IVInfoIcon = null;
            }

            if (LBBonusCodeTitle != null) {
                LBBonusCodeTitle.Dispose ();
                LBBonusCodeTitle = null;
            }

            if (LBBonusCodeVM != null) {
                LBBonusCodeVM.Dispose ();
                LBBonusCodeVM = null;
            }

            if (LBCodeTitle != null) {
                LBCodeTitle.Dispose ();
                LBCodeTitle = null;
            }

            if (LBCodeVM != null) {
                LBCodeVM.Dispose ();
                LBCodeVM = null;
            }

            if (LBDescription != null) {
                LBDescription.Dispose ();
                LBDescription = null;
            }

            if (LBPageTitle != null) {
                LBPageTitle.Dispose ();
                LBPageTitle = null;
            }

            if (LBResendSMS != null) {
                LBResendSMS.Dispose ();
                LBResendSMS = null;
            }

            if (LBToolbarTitle != null) {
                LBToolbarTitle.Dispose ();
                LBToolbarTitle = null;
            }

            if (TFBonusCode != null) {
                TFBonusCode.Dispose ();
                TFBonusCode = null;
            }

            if (TFSMSCodeFour != null) {
                TFSMSCodeFour.Dispose ();
                TFSMSCodeFour = null;
            }

            if (TFSMSCodeOne != null) {
                TFSMSCodeOne.Dispose ();
                TFSMSCodeOne = null;
            }

            if (TFSMSCodeThree != null) {
                TFSMSCodeThree.Dispose ();
                TFSMSCodeThree = null;
            }

            if (TFSMSCodeTwo != null) {
                TFSMSCodeTwo.Dispose ();
                TFSMSCodeTwo = null;
            }

            if (VCodeContainer != null) {
                VCodeContainer.Dispose ();
                VCodeContainer = null;
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