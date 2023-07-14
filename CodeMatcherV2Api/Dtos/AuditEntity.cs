using Microsoft.VisualBasic;
using System;

namespace CodeMatcherV2Api.Dtos
{
    public abstract class AuditEntity
    {
        public virtual int Id { get; set; }

        public virtual string CreatedBy { get; set; }

        private DateTime created;

        public virtual DateTime? CreatedTime
        { 
            get { return created; }
            private set { created = DateTime.Now; } 
        }

        public virtual string ModifiedBy { get; set; }

        private DateTime modified;

        public virtual DateTime? ModifiedTime
        {
            get { return modified; }
            private set { modified = DateTime.Now; }
        }

        public virtual bool? IsDeleted { get; set; }
    }
}
