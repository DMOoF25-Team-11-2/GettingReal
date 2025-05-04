using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GettingReal.Model
{
    internal class ActivityRepository : IRepository<Activity>
    {
        public FileDataHandler db = new FileDataHandler("./boxes.txt");

        public ActivityRepository() 
        {

        }

        public void Add(Activity item)
        {
            db.AppendLine(item.ToString());
            throw new NotImplementedException();
        }

        public Activity Get(int id)
        {
            db.ReadLine(id);
            return new Activity();
            throw new NotImplementedException();
        }

        public IEnumerable<Activity> GetAll()
        {
            return new List<Activity>();
            throw new NotImplementedException();
        }

        public void Remove(Activity item)
        {
            db.DeleteLine(item.UID);
        }

        public void Update(Activity item)
        {
            db.DeleteLine(item.UID);
            db.AppendLine(item.ToString());
        }
    }
}
