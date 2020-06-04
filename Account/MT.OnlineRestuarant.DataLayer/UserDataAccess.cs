using Microsoft.Extensions.Options;
using MT.OnlineRestaurant.DataLayer.Context;
using MT.OnlineRestaurant.DataLayer.Interfaces;
using MT.OnlineRestuarant.DataLayer;
using System.Linq;
using System.Text;

namespace MT.OnlineRestaurant.DataLayer
{
    /// <summary>
    /// Data Access Layer User
    /// </summary>
    public class UserDataAccess : IUserDataAccess
    {
        private readonly IOptions<ApplicationString> _connectionStrings;

        private readonly CustomerManagementContext _context;
        private readonly IOptions<ApplicationString> _dbSettings;

        public UserDataAccess(IOptions<ApplicationString> dbSettings)
        {
            _connectionStrings = dbSettings;
            _context = new CustomerManagementContext(_connectionStrings.Value.DB);
        }



        public TblCustomer UserLogin(string username, string password)
        {
            var Password = _context.TblCustomer.FirstOrDefault(t => t.Email == username).Password;
            var PasswordKey = _context.TblCustomer.FirstOrDefault(t => t.Email == username).PasswordKey;
            byte[] convpwdkey = System.Text.Encoding.UTF8.GetBytes(PasswordKey);
            if (Password != null)
            {
                using (var hmac = new System.Security.Cryptography.HMACSHA512(convpwdkey))
                {
                    var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                   
                    for (int i = 0; i < computedHash.Length; i++)
                    {
                     // byte[] bytes = Encoding.ASCII.GetBytes(Password[i]);
                        //System.Text.Encoding.UTF8.GetString(computedHash[i]);
                        if (System.Text.Encoding.UTF8.GetString(computedHash)== Password)
                            return null;
                    }
                }
            }
            return _context.TblCustomer.FirstOrDefault(t => t.Email == username);
        }
    }
}
