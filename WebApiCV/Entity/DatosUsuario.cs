using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiCV.Entity
{
    public class DatosUsuario
    {
        public string cPerCodigo { set; get; }
        public string cPerApellido { set; get; }
        public string cPerNombre { set; get; }
        public string cPerUsuCodigo { set; get; }
        public string cPerTipoDoc { set; get; }

        public string cPerNroDoc { set; get; }

        public string cPerEmail { set; get; }
        public string cPerUsuClave { set; get; }
        public int nTipo { set; get; }
        public string cTipoDesc { set; get; }
        public string cCargo { set; get; }

        public int nRol { set; get; }
        public bool bLegajo { set; get; }

        public string Area { set; get; }

        public string cToken { set; get; }

        public int cPerUsuEstado { set; get; }
        //public Boolean declaracionjuradaflag { set; get; }

        [NotMapped]
        public string LegAdminitrativaCarga { get; set; }
        [NotMapped]
        public string LegAdminitrativaCargaNoValida { get; set; }
        [NotMapped]
        public string LegCapacitaciones { get; set; }
        [NotMapped]
        public string LegCapacitacionesNoValida { get; set; }
        [NotMapped]
        public string LegCategoriaDocente { get; set; }
        [NotMapped]
        public string LegCategoriaDocenteNoValida { get; set; }
        [NotMapped]
        public string LegDocenciaUniv { get; set; }
        [NotMapped]
        public string LegDocenciaUnivNoValida { get; set; }
        [NotMapped]
        public string LegGradoTitulo { get; set; }
        [NotMapped]
        public string LegGradoTituloNoValida { get; set; }
        [NotMapped]
        public string LegIdiomaOfimatica { get; set; }
        [NotMapped]
        public string LegIdiomaOfimaticaNoValida { get; set; }
        [NotMapped]
        public string LegInvestigador { get; set; }
        [NotMapped]
        public string LegInvestigadorNoValida { get; set; }
        [NotMapped]
        public string LegParticipacionCongSem { get; set; }
        [NotMapped]
        public string LegParticipacionCongSemNoValida { get; set; }
        [NotMapped]
        public string LegProduccionCiencia { get; set; }
        [NotMapped]
        public string LegProduccionCienciaNoValida { get; set; }
        [NotMapped]
        public string LegProfesNoDocente { get; set; }
        [NotMapped]
        public string LegProfesNoDocenteNoValida { get; set; }
        [NotMapped]
        public string LegProyeccionSocial { get; set; }
        [NotMapped]
        public string LegProyeccionSocialNoValida { get; set; }
        [NotMapped]
        public string LegReconocimiento { get; set; }
        [NotMapped]
        public string LegReconocimientoNoValida { get; set; }
        [NotMapped]
        public string LegRegimenDedicacion { get; set; }
        [NotMapped]
        public string LegRegimenDedicacionNoValida { get; set; }
        [NotMapped]
        public string LegTesisAseJur { get; set; }
        [NotMapped]
        public string LegTesisAseJurNoValida { get; set; }
        [NotMapped]
        public string cArea { get; set; }
        


    }
}
