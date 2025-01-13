using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeadLockAdmin.Models
{
    public class User
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Login { get; set; }
        public string ApiToken { get; set; }
        public string Password { get; set; }
        public long? RoleId { get; set; }
        public string RememberToken { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

}
