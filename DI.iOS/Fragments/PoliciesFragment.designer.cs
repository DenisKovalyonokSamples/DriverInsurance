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
    [Register ("PoliciesFragment")]
    partial class PoliciesFragment
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView CVPolicyActive { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView CVPolicyContracts { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView CVPolicyInfoMessage { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView CVPolicyRequest { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (CVPolicyActive != null) {
                CVPolicyActive.Dispose ();
                CVPolicyActive = null;
            }

            if (CVPolicyContracts != null) {
                CVPolicyContracts.Dispose ();
                CVPolicyContracts = null;
            }

            if (CVPolicyInfoMessage != null) {
                CVPolicyInfoMessage.Dispose ();
                CVPolicyInfoMessage = null;
            }

            if (CVPolicyRequest != null) {
                CVPolicyRequest.Dispose ();
                CVPolicyRequest = null;
            }
        }
    }
}