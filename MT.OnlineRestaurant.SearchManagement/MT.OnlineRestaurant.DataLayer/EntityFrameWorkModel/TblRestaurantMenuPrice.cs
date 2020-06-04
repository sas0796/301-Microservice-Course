using System;
using System.Collections.Generic;
using System.Text;

namespace MT.OnlineRestaurant.DataLayer.EntityFrameWorkModel
{
    public class TblRestaurantMenuPrice
    {
        public int Id { get; set; }
        public int tblRestaurantId { get; set; }
        public int tblMenuId { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }

        public virtual TblRestaurant TblRestaurant { get; set; }
        public virtual TblMenu TblMenu { get; set; }
    }
}
