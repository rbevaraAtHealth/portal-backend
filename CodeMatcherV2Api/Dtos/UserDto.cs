using System.Collections.Generic;

namespace CodeMatcherV2Api.Dtos
{
    public class UserDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<string> Roles { get; set; }
        public string Email { get; set; }
    }
}
