using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace DI.Shared.Entities.SQL
{
    public class SmoochUserData
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string DeviceId { get; set; }

        public string UserId { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public string Email { get; set; }
    }
}
