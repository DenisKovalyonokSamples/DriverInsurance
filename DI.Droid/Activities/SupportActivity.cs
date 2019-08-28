using Android.App;
using Android.Widget;
using Android.OS;
using DI.Droid.Base;
using DI.Localization;
using Android.Content.PM;
using System.Collections.Generic;
using DI.Shared.Entities.Smooch;
using DI.Droid.Adapters;
using Android.Support.Design.Widget;
using System.Threading.Tasks;
using DI.Shared.DataAccess;
using System;
using System.Linq;
using DI.Shared.Interfaces;
using DI.Shared.Entities.SQL;
using Android.Views.InputMethods;
using Android.Graphics;
using Android.Support.V4.Content;
using DI.Shared.Managers;
using DI.Droid.Services;
using Android.Views;
using static Android.Views.ViewGroup;
using Android.Content;
using Android.Database;
using Android.Provider;
using System.IO;
using DI.Shared;

namespace DI.Droid
{
    [Activity(Label = "DI", ScreenOrientation = ScreenOrientation.Portrait, LaunchMode = LaunchMode.SingleTask, Theme = "@style/MasterLayoutTheme", ConfigurationChanges = (ConfigChanges.KeyboardHidden | ConfigChanges.Orientation | ConfigChanges.ScreenSize | ConfigChanges.Locale))]
    public class SupportActivity : BaseListActivity
    {
        SmoochUserData User;
        List<Shared.Entities.Smooch.Message> values = new List<Shared.Entities.Smooch.Message>();

        RelativeLayout layoutAddAttachment;
        RelativeLayout layoutSend;
        ImageView imageViewSend;
        EditText editMessage;

        LinearLayout layoutTableContainer;
        LinearLayout layoutControlPanel;
        LinearLayout layoutAttachmentPanel;
        LinearLayout separatorControlPanel;

        RelativeLayout layoutDocs;
        RelativeLayout layoutImages;
        RelativeLayout layoutCamera;

        bool IsAttachmentShown;
        bool LockTableUpdates = false;

        int PictureFromGallery = 122;
        int FileFromGallery = 123;
        int PictureFromCamera = 124;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.Support);
            SetTitleBack();

            layoutAddAttachment = FindViewById<RelativeLayout>(Resource.Id.layoutAddAttachment);
            layoutSend = FindViewById<RelativeLayout>(Resource.Id.layoutSend);
            imageViewSend = FindViewById<ImageView>(Resource.Id.imageViewSend);
            editMessage = FindViewById<EditText>(Resource.Id.editMessage);

            layoutTableContainer = FindViewById<LinearLayout>(Resource.Id.layoutTableContainer);
            layoutControlPanel = FindViewById<LinearLayout>(Resource.Id.layoutControlPanel);
            layoutAttachmentPanel = FindViewById<LinearLayout>(Resource.Id.layoutAttachmentPanel);
            separatorControlPanel = FindViewById<LinearLayout>(Resource.Id.separatorControlPanel);

            layoutDocs = FindViewById<RelativeLayout>(Resource.Id.layoutDocs);
            layoutImages = FindViewById<RelativeLayout>(Resource.Id.layoutImages);
            layoutCamera = FindViewById<RelativeLayout>(Resource.Id.layoutCamera);

            StrictMode.VmPolicy.Builder builder = new StrictMode.VmPolicy.Builder();
            StrictMode.SetVmPolicy(builder.Build());

            InitControls();
        }

        void InitControls()
        {
            base.InitControls();

            HideAttachmentsSegment();

            editMessage.Hint = AppResources.EnterMessage;

            adapter = new SupportRVAdapter(recyclerView.Context, values, Resources);
            recyclerView.SetAdapter(adapter);

            SetupGrid();

            SetupGestures();

            SynchronizationService.SyncSmoochMessages += MessagesUpdated;
            SynchronizationService.SyncSmoochNoMessages += NoMessages;

            if (AppWrapper.Service != null)
            {
                AppWrapper.Service.DataSyncHandler(null);
                AppWrapper.Service.SetPeriodForChatUpdating(5);
            }

            CheckPermissions();
        }

        void CheckPermissions()
        {
            if (ContextCompat.CheckSelfPermission(this, Android.Manifest.Permission.Camera) != Permission.Granted
                || ContextCompat.CheckSelfPermission(this, Android.Manifest.Permission.WriteExternalStorage) != Permission.Granted
                || ContextCompat.CheckSelfPermission(this, Android.Manifest.Permission.ReadExternalStorage) != Permission.Granted)
            {
                Android.Support.V4.App.ActivityCompat.RequestPermissions(this, new System.String[] { Android.Manifest.Permission.Camera, Android.Manifest.Permission.WriteExternalStorage, Android.Manifest.Permission.ReadExternalStorage }, 1);
            }
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Android.Content.PM.Permission[] grantResults)
        {
            if (CheckCallingOrSelfPermission(Android.Manifest.Permission.Camera) != Permission.Granted)
            {
            }
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                default:
                    if (AppWrapper.Service != null)
                    {
                        AppWrapper.Service.SetPeriodForChatUpdating(Constants.SMOOCH_SYNC_PERIOD);
                    }
                    return base.OnOptionsItemSelected(item);
            }
        }

        void HideAttachmentsSegment()
        {
            layoutAttachmentPanel.Visibility = ViewStates.Gone;
            separatorControlPanel.Visibility = ViewStates.Gone;
            layoutControlPanel.LayoutParameters.Height = this.Resources.GetDimensionPixelSize(Resource.Dimension.chat_hidden_attachment_height);

            ViewGroup.LayoutParams hidenAttachmentsForTableParameters = ((ViewGroup)layoutTableContainer).LayoutParameters;
            ((MarginLayoutParams)hidenAttachmentsForTableParameters).BottomMargin = this.Resources.GetDimensionPixelSize(Resource.Dimension.chat_hidden_attachment_height);

            IsAttachmentShown = false;
        }

        void ShowAttachmentsSegment()
        {
            ViewGroup.LayoutParams hidenAttachmentsForTableParameters = ((ViewGroup)layoutTableContainer).LayoutParameters;
            ((MarginLayoutParams)hidenAttachmentsForTableParameters).BottomMargin = this.Resources.GetDimensionPixelSize(Resource.Dimension.chat_attachment_height);

            layoutControlPanel.LayoutParameters.Height = this.Resources.GetDimensionPixelSize(Resource.Dimension.chat_attachment_height);
            layoutAttachmentPanel.Visibility = ViewStates.Visible;
            separatorControlPanel.Visibility = ViewStates.Visible;

            IsAttachmentShown = true;
        }

        void SetupGestures()
        {
            layoutAddAttachment.Click += delegate
            {
                if (IsAttachmentShown)
                {
                    HideAttachmentsSegment();
                }
                else
                {
                    ShowAttachmentsSegment();
                    recyclerView.ScrollToPosition(values.Count - 1);
                }
            };

            layoutDocs.Click += delegate
            {
                HideAttachmentsSegment();
                OpenFileGallery();
            };
            layoutImages.Click += delegate
            {
                HideAttachmentsSegment();
                OpenImageGallery();
            };
            layoutCamera.Click += delegate
            {
                HideAttachmentsSegment();
                OpenCamera();
            };

            layoutSend.Click += async delegate
            {
                if (!string.IsNullOrWhiteSpace(editMessage.Text))
                {
                    imageViewSend.SetImageResource(Resource.Mipmap.chat_send_blocked);
                    layoutSend.Enabled = false;

                    await SendMessage(editMessage.Text);

                    layoutSend.Enabled = true;
                    imageViewSend.SetImageResource(Resource.Mipmap.chat_send);
                }
            };
        }

        async Task SendMessage(string value)
        {
            if (User != null)
            {
                InputMethodManager imm = (InputMethodManager)GetSystemService(Activity.InputMethodService);
                imm.HideSoftInputFromWindow(editMessage.WindowToken, 0);

                var messageModel = new MessageModel();
                messageModel.Text = value;
                messageModel.Role = "appUser";
                messageModel.Type = "text";
                editMessage.Text = string.Empty;
                var message = await SmoochManager.SendMessage(User.UserId, messageModel);

                LockTableUpdates = false;

                var data = await SmoochManager.GetMessages(User.UserId, DateTimeOffset.UtcNow.AddMinutes(1).ToUnixTimeSeconds().ToString());
                if (data != null && data.Messages != null && data.Messages.Count > 0)
                {
                    if (AppWrapper.Service.Messages == null)
                    {
                        AppWrapper.Service.Messages = new MessagesResponse();
                        AppWrapper.Service.Messages.Messages = new List<Shared.Entities.Smooch.Message>();
                    }

                    AppWrapper.Service.Messages.Messages = data.Messages;
                    FillValues(data);

                    adapter.NotifyDataSetChanged();
                    recyclerView.ScrollToPosition(values.Count - 1);
                }
            }
        }

        async Task ProceedFileInChat(string filename, byte[] filedata)
        {
            if (AppWrapper.Service.Messages != null && AppWrapper.Service.Messages.Messages.Count > 0)
            {
            }
            else
            {
                AppWrapper.Service.Messages = new MessagesResponse();
                AppWrapper.Service.Messages.Messages = new List<Shared.Entities.Smooch.Message>();
            }

            var messageLoading = new Shared.Entities.Smooch.Message();
            messageLoading.Role = "LocalFileForUpload";

            string contentType = string.Empty;
            if (filename.Contains(".jpg"))
            {
                contentType = "image/jpg";
                if (!DataManager.IsASCII(filename))
                {
                    messageLoading.Name = String.Format("attachment_{0}.jpg", Guid.NewGuid());
                    filename = messageLoading.Name;
                }
            }
            if (filename.Contains(".jpeg"))
            {
                contentType = "image/jpeg";
                if (!DataManager.IsASCII(filename))
                {
                    messageLoading.Name = String.Format("attachment_{0}.jpeg", Guid.NewGuid());
                    filename = messageLoading.Name;
                }
            }
            if (filename.Contains(".png"))
            {
                contentType = "image/png";
                if (!DataManager.IsASCII(filename))
                {
                    messageLoading.Name = String.Format("attachment_{0}.png", Guid.NewGuid());
                    filename = messageLoading.Name;
                }
            }
            if (filename.Contains(".pdf"))
            {
                contentType = "application/pdf";
                if (!DataManager.IsASCII(filename))
                {
                    messageLoading.Name = String.Format("file_{0}.pdf", Guid.NewGuid());
                    filename = messageLoading.Name;
                }
            }
            if (filename.Contains(".doc"))
            {
                contentType = "application/msword";
                if (!DataManager.IsASCII(filename))
                {
                    messageLoading.Name = String.Format("file_{0}.doc", Guid.NewGuid());
                    filename = messageLoading.Name;
                }
            }
            if (filename.Contains(".docx"))
            {
                contentType = "application/msword";
                if (!DataManager.IsASCII(filename))
                {
                    messageLoading.Name = String.Format("file_{0}.docx", Guid.NewGuid());
                    filename = messageLoading.Name;
                }
            }

            messageLoading.Received = DataManager.DateTimeToUnixTimestamp(DateTime.UtcNow);
            AppWrapper.Service.Messages.Messages.Add(messageLoading);

            FillValues(AppWrapper.Service.Messages);

            adapter.NotifyDataSetChanged();
            recyclerView.ScrollToPosition(values.Count - 1);

            LockTableUpdates = true;

            var result = await APIDataManager.UploadAttachment(filename, contentType, filedata);
            if (result != null && result.Message != null && result.Message.Count > 0)
            {
                var message = result.Message.FirstOrDefault();

                if (!string.IsNullOrWhiteSpace(message.Value))
                {
                    await SendMessage(message.Value);

                    foreach (var item in AppWrapper.Service.Messages.Messages)
                    {
                        if (item.Role == "LocalFileForUpload")
                        {
                            item.Role = "appUser";
                        }
                    }
                }
            }
        }

        void MessagesUpdated()
        {
            if (AppWrapper.Service != null)
            {
                if (AppWrapper.Service.Messages != null && AppWrapper.Service.Messages.Messages.Count > 0)
                {
                    FillValues(AppWrapper.Service.Messages);

                    this.RunOnUiThread(() =>
                    {
                        if (DontHideProgressBar)
                        {
                            DontHideProgressBar = false;
                            HideLoadingBar();
                        }

                        adapter.NotifyDataSetChanged();
                        recyclerView.ScrollToPosition(values.Count - 1);
                    });
                }
            }
        }

        void NoMessages()
        {
            this.RunOnUiThread(() =>
            {
                if (DontHideProgressBar)
                {
                    DontHideProgressBar = false;
                    HideLoadingBar();
                }
            });
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            if (requestCode == FileFromGallery || requestCode == PictureFromGallery)
            {
                if ((resultCode == Result.Ok) && (data != null))
                {
                    try
                    {
                        Android.Net.Uri uri = data.Data;
                        string name = GetFileName(uri);
                        int size = GetFileSize(uri);
                        byte[] fileData = ReadFileData(uri);

                        if (fileData != null)
                        {
                            if (name.Contains(".jpg") || name.Contains(".jpeg") || name.Contains(".png")
                                || name.Contains(".doc") || name.Contains(".docx") || name.Contains(".pdf"))
                            {
                                ProceedFileInChat(name, fileData);
                            }
                            else
                            {
                                ShowWrongFormatDialog(AppResources.Error, AppResources.WrongFileFormatMessage);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        ShowWrongFormatDialog(AppResources.Error, AppResources.LoadingFileFailedMessage);
                    }
                }
            }
            else if (requestCode == PictureFromCamera)
            {
                Android.Net.Uri contentUri = Android.Net.Uri.FromFile(AppStorage._file);

                string name = AppStorage._file.Name;
                byte[] fileData = ReadFileData(contentUri);
                if (fileData != null)
                {
                    if (name.Contains(".jpg") || name.Contains(".jpeg") || name.Contains(".png"))
                    {
                        ProceedFileInChat(name, fileData);
                    }
                    else
                    {
                        ShowWrongFormatDialog(AppResources.Error, AppResources.WrongFileFormatMessage);
                    }
                }
            }
        }

        byte[] ReadFileData(Android.Net.Uri uri)
        {
            try
            {
                Stream inputStream = ContentResolver.OpenInputStream(uri);

                using (var memstream = new MemoryStream())
                {
                    inputStream.CopyTo(memstream);
                    return memstream.ToArray();
                }
            }
            catch (Exception ex) { }

            return null;
        }

        public string GetFileName(Android.Net.Uri uri)
        {
            ICursor cursor = ContentResolver.Query(uri, null, null, null, null, null);

            try
            {
                if (cursor != null && cursor.MoveToFirst())
                {
                    return cursor.GetString(cursor.GetColumnIndex(OpenableColumns.DisplayName));
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                cursor.Close();
            }

            return string.Empty;
        }

        public int GetFileSize(Android.Net.Uri uri)
        {
            ICursor cursor = ContentResolver.Query(uri, null, null, null, null, null);

            try
            {
                if (cursor != null && cursor.MoveToFirst())
                {
                    int sizeIndex = cursor.GetColumnIndex(OpenableColumns.Size);

                    string size = null;
                    if (!cursor.IsNull(sizeIndex))
                    {
                        size = cursor.GetString(sizeIndex);

                        return Convert.ToInt32(size);
                    }
                }
            }
            finally
            {
                cursor.Close();
            }

            return 0;
        }

        #region Media 

        public void OpenImageGallery()
        {
            var imageIntent = new Intent(Intent.ActionPick);
            imageIntent.SetType("image/*");
            imageIntent.SetAction(Intent.ActionGetContent);
            StartActivityForResult(Intent.CreateChooser(imageIntent, AppResources.SelectImage), PictureFromGallery);
        }

        public void OpenFileGallery()
        {
            var docIntent = new Intent(Intent.ActionPick);
            docIntent.SetType("*/*");
            docIntent.SetAction(Intent.ActionGetContent);
            StartActivityForResult(Intent.CreateChooser(docIntent, AppResources.SelectDocument), FileFromGallery);
        }

        public void OpenCamera()
        {
            if (IsThereAnAppToTakePictures())
            {
                try
                {
                    CreateDirectoryForPictures();

                    Intent intent = new Intent(MediaStore.ActionImageCapture);
                    AppStorage._file = new Java.IO.File(AppStorage._dir, String.Format("attachment_{0}.jpg", Guid.NewGuid()));
                    int sdk = (int)Android.OS.Build.VERSION.SdkInt;
                    intent.PutExtra(MediaStore.ExtraOutput, Android.Net.Uri.FromFile(AppStorage._file));
                    StartActivityForResult(intent, PictureFromCamera);
                }
                catch (Exception ex)
                {
                    if (ex != null)
                    {
                        HockeyApp.Android.Metrics.MetricsManager.TrackEvent("Open camera ERROR: " + ex.ToString());
                    }
                }
            }
        }

        private void CreateDirectoryForPictures()
        {
            AppStorage._dir = new Java.IO.File(Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryPictures), "DIFiles");
            if (!AppStorage._dir.Exists())
            {
                AppStorage._dir.Mkdirs();
            }
        }

        private bool IsThereAnAppToTakePictures()
        {
            Intent intent = new Intent(MediaStore.ActionImageCapture);
            IList<ResolveInfo> availableActivities = PackageManager.QueryIntentActivities(intent, PackageInfoFlags.MatchDefaultOnly);
            return availableActivities != null && availableActivities.Count > 0;
        }

        public String GetPath(Android.Net.Uri uri)
        {
            String[] projection = { Android.Provider.MediaStore.Files.FileColumns.Data };
            ICursor cursor = this.ContentResolver.Query(uri, projection, null, null, null);
            if (cursor == null) return null;
            int column_index = cursor.GetColumnIndexOrThrow(Android.Provider.MediaStore.Files.FileColumns.Data);
            cursor.MoveToFirst();
            String s = cursor.GetString(column_index);
            cursor.Close();
            return s;
        }

        private string GetPathToImage(Android.Net.Uri uri)
        {
            string path = string.Empty;

            string[] projection = new[] { Android.Provider.MediaStore.Images.Media.InterfaceConsts.Data };
            try
            {
                using (ICursor cursor = ManagedQuery(uri, projection, null, null, null))
                {
                    if (cursor != null)
                    {
                        int columnIndex = cursor.GetColumnIndexOrThrow(Android.Provider.MediaStore.Images.Media.InterfaceConsts.Data);
                        cursor.MoveToFirst();
                        path = cursor.GetString(columnIndex);
                    }
                }
            }
            catch (System.Exception ex)
            {
            }

            return path;
        }

        #endregion

        #region Dialogs 

        protected void ShowWrongFormatDialog(string title, string message)
        {
            Android.App.AlertDialog.Builder alert = new Android.App.AlertDialog.Builder(this);
            alert.SetTitle(title);
            alert.SetMessage(message);
            alert.SetNegativeButton(AppResources.Close.ToUpper(), (senderAlert, args) =>
            {
            });

            Android.App.Dialog dialog = alert.Create();
            dialog.Show();
        }

        #endregion

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
            return AppResources.Support;
        }

        protected override FloatingActionButton GetFloatingActionButton()
        {
            return null;
        }

        void FillValues(MessagesResponse data)
        {
            if (!LockTableUpdates)
            {
                values.Clear();

                bool useDateSeparators = true;

                if (!data.Messages.Any(e => DataManager.UnixTimeStampToDateTime(e.Received).Date != DateTime.Now.Date))
                {
                    useDateSeparators = false;
                }

                DateTime currentDate = DataManager.UnixTimeStampToDateTime(data.Messages.Min(e => e.Received));
                if (useDateSeparators)
                {
                    values.Add(VMManager.BuildTimeSeparatorForChatMessages(currentDate));
                }

                foreach (var entity in data.Messages)
                {
                    DateTime messageTime = DataManager.UnixTimeStampToDateTime(entity.Received);
                    if (currentDate.Date != messageTime.Date)
                    {
                        currentDate = messageTime;
                        if (useDateSeparators)
                        {
                            values.Add(VMManager.BuildTimeSeparatorForChatMessages(currentDate));
                        }
                    }

                    values.Add(entity);
                }
            }
        }

        protected override async Task UpdateDataAsync()
        {
            if (AppWrapper.Service != null && AppWrapper.Service.Messages != null && AppWrapper.Service.Messages.Messages.Count > 0)
            {
                MessagesUpdated();
            }
            else
            {
                if (AppWrapper.Service != null && AppWrapper.Service.Messages != null)
                {

                }
                else
                {
                    DontHideProgressBar = true;
                }
            }

            User = sqliteManager.GetSmoochUserData();
        }

        protected override void ItemClickedOn(int position)
        {
            if (values.Count == 0)
                return;

            Shared.Entities.Smooch.Message model = (Shared.Entities.Smooch.Message)values[position];
            if (model != null)
            {
                //TODO: if needed
            }
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

    public static class AppStorage
    {
        public static Java.IO.File _file;
        public static Java.IO.File _dir;
        public static Bitmap bitmap;
    }
}
