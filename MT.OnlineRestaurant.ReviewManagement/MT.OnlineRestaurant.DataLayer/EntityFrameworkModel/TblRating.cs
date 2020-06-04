using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MT.OnlineRestaurant.DataLayer.EntityFrameWorkModel
{
    public partial class TblRating
    {
        public string Rating { get; set; }
        [MaxLength(250)]
        public string Comments { get; set; }
        public int TblRestaurantId { get; set; }
        public int Id { get; set; }
        public DateTime RecordTimeStampCreated { get; set; }
        public DateTime RecordTimeStampUpdated { get; set; }

        public TblRestaurant TblRestaurant { get; set; }
    }
}
