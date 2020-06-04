using Microsoft.Azure.ServiceBus;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MT.OnlineRestaurant.BusinessLayer;

namespace MT.Online.Restaurant.MessagesManagement.Services
{
    public class Messages : IMessages
    {
        public ICustomerBusiness _customerBusiness { get; set; }
        const string ServiceBusConnectionString = "Endpoint=sb://capstoneapi.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=CLJQF4McnQ8UVBixOM+pTvK/Uo9ybJGESjQQMGnWscA=";
        const string TopicName = "itemoutofstock";
        const string SubscriptionName = "s1";
        static ISubscriptionClient subscriptionClient;
        public void RegisterOnMessageHandlerAndReceiveMessages()
        {
            subscriptionClient = new SubscriptionClient(ServiceBusConnectionString, TopicName, SubscriptionName);
            // Configure the message handler options in terms of exception handling, number of concurrent messages to deliver, etc.
            var messageHandlerOptions = new MessageHandlerOptions(ExceptionReceivedHandler)
            {
                // Maximum number of concurrent calls to the callback ProcessMessagesAsync(), set to 1 for simplicity.
                // Set it according to how many messages the application wants to process in parallel.
                MaxConcurrentCalls = 1,

                // Indicates whether MessagePump should automatically complete the messages after returning from User Callback.
                // False below indicates the Complete will be handled by the User Callback as in `ProcessMessagesAsync` below.
                AutoComplete = false
            };

            // Register the function that processes messages.
            subscriptionClient.RegisterMessageHandler(ProcessMessagesAsync, messageHandlerOptions);
        }
        async Task ProcessMessagesAsync(Message message, CancellationToken token)
        {
   
            string msg = Encoding.UTF8.GetString(message.Body);
            _customerBusiness.UpdateFromReceivedMessage(msg);
            await subscriptionClient.CompleteAsync(message.SystemProperties.LockToken);
        }

         Task ExceptionReceivedHandler(ExceptionReceivedEventArgs exceptionReceivedEventArgs)
        {
            Console.WriteLine($"Message handler encountered an exception {exceptionReceivedEventArgs.Exception}.");
            var context = exceptionReceivedEventArgs.ExceptionReceivedContext;
            Console.WriteLine("Exception context for troubleshooting:");
            Console.WriteLine($"- Endpoint: {context.Endpoint}");
            Console.WriteLine($"- Entity Path: {context.EntityPath}");
            Console.WriteLine($"- Executing Action: {context.Action}");
            return Task.CompletedTask;
        }
        //public async void SendMessagesAsync<T>(T obj) where T: class
        public async void SendMessagesAsync(string msg)
        {
            ITopicClient topicClient;
            topicClient = new TopicClient(ServiceBusConnectionString, TopicName);
            try
            {
                //string messageBody = JsonConvert.SerializeObject(obj);
                Message message = new Message(Encoding.UTF8.GetBytes(msg));
                await topicClient.SendAsync(message);         
            }
            catch (Exception exception)
            {
                Console.WriteLine($"{DateTime.Now} :: Exception: {exception.Message}");
            }
        }     
    }
}
