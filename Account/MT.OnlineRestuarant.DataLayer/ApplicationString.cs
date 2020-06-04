using System;
using System.Collections.Generic;
using System.Text;

namespace MT.OnlineRestuarant.DataLayer
{
   public class ApplicationString
    {
        private string _Db = "Data Source=vs2019vm\\SQLEXPRESS;Initial Catalog=AccountManagement;Integrated Security=True;";
        public string DB
        {
            get
            {
                return _Db;
            }
            set
            {
                _Db = value;
            }
        }
    }
}
