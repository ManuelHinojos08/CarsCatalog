using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Net.Http;
using System.Net;
using System.Text;
using CarsCatalog.BBL.Services;
using CarsCatalog.BBL.Models;
using System.Threading;
using System.Security.Principal;

namespace CarsCatalog
{
    public class BasicAuthenticationAttribute : AuthorizationFilterAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            if (actionContext.Request.Headers.Authorization == null)
            {
                actionContext.Response = actionContext.Request.
                    CreateResponse(HttpStatusCode.Unauthorized);
            }
            else
            {
                string aToken = actionContext.Request.Headers.Authorization.Parameter;
                string aTokenDecoded = Encoding.UTF8.GetString(Convert.FromBase64String(aToken));
                string[] usernamePassArray = aTokenDecoded.Split(':');
                string userName = usernamePassArray[0];
                string password = usernamePassArray[1];

                UserService uService = new UserService();
                UserLoginModel model = uService.UserAuthentication(userName, password);
                if (model.HasValidAccount)
                {
                    Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(userName), null);
                }
                else
                {
                    actionContext.Response = actionContext.Request.
                    CreateResponse(HttpStatusCode.Unauthorized);
                }
            }
        }
    }
}