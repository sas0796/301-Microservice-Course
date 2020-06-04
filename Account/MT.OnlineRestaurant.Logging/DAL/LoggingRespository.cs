#region References
using MT.OnlineRestaurant.Logging.Context;
#endregion

#region namepsace
namespace MT.OnlineRestaurant.Logging.DAL
{
    #region class definition
    /// <summary>
    /// capture the activities into the database
    /// </summary>
    public class LoggingRespository : ILogging
    {
        #region variables
        private readonly CustomerManagementContext context;
        #endregion

        #region Constructor
        /// <summary>
        /// initialized the object along with the database context
        /// </summary>
        public LoggingRespository(string connectionString)
        {
            context = new CustomerManagementContext(connectionString);
        }
        #endregion

        #region Interface methods
        /// <summary>
        /// Catpures the logs
        /// </summary>
        /// <param name="loggingInfo"></param>
        public void CaptureLogs(LoggingInfo loggingInfo)
        {
            context.Set<LoggingInfo>().Add(loggingInfo);
            context.SaveChanges();
        }
        #endregion
    }
    #endregion
}
#endregion
