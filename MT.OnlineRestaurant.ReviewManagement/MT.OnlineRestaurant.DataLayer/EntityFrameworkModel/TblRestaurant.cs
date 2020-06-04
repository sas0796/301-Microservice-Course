using System;
using System.Collections.Generic;

namespace MT.OnlineRestaurant.DataLayer.EntityFrameWorkModel
{
    public partial class TblRestaurant
    {
        public TblRestaurant()
        {
            TblRating = new HashSet<TblRating>();
        }

        public string Name { get; set; }
        public string ContactNo { get; set; }
        public int Id { get; set; }
        public DateTime RecordTimeStampCreated { get; set; }
        public DateTime RecordTimeStampUpdated { get; set; }
        public string Address { get; set; }
        public string Website { get; set; }
        public string OpeningTime { get; set; }
        public string CloseTime { get; set; }

        public ICollection<TblRating> TblRating { get; set; }
    }
}
