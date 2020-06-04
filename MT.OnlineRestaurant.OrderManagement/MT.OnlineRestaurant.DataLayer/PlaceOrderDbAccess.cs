using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MT.OnlineRestaurant.DataLayer.Context;
using MT.OnlineRestaurant.DataLayer.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MT.OnlineRestaurant.DataLayer
{
    public class PlaceOrderDbAccess : IPlaceOrderDbAccess
    {
        private readonly OrderManagementContext _context;

        public PlaceOrderDbAccess(OrderManagementContext context)
        {
            _context = context;
        }

        public int PlaceOrder(TblFoodOrder OrderedFoodDetails)
        {
            try
            {
                _context.TblFoodOrder.Add(OrderedFoodDetails);
                _context.SaveChanges();
                return OrderedFoodDetails.Id;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message); 
            }
        }

        public bool PlaceOrderMapping(IList<TblFoodOrderMapping> lstTblFoodOrderMappings)
        {
            try
            {
                _context.TblFoodOrderMapping.AddRange(lstTblFoodOrderMappings);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public int CancelOrder(int orderId)
        {
            var orderedFood = _context.TblFoodOrder.Include(p => p.TblFoodOrderMapping)
                .SingleOrDefault(p => p.Id == orderId);

            orderedFood.TblFoodOrderMapping.ToList().ForEach(p => _context.TblFoodOrderMapping.Remove(p));
            _context.TblFoodOrder.Remove(orderedFood);
            _context.SaveChanges();
            
            return (orderedFood != null ? orderedFood.Id : 0);
        }

        /// <summary>
        /// gets the customer placed order details
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public IQueryable<TblFoodOrder> GetReports(int customerId)
        {
            return _context.TblFoodOrder.Where(fo => fo.TblCustomerId == customerId);
        }
    }
}
