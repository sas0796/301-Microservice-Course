using System;
using System.Collections.Generic;

namespace MT.OnlineRestaurant.DataLayer.DataEntity
{
    public partial class MenuItems
    {

        public string Item { get; set; }
        public int TblCuisineId { get; set; }
        public int Id { get; set; }
        public int UserCreated { get; set; }
        public int UserModified { get; set; }
        public DateTime RecordTimeStamp { get; set; }
        public DateTime RecordTimeStampCreated { get; set; }

        public int Quantity { get; set; }
        public double Price { get; set; }
    }
}
