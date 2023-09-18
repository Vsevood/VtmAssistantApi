using System.IdentityModel.Tokens.Jwt;

namespace Auth
{
    public class UserData
    {
        //public int id { get; set; }
        public string name { get; set; }
        public string accsess_token { get; set; }

        public UserData(string name1, string accsess_token1)
        {
            //id = id1;
            name = name1;
            accsess_token = accsess_token1;
        }
    }
}
