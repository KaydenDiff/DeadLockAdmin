using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeadLockAdmin.Models
{
    public class Role
    {
        public long Id { get; set; }
        public string RoleCode { get; set; }
        public string RoleName { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

}
