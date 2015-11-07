using System;
using System.Collections.Generic;

namespace RealEstateCompanyWebSite.SQL.Identity
{
    /// <summary>
    /// Class that represents the Users table in the MySQL Database
    /// </summary>
    public class UserTable<TUser>
        where TUser :IdentityUser
    {
        private MySQLDatabase _database;

        /// <summary>
        /// Constructor that takes a MySQLDatabase instance 
        /// </summary>
        /// <param name="database"></param>
        public UserTable(MySQLDatabase database)
        {
            _database = database;
        }

        /// <summary>
        /// Returns the user's name given a user id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public string GetUserName(string userId)
        {
            string commandText = "Select Name from Clients where Client_GUID = @id";
            Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@id", userId } };

            return _database.GetStrValue(commandText, parameters);
        }

        /// <summary>
        /// Returns a User ID given a user name
        /// </summary>
        /// <param name="userName">The user's name</param>
        /// <returns></returns>
        public string GetUserId(string userName)
        {
            string commandText = "Select Id from Clients where UserName = @name";
            Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@name", userName } };

            return _database.GetStrValue(commandText, parameters);
        }

        /// <summary>
        /// Returns an TUser given the user's id
        /// </summary>
        /// <param name="userId">The user's id</param>
        /// <returns></returns>
        public TUser GetUserById(string userId)
        {
            TUser user = null;
            string commandText = "Select * from Clients where Client_GUID = @id";
            Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@id", userId } };

            var rows = _database.Query(commandText, parameters);
            if (rows != null && rows.Count == 1)
            {
                var row = rows[0];
                user = (TUser)Activator.CreateInstance(typeof(TUser));
                user.Id = row["Client_GUID"];
                user.UserName = row["UserName"];
                user.DBID = int.Parse(row["Client_ID"]);
                
                user.PasswordHash = string.IsNullOrEmpty(row["Client_Password"]) ? null : row["Client_Password"];
                user.SecurityStamp = string.IsNullOrEmpty(row["SecurityStamp"]) ? null : row["SecurityStamp"];
                user.Email = string.IsNullOrEmpty(row["Client_Email"]) ? null : row["Client_Email"];
                user.EmailConfirmed = row["EmailConfirmed"] == "1" ? true:false;
                user.PhoneNumber = string.IsNullOrEmpty(row["Client_Phone"]) ? null : row["Client_Phone"];
                user.PhoneNumberConfirmed = row["PhoneNumberConfirmed"] == "1" ? true : false;
                user.LockoutEnabled = row["LockoutEnabled"] == "1" ? true : false;
                user.TwoFactorEnabled = row["TwoFactorEnabled"] == "1" ? true : false;
                user.LockoutEndDateUtc = string.IsNullOrEmpty(row["LockoutEndDateUtc"]) ? DateTime.Now : DateTime.Parse(row["LockoutEndDateUtc"]);
                user.AccessFailedCount = string.IsNullOrEmpty(row["AccessFailedCount"]) ? 0 : int.Parse(row["AccessFailedCount"]);
                user.FirstName = string.IsNullOrEmpty(row["Client_Name"]) ? null : row["Client_Name"];
                user.LastName = string.IsNullOrEmpty(row["Client_Surname"]) ? null : row["Client_Surname"];
            }

            return user;
        }


        /// <summary>
        /// Returns an TUser given the user's email
        /// </summary>
        /// <param name="userId">The user's email</param>
        /// <returns></returns>
        public TUser GetUserByEmail(string email)
        {
            TUser user = null;
            string commandText = "Select * from Clients where Client_Email = @id";
            Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@id", email } };

            var rows = _database.Query(commandText, parameters);
            if (rows != null && rows.Count == 1)
            {
                var row = rows[0];
                user = (TUser)Activator.CreateInstance(typeof(TUser));
                user.Id = row["Client_GUID"];
                user.UserName = row["UserName"];
                user.DBID = int.Parse(row["Client_ID"]);

                user.PasswordHash = string.IsNullOrEmpty(row["Client_Password"]) ? null : row["Client_Password"];
                user.SecurityStamp = string.IsNullOrEmpty(row["SecurityStamp"]) ? null : row["SecurityStamp"];
                user.Email = string.IsNullOrEmpty(row["Client_Email"]) ? null : row["Client_Email"];
                user.EmailConfirmed = row["EmailConfirmed"] == "1" ? true : false;
                user.PhoneNumber = string.IsNullOrEmpty(row["Client_Phone"]) ? null : row["Client_Phone"];
                user.PhoneNumberConfirmed = row["PhoneNumberConfirmed"] == "1" ? true : false;
                user.LockoutEnabled = row["LockoutEnabled"] == "1" ? true : false;
                user.TwoFactorEnabled = row["TwoFactorEnabled"] == "1" ? true : false;
                user.LockoutEndDateUtc = string.IsNullOrEmpty(row["LockoutEndDateUtc"]) ? DateTime.Now : DateTime.Parse(row["LockoutEndDateUtc"]);
                user.AccessFailedCount = string.IsNullOrEmpty(row["AccessFailedCount"]) ? 0 : int.Parse(row["AccessFailedCount"]);
                user.FirstName = string.IsNullOrEmpty(row["Client_Name"]) ? null : row["Client_Name"];
                user.LastName = string.IsNullOrEmpty(row["Client_Surname"]) ? null : row["Client_Surname"];
            }

            return user;
        }

        /// <summary>
        /// Returns a list of TUser instances given a user name
        /// </summary>
        /// <param name="userName">User's name</param>
        /// <returns></returns>
        public List<TUser> GetUserByName(string userName)
        {
            List<TUser> users = new List<TUser>();
            string commandText = "Select * from Clients where UserName = @name";
            Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@name", userName } };

            var rows = _database.Query(commandText, parameters);
            foreach(var row in rows)
            {
                TUser user = (TUser)Activator.CreateInstance(typeof(TUser));
                user.Id = row["Client_GUID"];
                user.DBID = int.Parse(row["Client_ID"]);
                user.UserName = row["UserName"];
                user.PasswordHash = string.IsNullOrEmpty(row["Client_Password"]) ? null : row["Client_Password"];
                user.SecurityStamp = string.IsNullOrEmpty(row["SecurityStamp"]) ? null : row["SecurityStamp"];
                user.Email = string.IsNullOrEmpty(row["Client_Email"]) ? null : row["Client_Email"];
                user.EmailConfirmed = row["EmailConfirmed"] == "1" ? true : false;
                user.PhoneNumber = string.IsNullOrEmpty(row["Client_Phone"]) ? null : row["Client_Phone"];
                user.PhoneNumberConfirmed = row["PhoneNumberConfirmed"] == "1" ? true : false;
                user.LockoutEnabled = row["LockoutEnabled"] == "1" ? true : false;
                user.TwoFactorEnabled = row["TwoFactorEnabled"] == "1" ? true : false;
                user.LockoutEndDateUtc = string.IsNullOrEmpty(row["LockoutEndDateUtc"]) ? DateTime.Now : DateTime.Parse(row["LockoutEndDateUtc"]);
                user.AccessFailedCount = string.IsNullOrEmpty(row["AccessFailedCount"]) ? 0 : int.Parse(row["AccessFailedCount"]);
                user.FirstName = string.IsNullOrEmpty(row["Client_Name"]) ? null : row["Client_Name"];
                user.LastName = string.IsNullOrEmpty(row["Client_Surname"]) ? null : row["Client_Surname"];




                users.Add(user);
            }

            return users;
        }

        /// <summary>
        /// Return the user's password hash
        /// </summary>
        /// <param name="userId">The user's id</param>
        /// <returns></returns>
        public string GetPasswordHash(string userId)
        {
            string commandText = "Select Client_Password from Clients where Client_GUID = @id";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@id", userId);

            var passHash = _database.GetStrValue(commandText, parameters);
            if(string.IsNullOrEmpty(passHash))
            {
                return null;
            }

            return passHash;
        }

        /// <summary>
        /// Sets the user's password hash
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="passwordHash"></param>
        /// <returns></returns>
        public int SetPasswordHash(string userId, string passwordHash)
        {
            string commandText = "Update Clients set Client_Password = @pwdHash where Client_GUID = @id";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@pwdHash", passwordHash);
            parameters.Add("@id", userId);

            return _database.Execute(commandText, parameters);
        }

        /// <summary>
        /// Returns the user's security stamp
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public string GetSecurityStamp(string userId)
        {
            string commandText = "Select SecurityStamp from Clients where Client_GUID = @id";
            Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@id", userId } };
            var result = _database.GetStrValue(commandText, parameters);

            return result;
        }

        /// <summary>
        /// Inserts a new user in the Users table
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public int Insert(TUser user)
        {
            try
            {
                var emailU = GetUserByEmail(user.Email);
                if (emailU != null)
                    throw new Exception("Email already exists");
            }
            catch
            {
                throw new Exception("Email already exists");
            }
            string commandText = @"Insert into Clients (UserName, Client_GUID, Client_Password, SecurityStamp,Client_Email,EmailConfirmed,Client_Phone,PhoneNumberConfirmed, AccessFailedCount,LockoutEnabled,LockoutEndDateUtc,TwoFactorEnabled, Client_Name, Client_Surname)
                values (@name, @id, @pwdHash, @SecStamp,@email,@emailconfirmed,@phonenumber,@phonenumberconfirmed,@accesscount,@lockoutenabled,@lockoutenddate,@twofactorenabled, @firstname, @lastname)";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@name", user.UserName);
            parameters.Add("@id", user.Id);
            parameters.Add("@pwdHash", user.PasswordHash);
            parameters.Add("@SecStamp", user.SecurityStamp);
            parameters.Add("@email", user.Email);
            parameters.Add("@emailconfirmed", user.EmailConfirmed);
            parameters.Add("@phonenumber", user.PhoneNumber);
            parameters.Add("@phonenumberconfirmed", user.PhoneNumberConfirmed);
            parameters.Add("@accesscount", user.AccessFailedCount);
            parameters.Add("@lockoutenabled", user.LockoutEnabled);
            parameters.Add("@lockoutenddate", user.LockoutEndDateUtc);
            parameters.Add("@twofactorenabled", user.TwoFactorEnabled);
            parameters.Add("@firstname", user.FirstName);
            parameters.Add("@lastname", user.LastName);

            return _database.Execute(commandText, parameters);
        }

        /// <summary>
        /// Deletes a user from the Users table
        /// </summary>
        /// <param name="userId">The user's id</param>
        /// <returns></returns>
        private int Delete(string userId)
        {
            string commandText = "Delete from Clients where Client_GUID = @userId";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@userId", userId);

            return _database.Execute(commandText, parameters);
        }

        /// <summary>
        /// Deletes a user from the Users table
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public int Delete(TUser user)
        {
            return Delete(user.Id);
        }

        /// <summary>
        /// Updates a user in the Users table
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public int Update(TUser user)
        {
            string commandText = @"Update Clients set UserName = @userName, PasswordHash = @pswHash, SecurityStamp = @secStamp, 
                Email=@email, EmailConfirmed=@emailconfirmed, PhoneNumber=@phonenumber, PhoneNumberConfirmed=@phonenumberconfirmed,
                AccessFailedCount=@accesscount, LockoutEnabled=@lockoutenabled, LockoutEndDateUtc=@lockoutenddate, TwoFactorEnabled=@twofactorenabled,
                FirstName = @firstname, LastName = @lastname 
                WHERE Id = @userId";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@userName", user.UserName);
            parameters.Add("@pswHash", user.PasswordHash);
            parameters.Add("@secStamp", user.SecurityStamp);
            parameters.Add("@userId", user.Id);
            parameters.Add("@email", user.Email);
            parameters.Add("@emailconfirmed", user.EmailConfirmed);
            parameters.Add("@phonenumber", user.PhoneNumber);
            parameters.Add("@phonenumberconfirmed", user.PhoneNumberConfirmed);
            parameters.Add("@accesscount", user.AccessFailedCount);
            parameters.Add("@lockoutenabled", user.LockoutEnabled);
            parameters.Add("@lockoutenddate", user.LockoutEndDateUtc);
            parameters.Add("@twofactorenabled", user.TwoFactorEnabled);
            parameters.Add("@firstname", user.FirstName);
            parameters.Add("@lastname", user.LastName);

            return _database.Execute(commandText, parameters);
        }
    }
}
