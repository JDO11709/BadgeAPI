using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

//using BadgeAPI.Model.Badge;
using BadgeAPI.Data;
using BadgeAPI.APIException;

using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

using AttributeRouting.Web.Http;

namespace BadgeAPI.Controllers
{
    public class BadgeController : ApiController
    {
        [Authorize(Roles = "TheHive")]
        [Route("api/Badge")]
        [HttpGet]
        public List<Badge> GetBadge()
        {
            var result = QueryableDependencies.GetBadgeList();
            if (result == null)
            {
                var resp = new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    Content = new StringContent(string.Format("No Badge list found")),
                    ReasonPhrase = "Badge List Not Found"
                };
                throw new HttpResponseException(resp);
            }
            return result;
        }

        [Authorize(Roles = "TheHive")]
        [Route("api/Badge/{badgeid}")]
        [HttpGet]
        public List<Badge> GetBadge(int badgeid)
        {
            var result = QueryableDependencies.GetBadgeList().Where(u => u._id == badgeid).ToList(); 
            if (result == null)
            {
                var resp = new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    Content = new StringContent(string.Format("No badge with ID = {0}", badgeid.ToString())),
                    ReasonPhrase = "Badge ID Not Found"
                };
                throw new HttpResponseException(resp);
            }
            return result;
        }

        [Authorize(Roles = "TheHive")]
        [Route("api/RequestETicket")]
        [HttpPost]
        public HttpResponseMessage PostRequestETicket(ProfileInfo pi)
        {
            if (ModelState.IsValid)
            {
                var result = QueryableDependencies.InsertProfileInfo(pi);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }

        [Authorize(Roles = "CredentialSystem")]
        [Route("api/RecordAttemptResult")]
        [HttpPost]
        public HttpResponseMessage PostRecordAttemptResult(CredentialAttempt ca)
        {
            if (ModelState.IsValid)
            {
                var result = QueryableDependencies.UpdateCredentialAttemptResult(ca);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }

        [Authorize(Roles = "CredentialSystem")]
        [Route("api/AuthUpdate")]
        [HttpPost]
        public HttpResponseMessage PostAuth(Auth au)
        {
            if (ModelState.IsValid)
            {
                var result = QueryableDependencies.UpdateAuth(au);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }
    }
}