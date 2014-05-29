using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Linq;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

using BadgeAPI.Data;

namespace BadgeAPI.Data
{
    public class QueryableDependencies
    {
        private static MongoDatabase _badgeApiDB = null;
        private static MongoDatabase BadgeApiDB
        {
            get
            {
                _badgeApiDB = GetMongoDB("credential");
                return _badgeApiDB;
            }
        }
        private static MongoDatabase GetMongoDB(string dbName)
        {
            var client = new MongoClient(ConfigurationManager.AppSettings["MongoConnection"]);
            var server = client.GetServer();

            return server.GetDatabase(dbName);
        }
        
        public static List<string> GetLoginUserRoles(string userName, string password)
        {
            string pswd = "";

            SHA1 sha1 = new SHA1CryptoServiceProvider();
            byte[] hash =
            sha1.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

            foreach (byte b in hash)
                pswd += String.Format("{0,2:X2}", b);

            var collection = BadgeApiDB.GetCollection<Auth>("Auth");
            var query = Query.And(
                    Query.EQ("UserName", userName),
                    Query.EQ("Password", pswd.ToLower())
                );
            var result = collection.FindOne(query);

            return result.Roles;
        }
        public static List<Badge> GetBadgeList()
        {
            var collection = BadgeApiDB.GetCollection<Badge>("Badge");
            var result = collection.FindAll();

            return result.ToList();
        }
        public static string InsertProfileInfo(ProfileInfo pi)
        {
            
            CredentialAttempt ca = new CredentialAttempt();
            //Need to created ETicket???????
            ca.ETicket = Guid.NewGuid().ToString(); 
            ca.ProfileInfo = pi;
            TimeStamps ts = new TimeStamps(){Initiated = DateTime.UtcNow};
            ca.TimeStamps = ts;

            var newprofileinfo = BadgeApiDB.GetCollection("CredentialAttempt");
            try 
            {
                newprofileinfo.Insert(ca);
            }
            catch (Exception ex)
            {
                // To do find the result and return????
            }

            return ca.ToJson();
        }
        public static string UpdateCredentialAttemptResult(CredentialAttempt ca)
        {
            var collection = BadgeApiDB.GetCollection<CredentialAttempt>("CredentialAttempt");
            var query = Query.EQ("ETicket", ca.ETicket);
            var result = collection.FindOne(query);

            if (result != null)
            {
                var update = Update<CredentialAttempt>.Set(e => e.Result, ca.Result); // update modifiers
                update.Set(e => e.TimeStamps.ResultReceived, DateTime.UtcNow);
                collection.Update(query, update);
                return result.ToJson();
            }
            else
            {
                //CredentialAttempt ca = new CredentialAttempt();
                //Need to created ETicket???????
                ca.ETicket = ca.ETicket;
                TimeStamps ts = new TimeStamps() { Initiated = DateTime.UtcNow, ResultReceived = DateTime.UtcNow };
                ca.TimeStamps = ts;

                var newprofileinfo = BadgeApiDB.GetCollection("CredentialAttempt");
                newprofileinfo.Insert(ca);
                return ca.ToJson();
            }

            
        }
        public static string UpdateAuth(Auth au)
        {
            var collection = BadgeApiDB.GetCollection<Auth>("Auth");
            var query = Query.EQ("UserName", au.UserName);
            var result = collection.FindOne(query);
            if (result != null)
            {
                var update = Update<Auth>.Set(e => e.Password, au.Password);
                update.Set(e => e.Roles , au.Roles);
                collection.Update(query, update);
                return result.ToJson();
            }
            else
            {
                var newauth = BadgeApiDB.GetCollection("Auth");
                newauth.Insert(au);
                return au.ToJson();
            }
        }
    }
}