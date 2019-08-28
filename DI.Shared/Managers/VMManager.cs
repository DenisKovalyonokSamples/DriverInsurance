using System;
using System.Collections.Generic;
using System.Text;
using DI.Shared.Entities.Smooch;
using DI.Localization;
using DI.Shared.Entities.API;

namespace DI.Shared.Managers
{
    public class VMManager
    {
        public static Message BuildTimeSeparatorForChatMessages(DateTime date)
        {
            var model = new Message();
            model.Role = "timeSeparator";

            if (DateTime.Now.Date == date.Date)
            {
                model.Name = AppResources.Today;
            }
            else if ((DateTime.Now.Date - date.Date).TotalDays == 1)
            {
                model.Name = AppResources.Yesterday;
            }
            else
            {
                model.Name = date.ToString("dd.MM.yyyy");
            }

            return model;
        }

        public static UserForUpdate ToUserForUpdate(User entity)
        {
            var user = new UserForUpdate();

            user.SmoochId = entity.SmoochId;

            return user;
        }
    }
}
