using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business
{
   
    public enum Database
    {
        [Display(Name= "SQL Server")]
        SQLServer ,
        [Display(Name= "MySQL")]
        MySQL,
     
    }
}
