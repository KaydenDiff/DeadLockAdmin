using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeadLockAdmin.Models;

namespace DeadLockAdmin.ViewModels
{
    public static class Data
    {
        public static string Host { get; set; } = "http://deadlock/api";
        public static Character CurrentCharacter { get; set; }
    }
}
