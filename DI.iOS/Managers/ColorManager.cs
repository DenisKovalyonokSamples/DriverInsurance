using System;
using System.Collections.Generic;
using System.Text;
using UIKit;

namespace DI.iOS.Managers
{
    public static class ColorManager
    {
        public static UIColor actionbar_blue = FromHexString("#353543");
        public static UIColor statusbar_blue = FromHexString("#2F2F3C");
        public static UIColor submit_button_color = FromHexString("#28D2B9");
        public static UIColor control_default_color = FromHexString("#42434F");
        public static UIColor description_icon_color = FromHexString("#96969D");
        public static UIColor description_message_color = FromHexString("#B5B5B9");
        public static UIColor message_background_color = FromHexString("#43434F");
        public static UIColor panel_background_color = FromHexString("#4F4F5B");
        public static UIColor grid_separator_color = FromHexString("#434250");
        public static UIColor text_orange = FromHexString("#EAC481");
        public static UIColor bgr_white = FromHexString("#FFFFFF");
        public static UIColor bgr_black = FromHexString("#000000");
        public static UIColor content_gray = FromHexString("#B3B3B3");
        public static UIColor content_green = FromHexString("#2BF60E");
        public static UIColor background_dark_blue = FromHexString("#2E2E3B");
        public static UIColor background_light_blue = FromHexString("#2A4069");
        public static UIColor content_separator_blue = FromHexString("#2A4069");
        public static UIColor text_dark_blue = FromHexString("#265EE5");
        public static UIColor text_light_blue = FromHexString("#769DFA");
        public static UIColor statusbar_red = FromHexString("#D01F29");
        public static UIColor text_gray = FromHexString("#5A5957");
        public static UIColor bgr_chat_panel = FromHexString("#F6F7F8");
        public static UIColor bgr_chat_panel_border = FromHexString("#F1F2F3");
        public static UIColor bgr_chat_incoming_message = FromHexString("#DCE8F4");

        #region Converters

        public static UIColor FromHexString(string hexValue)
        {
            var colorString = hexValue.Replace("#", "");
            float red, green, blue;

            switch (colorString.Length)
            {
                case 3: // #RGB
                    {
                        red = Convert.ToInt32(string.Format("{0}{0}", colorString.Substring(0, 1)), 16) / 255f;
                        green = Convert.ToInt32(string.Format("{0}{0}", colorString.Substring(1, 1)), 16) / 255f;
                        blue = Convert.ToInt32(string.Format("{0}{0}", colorString.Substring(2, 1)), 16) / 255f;
                        return UIColor.FromRGB(red, green, blue);
                    }
                case 6: // #RRGGBB
                    {
                        red = Convert.ToInt32(colorString.Substring(0, 2), 16) / 255f;
                        green = Convert.ToInt32(colorString.Substring(2, 2), 16) / 255f;
                        blue = Convert.ToInt32(colorString.Substring(4, 2), 16) / 255f;
                        return UIColor.FromRGB(red, green, blue);
                    }
                case 8: // #AARRGGBB
                    {
                        var alpha = Convert.ToInt32(colorString.Substring(0, 2), 16) / 255f;
                        red = Convert.ToInt32(colorString.Substring(2, 2), 16) / 255f;
                        green = Convert.ToInt32(colorString.Substring(4, 2), 16) / 255f;
                        blue = Convert.ToInt32(colorString.Substring(6, 2), 16) / 255f;
                        return UIColor.FromRGBA(red, green, blue, alpha);
                    }
                default:
                    throw new ArgumentOutOfRangeException(string.Format("Invalid color value {0} is invalid. It should be a hex value of the form #RBG, #RRGGBB, or #AARRGGBB", hexValue));

            }
        }

        #endregion
    }
}
