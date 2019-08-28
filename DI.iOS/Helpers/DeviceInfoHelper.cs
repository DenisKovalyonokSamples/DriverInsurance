using System;
using System.Collections.Generic;
using System.Text;
using UIKit;

namespace DI.iOS.Helpers
{
    public class DeviceInfoHelper
    {
        public static string GetDeviceUniqID()
        {
            try
            {
                if (ObjCRuntime.Runtime.Arch == ObjCRuntime.Arch.SIMULATOR)
                {
                    return "000000000000000";
                }
                else
                {
                    return UIDevice.CurrentDevice.IdentifierForVendor.ToString();
                }
            }
            catch(Exception ex)
            {
                return "000000000000000";
            }
        }
    }
}
