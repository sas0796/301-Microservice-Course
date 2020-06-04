using System;
using System.Collections.Generic;

namespace MT.OnlineRestaurant.DataLayer.EntityFrameWorkModel
{
    public partial class TblRestaurantDetails
    {
        public int TblRestaurantId { get; set; }
        public int TableCount { get; set; }
        public int TableCapacity { get; set; }
        public bool Active { get; set; }
        public int Id { get; set; }
        public int UserCreated { get; set; }
        public int UserModified { get; set; }
        public DateTime RecordTimeStamp { get; set; }
        public DateTime RecordTimeStampCreated { get; set; }

        public TblRestaurant TblRestaurant { get; set; }
    }
}
