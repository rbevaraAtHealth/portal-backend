namespace CodeMatcherV2Api.Dtos
{
    public abstract class ClientAuditEntity : AuditEntity
    {
        public virtual string ClientId { get; set; }
    }
}
