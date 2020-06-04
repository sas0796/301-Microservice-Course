using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using MT.OnlineRestaurant.BusinessEntities;
using MT.OnlineRestaurant.DataLayer.Repository;
using Newtonsoft.Json;

namespace MessengerManagement
{
    public class Messenger : IMessenger
    {
        const string ServiceBusConnectionString = "Endpoint=sb://foodorderingservicebus.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=+ctIcSNtEphvH/io3sjz2Uz4gCvqMmgZ6VGVYS08fzU=";
        const string TopicName = "foodorderingtopic";
        static ITopicClient topicClient;
        
        private readonly ISearchRepository _searchRepository;
        private readonly ISubscriptionClient _subscriptionClient;

        public Messenger(ISubscriptionClient subscriptionClient, ISearchRepository searchRepository)
        {
            _subscriptionClient = subscriptionClient;
            _searchRepository = searchRepository;
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
                topicClient = new TopicClient(ServiceBusConnectionString, TopicName);


                // Create a new message to send to the topic.
                var message = new Message(Encoding.UTF8.GetBytes(messageBody));

                // Send the message to the topic.
                await topicClient.SendAsync(message);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                await topicClient.CloseAsync();
            }
        }
        /// <summary>
        ///  Receive Message Function, the output can be serialized, de-serialize in the function from where it is called
        /// </summary>
        /// <returns></returns>
        public async Task ReceiveMessageAsync()
        {
            RegisterOnMessageHandlerAndReceiveMessages();

            //await _subscriptionClient.CloseAsync();
        }

        void RegisterOnMessageHandlerAndReceiveMessages()
        {
            // Register the function that processes messages.
            _subscriptionClient.RegisterMessageHandler((message, token) =>
            {
                string json = Encoding.UTF8.GetString(message.Body);
                CartItemsReceiverDetails cartItems = JsonConvert.DeserializeObject<CartItemsReceiverDetails>(json);
                int quantityDifference = 1;

                bool result = _searchRepository.UpdateQuantitiesForMenuOfRestaurant(cartItems.RestaurantId, cartItems.MenuId, cartItems.Quantity,
                    out quantityDifference);

                //if (quantityDifference < 1 && result)
                //{
                //    cartItems.Quantity = quantityDifference;
                //    string jsonForOrderService = JsonConvert.SerializeObject(cartItems);
                //    SendMessageAsync(jsonForOrderService);
                //}
                Console.WriteLine($"{cartItems.Quantity} plates of Menu {cartItems.MenuId} ordered from Restaurant {cartItems.RestaurantId}");

                return _subscriptionClient.CompleteAsync(message.SystemProperties.LockToken);
            }, new MessageHandlerOptions(ExceptionReceivedHandler)
            {
                AutoComplete = false,
                MaxConcurrentCalls = 1
            });
        }
        #endregion

        #region Private Methods
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
        #endregion
    }
}
