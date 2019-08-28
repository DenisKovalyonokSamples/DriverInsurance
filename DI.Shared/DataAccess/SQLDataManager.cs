#if __ANDROID__
using DI.Droid.Helpers;
#endif
#if __IOS__
using DI.iOS.Helpers;
#endif
using DI.Shared.Entities.SQL;
using DI.Shared.Enums;
using DI.Shared.Managers;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DI.Shared.DataAccess
{
    public class SQLDataManager
    {
#region Initialization

        static object locker = new object();
        SQLiteConnection _database;

#if __IOS__
        HockeyApp.iOS.BITHockeyManager HockeyManager = HockeyApp.iOS.BITHockeyManager.SharedHockeyManager;
#endif

        public SQLDataManager()
        {
            _database = GetDBConnection();

            // create the tables
            _database.CreateTable<User>();
            _database.CreateTable<SmoochUserData>();
            _database.CreateTable<DemoModeData>();
        }
        public SQLiteConnection GetDBConnection()
        {
            var databaseName = "DILocalDB.db3";
            string folderPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            var databasePath = Path.Combine(folderPath, databaseName);

            var connection = new SQLiteConnection(databasePath);

            return connection;
        }

#endregion

#region DemoModeData

        public DemoModeData GetDemoModeData()
        {
            try
            {
                lock (locker)
                {
                    return _database.Table<DemoModeData>().FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
#if __ANDROID__
                HockeyApp.Android.Metrics.MetricsManager.TrackEvent("SQLite GetDemoModeData ERROR: " + ex.ToString());
#endif
#if __IOS__
                HockeyManager.MetricsManager.TrackEvent("SQLite GetDemoModeData ERROR: " + ex.ToString());
#endif

                return null;
            }
        }

        public void ClearDemoModeDatas()
        {
            try
            {
                lock (locker)
                {
                    List<DemoModeData> data = (from i in _database.Table<DemoModeData>() select i).ToList();
                    if (null != data && data.Count > 0)
                    {
                        foreach (var entity in data)
                        {
                            _database.Delete<DemoModeData>(entity.Id);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
#if __ANDROID__
                HockeyApp.Android.Metrics.MetricsManager.TrackEvent("SQLite ClearDemoModeDatas ERROR: " + ex.ToString());
#endif
#if __IOS__
                HockeyManager.MetricsManager.TrackEvent("SQLite ClearDemoModeDatas ERROR: " + ex.ToString());
#endif
            }
        }

        public int SaveDemoModeData(DemoModeData value)
        {
            try
            {
                lock (locker)
                {
                    if (value.Id != 0)
                    {
                        _database.Update(value);
                        return value.Id;
                    }
                    else
                    {
                        return _database.Insert(value);
                    }
                }
            }
            catch (Exception ex)
            {
#if __ANDROID__
                HockeyApp.Android.Metrics.MetricsManager.TrackEvent("SQLite SaveDemoModeData ERROR: " + ex.ToString());
#endif
#if __IOS__
                HockeyManager.MetricsManager.TrackEvent("SQLite SaveDemoModeData ERROR: " + ex.ToString());
#endif

                return 0;
            }
        }

#endregion

#region User

        public int Register(string phone, string token, string username)
        {
            try
            {
                lock (locker)
                {
                    var user = _database.Table<User>().FirstOrDefault();
                    if (user != null)
                        ClearUsers();

                    var entity = new User();
                    entity.Phone = phone;
                    entity.Token = token;
                    entity.Name = username;
                    entity.IsPhoneVerified = false;
                    entity.Salt = SecurityManager.CreateSalt();
                    entity.Password = SecurityManager.CreateHash(string.Empty, entity.Salt);
                    entity.PINAttemptsCount = 3;

#if __ANDROID__
                    entity.DeviceUID = DeviceInfoHelper.GetDeviceUniqID().ToString();
#endif
#if __IOS__
                    entity.DeviceUID = DeviceInfoHelper.GetDeviceUniqID();
#endif

                    SaveUser(entity);

                    return entity.Id;
                }
            }
            catch (Exception ex)
            {
#if __ANDROID__
                HockeyApp.Android.Metrics.MetricsManager.TrackEvent("SQLite Register ERROR: " + ex.ToString());
#endif
#if __IOS__
                HockeyManager.MetricsManager.TrackEvent("SQLite Register ERROR: " + ex.ToString());
#endif

                return 0;
            }
        }

        public ResultCode Login(string password)
        {
            try
            {
                lock (locker)
                {
                    var user = _database.Table<User>().FirstOrDefault();
                    if (user == null)
                        return ResultCode.Error;

                    if (SecurityManager.CreateHash(password, user.Salt) == user.Password)
                    {
                        user.PINAttemptsCount = 3;
                        SaveUser(user);

                        return ResultCode.Success;
                    }
                    else
                    {
                        user.PINAttemptsCount--;

                        if (user.PINAttemptsCount > 0)
                        {
                            SaveUser(user);
                        }
                        else
                        {
                            ClearUsers();
                        }
                    }

                    return ResultCode.Error;
                }
            }
            catch (Exception ex)
            {
#if __ANDROID__
                HockeyApp.Android.Metrics.MetricsManager.TrackEvent("SQLite Login ERROR: " + ex.ToString());
#endif
#if __IOS__
                HockeyManager.MetricsManager.TrackEvent("SQLite Login ERROR: " + ex.ToString());
#endif

                return ResultCode.Error;
            }
        }

        public User GetUser()
        {
            try
            {
                lock (locker)
                {
                    return _database.Table<User>().FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
#if __ANDROID__
                HockeyApp.Android.Metrics.MetricsManager.TrackEvent("SQLite GetUser ERROR: " + ex.ToString());
#endif
#if __IOS__
                HockeyManager.MetricsManager.TrackEvent("SQLite GetUser ERROR: " + ex.ToString());
#endif

                return null;
            }
        }

        public int SaveUser(User value)
        {
            try
            {
                lock (locker)
                {
                    if (value.Id != 0)
                    {
                        _database.Update(value);
                        return value.Id;
                    }
                    else
                    {
                        return _database.Insert(value);
                    }
                }
            }
            catch (Exception ex)
            {
#if __ANDROID__
                HockeyApp.Android.Metrics.MetricsManager.TrackEvent("SQLite SaveUser ERROR: " + ex.ToString());
#endif
#if __IOS__
                HockeyManager.MetricsManager.TrackEvent("SQLite SaveUser ERROR: " + ex.ToString());
#endif

                return 0;
            }
        }

        public void ClearUsers()
        {
            try
            {
                lock (locker)
                {
                    List<User> data = (from i in _database.Table<User>() select i).ToList();
                    if (null != data && data.Count > 0)
                    {
                        foreach (var entity in data)
                        {
                            _database.Delete<User>(entity.Id);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
#if __ANDROID__
                HockeyApp.Android.Metrics.MetricsManager.TrackEvent("SQLite ClearUsers ERROR: " + ex.ToString());
#endif
#if __IOS__
                HockeyManager.MetricsManager.TrackEvent("SQLite ClearUsers ERROR: " + ex.ToString());
#endif
            }
        }

#endregion

#region Smooch User Data

        public SmoochUserData GetSmoochUserData()
        {
            try
            {
                lock (locker)
                {
                    return _database.Table<SmoochUserData>().FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
#if __ANDROID__
                HockeyApp.Android.Metrics.MetricsManager.TrackEvent("SQLite GetSmoochUserData ERROR: " + ex.ToString());
#endif
#if __IOS__
                HockeyManager.MetricsManager.TrackEvent("SQLite GetSmoochUserData ERROR: " + ex.ToString());
#endif

                return null;
            }
        }

        public int SaveSmoochUserData(SmoochUserData value)
        {
            try
            {
                lock (locker)
                {
                    if (value.Id != 0)
                    {
                        _database.Update(value);
                        return value.Id;
                    }
                    else
                    {
                        return _database.Insert(value);
                    }
                }
            }
            catch (Exception ex)
            {
#if __ANDROID__
                HockeyApp.Android.Metrics.MetricsManager.TrackEvent("SQLite SaveSmoochUserData ERROR: " + ex.ToString());
#endif
#if __IOS__
                HockeyManager.MetricsManager.TrackEvent("SQLite SaveSmoochUserData ERROR: " + ex.ToString());
#endif

                return 0;
            }
        }

        public void ClearSmoochUserDatas()
        {
            try
            {
                lock (locker)
                {
                    List<SmoochUserData> data = (from i in _database.Table<SmoochUserData>() select i).ToList();
                    if (null != data && data.Count > 0)
                    {
                        foreach (var entity in data)
                        {
                            _database.Delete<SmoochUserData>(entity.Id);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
#if __ANDROID__
                HockeyApp.Android.Metrics.MetricsManager.TrackEvent("SQLite ClearSmoochUserDatas ERROR: " + ex.ToString());
#endif
#if __IOS__
                HockeyManager.MetricsManager.TrackEvent("SQLite ClearSmoochUserDatas ERROR: " + ex.ToString());
#endif
            }
        }

#endregion
    }
}
