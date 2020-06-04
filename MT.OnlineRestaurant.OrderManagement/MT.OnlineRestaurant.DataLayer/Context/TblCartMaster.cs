using System;
using System.Collections.Generic;

namespace MT.OnlineRestaurant.DataLayer.Context
{
    public partial class TblCartMaster
    {
        public TblCartMaster()
        {
            TblCartItems = new HashSet<TblCartItems>();
        }

        public int TblCustomerId { get; set; }
        public int TblRestaurantId { get; set; }
        public int Id { get; set; }
        public bool IsActive { get; set; }
        public DateTime RecordTimeStampCreated { get; set; }

        public virtual ICollection<TblCartItems> TblCartItems { get; set; }
    }
}
