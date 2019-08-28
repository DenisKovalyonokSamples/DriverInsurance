using Android.App;
using Android.Widget;
using Android.OS;
using DI.Droid.Base;
using System.Linq;
using DI.Localization;
using Android.Content.PM;
using DI.Shared.ViewModels;
using System.Collections.Generic;
using DI.Droid.Adapters;
using Android.Support.Design.Widget;
using System.Threading.Tasks;
using Android.Views;
using DI.Shared.Interfaces;
using Android.Content;
using DI.Droid.Fragments;
using DI.Shared.DataAccess;
using DI.Shared.Managers;

namespace DI.Droid
{
    [Activity(Label = "DI", ScreenOrientation = ScreenOrientation.Portrait, LaunchMode = LaunchMode.SingleTask, Theme = "@style/MasterLayoutTheme", ConfigurationChanges = (ConfigChanges.KeyboardHidden | ConfigChanges.Orientation | ConfigChanges.ScreenSize | ConfigChanges.Locale))]
    public class EventsActivity : BaseListActivity
    {
        List<EventViewModel> values = new List<EventViewModel>();

        MenuFragment menuFragment;
        TextView textInsuranceBusinessesTitle;
        TextView textDeclareNewEventTitle;
        RelativeLayout layoutCreateNewEvent;
        LinearLayout layoutTableContainer;

        RelativeLayout layoutLocalCall;
        TextView localCallDescription;
        TextView localCallNumber;

        TextView textTitleOne;
        TextView textDescriptionOne;
        TextView textDescriptionTwo;
        TextView textDescriptionThree;
        TextView textDescriptionFour;
        TextView textDescriptionFive;
        TextView textDescriptionSix;

        TextView textTitleTwo;
        TextView textDescriptionSeven;
        TextView textDescriptionEight;
        TextView textDescriptionNine;
        TextView textDescriptionTen;
        TextView textDescriptionEleven;
        TextView textDescriptionTwelve;
        TextView textDescriptionThirteen;
        TextView textDescriptionFourteen;
        TextView textDescriptionFifteen;
        TextView textDescriptioSixteen;

        LinearLayout layoutDescriptionContainer;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.Events);
            SetTitleBack();

            layoutLocalCall = FindViewById<RelativeLayout>(Resource.Id.layoutLocalCall);
            localCallDescription = FindViewById<TextView>(Resource.Id.localCallDescription);
            localCallNumber = FindViewById<TextView>(Resource.Id.localCallNumber);

            textTitleOne = FindViewById<TextView>(Resource.Id.textTitleOne);
            textDescriptionOne = FindViewById<TextView>(Resource.Id.textDescriptionOne);
            textDescriptionTwo = FindViewById<TextView>(Resource.Id.textDescriptionTwo);
            textDescriptionThree = FindViewById<TextView>(Resource.Id.textDescriptionThree);
            textDescriptionFour = FindViewById<TextView>(Resource.Id.textDescriptionFour);
            textDescriptionFive = FindViewById<TextView>(Resource.Id.textDescriptionFive);
            textDescriptionSix = FindViewById<TextView>(Resource.Id.textDescriptionSix);

            textTitleTwo = FindViewById<TextView>(Resource.Id.textTitleTwo);
            textDescriptionSeven = FindViewById<TextView>(Resource.Id.textDescriptionSeven);
            textDescriptionEight = FindViewById<TextView>(Resource.Id.textDescriptionEight);
            textDescriptionNine = FindViewById<TextView>(Resource.Id.textDescriptionNine);
            textDescriptionTen = FindViewById<TextView>(Resource.Id.textDescriptionTen);
            textDescriptionEleven = FindViewById<TextView>(Resource.Id.textDescriptionEleven);
            textDescriptionTwelve = FindViewById<TextView>(Resource.Id.textDescriptionTwelve);
            textDescriptionThirteen = FindViewById<TextView>(Resource.Id.textDescriptionThirteen);
            textDescriptionFourteen = FindViewById<TextView>(Resource.Id.textDescriptionFourteen);
            textDescriptionFifteen = FindViewById<TextView>(Resource.Id.textDescriptionFifteen);
            textDescriptioSixteen = FindViewById<TextView>(Resource.Id.textDescriptioSixteen);

            textInsuranceBusinessesTitle = FindViewById<TextView>(Resource.Id.textInsuranceBusinessesTitle);
            textDeclareNewEventTitle = FindViewById<TextView>(Resource.Id.textDeclareNewEventTitle);
            layoutCreateNewEvent = FindViewById<RelativeLayout>(Resource.Id.layoutCreateNewEvent);
            layoutTableContainer = FindViewById<LinearLayout>(Resource.Id.layoutTableContainer);
            layoutDescriptionContainer = FindViewById<LinearLayout>(Resource.Id.layoutDescriptionContainer);

            menuFragment = new MenuFragment();
            if (!this.IsFinishing)
            {
                var partialMenuSetup = SupportFragmentManager.BeginTransaction();
                partialMenuSetup.Add(Resource.Id.fragmentMenu, menuFragment, "MenuFragment");
                partialMenuSetup.CommitAllowingStateLoss();
            }

            InitControls();
        }

        void InitControls()
        {
            base.InitControls();

            localCallDescription.Text = AppResources.ContactCenterPhone;
            localCallNumber.Text = "8 (800)";

            textTitleOne.Text = AppResources.WhatToDoInAccident;
            textDescriptionOne.Text = AppResources.ImmediatelyStopTheVehicle;
            textDescriptionTwo.Text = AppResources.MarkTheEmergencyStopSign;
            textDescriptionThree.Text = AppResources.ContactEmergencyService;
            textDescriptionFour.Text = AppResources.FixWithTheHelpPhotoAndVideo;
            textDescriptionFive.Text = AppResources.FreeTheRoadway;
            textDescriptionSix.Text = AppResources.CallTheContactCenter;

            textTitleTwo.Text = AppResources.HowToReceiveInsuranceCompensation;
            textDescriptionSeven.Text = AppResources.WithinCalendarDays;
            textDescriptionEight.Text = AppResources.PresentDamagedVehicle;
            textDescriptionNine.Text = AppResources.PrepareAndSend;
            textDescriptionTen.Text = AppResources.NotificationInsuredEvent;
            textDescriptionEleven.Text = AppResources.CertificateOnForm;
            textDescriptionTwelve.Text = AppResources.TheOriginalCertified;
            textDescriptionThirteen.Text = AppResources.AdministrativeOffense;
            textDescriptionFourteen.Text = AppResources.RegistrationDocumentsTS;
            textDescriptionFifteen.Text = AppResources.DriverLicense;
            textDescriptioSixteen.Text = AppResources.DocumentProvingIdentity;

            textInsuranceBusinessesTitle.Text = AppResources.InsuranceBusinesses.ToUpper();
            textDeclareNewEventTitle.Text = AppResources.DeclareANewEvent;
            GetEmptyListTextView().Text = AppResources.NoGridItems;
            GetEmptyListTextView().Visibility = ViewStates.Gone;
            layoutCreateNewEvent.Visibility = ViewStates.Gone;

            adapter = new EventRVAdapter(recyclerView.Context, values, Resources);
            recyclerView.SetAdapter(adapter);
            SetupGrid();

            if (SessionManager.СontractData != null)
            {
                layoutDescriptionContainer.Visibility = ViewStates.Gone;
            }
            else
            {
                layoutDescriptionContainer.Visibility = ViewStates.Gone;
            }

            SetupGestures();
        }

        void SetupGestures()
        {
            layoutLocalCall.Click += delegate {
                var uri = Android.Net.Uri.Parse("tel:+8");
                var intent = new Intent(Intent.ActionDial, uri);
                StartActivity(intent);
            };

            layoutCreateNewEvent.Click += delegate
            {
                var activity = new Intent(this, typeof(EventStatementActivity));
                StartActivity(activity);

                Finish();
            };
        }

        #region abstract

        protected override int GetStatusBarColor()
        {
            return Resource.Color.statusbar_blue;
        }

        protected override int GetActiveBarColor()
        {
            return Resource.Color.actionbar_blue;
        }

        protected override string GetTitle()
        {
            return AppResources.Events;
        }

        protected override FloatingActionButton GetFloatingActionButton()
        {
            return null;
        }

        protected override async Task UpdateDataAsync()
        {
            values.Clear();

            var contract = await APIDataManager.GetCurrentContract(SessionManager.СontractorData.Id.ToString());
            if (contract != null)
            {
                layoutCreateNewEvent.Visibility = ViewStates.Visible;

                var dictionaryItems = await APIDataManager.GetDictionaryItems("lossstatus");

                var losses = await APIDataManager.GetLosses(contract.Id.ToString());
                if (losses != null)
                {
                    foreach (var loss in losses)
                    {
                        var item = new EventViewModel();
                        item.Id = loss.Id;
                        item.Number = loss.Number;
                        item.Date = loss.IncidentDate.ToString("dd.MM.yyyy");

                        if (dictionaryItems != null && dictionaryItems.Count > 0)
                        {
                            var dictionaryItem = dictionaryItems.Where(e => e.Code == loss.Status).FirstOrDefault();
                            if (dictionaryItem != null)
                            {
                                item.Status = dictionaryItem.Value;
                            }
                            else
                            {
                                item.Status = string.Empty;
                            }
                        }

                        values.Add(item);
                    }
                }
            }

            if (values.Count == 0)
            {
                GetEmptyListTextView().Visibility = ViewStates.Visible;
            }
        }

        protected override void ItemClickedOn(int position)
        {
            if (values.Count == 0)
                return;
        }

        protected override ISelectable GetSelectedItem(int position)
        {
            return null;
        }

        protected override async Task AddNewAction()
        {
        }

        protected override async Task EditAction(ISelectable selectedItem)
        {
        }

        protected override async Task DeleteAction(ISelectable selectedItem)
        {
        }

        #endregion
    }
}
