using Microsoft.VisualBasic;
using System;

namespace CodeMappingEfCore.DatabaseModels
{
    public abstract class AuditEntity
    {
        //public virtual int Id { get; set; }
        public virtual string CreatedBy { get; set; }
        public DateTime CreatedTime { get; set; } = DateTime.Now;
        public virtual string? ModifiedBy { get; set; }
        public virtual DateTime? ModifiedTime { get; set; }

        public virtual bool IsDeleted { get; set; } = false;
    }
}
