using CoreAnimation;
using CoreGraphics;
using UIKit;

namespace DI.iOS.Extensions
{
    public static class TextFieldExtensions
    {
        public static void SetBottomBorder(this UITextField control, UIColor color)
        {
            control.Layer.CornerRadius = 0;
            control.Layer.MasksToBounds = true;
            control.BorderStyle = UITextBorderStyle.None;

            var border = new CALayer();
            var borderWidth = 1f;
            border.BorderColor = color.CGColor;

            control.SetNeedsLayout();
            control.LayoutIfNeeded();
            border.Frame = new CGRect(0, control.Frame.Size.Height - borderWidth, control.Frame.Size.Width, 1);

            border.BorderWidth = borderWidth;
            control.Layer.AddSublayer(border);
        }

        public static void SetBottomBorder(this UITextView control, UIColor color)
        {
            control.Layer.CornerRadius = 0;
            control.Layer.MasksToBounds = true;

            var border = new CALayer();
            var borderWidth = 1f;
            border.BorderColor = color.CGColor;

            control.SetNeedsLayout();
            control.LayoutIfNeeded();
            border.Frame = new CGRect(0, control.Frame.Size.Height - borderWidth, control.Frame.Size.Width, 1);

            border.BorderWidth = borderWidth;
            control.Layer.AddSublayer(border);
        }
    }
}
