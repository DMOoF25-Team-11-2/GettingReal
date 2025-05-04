using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GettingReal.Model
{
    internal class WorkshopRepository : IRepository<Workshop>
    {
        public FileDataHandler db = new FileDataHandler("./workshops.txt");

        public WorkshopRepository()
        {

        }

        public void Add(Workshop item)
        {
            db.AppendLine(item.ToString());
            throw new NotImplementedException();
        }

        public Workshop Get(int id)
        {
            db.ReadLine(id);
            return new Workshop();
            throw new NotImplementedException();
        }

        public IEnumerable<Workshop> GetAll()
        {   
            return new List<Workshop>();
            throw new NotImplementedException();
        }

        public void Remove(int id)
        {
            db.DeleteLine();
            throw new NotImplementedException();
        }

        public void Update(Workshop item)
        {
            db.DeleteLine();
            db.AppendLine(item.ToString());
            throw new NotImplementedException();
        }
    }
}
