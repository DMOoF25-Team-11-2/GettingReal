using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GettingReal.Model
{
    internal class BoxRepository : IRepository<Box>
    {
        public FileDataHandler db = new FileDataHandler("./boxes.txt");

        public BoxRepository() 
        { 

        }

        public void Add(Box item)
        {
            db.AppendLine(item.ToString());
            throw new NotImplementedException();
        }

        public Box Get(int id)
        {
            db.ReadLine(id);
            return new Box();
            throw new NotImplementedException();
        }

        public IEnumerable<Box> GetAll()
        {
            return new List<Box>();
            throw new NotImplementedException();
        }

        public void Remove(Box item)
        {
            db.DeleteLine(item.ID);
            throw new NotImplementedException();
        }

        public void Remove(int id)
        {
            db.DeleteLine(item.ID);
            throw new NotImplementedException();
        }

        public void Update(Box item)
        {
            db.DeleteLine(item.ID);
            db.AppendLine(item.ToString());
            throw new NotImplementedException();
        }
    }
}
