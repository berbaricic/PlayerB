using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SessionLibrary
{
    public class Session
    {
        [Key]
        public string Id { get; set; }
        public string Status { get; set; }
        public string UserAdress { get; set; }
        public string IdVideo { get; set; }
        public int RequestTime { get; set; }
    }
}
