using System.Threading.Tasks;

namespace MessengerManagement
{
    public interface IMessenger
    {
        /// <summary>
        /// Used for sending message using Azure Service Bus,
        /// Serialize the model and send it as a string
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        Task SendMessageAsync(string message);
        /// <summary>
        /// Used for receiving message using Azure Service Bus,
        /// De-serialize the model after receiving it in the calling function
        /// </summary>
        /// <returns></returns>
        //Task ReceiveMessageAsync();
    }
}
