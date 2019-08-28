using DI.Shared.DataAccess;
using DI.Shared.Managers;
using Foundation;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using UIKit;

namespace DI.iOS.Controllers.Base
{
    public abstract partial class BaseController : UIViewController
    {
        protected VMManager vmManager;
        protected SQLDataManager sqliteManager;

        public BaseController(IntPtr handle) : base(handle)
        {
            sqliteManager = SessionManager.DBConnection;
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            SetNeedsStatusBarAppearanceUpdate();
            View.AddGestureRecognizer(new UITapGestureRecognizer(() => View.EndEditing(true)));
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
        }

        #region Toolbar Settings

        protected void SetStatusBarColor(UIColor color)
        {
            var statusBar = (UIApplication.SharedApplication.ValueForKey(new NSString("statusBarWindow")).ValueForKey(new NSString("statusBar")) as UIView);
            if (statusBar != null)
            {
                statusBar.BackgroundColor = color;
            }
        }

        protected void SetUpActionBar(UIView toolbar, UILabel title, UIColor color, string pageTitle)
        {
            toolbar.BackgroundColor = color;
            if (pageTitle != string.Empty)
            {
                title.Text = pageTitle;
            }
        }

        #endregion

        #region Animation

        protected void FlipImageHorizontaly(UIImageView image)
        {
            var options = UIViewAnimationOptions.TransitionFlipFromLeft | UIViewAnimationOptions.Repeat;
            UIView.Transition(image, 2, options, null, null);
        }

        protected void StopImageFlipping(UIImageView image)
        {
            var options = UIViewAnimationOptions.TransitionFlipFromLeft | UIViewAnimationOptions.TransitionNone;
            UIView.Transition(image, 0, options, null, null);
        }

        #endregion

        protected virtual bool LimitTextLenth(UITextField textField, NSRange range, string replacementString, int maxLength, bool numbersOnly = false, bool isInteger = false)
        {
            string numbers = "0123456789.,";
            if (isInteger)
            {
                numbers = "0123456789";
            }

            var length = textField.Text.Length - range.Length + replacementString.Length;
            bool requiredLenth = length <= maxLength;
            if (!requiredLenth)
            {
                return false;
            }
            else
            {
                if (!numbersOnly)
                    return true;
                if (numbers.IndexOf(replacementString) >= 0)
                {
                    if (!isInteger)
                    {
                        double number;
                        if (!double.TryParse(textField.Text.Replace(',', '.') + replacementString, NumberStyles.Any, CultureInfo.InvariantCulture, out number))
                        {
                            return false;
                        }
                    }
                    return true;
                }

                return false;
            }
        }

        protected virtual bool LimitTextLenth(UITextView textField, NSRange range, string replacementString, int maxLength)
        {
            var length = textField.Text.Length - range.Length + replacementString.Length;
            return length <= maxLength;
        }
    }
}
