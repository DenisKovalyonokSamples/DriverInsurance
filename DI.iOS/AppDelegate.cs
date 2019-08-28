using Foundation;
using UIKit;
using HockeyApp.iOS;
using DI.Shared.Managers;
using DI.Shared.DataAccess;

namespace DI.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the
    // User Interface of the application, as well as listening (and optionally responding) to application events from iOS.
    [Register("AppDelegate")]
    public class AppDelegate : UIApplicationDelegate
    {
        // class-level declarations
        const string HOCKEY_APP_IOS = "";
        const double MINIMUM_BACKGROUND_FETCH_INTERVAL = 300;

        public override UIWindow Window
        {
            get;
            set;
        }

        public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
        {
            // create a new window instance based on the screen size
            Window = new UIWindow(UIScreen.MainScreen.Bounds);

            UIStoryboard storyboard = UIStoryboard.FromName("Main_iPhone", NSBundle.MainBundle);
            ProceedLogin(storyboard);

            var manager = BITHockeyManager.SharedHockeyManager;
            manager.Configure(HOCKEY_APP_IOS);
            manager.StartManager();

            return true;
        }

        async void ProceedLogin(UIStoryboard storyboard)
        {
            DI.Shared.Entities.SQL.User entity = SessionManager.DBConnection.GetUser();
            if (entity != null)
            {
                if (!string.IsNullOrEmpty(entity.Token))
                {
                    APIDataManager.SetUserToken(entity.Token);

                    SessionManager.UserData = await APIDataManager.GetUserByName(entity.Name);
                    if (SessionManager.UserData != null && SessionManager.UserData.CompanyId.HasValue)
                    {
                        SessionManager.СontractorData = await APIDataManager.GetCompany(SessionManager.UserData.CompanyId.Value);
                        if (SessionManager.СontractorData != null)
                        {
                            SessionManager.СontractData = await APIDataManager.GetCurrentContract(SessionManager.СontractorData.Id.ToString());
                        }
                    }

                    if (SessionManager.UserData != null)
                    {
                        Window.RootViewController = storyboard.InstantiateViewController("MainController") as MainController;
                    }
                    else
                    {
                        Window.RootViewController = storyboard.InstantiateViewController("ErrorInfoController") as ErrorInfoController;
                    }
                }
            }
            else
            {
                Window.RootViewController = storyboard.InstantiateViewController("RegisterPhoneController") as RegisterPhoneController;
            }

            Window.MakeKeyAndVisible();
        }

        public override void OnResignActivation(UIApplication application)
        {
            // Invoked when the application is about to move from active to inactive state.
            // This can occur for certain types of temporary interruptions (such as an incoming phone call or SMS message) 
            // or when the user quits the application and it begins the transition to the background state.
            // Games should use this method to pause the game.
        }

        public override void DidEnterBackground(UIApplication application)
        {
            // Use this method to release shared resources, save user data, invalidate timers and store the application state.
            // If your application supports background exection this method is called instead of WillTerminate when the user quits.
        }

        public override void WillEnterForeground(UIApplication application)
        {
            // Called as part of the transiton from background to active state.
            // Here you can undo many of the changes made on entering the background.
        }

        public override void OnActivated(UIApplication application)
        {
            // Restart any tasks that were paused (or not yet started) while the application was inactive. 
            // If the application was previously in the background, optionally refresh the user interface.
        }

        public override void WillTerminate(UIApplication application)
        {
            // Called when the application is about to terminate. Save data, if needed. See also DidEnterBackground.
        }
    }
}


