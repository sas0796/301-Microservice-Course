using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MT.Online.Restaurant.MessagesManagement.Services
{
    public interface IMessages
    {
        void RegisterOnMessageHandlerAndReceiveMessages();
        Task SendMessagesAsync<T>(T senderObj);
    }
}
