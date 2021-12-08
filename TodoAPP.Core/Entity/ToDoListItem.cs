using System.Collections.Generic;

namespace TodoAPP.Core.Entity
{
    public abstract class ToDoListItem: CouchbaseEntity<ToDoListItem>
    {
        public string Name { get; set; }
        public List<TaskItem> Tasks { get; set; } = new();
    }
}