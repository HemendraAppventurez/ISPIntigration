using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CCSHealthFamilyWelfareDept.Models
{
    public class CancleNUHregistration
    {
        [Required(ErrorMessage = "Please Enter Reason")]
        public string Resion { get; set; }
        public string FilePath { get; set; }
        public string NUHId { get; set; }
        public string hdnNUHId { get; set; }
        public Int64 UserID { get; set; }
        public string Ip { get; set; }
        public int IsSuccess { get; set; }
    }
}