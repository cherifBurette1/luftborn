using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Newtonsoft.Json;
using luftborn.Data;
using luftborn.Data.AuditEntites;
using luftborn.Data.Entities;
using luftborn.Ground;
using luftborn.Service.Features.Common.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using luftborn.Service.Features.Common.Interfaces;

namespace luftborn.Presistance
{
    public class luftbornEntities : DbContext, IluftbornEntities
    {
        private readonly IAuditRepository _auditRepository;

        public luftbornEntities(DbContextOptions<luftbornEntities> options,
            IAuditRepository auditRepository)
            : base(options)
        {
            _auditRepository = auditRepository;
        }
        private IAuditRepository AuditRepository => _auditRepository;

        #region DBSets
        public virtual DbSet<Employee> Employees { get; set; }
        #endregion
        public new bool SaveChanges(bool enableAuditLog = true)
        {
            this.ChangeTracker.DetectChanges();

            try
            {
                // Avoid automatically detecting changes again here
                this.ChangeTracker.AutoDetectChangesEnabled = false;


                bool trackerHasChanges = this.ChangeTracker.HasChanges();


                int changesSavedToDb = base.SaveChanges();

                //this condition is used to return true if no changes occured in the entity
                //ex. if the entity properties updated with the same value, then no changes will be sent to DB,
                //in this case we need to return true, not false
                return changesSavedToDb > 0 || !trackerHasChanges;
            }
            finally
            {
                this.ChangeTracker.AutoDetectChangesEnabled = true;
            }
        }
        public bool GetLazyLoadingEnabledFlag()
        {
            return ChangeTracker.LazyLoadingEnabled;
        }
        public int SaveChangesMigration()
        {
            return base.SaveChanges();
        }
        public async Task<bool> SaveChangesAsync(bool enableAuditLog = true, CancellationToken cancellationToken = default)
        {
            this.ChangeTracker.DetectChanges();

            try
            {
                // Avoid automatically detecting changes again here
                this.ChangeTracker.AutoDetectChangesEnabled = false;

                AddMetadataToAddedOrUpdatedEntites();

                bool trackerHasChanges = this.ChangeTracker.HasChanges();

                LogEntityChanges(trackerHasChanges, enableAuditLog);

                int changesSavedToDb = await base.SaveChangesAsync(cancellationToken);

                //this condition is used to return true if no changes occured in the entity
                //ex. if the entity properties updated with the same value, then no changes will be sent to DB,
                //in this case we need to return true, not false
                return changesSavedToDb > 0 || !trackerHasChanges;
            }
            finally
            {
                this.ChangeTracker.AutoDetectChangesEnabled = true;
            }
        }
        public async Task ExecuteQuery(string queryString)
        {
            await Task.Run(() =>
            {
                IDbConnection connection = GetDbContextConnection();
                IDbCommand command = connection.CreateCommand();
                command.CommandText = $"{queryString};";
                command.ExecuteNonQuery();
            });
        }
        public IDbConnection GetDbContextConnection()
        {
            var connection = /*DbContext*/Database.GetDbConnection();
            if (connection.State == ConnectionState.Open)
            {
                return connection;
            }
            connection.Open();
            return connection;
        }
        private void AddMetadataToAddedOrUpdatedEntites()
        {
            var now = DateTime.UtcNow;
            var minDate = DateTime.MinValue;

            ;

            foreach (var entry in this.ChangeTracker.Entries<IEntityTracker>())
            {
                var entity = entry.Entity;
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Property(e => e.CreateDate).CurrentValue = now;
                        entry.Property(e => e.UpdateDate).CurrentValue = now;
                        break;

                    case EntityState.Modified:
                        entry.Property(e => e.UpdateDate).CurrentValue = now;
                        break;
                }
            }
        }
        private void LogEntityChanges(bool trackerHasChanges, bool enableAuditLog = true)
        {
            if (enableAuditLog && trackerHasChanges)
                AuditRepository.LogEntityChanges(GetAuditLogs());
        }

        private AuditActionTypes GetAuditActionTypeForEntity(EntityState entityState)
        {
            AuditActionTypes actionType = AuditActionTypes.Unknown;
            switch (entityState)
            {
                case EntityState.Added:
                    actionType = AuditActionTypes.Create;
                    break;
                case EntityState.Deleted:
                    actionType = AuditActionTypes.Delete;
                    break;
                case EntityState.Modified:
                    actionType = AuditActionTypes.Update;
                    break;
            }

            return actionType;
        }
        private string GetTableName(EntityEntry entry)
        {
            string entityFullName;
            var entityType = entry.Entity.GetType();
            if (entityType.FullName.StartsWith("Castle.Proxies."))
                entityFullName = entityType.BaseType.FullName;
            else
                entityFullName = entityType.FullName;

            var entityMetadata = this.Model.FindEntityType(entityFullName);
            return entityMetadata == null ? entityType.Name : $"[{entityMetadata.GetSchema() ?? "dbo"}].[{entityMetadata.GetTableName()}]";
        }
        private List<EntityAuditLog> GetAuditLogs()
        {
            //prepare lists
            var auditLogs = new List<EntityAuditLog>();
            var keyValues = new Dictionary<string, object>();
            var oldValues = new Dictionary<string, object>();
            var newValues = new Dictionary<string, object>();
            var modifiedColumns = new List<string>();
            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.Entity is EntityAuditLog || entry.State == EntityState.Detached || entry.State == EntityState.Unchanged)
                    continue;

                //add main data
                var auditEntry = new EntityAuditLog
                {
                    TableName = GetTableName(entry),
                    ActionType = GetAuditActionTypeForEntity(entry.State)
                };

                //primary keys, added, modified, and deleted entities
                foreach (var property in entry.Properties)
                {
                    string propertyName = property.Metadata.Name;
                    if (property.Metadata.IsPrimaryKey())
                    {
                        keyValues[propertyName] = property.CurrentValue;
                        continue;
                    }

                    switch (entry.State)
                    {
                        case EntityState.Added:
                            newValues[propertyName] = property.CurrentValue;
                            break;
                        case EntityState.Deleted:
                            oldValues[propertyName] = property.OriginalValue;
                            break;
                        case EntityState.Modified:
                            if (property.IsModified)
                            {
                                modifiedColumns.Add(propertyName);
                                oldValues[propertyName] = property.OriginalValue;
                                newValues[propertyName] = property.CurrentValue;
                            }
                            break;
                    }
                }

                auditEntry.PrimaryKey = JsonConvert.SerializeObject(keyValues);
                auditEntry.OldValues = oldValues.Count == 0 ? null : JsonConvert.SerializeObject(oldValues);
                auditEntry.NewValues = newValues.Count == 0 ? null : JsonConvert.SerializeObject(newValues);
                auditEntry.ModifiedColumns = modifiedColumns.Count == 0 ? null : JsonConvert.SerializeObject(modifiedColumns);

                auditLogs.Add(auditEntry);

                //clear entity data
                keyValues.Clear();
                oldValues.Clear();
                newValues.Clear();
                modifiedColumns.Clear();
            }

            return auditLogs;
        }
    }
}