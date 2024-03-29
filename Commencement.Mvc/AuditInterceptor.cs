﻿using System;
using System.Security.Principal;
using Commencement.Core.Domain;
using Commencement.Core.Helpers;
using NHibernate;
using UCDArch.Core.PersistanceSupport;

namespace Commencement
{
    public class AuditInterceptor : EmptyInterceptor
    {
        public IPrincipal Principal;
        public IRepository<Audit> AuditRepository { get; set; }

        public AuditInterceptor(IPrincipal principal, IRepository<Audit> auditRepository)
        {
            Principal = principal;
            AuditRepository = auditRepository;
        }

        public override void OnDelete(object entity, object id, object[] state, string[] propertyNames, NHibernate.Type.IType[] types)
        {
            AuditObjectModification(entity, id, AuditActionType.Delete);

            base.OnDelete(entity, id, state, propertyNames, types);
        }

        public override bool OnSave(object entity, object id, object[] state, string[] propertyNames, NHibernate.Type.IType[] types)
        {
            AuditObjectModification(entity, id, AuditActionType.Create);

            return base.OnSave(entity, id, state, propertyNames, types);
        }

        public override bool OnFlushDirty(object entity, object id, object[] currentState, object[] previousState, string[] propertyNames, NHibernate.Type.IType[] types)
        {
            AuditObjectModification(entity, id, AuditActionType.Update);

            return base.OnFlushDirty(entity, id, currentState, previousState, propertyNames, types);
        }

        public void AuditObjectModification(object entity, object id, AuditActionType auditActionType)
        {
            if (entity is Audit) return;

            var audit = new Audit
            {
                AuditDate = DateTime.UtcNow.ToPacificTime(),
                ObjectName = entity.GetType().Name,
                ObjectId = id == null ? null : id.ToString()
            };

            try
            {
                audit.Username = string.IsNullOrEmpty(Principal.Identity.Name) ? "NoUser" : Principal.Identity.Name;
            }
            catch
            {
                audit.Username = "NoUser";
            }

            audit.SetActionCode(auditActionType);

            AuditRepository.EnsurePersistent(audit, false /*Do not force save*/, false /*Do not flush changes since we are in the middle of other actions*/);
        }
    }
}