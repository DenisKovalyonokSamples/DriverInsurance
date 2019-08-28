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
    [Register ("MainFragment")]
    partial class MainFragment
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIActivityIndicatorView AIProgressBar { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.NSLayoutConstraint CCurrentRatingHeight { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.NSLayoutConstraint CDynamicsHeight { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView CVCurrentRating { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView CVDemoMode { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView CVDynamics { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel LBCurrentRatingTitle { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel LBDynamicsTitle { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView VCurrentRatingSeparator { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView VCurrentRatingTab { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView VDynamicsSeparator { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView VDynamicsTab { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (AIProgressBar != null) {
                AIProgressBar.Dispose ();
                AIProgressBar = null;
            }

            if (CCurrentRatingHeight != null) {
                CCurrentRatingHeight.Dispose ();
                CCurrentRatingHeight = null;
            }

            if (CDynamicsHeight != null) {
                CDynamicsHeight.Dispose ();
                CDynamicsHeight = null;
            }

            if (CVCurrentRating != null) {
                CVCurrentRating.Dispose ();
                CVCurrentRating = null;
            }

            if (CVDemoMode != null) {
                CVDemoMode.Dispose ();
                CVDemoMode = null;
            }

            if (CVDynamics != null) {
                CVDynamics.Dispose ();
                CVDynamics = null;
            }

            if (LBCurrentRatingTitle != null) {
                LBCurrentRatingTitle.Dispose ();
                LBCurrentRatingTitle = null;
            }

            if (LBDynamicsTitle != null) {
                LBDynamicsTitle.Dispose ();
                LBDynamicsTitle = null;
            }

            if (VCurrentRatingSeparator != null) {
                VCurrentRatingSeparator.Dispose ();
                VCurrentRatingSeparator = null;
            }

            if (VCurrentRatingTab != null) {
                VCurrentRatingTab.Dispose ();
                VCurrentRatingTab = null;
            }

            if (VDynamicsSeparator != null) {
                VDynamicsSeparator.Dispose ();
                VDynamicsSeparator = null;
            }

            if (VDynamicsTab != null) {
                VDynamicsTab.Dispose ();
                VDynamicsTab = null;
            }
        }
    }
}