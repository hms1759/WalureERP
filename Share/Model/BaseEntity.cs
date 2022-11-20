using Share.Extensions;
using Share.Utils.GUIDs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Model
{
    public interface IEntity
    {
        Guid Id { get; set; }
    }

    public interface IDateAudit
    {
        DateTime CreatedOn { get; set; }
        DateTime? ModifiedOn { get; set; }
    }

    public interface IActorAudit
    {
        string? CreatedBy { get; set; }
        string? ModifiedBy { get; set; }
    }

    public interface ISoftDelete
    {
        bool IsDeleted { get; set; }
        DateTime? DeletedOn { get; set; }
    }

    public interface IDeleteActorAudit : ISoftDelete
    {
        string? DeletedBy { get; set; }
    }

    public interface IAudit : IDateAudit, IActorAudit
    {
    }

    public interface IFullAudit : IAudit, IDeleteActorAudit
    {
    }

    public abstract class Entity : IEntity
    {
        public virtual Guid Id { get; set; }
    }

    public abstract class AuditedEntity : Entity, IAudit
    {
        public DateTime CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }

        public string? CreatedBy { get; set; }
        public string? ModifiedBy { get; set; }
    }

    public abstract class BaseEntity : AuditedEntity, IFullAudit
    {
        public BaseEntity()
        {
            Id = SequentialGuidGenerator.Instance.Create();
            CreatedOn = DateTime.Now.GetDateUtcNow().ToLocalTime();
        }

        public string? DeletedBy { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedOn { get; set; }
    }
}