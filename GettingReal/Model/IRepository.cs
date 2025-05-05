using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GettingReal.Model;

internal interface IRepository<T>
{
    void Add(T item);
    T Get(int id);
    IEnumerable<T> GetAll();
    void Update(T item);
    void Remove(int id);
}
