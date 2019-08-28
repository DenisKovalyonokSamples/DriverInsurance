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
    [Register ("DynamicsFragment")]
    partial class DynamicsFragment
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIActivityIndicatorView AIMileageProgressBar { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIActivityIndicatorView AIRatingProgressBar { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel LBMileageTitle { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel LBMonthTitle { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel LBRatingTitle { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel LBWeekTitle { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView VMileageChartContainer { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView VMonthSeparator { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView VMonthTab { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView VRatingChartContainer { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView VWeekSeparator { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView VWeekTab { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (AIMileageProgressBar != null) {
                AIMileageProgressBar.Dispose ();
                AIMileageProgressBar = null;
            }

            if (AIRatingProgressBar != null) {
                AIRatingProgressBar.Dispose ();
                AIRatingProgressBar = null;
            }

            if (LBMileageTitle != null) {
                LBMileageTitle.Dispose ();
                LBMileageTitle = null;
            }

            if (LBMonthTitle != null) {
                LBMonthTitle.Dispose ();
                LBMonthTitle = null;
            }

            if (LBRatingTitle != null) {
                LBRatingTitle.Dispose ();
                LBRatingTitle = null;
            }

            if (LBWeekTitle != null) {
                LBWeekTitle.Dispose ();
                LBWeekTitle = null;
            }

            if (VMileageChartContainer != null) {
                VMileageChartContainer.Dispose ();
                VMileageChartContainer = null;
            }

            if (VMonthSeparator != null) {
                VMonthSeparator.Dispose ();
                VMonthSeparator = null;
            }

            if (VMonthTab != null) {
                VMonthTab.Dispose ();
                VMonthTab = null;
            }

            if (VRatingChartContainer != null) {
                VRatingChartContainer.Dispose ();
                VRatingChartContainer = null;
            }

            if (VWeekSeparator != null) {
                VWeekSeparator.Dispose ();
                VWeekSeparator = null;
            }

            if (VWeekTab != null) {
                VWeekTab.Dispose ();
                VWeekTab = null;
            }
        }
    }
}