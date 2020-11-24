using Backend.Dao;
using Backend.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Backend.Repository
{
    public class UserMasterRepository
    {

        public async Task<User> ValidateUser(string email, string password)
        {
            return await Task.Run(() => UserDao.findAllAsync().Result.FirstOrDefault(user => user.email.Equals(email) && BCrypt.Net.BCrypt.EnhancedVerify(password, user.password)));
        }
    }
}