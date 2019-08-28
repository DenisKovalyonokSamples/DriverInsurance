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
    [Register ("PolicyInfoMessageFragment")]
    partial class PolicyInfoMessageFragment
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel LBPolicyRequestMessage { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (LBPolicyRequestMessage != null) {
                LBPolicyRequestMessage.Dispose ();
                LBPolicyRequestMessage = null;
            }
        }
    }
}