using System;
using System.Collections.Generic;

namespace MT.OnlineRestaurant.DataLayer.EntityFrameWorkModel
{
    public partial class TblLocation
    {
        public TblLocation()
        {
            TblRestaurant = new HashSet<TblRestaurant>();
        }

        public decimal? X { get; set; }
        public decimal? Y { get; set; }
        public int Id { get; set; }
        public int UserCreated { get; set; }
        public int UserModified { get; set; }
        public DateTime RecordTimeStamp { get; set; }
        public DateTime RecordTimeStampCreated { get; set; }

        public ICollection<TblRestaurant> TblRestaurant { get; set; }
    }
}
