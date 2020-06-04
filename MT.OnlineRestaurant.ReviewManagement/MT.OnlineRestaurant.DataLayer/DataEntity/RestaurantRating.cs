using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MT.OnlineRestaurant.DataLayer.DataEntity
{
    public class RestaurantRating
    {
        public int RatingId { get; set; }
        public int RestaurantId { get; set; }
        [Required]
        public string rating { get; set; }
        [MaxLength(250)]
        public string user_Comments { get; set; }
    }
}
