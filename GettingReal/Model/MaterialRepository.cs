using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GettingReal.Model
{
    internal class MaterialRepository : IRepository<Material>
    {
        public FileDataHandler db = new FileDataHandler("./boxes.txt");

        public MaterialRepository()
        {
        }

        public void Add(Material item)
        {
            db.AppendLine(item.ToString());
            throw new NotImplementedException();
        }

        public Material Get(int id)
        {
            db.ReadLine(id);
            return new Material();
            throw new NotImplementedException();
        }

        public IEnumerable<Material> GetAll()
        {
            return new List<Material>();
            throw new NotImplementedException();
        }

        public void Remove(int id)
        {
            db.DeleteLine();
            throw new NotImplementedException();
        }

        public void Update(Material item)
        {
            db.DeleteLine(item.UID);
            db.AppendLine(item.ToString());
            throw new NotImplementedException();
        }
    }
}
