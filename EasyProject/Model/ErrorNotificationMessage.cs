using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyProject.Model
{
    public class ErrorNotificationMessage : NotificationMessage
    {
        public ErrorNotificationMessage()
        {
            Title = "알림";
        }
    }

}
