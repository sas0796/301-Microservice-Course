using System;
using System.Collections.Generic;

namespace MT.OnlineRestaurant.DataLayer.Context
{
    public partial class TblCartItems
    {
        public int TblMenuId { get; set; }
        public int Quantity { get; set; }
        public int TblCartMasterId { get; set; }
        public int Id { get; set; }
        public bool IsItemAvailable { get; set; }
        public DateTime RecordTimeStampCreated { get; set; }

        public virtual TblCartMaster TblCartMaster { get; set; }
    }
}
