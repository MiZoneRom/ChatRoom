using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;

namespace Services
{
    public class ServiceBase
    {
        protected Entities context;

        public ServiceBase()
        {
            context = new Entities();
        }

        public void Dispose()
        {
            if (context != null)
            {
                context.Dispose();
            }
        }
    }
}
