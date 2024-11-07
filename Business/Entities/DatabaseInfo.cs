using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Entities
{
    [Table(nameof(DatabaseInfo))]
    public partial class DatabaseInfo
    {
        public DatabaseInfo()
        {
            BackupLogs = new HashSet<BackupLog>();
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public Database Database { get; set; }
        [Required]
        [MaxLength(1024)]
        public string ConnectionString { get; set; }
        public ICollection<BackupLog> BackupLogs { get; set; }


    }
}
