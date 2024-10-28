using System;
using System.Collections.Generic;
using System.Text;

namespace HMS.Dto
{
    public class BaseEntity
    {
        public int Id { get; set; }
    }

    public class Auditable : BaseEntity
    {
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; } = string.Empty;
        public DateTime ModifiedOn { get; set; }
    }
}
