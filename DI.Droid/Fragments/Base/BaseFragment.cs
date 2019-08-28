using Android.Support.V4.App;
using Android.OS;
using Android.Views;
using DI.Shared.Managers;
using DI.Shared.DataAccess;

namespace DI.Droid.Fragments.Base
{
    public abstract class BaseFragment : Fragment
    {
        protected SQLDataManager sqliteManager;
        protected VMManager vmManager;
        protected View _partial;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            sqliteManager = SessionManager.DBConnection;
            vmManager = new VMManager();
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            _partial = inflater.Inflate(GetLayoutId(), container, false);

            return _partial;
        }

        protected abstract int GetLayoutId();

        protected View GetPartialView()
        {
            return _partial;
        }
    }
}
