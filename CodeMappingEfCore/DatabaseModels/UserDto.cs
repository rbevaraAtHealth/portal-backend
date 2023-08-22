using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CodeMappingEfCore.DatabaseModels
{
    public class UserDto : AuditEntity
    {
        [Key]
        public int Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Roles { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string ClientId { get; set; } = null!;
    }
}
