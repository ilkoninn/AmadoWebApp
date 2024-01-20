using AmadoApp.Core.Entities.BaseEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmadoApp.Core.Entities
{
    public class Subscribe : BaseAuditableEntity
    {
        public string EmailAddress { get; set; }
    }
}
