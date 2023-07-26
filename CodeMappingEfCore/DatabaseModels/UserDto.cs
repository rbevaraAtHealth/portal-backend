using System.Collections.Generic;

namespace CodeMappingEfCore.DatabaseModels
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
