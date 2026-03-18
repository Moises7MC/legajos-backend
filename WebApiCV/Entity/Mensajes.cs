using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiCV.Entity
{
    public class Mensajes
    {
        public int ncode { get; set; }
        public Boolean cstate { get; set; }
        public String cmessage { get; set; }
        public Object odata { get; set; }

        public Mensajes(int pncode, Boolean pcstate, String pcmessage, Object podata)
        {
            ncode = pncode;
            cstate = pcstate;
            cmessage = pcmessage;
            odata = podata;
        }

        public Mensajes(String pcmessage)
        {
            ncode = 400;
            cstate = false;
            cmessage = pcmessage;
            odata = null;
        }
    }
}
