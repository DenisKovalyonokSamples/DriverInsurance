using DI.Shared.DataAccess;
using DI.Shared.Entities.API;
using DI.Shared.Enums;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace DI.Shared.Managers
{
    public static class SessionManager
    {
        public static PartialType ShowPartialOnMain { get; set; }

        public static User UserData { get; set; }

        public static Company СontractorData { get; set; }

        public static Contract СontractData { get; set; }

        public static string AppVersion { get; set; }

        static SQLDataManager _database;
        public static SQLDataManager DBConnection
        {
            get
            {
                if (_database == null)
                {
                    _database = new SQLDataManager();
                }

                return _database;
            }
        }
    }
}
