namespace CodeMappingEfCore.DatabaseModels
{
    public abstract class ClientAuditEntity : AuditEntity
    {
        public virtual string ClientId { get; set; } = null!;
    }
}
