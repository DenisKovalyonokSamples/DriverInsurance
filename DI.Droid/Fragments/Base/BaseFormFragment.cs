using Android.OS;
using Android.Views;
using Android.Widget;
using System.Threading.Tasks;

namespace DI.Droid.Fragments.Base
{
    public abstract class BaseFormFragment : BaseFragment
    {
        protected ProgressBar _progressBar;
        protected Button _buttonSubmit;
        protected Button _buttonCancel;
        protected bool IsDataLoaded { get; set; }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

        }
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View partial = base.OnCreateView(inflater, container, savedInstanceState);
            _progressBar = partial.FindViewById<ProgressBar>(Resource.Id.progressBar);
            _progressBarLoading = partial.FindViewById<ProgressBar>(Resource.Id.progressBarLoading);

            _buttonSubmit = GetSubmitButton();
            _buttonCancel = GetCancelButton();


            return partial;
        }

        protected virtual void InitControls()
        {
            InitButtons();
        }

        protected virtual void InitButtons()
        {
            if (_buttonSubmit != null)
            {
                _buttonSubmit.Click += async delegate
                {
                    ShowProgressBar();
                    if (await IsDataValidAsync())
                    {
                        await OnSubmitAsync();
                        return;
                    }
                    else
                    {
                        OnDataNotValid();
                    }
                    HideProgressBar();
                };

            }

            if (_buttonCancel != null)
            {
                _buttonCancel.Click += async delegate
                {
                    ShowProgressBar();

                    await OnCancelAsync();
                };

            }
        }

        protected virtual void ShowProgressBar()
        {
            if (_buttonSubmit != null)
                _buttonSubmit.Visibility = ViewStates.Gone;

            if (_progressBar == null)
                return;

            _progressBar.Indeterminate = true;
            _progressBar.Visibility = ViewStates.Visible;
        }

        protected virtual void HideProgressBar()
        {

            if (_buttonSubmit != null)
                _buttonSubmit.Visibility = ViewStates.Visible;

            if (_progressBar == null)
                return;

            _progressBar.Visibility = ViewStates.Gone;
        }

        #region Progress Bar Loading

        protected ProgressBar _progressBarLoading;
        protected bool loadingFinished = false;

        protected virtual void ShowLoadingBar()
        {
            if (_progressBarLoading == null)
                return;

            _progressBarLoading.Indeterminate = true;
            _progressBarLoading.Visibility = ViewStates.Visible;
        }

        protected bool DontHideProgressBar = false;
        protected virtual void HideLoadingBar()
        {
            if (_progressBarLoading == null)
                return;

            if (!DontHideProgressBar)
            {
                _progressBarLoading.Visibility = ViewStates.Gone;
            }
        }

        #endregion

        protected async void LoadData()
        {
            ShowLoadingBar();

            await LoadDataAsync();

            HideLoadingBar();

            IsDataLoaded = true;
        }

        public override void OnResume()
        {
            base.OnResume();

            LoadData();
        }

        #region abstract

        protected abstract Task LoadDataAsync();

        protected abstract Task OnCancelAsync();

        protected abstract Task OnSubmitAsync();

        protected abstract void OnDataNotValid();

        protected abstract Task<bool> IsDataValidAsync();

        protected abstract Button GetSubmitButton();

        protected abstract Button GetCancelButton();

        #endregion
    }
}
