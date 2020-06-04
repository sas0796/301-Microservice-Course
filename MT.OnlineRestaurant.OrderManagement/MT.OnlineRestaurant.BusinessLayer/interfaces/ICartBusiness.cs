#region References
using MT.OnlineRestaurant.BusinessEntities;
using System.Threading.Tasks;
#endregion

#region Namespace Definition
namespace MT.OnlineRestaurant.BusinessLayer.interfaces
{
    #region Interface Definition
    public interface ICartBusiness
    {
        /// <summary>
        /// Gets the cart details
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        Task<CartDetails> GetCartDetails(int customerId);

        /// <summary>
        /// Inserts cart items information
        /// </summary>
        /// <param name="tblCartMaster"></param>
        /// <returns></returns>
        Task<bool> InsertCartDetails(int customerId, InsertCartDetails insertCartDetails);

        /// <summary>
        /// Updates cart items information
        /// </summary>
        /// <param name="tblCartMaster"></param>
        /// <returns></returns>
        Task<bool> UpdateCartDetails(int customerId, InsertCartDetails updateCartDetails);

        /// <summary>
        /// Removes cart master information
        /// </summary>
        /// <param name="tblCartMaster"></param>
        /// <returns></returns>
        bool RemoveCartDetails(int customerId, int restaurantId, int menuId);
    }
    #endregion
}
#endregion

