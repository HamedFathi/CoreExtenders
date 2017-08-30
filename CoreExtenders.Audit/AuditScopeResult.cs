using System.Collections.Generic;

namespace CoreExtenders
{
    public class AuditScopeResult<T>
    {
        public List<T> DeletedObjects { get; set; }

        public List<T> InsertedObjects { get; set; }

        public List<T> UpdatedObjects { get; set; }

        public AuditScopeResult()
        {
            InsertedObjects = new List<T>();
            UpdatedObjects = new List<T>();
            DeletedObjects = new List<T>();
        }
    }
}
