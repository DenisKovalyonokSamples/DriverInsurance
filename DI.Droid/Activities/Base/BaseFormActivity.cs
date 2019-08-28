using Android.OS;
using Android.Views;
using Android.Widget;
using System.Threading.Tasks;

namespace DI.Droid.Base
{
    public abstract class BaseFormActivity : BaseActivity
    {
        protected ProgressBar _progressBar;
        private Button _buttonSubmit;
        private Button _buttonCancel;
        protected bool IsDataLoaded { get; set; }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        protected override void InitControls()
        {
            base.InitControls();

            InitProgressBars();

            _buttonSubmit = GetSubmitButton();
            _buttonCancel = GetCancelButton();
            InitSubmitButton();
        }

        protected virtual void InitSubmitButton()
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

                    HideProgressBar();
                };
            }
        }

        protected void InitProgressBars()
        {
            _progressBar = FindViewById<ProgressBar>(Resource.Id.progressBar);
        }

        protected async void LoadData()
        {
            await LoadDataAsync();

            IsDataLoaded = true;
        }

        protected override void OnResume()
        {
            base.OnResume();

            if (!IsDataLoaded)
                LoadData();
        }

        protected virtual void ShowProgressBar()
        {
            if (_buttonSubmit != null)
                _buttonSubmit.Visibility = ViewStates.Gone;

            if (_buttonCancel != null)
                _buttonCancel.Visibility = ViewStates.Gone;

            if (_progressBar == null)
                return;

            _progressBar.Indeterminate = true;
            _progressBar.Visibility = ViewStates.Visible;
        }

        protected virtual void HideProgressBar()
        {

            if (_buttonSubmit != null)
                _buttonSubmit.Visibility = ViewStates.Visible;

            if (_buttonCancel != null)
                _buttonCancel.Visibility = ViewStates.Visible;

            if (_progressBar == null)
                return;

            _progressBar.Visibility = ViewStates.Gone;
        }

        #region abstract

        protected abstract Task LoadDataAsync();

        protected abstract void OnDataNotValid();

        protected abstract Task OnSubmitAsync();

        protected abstract Task OnCancelAsync();

        protected abstract Task<bool> IsDataValidAsync();

        protected abstract Button GetSubmitButton();

        protected abstract Button GetCancelButton();

        #endregion
    }
}
