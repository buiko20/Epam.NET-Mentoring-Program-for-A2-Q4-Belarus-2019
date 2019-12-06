using System;

namespace PostSharpCodeRewriting.Logic
{
    public interface IRepository
    {
        void Create(DbEntity entity);

        DbEntity Get(Guid id);
    }
}
