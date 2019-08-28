using DI.Shared.DataAccess;
using DI.Shared.Managers;
using System;
using System.Collections.Generic;
using System.Text;
using UIKit;

namespace DI.iOS.Fragments.Base
{
    public abstract partial class BaseFragment : UIViewController
    {
        protected SQLDataManager sqliteManager;

        public BaseFragment(IntPtr handle) : base(handle)
        {
            sqliteManager = SessionManager.DBConnection;
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
        }
    }
}
