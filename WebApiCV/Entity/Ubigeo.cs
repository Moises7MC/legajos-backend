using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace WebApiCV.Entity
{
    public class Ubigeo
    {
        public string Id { get; set; }
        public string cUbiDepartamento { get; set; }
        public string cUbiProvincia { get; set; }
        public string cUbiDistrito { get; set; }
        //public List<Curriculo> Curriculos { get; set; }
    }
}
