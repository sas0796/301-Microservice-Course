using System;
using System.Collections.Generic;

namespace MT.OnlineRestaurant.DataLayer.EntityFrameWorkModel
{
    public partial class TblRestaurant
    {
        public TblRestaurant()
        {
            TblOffer = new HashSet<TblOffer>();
            TblRating = new HashSet<TblRating>();
            TblRestaurantDetails = new HashSet<TblRestaurantDetails>();
            TblRestaurantMenuPrice = new HashSet<TblRestaurantMenuPrice>();
        }

        public string Name { get; set; }
        public int TblLocationId { get; set; }
        public string ContactNo { get; set; }
        public int Id { get; set; }
        public int UserCreated { get; set; }
        public int UserModified { get; set; }
        public DateTime RecordTimeStamp { get; set; }
        public DateTime RecordTimeStampCreated { get; set; }
        public string Address { get; set; }
        public string Website { get; set; }
        public string OpeningTime { get; set; }
        public string CloseTime { get; set; }

        public TblLocation TblLocation { get; set; }
        public ICollection<TblOffer> TblOffer { get; set; }
        public ICollection<TblRating> TblRating { get; set; }
        public ICollection<TblRestaurantDetails> TblRestaurantDetails { get; set; }
        public ICollection<TblRestaurantMenuPrice> TblRestaurantMenuPrice { get; set; }
    }
}
