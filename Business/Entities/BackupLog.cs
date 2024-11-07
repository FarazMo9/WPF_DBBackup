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
    public class BackupLog
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        [Required]
        public int DatabaseInfoID { get; set; }
        [Required]
        public DateTime Date { get; set; }
        public bool IsSuccessful { get; set; }
        public string? Message { get; set; }
        public virtual DatabaseInfo DatabaseInfo { get; set; }
    }
}
