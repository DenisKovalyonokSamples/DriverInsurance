using System;
using System.Threading.Tasks;
using UIKit;

namespace DI.iOS.Controllers.Base
{
    public abstract partial class BaseFormController : BaseController
    {
        protected UIActivityIndicatorView _progressBar;
        protected UIActivityIndicatorView _progressBarLoading;
        private UIButton _buttonSubmit;
        private UIButton _buttonCancel;
        protected bool IsDataLoaded { get; set; }

        public BaseFormController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            _buttonSubmit = GetSubmitButton();
            _buttonCancel = GetCancelButton();

            InitButtons();
            InitProgressBars();
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            if (!IsDataLoaded)
                LoadData();
        }

        protected virtual void InitButtons()
        {
            if (_buttonSubmit != null)
            {
                _buttonSubmit.TouchUpInside += async delegate
                {
                    ShowProgressBar();
                    if (await IsDataValidAsync())
                    {
                        await OnSubmitAsync();
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

                _buttonCancel.TouchUpInside += async delegate
                {
                    ShowProgressBar();

                    await OnCancelAsync();

                    HideProgressBar();
                };
            }
        }

        protected virtual void InitProgressBars()
        {
            _progressBar = GetProgressBar();
            _progressBarLoading = GetProgressBarLoading();
        }

        protected async void LoadData()
        {
            ShowLoadingBar();

            await LoadDataAsync();

            HideLoadingBar();

            IsDataLoaded = true;
        }

        protected virtual void ShowLoadingBar()
        {
            if (_progressBarLoading == null)
                return;

            _progressBarLoading.StartAnimating();
            _progressBarLoading.Hidden = false;
        }

        protected virtual void HideLoadingBar()
        {
            if (_progressBarLoading == null)
                return;

            _progressBarLoading.StopAnimating();
            _progressBarLoading.Hidden = true;

        }

        protected virtual void ShowProgressBar()
        {
            if (_progressBar == null)
                return;

            if (_buttonSubmit != null)
                _buttonSubmit.Hidden = true;

            if (_buttonCancel != null)
                _buttonCancel.Hidden = true;

            _progressBar.StartAnimating();
            _progressBar.Hidden = false;
        }

        protected virtual void HideProgressBar()
        {
            if (_progressBar == null)
                return;

            if (_buttonSubmit != null)
                _buttonSubmit.Hidden = false;

            if (_buttonCancel != null)
                _buttonCancel.Hidden = false;

            _progressBar.StopAnimating();
            _progressBar.Hidden = true;
        }

        #region Abstract

        protected abstract Task LoadDataAsync();

        protected abstract void OnDataNotValid();

        protected abstract Task OnSubmitAsync();

        protected abstract Task OnCancelAsync();

        protected abstract Task<bool> IsDataValidAsync();

        protected abstract UIButton GetSubmitButton();

        protected abstract UIButton GetCancelButton();

        protected abstract UIActivityIndicatorView GetProgressBar();

        protected abstract UIActivityIndicatorView GetProgressBarLoading();

        #endregion
    }
}