using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using DI.Droid.Fragments.Base;
using DI.Localization;
using DI.Shared.Entities.SQL;
using DI.Shared.Enums;

namespace DI.Droid.Fragments
{
    public class PasscodeFragment : BaseFragment
    {
        ImageView imageViewPinOne;
        ImageView imageViewPinTwo;
        ImageView imageViewPinThree;
        ImageView imageViewPinFour;

        RelativeLayout layoutOne;
        RelativeLayout layoutTwo;
        RelativeLayout layoutThree;
        RelativeLayout layoutFour;
        RelativeLayout layoutFive;
        RelativeLayout layoutSix;
        RelativeLayout layoutSeven;
        RelativeLayout layoutEight;
        RelativeLayout layoutNine;
        RelativeLayout layoutZero;

        ImageView imageViewOne;
        ImageView imageViewTwo;
        ImageView imageViewThree;
        ImageView imageViewFour;
        ImageView imageViewFive;
        ImageView imageViewSix;
        ImageView imageViewSeven;
        ImageView imageViewEight;
        ImageView imageViewNine;
        ImageView imageViewZero;

        RelativeLayout layoutPinReset;
        TextView textPinReset;

        TextView textPinTitle;
        TextView textPinAdditionalTitle;

        int pinItemsFilled = 0;
        string pinCode = string.Empty;
        string pinCodeForConfirmation = string.Empty;

        User CurrentUser;

        public bool IsRegistration = false;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View partial = base.OnCreateView(inflater, container, savedInstanceState);

            imageViewPinOne = partial.FindViewById<ImageView>(Resource.Id.imageViewPinOne);
            imageViewPinTwo = partial.FindViewById<ImageView>(Resource.Id.imageViewPinTwo);
            imageViewPinThree = partial.FindViewById<ImageView>(Resource.Id.imageViewPinThree);
            imageViewPinFour = partial.FindViewById<ImageView>(Resource.Id.imageViewPinFour);

            layoutOne = partial.FindViewById<RelativeLayout>(Resource.Id.layoutOne);
            layoutTwo = partial.FindViewById<RelativeLayout>(Resource.Id.layoutTwo);
            layoutThree = partial.FindViewById<RelativeLayout>(Resource.Id.layoutThree);
            layoutFour = partial.FindViewById<RelativeLayout>(Resource.Id.layoutFour);
            layoutFive = partial.FindViewById<RelativeLayout>(Resource.Id.layoutFive);
            layoutSix = partial.FindViewById<RelativeLayout>(Resource.Id.layoutSix);
            layoutSeven = partial.FindViewById<RelativeLayout>(Resource.Id.layoutSeven);
            layoutEight = partial.FindViewById<RelativeLayout>(Resource.Id.layoutEight);
            layoutNine = partial.FindViewById<RelativeLayout>(Resource.Id.layoutNine);
            layoutZero = partial.FindViewById<RelativeLayout>(Resource.Id.layoutZero);

            imageViewOne = partial.FindViewById<ImageView>(Resource.Id.imageViewOne);
            imageViewTwo = partial.FindViewById<ImageView>(Resource.Id.imageViewTwo);
            imageViewThree = partial.FindViewById<ImageView>(Resource.Id.imageViewThree);
            imageViewFour = partial.FindViewById<ImageView>(Resource.Id.imageViewFour);
            imageViewFive = partial.FindViewById<ImageView>(Resource.Id.imageViewFive);
            imageViewSix = partial.FindViewById<ImageView>(Resource.Id.imageViewSix);
            imageViewSeven = partial.FindViewById<ImageView>(Resource.Id.imageViewSeven);
            imageViewEight = partial.FindViewById<ImageView>(Resource.Id.imageViewEight);
            imageViewNine = partial.FindViewById<ImageView>(Resource.Id.imageViewNine);
            imageViewZero = partial.FindViewById<ImageView>(Resource.Id.imageViewZero);

            layoutPinReset = partial.FindViewById<RelativeLayout>(Resource.Id.layoutPinReset);

            textPinReset = partial.FindViewById<TextView>(Resource.Id.textPinReset);
            textPinTitle = partial.FindViewById<TextView>(Resource.Id.textPinTitle);
            textPinAdditionalTitle = partial.FindViewById<TextView>(Resource.Id.textPinAdditionalTitle);

            InitControls();

            return partial;
        }

        protected void InitControls()
        {
            SetupGestures();

            textPinTitle.Text = AppResources.EnterPINCode.ToUpper();

            if (IsRegistration)
            {
                textPinAdditionalTitle.Text = string.Empty;
            }
            else
            {
                CurrentUser = sqliteManager.GetUser();

                if (CurrentUser.PINAttemptsCount == 3)
                {
                    textPinAdditionalTitle.Text = string.Empty;
                }
                if (CurrentUser.PINAttemptsCount == 2)
                {
                    textPinAdditionalTitle.Text = AppResources.TwoMoreAttemptsLeft.ToUpper();
                }
                if (CurrentUser.PINAttemptsCount == 1)
                {
                    textPinAdditionalTitle.Text = AppResources.OneMoreAttemptLeft.ToUpper();
                }
            }
        }

        void SetupGestures()
        {
            layoutOne.Touch += layoutOne_OnTouch;
            layoutTwo.Touch += layoutTwo_OnTouch;
            layoutThree.Touch += layoutThree_OnTouch;
            layoutFour.Touch += layoutFour_OnTouch;
            layoutFive.Touch += layoutFive_OnTouch;
            layoutSix.Touch += layoutSix_OnTouch;
            layoutSeven.Touch += layoutSeven_OnTouch;
            layoutEight.Touch += layoutEight_OnTouch;
            layoutNine.Touch += layoutNine_OnTouch;
            layoutZero.Touch += layoutZero_OnTouch;

            layoutPinReset.Touch += layoutPinReset_OnTouch;
            textPinReset.Click += delegate
            {
                ClearPinValue();
            };
        }

        #region Touch Events

        void layoutOne_OnTouch(object sender, View.TouchEventArgs touchEventArgs)
        {
            switch (touchEventArgs.Event.Action)
            {
                case MotionEventActions.Down:
                    imageViewOne.SetImageResource(Resource.Mipmap.oval);
                    SetPinValue(1);
                    break;
                case MotionEventActions.Move:
                    imageViewOne.SetImageResource(Resource.Mipmap.oval);
                    break;
                case MotionEventActions.Up:
                    imageViewOne.SetImageResource(Resource.Mipmap.oval_empty);
                    break;
                default:
                    break;
            }
        }

        void layoutTwo_OnTouch(object sender, View.TouchEventArgs touchEventArgs)
        {
            switch (touchEventArgs.Event.Action)
            {
                case MotionEventActions.Down:
                    imageViewTwo.SetImageResource(Resource.Mipmap.oval);
                    SetPinValue(2);
                    break;
                case MotionEventActions.Move:
                    imageViewTwo.SetImageResource(Resource.Mipmap.oval);
                    break;
                case MotionEventActions.Up:
                    imageViewTwo.SetImageResource(Resource.Mipmap.oval_empty);
                    break;
                default:
                    break;
            }
        }

        void layoutThree_OnTouch(object sender, View.TouchEventArgs touchEventArgs)
        {
            switch (touchEventArgs.Event.Action)
            {
                case MotionEventActions.Down:
                    imageViewThree.SetImageResource(Resource.Mipmap.oval);
                    SetPinValue(3);
                    break;
                case MotionEventActions.Move:
                    imageViewThree.SetImageResource(Resource.Mipmap.oval);
                    break;
                case MotionEventActions.Up:
                    imageViewThree.SetImageResource(Resource.Mipmap.oval_empty);
                    break;
                default:
                    break;
            }
        }

        void layoutFour_OnTouch(object sender, View.TouchEventArgs touchEventArgs)
        {
            switch (touchEventArgs.Event.Action)
            {
                case MotionEventActions.Down:
                    imageViewFour.SetImageResource(Resource.Mipmap.oval);
                    SetPinValue(4);
                    break;
                case MotionEventActions.Move:
                    imageViewFour.SetImageResource(Resource.Mipmap.oval);
                    break;
                case MotionEventActions.Up:
                    imageViewFour.SetImageResource(Resource.Mipmap.oval_empty);
                    break;
                default:
                    break;
            }
        }

        void layoutFive_OnTouch(object sender, View.TouchEventArgs touchEventArgs)
        {
            switch (touchEventArgs.Event.Action)
            {
                case MotionEventActions.Down:
                    imageViewFive.SetImageResource(Resource.Mipmap.oval);
                    SetPinValue(5);
                    break;
                case MotionEventActions.Move:
                    imageViewFive.SetImageResource(Resource.Mipmap.oval);
                    break;
                case MotionEventActions.Up:
                    imageViewFive.SetImageResource(Resource.Mipmap.oval_empty);
                    break;
                default:
                    break;
            }
        }

        void layoutSix_OnTouch(object sender, View.TouchEventArgs touchEventArgs)
        {
            switch (touchEventArgs.Event.Action)
            {
                case MotionEventActions.Down:
                    imageViewSix.SetImageResource(Resource.Mipmap.oval);
                    SetPinValue(6);
                    break;
                case MotionEventActions.Move:
                    imageViewSix.SetImageResource(Resource.Mipmap.oval);
                    break;
                case MotionEventActions.Up:
                    imageViewSix.SetImageResource(Resource.Mipmap.oval_empty);
                    break;
                default:
                    break;
            }
        }

        void layoutSeven_OnTouch(object sender, View.TouchEventArgs touchEventArgs)
        {
            switch (touchEventArgs.Event.Action)
            {
                case MotionEventActions.Down:
                    imageViewSeven.SetImageResource(Resource.Mipmap.oval);
                    SetPinValue(7);
                    break;
                case MotionEventActions.Move:
                    imageViewSeven.SetImageResource(Resource.Mipmap.oval);
                    break;
                case MotionEventActions.Up:
                    imageViewSeven.SetImageResource(Resource.Mipmap.oval_empty);
                    break;
                default:
                    break;
            }
        }

        void layoutEight_OnTouch(object sender, View.TouchEventArgs touchEventArgs)
        {
            switch (touchEventArgs.Event.Action)
            {
                case MotionEventActions.Down:
                    imageViewEight.SetImageResource(Resource.Mipmap.oval);
                    SetPinValue(1);
                    break;
                case MotionEventActions.Move:
                    imageViewEight.SetImageResource(Resource.Mipmap.oval);
                    break;
                case MotionEventActions.Up:
                    imageViewEight.SetImageResource(Resource.Mipmap.oval_empty);
                    break;
                default:
                    break;
            }
        }

        void layoutNine_OnTouch(object sender, View.TouchEventArgs touchEventArgs)
        {
            switch (touchEventArgs.Event.Action)
            {
                case MotionEventActions.Down:
                    imageViewNine.SetImageResource(Resource.Mipmap.oval);
                    SetPinValue(1);
                    break;
                case MotionEventActions.Move:
                    imageViewNine.SetImageResource(Resource.Mipmap.oval);
                    break;
                case MotionEventActions.Up:
                    imageViewNine.SetImageResource(Resource.Mipmap.oval_empty);
                    break;
                default:
                    break;
            }
        }

        void layoutZero_OnTouch(object sender, View.TouchEventArgs touchEventArgs)
        {
            switch (touchEventArgs.Event.Action)
            {
                case MotionEventActions.Down:
                    imageViewZero.SetImageResource(Resource.Mipmap.oval);
                    SetPinValue(1);
                    break;
                case MotionEventActions.Move:
                    imageViewZero.SetImageResource(Resource.Mipmap.oval);
                    break;
                case MotionEventActions.Up:
                    imageViewZero.SetImageResource(Resource.Mipmap.oval_empty);
                    break;
                default:
                    break;
            }
        }

        void layoutPinReset_OnTouch(object sender, View.TouchEventArgs touchEventArgs)
        {
            switch (touchEventArgs.Event.Action)
            {
                case MotionEventActions.Down:
                    ClearPinValue();
                    break;
                default:
                    break;
            }
        }

        #endregion

        void SetPinValue(int value)
        {
            if (pinItemsFilled == 0)
            {
                imageViewPinOne.SetImageResource(Resource.Mipmap.pass_item_filled);
            }
            if (pinItemsFilled == 1)
            {
                imageViewPinTwo.SetImageResource(Resource.Mipmap.pass_item_filled);
            }
            if (pinItemsFilled == 2)
            {
                imageViewPinThree.SetImageResource(Resource.Mipmap.pass_item_filled);
            }
            if (pinItemsFilled == 3)
            {
                imageViewPinFour.SetImageResource(Resource.Mipmap.pass_item_filled);
            }

            pinItemsFilled++;
            pinCode += value.ToString();

            if (pinItemsFilled == 4)
            {
                if (IsRegistration)
                {
                    if (pinCodeForConfirmation == string.Empty)
                    {
                        pinCodeForConfirmation = pinCode;
                        ClearPinValue();
                        textPinTitle.Text = AppResources.ConfirmPINCode.ToUpper();
                        textPinAdditionalTitle.Text = string.Empty;
                    }
                    else
                    {
                        if (pinCodeForConfirmation == pinCode)
                        {
                            (this.Activity as CreatePinCodeActivity).ProceedPINCode(pinCode);
                        }
                        else
                        {
                            textPinTitle.Text = AppResources.IncorrectPINCode.ToUpper();
                            textPinAdditionalTitle.Text = AppResources.TryAgain.ToUpper();

                            ClearPinValue();
                            pinCodeForConfirmation = string.Empty;
                        }
                    }
                }
                else
                {
                    if (sqliteManager.Login(pinCode) == ResultCode.Success)
                    {
                    }
                    else
                    {
                        CurrentUser = sqliteManager.GetUser();

                        if (CurrentUser != null)
                        {
                            if (CurrentUser.PINAttemptsCount == 2)
                            {
                                textPinAdditionalTitle.Text = AppResources.TwoMoreAttemptsLeft.ToUpper();
                                ClearPinValue();
                            }
                            if (CurrentUser.PINAttemptsCount == 1)
                            {
                                textPinAdditionalTitle.Text = AppResources.OneMoreAttemptLeft.ToUpper();
                                ClearPinValue();
                            }
                        }
                        else
                        {
                            var activity = new Intent(this.Activity, typeof(RegisterPhoneActivity));
                            StartActivity(activity);

                            this.Activity.Finish();
                        }
                    }
                }
            }
        }

        void ClearPinValue()
        {
            imageViewPinOne.SetImageResource(Resource.Mipmap.pass_item_empty);
            imageViewPinTwo.SetImageResource(Resource.Mipmap.pass_item_empty);
            imageViewPinThree.SetImageResource(Resource.Mipmap.pass_item_empty);
            imageViewPinFour.SetImageResource(Resource.Mipmap.pass_item_empty);

            pinItemsFilled = 0;
            pinCode = string.Empty;
        }

        #region abstract

        protected override int GetLayoutId()
        {
            return Resource.Layout.Passcode;
        }

        #endregion
    }
}
