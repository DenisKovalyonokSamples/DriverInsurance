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
    [Register ("PolicyRequestFragment")]
    partial class PolicyRequestFragment
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton BTSubmit { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel LBCostTitle { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel LBCurrentScoreTitle { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel LBScore { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (BTSubmit != null) {
                BTSubmit.Dispose ();
                BTSubmit = null;
            }

            if (LBCostTitle != null) {
                LBCostTitle.Dispose ();
                LBCostTitle = null;
            }

            if (LBCurrentScoreTitle != null) {
                LBCurrentScoreTitle.Dispose ();
                LBCurrentScoreTitle = null;
            }

            if (LBScore != null) {
                LBScore.Dispose ();
                LBScore = null;
            }
        }
    }
}