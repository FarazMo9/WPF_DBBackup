using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Entities
{
    [Table(nameof(BackupLog))]
    public partial class Config
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        [Required]
        public int Interval { get; set; }

        public long HostSize { get; set; }

        public string? FTPEncodedUrl { get; set; }

        public string? FTPEncodedUsername { get; set; }

        public string? FTPEncodedPassword { get; set; }



       
    }
}
