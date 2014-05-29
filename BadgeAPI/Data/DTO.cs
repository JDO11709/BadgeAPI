using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using MongoDB.Driver;
using MongoDB.Bson;


namespace BadgeAPI.Data
{
    //[TypeScriptInterface(Module = "HiveAdmin.Api.Data")]
    public class LoginCredential
    {
        public string UserName { get; set; }
        public string Password { get; set; }

        public bool AreValid
        {
            get
            {
                return UserName != null && Password != null;
            }
        }
    }

    //[TypeScriptInterface(Module = "Badge.Api.Data")]
    public class LoginResult
    {
        public bool Authenticated { get; set; }
        public string Token { get; set; }
    }

    public class Badge
    {
        public int _id { get; set; }
        public bool IsAssessment { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public string Description { get; set; }
        public int RetakeIntervalinDays { get; set; }
        public SeenOnlyBy SeenOnlyBy { get; set; }
        public Credential Credential { get; set; }
        public List<object> Tags { get; set; }
    }
    public class SeenOnlyBy
    {
        public List<object> CandidateFollowingOrganizationId { get; set; }
        public List<object> OrganizationId { get; set; }
    }
    public class Credential
    {
        public int Provider_id { get; set; }
        public string CredentialUrl { get; set; }
        public int PassingScore { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class CredentialAttempt
    {
        public ObjectId _id { get; set; }
        public string ETicket { get; set; }
        public ProfileInfo ProfileInfo { get; set; }
        public string Result { get; set; }
        public string TestID { get; set; }
        public TimeStamps TimeStamps { get; set; }
    }
    public class ProfileInfo
    {
        public string Profile_IdAsString { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    public class TimeStamps
    {
        public DateTime ETicketReceived { get; set; }
        public DateTime Initiated { get; set; }
        public DateTime ResultReceived { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class Auth
    {
        public int _id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public List<string> Roles { get; set; }
    }

}