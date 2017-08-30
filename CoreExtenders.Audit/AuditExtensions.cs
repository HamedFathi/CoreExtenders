using Audit.Core;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace CoreExtenders
{
    public static class AuditExtensions
    {
        public static AuditScopeResult<T> ExtractObjects<T>(this AuditScope scope, IEqualityComparer<T> comparer)
        {
            if (scope.Event.Target.SerializedOld == null || scope.Event.Target.SerializedNew == null)
                return null;

            var oldOnes = JsonConvert.DeserializeObject<List<T>>(JsonConvert.SerializeObject(scope.Event.Target.SerializedOld));
            var newOnes = JsonConvert.DeserializeObject<List<T>>(JsonConvert.SerializeObject(scope.Event.Target.SerializedNew));

            var updated = newOnes.Intersect(oldOnes, comparer).ToList();
            var deleted = oldOnes.Except(newOnes, comparer).ToList();
            var inserted = newOnes.Except(oldOnes, comparer).ToList();

            var result = new AuditScopeResult<T>();
            result.InsertedObjects.AddRange(inserted);
            result.UpdatedObjects.AddRange(updated);
            result.DeletedObjects.AddRange(deleted);

            return result;
        }
    }
}
