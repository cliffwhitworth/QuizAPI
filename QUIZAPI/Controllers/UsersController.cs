using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using QuizAPI.Models;
using System.Collections;
using QuizAPI.Sql;

namespace QuizAPI.Controllers
{
    [Authorize]
    public class UsersController : ApiController
    {
        // GET: api/Users        
        public ArrayList Get()
        {
            // return new string[] { "value1", "value2" };
            UsersSql db = new UsersSql();
            return db.getUsers();
        }

        // GET: api/Users/5
        // public String Get(int id)
        public User Get(int id)
        {
            UsersSql db = new UsersSql();
            User user = db.getUser(id);
            // user.Id = id;
            // user.Lastname = "World";
            // user.Firstname = "Hello";

            // return "value";
            return user;
        }

        // POST: api/User
        public HttpResponseMessage Post([FromBody]User user)
        {
            UsersSql db = new UsersSql();
            long id;
            id = db.postUser(user);
            user.Id = (int)id;
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created);
            response.Headers.Location = new Uri(Request.RequestUri, String.Format("user/{0}", id));
            return response;
        }

        // PUT: api/Users/5
        // public void Put(int id, [FromBody]User value)
        public HttpResponseMessage Put(int id, [FromBody]User user)
        {
            UsersSql db = new UsersSql();
            bool updateSuccess = false;
            HttpResponseMessage response;
            updateSuccess = db.putUser(id, user);
            if (updateSuccess)
            {
                response = Request.CreateResponse(HttpStatusCode.NoContent);
            }
            else
            {
                response = Request.CreateResponse(HttpStatusCode.NotFound);
            }

            return response;
        }

        // DELETE: api/Users/5
        // public void Delete(int id)
        public HttpResponseMessage Delete(int id)
        {
            UsersSql db = new UsersSql();
            bool deleteSuccess = false;
            HttpResponseMessage response;
            deleteSuccess = db.deleteUser(id);
            if (deleteSuccess)
            {
                response = Request.CreateResponse(HttpStatusCode.NoContent);
            }
            else
            {
                response = Request.CreateResponse(HttpStatusCode.NotFound);
            }

            return response;

        }
    }
}
