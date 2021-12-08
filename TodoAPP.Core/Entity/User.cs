using System.Collections.Generic;

namespace TodoAPP.Core.Entity
{
    public class User : CouchbaseEntity<User>
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public List<ToDoListItem> Projects { get; set; } = new();
    }
}