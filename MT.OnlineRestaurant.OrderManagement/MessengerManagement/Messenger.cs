using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using MT.OnlineRestaurant.BusinessEntities;
using MT.OnlineRestaurant.DataLayer.interfaces;
using Newtonsoft.Json;

namespace MessengerManagement
{
    public class Messenger : IMessenger
    {
        const string ServiceBusConnectionString = "Endpoint=sb://foodorderingservicebus.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=+ctIcSNtEphvH/io3sjz2Uz4gCvqMmgZ6VGVYS08fzU=";
        const string TopicName = "foodorderingtopic";
        private readonly ITopicClient _topicClient;
        const string SubscriptionName = "FoodOrderingSubscription";
        private readonly ICartRepository _cartRepository;
        static ISubscriptionClient subscriptionClient;

        public Messenger(ITopicClient topicClient, ICartRepository cartRepository)
        {
            _topicClient = topicClient;
            _cartRepository = cartRepository;
        }

        #region Public Methods
        /// <summary>
        /// Send Message Function, serialize the content if it's a model, before sending it in cloud using service bus
        /// </summary>
        /// <param name="messageBody"></param>
        /// <returns></returns>
        public async Task SendMessageAsync(string messageBody)
        {
            try
            {
                var message = new Message(Encoding.UTF8.GetBytes(messageBody));

                // Send the message to the topic.
                await _topicClient.SendAsync(message);
                
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /////  Receive Message Function, the output can be serialized, de-serialize in the function from where it is called
        ///// </summary>
        ///// <returns></returns>
        //public async Task ReceiveMessageAsync()
        //{
        //    subscriptionClient = new SubscriptionClient(ServiceBusConnectionString, TopicName, SubscriptionName);

        //    // Register subscription message handler and receive messages in a loop
        //    //RegisterOnMessageHandlerAndReceiveMessages();

        //    await subscriptionClient.CloseAsync();
        //}

        //void RegisterOnMessageHandlerAndReceiveMessages()
        //{
        //    var messageHandlerOptions = new MessageHandlerOptions(ExceptionReceivedHandler)
        //    {
        //        MaxConcurrentCalls = 1,
        //        AutoComplete = false
        //    };

        //    //subscriptionClient.RegisterMessageHandler((message, token) =>
        //    //{
        //    //    string json = Encoding.UTF8.GetString(message.Body);
        //    //    CartItemsSenderDetails cartItem = JsonConvert.DeserializeObject<CartItemsSenderDetails>(json);

        //    //    _cartRepository.UpdateCartItemsForOutOfStock(cartItem.RestaurantId, cartItem.MenuId);
        //    //    return subscriptionClient.CompleteAsync(message.SystemProperties.LockToken);
        //    //}, new MessageHandlerOptions(args => Task.CompletedTask)
        //    //{
        //    //    AutoComplete = false,
        //    //    MaxConcurrentCalls = 1
        //    //});
        //    subscriptionClient.RegisterMessageHandler(ProcessMessagesAsync, messageHandlerOptions);
        //}
        //#endregion

        //#region Private Methods
        //async Task ProcessMessagesAsync(Message message, CancellationToken token)
        //{
        //    // Process the message.
        //    string json = Encoding.UTF8.GetString(message.Body);
        //    CartItemsSenderDetails cartItem = JsonConvert.DeserializeObject<CartItemsSenderDetails>(json);

        //    _cartRepository.UpdateCartItemsForOutOfStock(cartItem.RestaurantId, cartItem.MenuId);

        //    // Complete the message so that it is not received again.
        //    // This can be done only if the subscriptionClient is created in ReceiveMode.PeekLock mode (which is the default).
        //    await subscriptionClient.CompleteAsync(message.SystemProperties.LockToken);

        //    // Note: Use the cancellationToken passed as necessary to determine if the subscriptionClient has already been closed.
        //    // If subscriptionClient has already been closed, you can choose to not call CompleteAsync() or AbandonAsync() etc.
        //    // to avoid unnecessary exceptions.
        //}

        //static Task ExceptionReceivedHandler(ExceptionReceivedEventArgs exceptionReceivedEventArgs)
        //{
        //    Console.WriteLine($"Message handler encountered an exception {exceptionReceivedEventArgs.Exception}.");
        //    var context = exceptionReceivedEventArgs.ExceptionReceivedContext;
        //    Console.WriteLine("Exception context for troubleshooting:");
        //    Console.WriteLine($"- Endpoint: {context.Endpoint}");
        //    Console.WriteLine($"- Entity Path: {context.EntityPath}");
        //    Console.WriteLine($"- Executing Action: {context.Action}");
        //    return Task.CompletedTask;
        //}
        #endregion
    }
}
