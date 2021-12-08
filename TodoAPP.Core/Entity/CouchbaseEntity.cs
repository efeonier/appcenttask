using System;

namespace TodoAPP.Core.Entity
{
    public abstract class CouchbaseEntity<T>
    {
        private Guid _id { get; set; }
        public Guid Id
        {
            get
            {
                if (_id == Guid.Empty)
                    _id = Guid.NewGuid();
                return _id;
            }
            set => _id = value;
        }
        public string Type => typeof(T).Name.ToLower();
    }
}