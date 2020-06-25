using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace SessionControl.Models
{
    public class Session
    {
        [Key]
        public string Id { get; set; }
        public string Status { get; set; }
        public string UserAdress { get; set; }
        public string IdVIdeo { get; set; }
        public int RequestTime { get; set; }
    }
}
