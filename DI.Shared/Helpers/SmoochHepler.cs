using DI.Shared.DataAccess;
using DI.Shared.Entities.Smooch;
using DI.Shared.Entities.SQL;
using DI.Shared.Managers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DI.Shared.Helpers
{
    public static class SmoochHepler
    {
        public static async Task InitSmooch(SQLDataManager sqliteManager)
        {
            var entity = sqliteManager.GetSmoochUserData();
            if (entity == null)
            {
                var model = new InitRequestModel();
                model.Device = new Device();
                model.Device.Id = Guid.NewGuid().ToString();
                model.Device.Platform = "android";
                model.Device.AppVersion = SessionManager.AppVersion;
                model.UserId = Guid.NewGuid().ToString();

                var registration = await SmoochManager.Init(model);
                if (registration != null)
                {
                    var info = new Info();
                    info.AppName = "DI.Droid"; 
                    info.DevicePlatform = "Android";
                    info.OS = "Android";
                    info.OSVersion = "7.1";
                    var result = await SmoochManager.UpdateDevice(model.UserId, model.Device.Id, info);

                    var userModel = new AppUserData();
                    userModel.Email = string.Empty;
                    userModel.GivenName = "Client";
                    userModel.Surname = "(Android Mobile)";

                    var user = await SmoochManager.UpdateAppUser(registration.AppUser.UserId, userModel);
                    if (user != null)
                    {
                        var smoochUser = new SmoochUserData();

                        smoochUser.DeviceId = model.Device.Id;
                        smoochUser.UserId = registration.AppUser.UserId;
                        smoochUser.Name = userModel.GivenName;
                        smoochUser.Surname = userModel.Surname;
                        smoochUser.Email = userModel.Email;

                        sqliteManager.SaveSmoochUserData(smoochUser);
                    }
                }
            }
        }

        public static async Task SyncSmooch(SQLDataManager sqliteManager)
        {
            if (SessionManager.UserData != null && string.IsNullOrEmpty(SessionManager.UserData.SmoochId))
            {
                //Update API User from local DB               
                var userData = sqliteManager.GetSmoochUserData();

                if (userData == null)
                {
                    await InitSmooch(sqliteManager);

                    userData = sqliteManager.GetSmoochUserData();
                }

                if (userData != null && !string.IsNullOrEmpty(userData.UserId))
                {
                    SessionManager.UserData.SmoochId = userData.UserId;
                    bool result = await APIDataManager.UpdateUser(VMManager.ToUserForUpdate(SessionManager.UserData), SessionManager.UserData.Id.ToString());

                    if (SessionManager.СontractorData != null)
                    {
                        if (userData.Name != SessionManager.СontractorData.FirstName || userData.Surname != SessionManager.СontractorData.LastName)
                        {
                            userData.Name = SessionManager.СontractorData.FirstName;
                            userData.Surname = SessionManager.СontractorData.LastName;

                            var userModel = new AppUserData();
                            userModel.Email = string.Empty;
                            userModel.GivenName = userData.Name;
                            userModel.Surname = userData.Surname;

                            var user = await SmoochManager.UpdateAppUser(userData.UserId, userModel);
                            sqliteManager.SaveSmoochUserData(userData);
                        }
                    }
                }
            }
            else if (SessionManager.UserData != null && !string.IsNullOrEmpty(SessionManager.UserData.SmoochId))
            {
                //Update local DB User from API
                SmoochUserData smoochData = sqliteManager.GetSmoochUserData();

                if (smoochData == null)
                {
                    smoochData = new SmoochUserData();
                    smoochData.Id = 0;
                    smoochData.UserId = SessionManager.UserData.SmoochId;

                    if (SessionManager.СontractorData != null)
                    {
                        smoochData.Name = SessionManager.СontractorData.FirstName;
                        smoochData.Surname = SessionManager.СontractorData.LastName;

                        AppUserData model = new AppUserData();
                        model.Email = string.Empty;
                        model.GivenName = smoochData.Name;
                        model.Surname = smoochData.Surname;

                        var user = await SmoochManager.UpdateAppUser(smoochData.UserId, model);
                    }

                    sqliteManager.SaveSmoochUserData(smoochData);
                }
                else
                {
                    if (string.IsNullOrEmpty(smoochData.UserId)
                        || smoochData.Name != SessionManager.СontractorData.FirstName
                        || smoochData.Surname != SessionManager.СontractorData.LastName)
                    {
                        smoochData.UserId = SessionManager.UserData.SmoochId;
                        if (SessionManager.СontractorData != null)
                        {
                            if (smoochData.Name != SessionManager.СontractorData.FirstName || smoochData.Surname != SessionManager.СontractorData.LastName)
                            {
                                smoochData.Name = SessionManager.СontractorData.FirstName;
                                smoochData.Surname = SessionManager.СontractorData.LastName;

                                var userModel = new AppUserData();
                                userModel.Email = string.Empty;
                                userModel.GivenName = smoochData.Name;
                                userModel.Surname = smoochData.Surname;

                                var user = await SmoochManager.UpdateAppUser(smoochData.UserId, userModel);
                            }
                        }

                        sqliteManager.SaveSmoochUserData(smoochData);
                    }
                }
            }
        }
    }
}

