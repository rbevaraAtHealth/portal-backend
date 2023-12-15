﻿namespace CodeMatcherV2Api.Models
{
    public class LoginModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public string ClientId { get; set; }
        public bool IsApiKeyExist { get; set; }
    }
}
