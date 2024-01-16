using System;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Text.Encodings.Web;
using System.Net.Http.Headers;
using System.Text;
using WebAPIProduco.Model;
using WebAPIProduco.Data; 
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace WebAPIProduct.Handler
{
    public class Authentication : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly DataContext _dbContext;
        public Authentication(IOptionsMonitor<AuthenticationSchemeOptions> option, ILoggerFactory logger,
        UrlEncoder encoder, ISystemClock clock, DataContext dbContext) : base(option, logger, encoder, clock)
        {
            _dbContext = dbContext;
        }
        protected async override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey("Authorization"))
                return AuthenticateResult.Fail("No header found");

            var _haedervalue = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
            var bytes = Convert.FromBase64String(_haedervalue.Parameter!=null?_haedervalue.Parameter:string.Empty);
            string credentials = Encoding.UTF8.GetString(bytes);
            if (!string.IsNullOrEmpty(credentials))
            {
                string[] array = credentials.Split(":");
                string username = array[0];
                string password = array[1];
                 var user=await this._dbContext.Forms.FirstOrDefaultAsync(v=>v.Email==username);
                 if(user==null)
                   return AuthenticateResult.Fail("UnAuthorized");

           // Generate Ticket
           var claim=new[]{new Claim(ClaimTypes.Name,username)};
           var identity=new ClaimsIdentity(claim,Scheme.Name);
           var principal=new ClaimsPrincipal(identity);
           var ticket=new AuthenticationTicket(principal,Scheme.Name);

                return AuthenticateResult.Success(ticket);
            }else{
                return AuthenticateResult.Fail("UnAuthorized");

            }
        }
    }
}