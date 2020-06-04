using System;
using System.Collections.Generic;
using System.Text;
using MT.OnlineRestaurant.DataLayer.EntityFrameWorkModel;

namespace MT.OnlineRestaurant.DataLayer.DataEntity
{
    public class MenuDetails
    {
        public TblOffer tbl_Offer { get; set; }
        public TblMenu tbl_Menu { get; set; }
        public TblCuisine tbl_Cuisine { get; set; }
    }
}
