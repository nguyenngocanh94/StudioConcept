using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudioConcept.Wrapper
{
    public class Wrapper
    {
        public object EventMouse { get; set; }
        public object Context { get; set; }

        public Wrapper(object e, object c)
        {
            EventMouse = e;
            Context = c;
        }
    }
}
