using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Ocsp;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection.PortableExecutable;
using System.Security.Claims;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Xml.Linq;

namespace Auth.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class VtmAuthController : Controller
    {
        [HttpPost]
        [Route("/Login")]
        public ActionResult<UserData> Post(LoginData User)
        {
            //поиск пользователя
            var reader = Connection.ConnectToMySQL(String.Format("SELECT * FROM user WHERE username='{0}'", User.login));

            string log = "";
            string pass= "";
            while (reader.Read())
            {
                log = reader[0].ToString();
                pass = reader[1].ToString();
                //Console.WriteLine("id={0}, value={1}", log, pass);
            }
            reader.Close();

            if (User.login.Equals(log) && User.password.Equals(pass))
            {
                //токен
                //var claims = new List<Claim> { new Claim(ClaimTypes.Name, User.login) };
                var claims = new List<Claim> { new Claim(ClaimTypes.Name, User.login) };
                // создаем JWT-токен
                var jwt = new JwtSecurityToken(
                        issuer: AuthOptions.ISSUER,
                        audience: AuthOptions.AUDIENCE,
                        claims: claims,
                        expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(2)),
                        signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));


                UserData data = new UserData(log, new JwtSecurityTokenHandler().WriteToken(jwt));
                return Ok(data);
            }
            else
            {
                var errorData = new { Message = "Custom Error Message", ErrorCode = 123 };
                return BadRequest(errorData);
            }

        }
        
        [HttpPost]
        [Route("/Reg")]
        public ActionResult Post1(LoginData User)
        {
            var reader = Connection.ConnectToMySQL(String.Format("SELECT * FROM user WHERE username='{0}'", User.login));

            string log = "";
            string pass = "";
            while (reader.Read())
            {
                log = reader[0].ToString();
                pass = reader[1].ToString();
                Console.WriteLine("id={0}, value={1}", log, pass);
            }
            reader.Close();


            if (User.login.Equals(log))
            {
                return BadRequest();
            }
            else
            {
                Connection.ConnectToMySQL(String.Format("INSERT INTO user (username, password) VALUES('{0}', '{1}'); ", User.login, User.password));
                return Ok();
            }
        }
        

        [HttpDelete]
        [Authorize]
        [Route("/Delete")]
        public ActionResult Delete() 
        {
            string token = Request.Headers["Authorization"];
            string[] t1 = token.Split(' ');
            
            var handler = new JwtSecurityTokenHandler();
            JwtSecurityToken jwt = (JwtSecurityToken)handler.ReadToken(t1[1]);
            var claims = jwt.Claims.ToArray();

            Connection.ConnectToMySQL(String.Format("DELETE FROM user WHERE username='{0}';", claims[0].Value));

            if (1 == 1)
                return Ok();
            else
                return BadRequest();
        } 

    }
}
