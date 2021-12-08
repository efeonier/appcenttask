using System;

namespace TodoAPP.Core.Entity
{
    public abstract class TaskItem : CouchbaseEntity<TaskItem>
    {
        public string Name { get; set; }
        public bool IsCompleted { get; set; }
        public bool IsImportant { get; set; }
        public DateTime? Deadline { get; set; }
    }
}