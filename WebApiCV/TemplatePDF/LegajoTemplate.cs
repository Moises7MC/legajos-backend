using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Text;
using WebApiCV.Entity;

namespace WebApiCV.TemplatePDF
{
    public static class LegajoTemplate
    {

        
        public static string mestxt(int mes)
        {
            string[] meses = new string[12] { "Enero", "Febrero", "Marzo","Abril","Mayo","Junio","Julio","Agosto","Setiembre","Octubre","Noviembre","Diciembre" };
            return meses[mes];
        }
        public static string GetHTMLLegajo(LegDatosGenerales obj, Boolean bDocente, IConfiguration config)
        {
            var sb = new StringBuilder();
            
            string nombre = obj.CLegDatApellidoPaterno + " " + obj.CLegDatApellidoMaterno + " " + obj.CLegDatNombres;
            string direccion = obj.vZona.CConDescripcion + " " + obj.vTipoDomicilio.CConDescripcion + " ";
            direccion += obj.CLegDatCalleDomicilio + " " + (obj.CLegDatNroDomicilio == "" || obj.CLegDatNroDomicilio == null ? "" : obj.CLegDatNroDomicilio)
                + (obj.CLegDatMzaDomicilio == "" || obj.CLegDatMzaDomicilio == null ? "" : "MZ." + obj.CLegDatMzaDomicilio +
                    (obj.CLegDatLtDomicilio == "" || obj.CLegDatLtDomicilio == null ? "" : "LT." + obj.CLegDatLtDomicilio))
                + (obj.CLegDatDptoDomicilio == "" || obj.CLegDatDptoDomicilio == null ? "" : "DPTO." + obj.CLegDatDptoDomicilio)
                + " " + obj.CLegDatReferencia;
            string telefonos = "";
            telefonos = obj.CLegDatMovil == "" || obj.CLegDatMovil == null ?
                (obj.CLegDatTelefono == "" || obj.CLegDatTelefono == null ? "" : obj.CLegDatTelefono) : 
                obj.CLegDatMovil + (obj.CLegDatTelefono == "" || obj.CLegDatTelefono == null ? "" : " / " + obj.CLegDatTelefono);
            DateTime fechanac = obj.DLegDatFechaNacimiento;
            var codigodg = obj.NLegDatCodigo;
            sb.Append(@"
                        <html>
                            <head>
                            </head>
                            <body>");
            sb.AppendFormat(@" <div class='header'><h1>{0}</h1></div>", nombre);
            sb.Append(@"<h3>I. DATOS GENERALES</h3>");
            sb.Append(@"<section class='sec-datos'>");
                sb.AppendFormat(@"<div><span class='item-dg'>1.1. Fecha de nacimiento </span><span class='dp-dg'>:</span>
                <span class='txt-dg'>{0} de {1} de {2}</span>
                </div>", fechanac.Day.ToString().PadLeft(2, '0'), mestxt(fechanac.Month-1), fechanac.Year);
                sb.AppendFormat(@"<div><span class='item-dg'>1.2. Documento de identidad </span><span class='dp-dg'>:</span>
                <span class='txt-dg'>{0}</span></div>", obj.CLegDatNroDoc.ToString());
                sb.AppendFormat(@"<div><span class='item-dg'>1.3. Domicilio </span><span class='dp-dg'>:</span>
                <span class='dir-dg'>{0}</span></div>", direccion);
                sb.AppendFormat(@"<div><span class='item-dg'>1.4. Teléfonos </span><span class='dp-dg'>:</span>
                <span class='txt-dg'>{0}</span></div>", telefonos);
                sb.AppendFormat(@"<div><span class='item-dg'>1.5. E-mail </span><span class='dp-dg'>:</span>
                <span class='txt-dg'>{0}</span></div>", obj.CLegDatEmail.ToString());
            sb.Append(@"</section>");
            sb.Append(@"<section class='sec-foto'>");
            sb.AppendFormat(@"<img src ='{0}' />", config.GetValue<string>("fotoseuss") + obj.cPerCodigo);
            sb.Append(@"</section>");


            sb.Append(@"<h3 class='title-h3'>II. SALUD/ANTECEDENTES </h3>");

            sb.Append(@"<h3 class='title-h3'>III. GRADOS Y TÍTULOS</h3>");
                sb.Append(@"<table>
                                <tr>
                                    <th class='col-hd col-t1'>CARRERA PROFESIONAL</th>
                                    <th class='col-hd col-t1'>INSTITUCIÓN</th>
                                    <th class='col-hd'>PAIS</th>
                                    <th class='col-hd col-t2'>GRADO ACADÉMICO</th>
                                    <th class='col-hd'>FECHA DE OBTENCIÓN</th>
                                </tr>
                            ");

            foreach (LegGradoTitulo objx in obj.LegGradoTitulo)
            {
                sb.AppendFormat(@"<tr>
                        <td class='col-bd col-l col-t1'>{0}</td>
                        <td class='col-bd col-l col-t1'>{1}</td>
                        <td class='col-bd col-l'>{2}</td>
                        <td class='col-bd col-l col-t2'>{3}</td>
                        <td class='col-bd col-c'>{4}</td>
                    </tr>",
                    objx.CLegGraCarreraProf,
                    objx.CLegGraInstitucionNavigation == null
                        ? objx.CLegGraOtraInst
                        : objx.CLegGraInstitucionNavigation.CPerNombre,
                    objx.vPais?.CIntDescripcion ?? "",
                    (objx.vGradoAcad?.CIntNombre ?? "").ToUpper(),
                    objx.DLegGraFecha.ToShortDateString());
            }

            sb.Append(@"</table>");

                if (obj.CLegDatColegioProf != null)
                {

                    sb.AppendFormat(@"<table border='1' class='tbcol'>
                                        <tr>
                                            <td class='col-col col-t2' rowspan='2'><b>COLEGIO PROFESIONAL AL QUE PERTENECE:</b></td>
                                            <td class='col-col col-t1' rowspan='2'>{0}</td>
                                            <td class='col-col col-t2' rowspan='2'><b>COLEGIATURA N°</b> {1}</td>
                                            <td class='col-col col-t2 ' rowspan='2'><b>CONDICIÓN:</b> {2}</td>
                                            <td class='col-col col-t1'><b>FECHA EMISIÓN:</b> {3}</td>
                                        </tr>
                                        <tr>
                                            <td class='col-col col-t1'><b>FECHA EXPIRACIÓN:</b> {4}</td>
                                        </tr>
                                    ", obj.CLegDatColegioProf == null ? "-" : obj.CLegDatColegioProfNavigation.CPerNombre.ToUpper(), obj.CLegDatNroColegiatura == null ? "-" : obj.CLegDatNroColegiatura.ToUpper(), obj.NLegDatCondicionColeg == null ? "-" : obj.vCondicionColeg.CConDescripcion.ToUpper(), obj.DLegDatosFechaEmisionColeg == null ? "-" : obj.DLegDatosFechaEmisionColeg, obj.DLegDatosFechaEmisionColeg == null ? "-" : obj.DLegDatosFechaExpiraColeg);
                    sb.Append(@"</table>");
                }
            if (bDocente)
            {
                sb.Append(@"<h3 class='title-h3'>IV. EXPERIENCIA EN DOCENCIA UNIVERSITARIA</h3>");
                sb.Append(@"<table>
                                <tr>
                                    <th class='col-hd col-t1'>UNIVERSIDAD</th>
                                    <th class='col-hd col-t2'>RÉGIMEN DEDICACIÓN</th>
                                    <th class='col-hd col-t2'>CARGO</th>
                                    <th class='col-hd'>FECHA INICIO</th>
                                    <th class='col-hd'>FECHA FIN</th>
                                </tr>
                            ");

                foreach (LegDocenciaUniv objx in obj.LegDocenciaUniv)
                {
                    sb.AppendFormat(@"<tr>
                                    <td class='col-bd col-l col-t1'>{0}</td>
                                    <td class='col-bd col-l col-t2'>{1}</td>
                                    <td class='col-bd col-l col-t2'>{2}</td>
                                    <td class='col-bd col-c'>{3}</td>
                                    <td class='col-bd col-c'>{4}</td>
                                </tr>", 
                                objx.CLegDocUniversidadNavigation == null ? objx.CLegDocOtraInst : objx.CLegDocUniversidadNavigation.CPerNombre == null ? "": objx.CLegDocUniversidadNavigation.CPerNombre.ToUpper(),
                                objx.vRegimen.CConDescripcion == null ? "" : objx.vRegimen.CConDescripcion.ToUpper(),
                                objx.vCategoria.CConDescripcion == null ? "": objx.vCategoria.CConDescripcion.ToUpper(),
                                objx.DLegDocFechaInicio.ToShortDateString(), objx.DLegDocFechaFin.ToShortDateString());
                }
                sb.Append(@"</table>");
            }

            if (bDocente)
            {
                sb.Append(@"<h3 class='title-h3'>V. CATEGORÍA DOCENTE </h3>");
                sb.Append(@"<table>
                                <tr>
                                    <th class='col-hd col-t1'>INSTITUCIÓN</th>
                                    <th class='col-hd col-t2'>CARGO DESEMPEÑADO</th>
                                    <th class='col-hd'>FECHA INICIO</th>
                                    <th class='col-hd'>FECHA FIN</th>
                                </tr>
                            ");

                foreach (LegCategoriaDocente objx in obj.LegCategoriaDocente)
                {
                    sb.AppendFormat(@"<tr>
                                    <td class='col-bd col-l col-t1'>{0}</td>
                                    <td class='col-bd col-l col-t2'>{1}</td>
                                    <td class='col-bd col-c'>{2}</td>
                                    <td class='col-bd col-c'>{3}</td>
                                </tr>", objx.CLegCatInstitucionNavigation == null ? objx.CLegCatOtraInst : objx.CLegCatInstitucionNavigation.CPerNombre == null ? "": objx.CLegCatInstitucionNavigation.CPerNombre.ToUpper(), objx.vCategoria.CConDescripcion == null ? "": objx.vCategoria.CConDescripcion.ToUpper(),  objx.DLegCatFechaInicio.ToShortDateString(), objx.DLegCatFechaFin.ToShortDateString());
                }
                sb.Append(@"</table>");
            }

            if (bDocente)
            {
                sb.Append(@"<h3 class='title-h3'>VI. RÉGIMEN DE DEDICACIÓN DOCENTE</h3>");
                sb.Append(@"<table>
                                <tr>
                                    <th class='col-hd col-t1'>INSTITUCIÓN</th>
                                    <th class='col-hd col-t2'>CARGO DESEMPEÑADO</th>
                                    <th class='col-hd'>FECHA INICIO</th>
                                    <th class='col-hd'>FECHA FIN</th>
                                </tr>
                            ");

                foreach (LegRegimenDedicacion objx in obj.LegRegimenDedicacion)
                {
                    sb.AppendFormat(@"<tr>
                                    <td class='col-bd col-l col-t1'>{0}</td>
                                    <td class='col-bd col-l col-t2'>{1}</td>
                                    <td class='col-bd col-c'>{2}</td>
                                    <td class='col-bd col-c'>{3}</td>
                                </tr>", objx.CLegCatInstitucionNavigation == null ? objx.CLegRegOtraInst : objx.CLegCatInstitucionNavigation.CPerNombre.ToUpper(), objx.vDedicacion.CConDescripcion == null ? "": objx.vDedicacion.CConDescripcion.ToUpper(), objx.DLegRegFechaInicio.ToShortDateString(), objx.DLegRegFechaFin.ToShortDateString());
                }
                sb.Append(@"</table>");
            }


            sb.AppendFormat(@"<h3 class='title-h3'>{0}</h3>", bDocente ? "VII. EXPERIENCIA PROFESIONAL NO DOCENTE" : "II. EXPERIENCIA PROFESIONAL");
            sb.Append(@"<table>
    <tr>
        <th class='col-hd col-t1'>INSTITUCIÓN</th>
        <th class='col-hd col-t1'>DEDICACIÓN</th>
        <th class='col-hd col-t1'>CARGO</th>
        <th class='col-hd col-t1'>DESCRIPCIÓN</th>
        <th class='col-hd'>FECHA INICIO</th>
        <th class='col-hd'>FECHA FIN</th>
    </tr>");

            foreach (LegProfesNoDocente objx in obj.LegProfesNoDocente)
            {
                // Validamos la Dedicación: Si vCargo es nulo pero hay un valor guardado, 
                // lo ideal es que el Backend haga el Include, pero aquí protegemos el diseño.
                string dedicacion = objx.vCargo?.CConDescripcion ?? "-";

                // Validamos la Fecha Fin: Si es año 1 o nula, ponemos "ACTUALIDAD"
                string fechaFinStr = (objx.DLegProFechaFin.Year <= 1900)
                                     ? "ACTUALIDAD"
                                     : objx.DLegProFechaFin.ToShortDateString();

                sb.AppendFormat(@"<tr>
            <td class='col-bd col-l col-t1'>{0}</td>
            <td class='col-bd col-l'>{1}</td>
            <td class='col-bd col-l col-t1'>{2}</td>
            <td class='col-bd col-l col-t1'>{3}</td>
            <td class='col-bd col-c'>{4}</td>
            <td class='col-bd col-c'>{5}</td>
        </tr>",
                    // {0} Institución
                    (objx.CLegProInstitucionNavigation == null ? objx.CLegProOtraInst : objx.CLegProInstitucionNavigation.CPerNombre)?.ToUpper() ?? "-",

                    // {1} Dedicación (Tipo)
                    dedicacion.ToUpper(),

                    // {2} Cargo
                    (objx.CLegProCargoProf ?? "").ToUpper(),

                    // {3} Descripción
                    (objx.cLegDescCargo ?? "").ToUpper(),

                    // {4} Fecha Inicio
                    objx.DLegProFechaInicio.ToShortDateString(),

                    // {5} Fecha Fin corregida
                    fechaFinStr);
            }
            sb.Append(@"</table>");

            sb.AppendFormat(@"<h3 class='title-h3'>{0}</h3>", bDocente ? "VIII. DOMINIO DE IDIOMAS – HERRAMIENTAS DE OFIMÁTICA" : "III. IDIOMAS – COMPUTACIÓN:");
            sb.Append(@"<h3 class='subtitle-h3'>8.1. DOMINIO DE UN IDIOMA DISTINTO AL MATERNO</h3>");
                sb.Append(@"<table>
                                    <tr>
                                        <th class='col-hd col-tw'>IDIOMA</th>
                                        <th class='col-hd col-tw'>NIVEL</th>
                                        <th class='col-hd'>FECHA CERTIFICACIÓN</th>
                                    </tr>
                                ");

                foreach (LegIdiomaOfimatica objx in obj.LegIdiomaOfimatica)
                {
                    if (objx.CLegIdOfTipo == false)
                    {
                        sb.AppendFormat(@"<tr>
                                            <td class='col-bd col-l'>{0}</td>
                                            <td class='col-bd col-l'>{1}</td>
                                            <td class='col-bd col-c'>{2}</td>
                                        </tr>", objx.vCodigoDesc.CConDescripcion == null ? "": objx.vCodigoDesc.CConDescripcion.ToUpper(), objx.vNivel.CConDescripcion == null ? "": objx.vNivel.CConDescripcion.ToUpper(), objx.DLegIdOfFecha.ToShortDateString());
                    }
                }
                sb.Append(@"</table>");
            sb.Append(@"<h3 class='subtitle-h3'>8.2. DOMINIO DE HERRAMIENTAS DE OFIMÁTICA</h3>");
                sb.Append(@"<table>
                                        <tr>
                                            <th class='col-hd col-tw'>TIC´s</th>
                                            <th class='col-hd col-tw'>NIVEL</th>
                                            <th class='col-hd'>FECHA CERTIFICACIÓN</th>
                                        </tr>
                                    ");

                foreach (LegIdiomaOfimatica objx in obj.LegIdiomaOfimatica)
                {
                    if (objx.CLegIdOfTipo == true)
                    {
                        sb.AppendFormat(@"<tr>
                                                <td class='col-bd col-l'>{0}</td>
                                                <td class='col-bd col-l'>{1}</td>
                                                <td class='col-bd col-c'>{2}</td>
                                            </tr>", objx.vCodigoDesc.CConDescripcion == null ? "": objx.vCodigoDesc.CConDescripcion.ToUpper(), objx.vNivel.CConDescripcion == null ? "": objx.vNivel.CConDescripcion.ToUpper(), objx.DLegIdOfFecha.ToShortDateString());
                    }
                }
                sb.Append(@"</table>");
            if (bDocente)
            {
                sb.Append(@"<h3 class='title-h3'>IX. REGISTRO COMO DOCENTE INVESTIGADOR</h3>");
                sb.Append(@"<table>
                                <tr>
                                    <th class='col-hd col-t1'>CENTRO DE REGISTRO</th>
                                    <th class='col-hd col-t2'>CODIGO INVESTIGADOR</th>
                                    <th class='col-hd col-t2'>NIVEL</th>
                                    <th class='col-hd'>FECHA INICIO</th>
                                    <th class='col-hd'>FECHA FIN</th>
                                </tr>
                            ");

                foreach (LegInvestigador objx in obj.LegInvestigador)
                {
                    sb.AppendFormat(@"<tr>
                                    <td class='col-bd col-l col-t1'>{0}</td>
                                    <td class='col-bd col-l col-t2'>{1}</td>
                                    <td class='col-bd col-l col-t2'>{2}</td>
                                    <td class='col-bd col-c'>{3}</td>
                                    <td class='col-bd col-c'>{4}</td>
                                </tr>", 
                                objx.vCentroRegistro.CIntDescripcion == null ? "" : objx.vCentroRegistro.CIntDescripcion.ToUpper(), 
                                objx.CLegInvNroRegistro.ToUpper(),
                                objx.vNivelRenacyt.CIntDescripcion == null ? "" : objx.vNivelRenacyt.CIntDescripcion.ToUpper(),
                                objx.DLegInvFechaInicio.ToShortDateString(), 
                                objx.DLegInvFechaFin.ToShortDateString());
                }
                sb.Append(@"</table>");
            }

            if (bDocente)
            {
                sb.Append(@"<h3 class='title-h3'>X. ACTIVIDADES DE SERVICIO (Dentro y fuera de la Institución)</h3>");
                sb.Append(@"<table>
                                <tr>
                                    <th class='col-hd col-t2'>ACTIVIDAD</th>
                                    <th class='col-hd col-t2'>NIVEL</th>
                                    <th class='col-hd'>FECHA</th>
                                    <th class='col-hd'>N° RESOLUCIÓN</th>
                                </tr>
                            ");

                foreach (LegTesisAseJur objx in obj.LegTesisAseJur)
                {
                    sb.AppendFormat(@"<tr>
                                    <td class='col-bd col-l col-t1'>{0}</td>
                                    <td class='col-bd col-l col-t2'>{1}</td>
                                    <td class='col-bd col-c'>{2}</td>
                                    <td class='col-bd col-l col-t2'>{3}</td>
                                </tr>", objx.vTipo.CIntDescripcion == null ? "": objx.vTipo.CIntDescripcion.ToUpper(), objx.vNivel.CConDescripcion == null ? "" : objx.vNivel.CConDescripcion.ToUpper(), objx.DLegTesFecha.ToShortDateString(), objx.CLegTesNroResolucion.ToUpper());
                }
                sb.Append(@"</table>");
            }
            if (bDocente)
            {
                sb.Append(@"<h3 class='title-h3'>XI. PRODUCCIÓN CIENTÍFICA, LECTIVA Y DE INVESTIGACIÓN</h3>");
                sb.Append(@"<table>
                    <tr>
                        <th class='col-hd col-tw'>TÍTULO</th>
                        <th class='col-hd'>FECHA</th>
                        <th class='col-hd col-t2'>TIPO PUBLICACIÓN</th>
                        <th class='col-hd col-t2'>PAÍS</th>
                        <th class='col-hd col-t2'>PARTICIPACIÓN</th>
                        <th class='col-hd COL-t2'>N° REGISTRO / N° RESOLUCIÓN</th>
                    </tr>
                ");

                foreach (LegProduccionCiencia objx in obj.LegProduccionCiencia)
                {
                    // Lógica para el texto de participación
                    string participacion = "---";
                    if (objx.NLegProdParticipacion == 1) participacion = "AUTOR";
                    else if (objx.NLegProdParticipacion == 2) participacion = "COAUTOR";

                    // Lógica para el nombre del país
                    string nombrePais = (objx.vPais != null && !string.IsNullOrEmpty(objx.vPais.CIntDescripcion))
                                        ? objx.vPais.CIntDescripcion.ToUpper()
                                        : "---";

                    sb.AppendFormat(@"<tr>
                            <td class='col-bd col-l col-tw'>{0}</td>
                            <td class='col-bd col-c'>{1}</td>
                            <td class='col-bd col-l col-t2'>{2}</td>
                            <td class='col-bd col-l col-t2'>{3}</td>
                            <td class='col-bd col-l col-t2'>{4}</td>
                            <td class='col-bd col-l col-t2'>{5}</td>
                        </tr>",
                                    objx.CLegProdTitulo.ToUpper(),
                                    objx.DLegProdFecha.ToShortDateString(),
                                    objx.vTipo?.CIntDescripcion?.ToUpper() ?? "",
                                    nombrePais,
                                    participacion,
                                    objx.CLegProdNroResolucion.ToUpper());
                }
                sb.Append(@"</table>");
            }

            if (bDocente)
            {
                sb.Append(@"<h3 class='title-h3'>XII. PARTICIPACIÓN EN CONGRESOS, TALLERES, SEMINARIOS Y OTROS</h3>");
                sb.Append(@"<table>
                                        <tr>
                                            <th class='col-hd col-t1'>INSTITUCIÓN</th>
                                            <th class='col-hd'>ROL</th>
                                            <th class='col-hd col-t1'>NOMBRE EVENTO</th>
                                            <th class='col-hd '>AMBITO</th>
                                            <th class='col-hd'>FECHA</th>
                                        </tr>
                                    ");

                foreach (LegParticipacionCongSem objx in obj.LegParticipacionCongSem)
                {
                    sb.AppendFormat(@"<tr>
                                            <td class='col-bd col-l col-t1'>{0}</td>
                                            <td class='col-bd col-l col-t1'>{1}</td>
                                            <td class='col-bd col-c'>{2}</td>
                                            <td class='col-bd col-c'>{3}</td>
                                            <td class='col-bd col-c'>{4}</td>
                                        </tr>", objx.CLegParInstitucionNavigation == null ? objx.CLegParOtraInst : objx.CLegParInstitucionNavigation.CPerNombre == null ? "": objx.CLegParInstitucionNavigation.CPerNombre.ToUpper(), objx.vRol.CIntDescripcion == null ? "": objx.vRol.CIntDescripcion.ToUpper(), objx.CLegParNombre == null ? "": objx.CLegParNombre.ToUpper(), objx.vAmbito.CIntDescripcion == null ? "": objx.vAmbito.CIntDescripcion.ToUpper(), objx.DLegParFecha.ToShortDateString());
                }
                sb.Append(@"</table>");
            }

            if (bDocente)
            {
                sb.Append(@"<h3 class='title-h3'>XIII. CARGA ADMINISTRATIVA UNIVERSITARIA</h3>");
                sb.Append(@"<table>
                                        <tr>
                                            <th class='col-hd col-t2'>CARGO</th>
                                            <th class='col-hd'>FECHA INICIO</th>
                                            <th class='col-hd'>FECHA FIN</th>
                                            <th class='col-hd col-2'>DOCUMENTO</th>
                                            <th class='col-hd col-t1'>INSTITUCIÓN</th>
                                            
                                        </tr>
                                    ");

                foreach (LegAdminitrativaCarga objx in obj.LegAdminitrativaCarga)
                {
                    sb.AppendFormat(@"<tr>
                                            <td class='col-bd col-l col-t1'>{0}</td>
                                            <td class='col-bd col-l '>{1}</td>
                                            <td class='col-bd col-c'>{2}</td>
                                            <td class='col-bd col-l col-t2'>{3}</td>
                                            <td class='col-bd col-l col-t1'>{4}</td>
                                        </tr>", objx.vCargo.CConDescripcion == null ? "": objx.vCargo.CConDescripcion.ToUpper(), objx.DLegAdmFechaInicio.ToShortDateString(), objx.DLegAdmFechaFin.ToShortDateString(), objx.CLegAdmDocumento.ToUpper(), objx.CLegAdmInstitucionNavigation == null ? objx.CLegAdmOtraInst : objx.CLegAdmInstitucionNavigation.CPerNombre == null ? "" : objx.CLegAdmInstitucionNavigation.CPerNombre.ToUpper());
                }
                sb.Append(@"</table>");
            }

            if (bDocente)
            {
                sb.Append(@"<h3 class='title-h3'>XIV. HONORES Y RECONOCIMIENTOS</h3>");
                sb.Append(@"<table>
                                        <tr>
                                            <th class='col-hd col-t2'>DOCUMENTO/EVIDENCIA</th>
                                            <th class='col-hd'>FECHA</th>
                                            <th class='col-hd col-2'>TIPO RECONOCIMIENTO</th>
                                            <th class='col-hd col-t1'>INSTITUCIÓN</th>
                                            
                                        </tr>
                                    ");

                foreach (LegReconocimiento objx in obj.LegReconocimiento)
                {
                    sb.AppendFormat(@"<tr>
                                            <td class='col-bd col-l col-t2'>{0}</td>
                                            <td class='col-bd col-c'>{1}</td>
                                            <td class='col-bd col-l col-t1'>{2}</td>
                                            <td class='col-bd col-l col-t1'>{3}</td>
                                        </tr>", objx.vDocumento.CConDescripcion.ToUpper(), objx.DLegRecFecha.ToShortDateString(), objx.vTipo.CConDescripcion == null ? "": objx.vTipo.CConDescripcion.ToUpper(), objx.CLegRecInstitucionNavigation == null ? objx.CLegRecOtraInst :  objx.CLegRecInstitucionNavigation.CPerNombre == null ? "": objx.CLegRecInstitucionNavigation.CPerNombre.ToUpper());
                }
                sb.Append(@"</table>");
            }


            sb.AppendFormat(@"<h3 class='title-h3'>{0}</h3>", bDocente ? "XIII. CAPACITACIONES" : "IV. CAPACITACIONES");
                  
                sb.AppendFormat(@"<table>
                                    <tr>
                                        <th class='col-hd col-t1'>NOMBRE CAPACITACIÓN</th>
                                        <th class='col-hd'>TIPO</th>
                                        <th class='col-hd col-t1'>INSTITUCIÓN</th>
                                        <th class='col-hd '>{0}</th>
                                        <th class='col-hd'>CANTIDAD</th>
                                        <th class='col-hd'>FECHA</th>
                                    </tr>
                                ", bDocente ? "ESPECIALIDAD" : "TIPO DURACIÓN");

                foreach (LegCapacitaciones objx in obj.LegCapacitaciones)
                {
                    sb.AppendFormat(@"<tr>
                                        <td class='col-bd col-l col-t1'>{0}</td>
                                        <td class='col-bd col-l'>{1}</td>
                                        <td class='col-bd col-l col-t1'>{2}</td>
                                        <td class='col-bd col-c'>{3}</td>
                                        <td class='col-bd col-r'>{4}</td>
                                        <td class='col-bd col-c'>{5}</td>
                                    </tr>", objx.CLegCapNombre.ToUpper(), objx.vTipo.CConDescripcion == null ? "" : objx.vTipo.CConDescripcion.ToUpper(), objx.vInstitucion == null ? objx.CLegCapOtraInst : objx.vInstitucion.CPerNombre == null ? "" : objx.vInstitucion.CPerNombre.ToUpper(), objx.vTipoEsp.CConDescripcion == null ? "" : objx.vTipoEsp.CConDescripcion.ToUpper(), objx.NLegCapHoras, objx.DLegCapFechaInicio.ToShortDateString());
            }
                    sb.Append(@"</table>");

            if (!bDocente)
            {
                sb.Append(@"<h3 class='title-h3'>XV. PARTICIPACIÓN EN CONGRESOS, CURSO, SEMINARIOS, TALLERES Y OTROS</h3>");

                    sb.Append(@"<table>
                                        <tr>
                                            <th class='col-hd col-t1'>INSTITUCIÓN</th>
                                            <th class='col-hd'>ROL</th>
                                            <th class='col-hd col-t1'>NOMBRE EVENTO</th>
                                            <th class='col-hd '>AMBITO</th>
                                            <th class='col-hd'>FECHA</th>
                                        </tr>
                                    ");

                    foreach (LegParticipacionCongSem objx in obj.LegParticipacionCongSem)
                    {
                        sb.AppendFormat(@"<tr>
                                            <td class='col-bd col-l col-t1'>{0}</td>
                                            <td class='col-bd col-l col-t1'>{1}</td>
                                            <td class='col-bd col-c'>{2}</td>
                                            <td class='col-bd col-c'>{3}</td>
                                            <td class='col-bd col-c'>{4}</td>
                                        </ tr > ", objx.CLegParInstitucionNavigation == null ? objx.CLegParOtraInst :  objx.CLegParInstitucionNavigation.CPerNombre == null ? "": objx.CLegParInstitucionNavigation.CPerNombre.ToUpper(), objx.vRol.CIntDescripcion == null? "": objx.vRol.CIntDescripcion.ToUpper(), objx.CLegParNombre.ToUpper(), objx.vAmbito.CIntDescripcion == null ? "" : objx.vAmbito.CIntDescripcion.ToUpper(),  objx.DLegParFecha.ToShortDateString());
                    }
                    sb.Append(@"</table>");
            }

            if (bDocente)
            {
                sb.Append(@"<h3 class='title-h3'>XVI. PROYECCIÓN SOCIAL</h3>");
                sb.Append(@"<table>
                                        <tr>
                                            <th class='col-hd col-t1'>PROYECTO</th>
                                            <th class='col-hd'>FECHA INICIO</th>
                                            <th class='col-hd'>FECHA FIN</th>
                                            <th class='col-hd col-2'>TIPO PARTICIPACIÓN</th>
                                            <th class='col-hd col-t1'>INSTITUCIÓN</th>
                                            
                                        </tr>
                                    ");

                foreach (LegProyeccionSocial objx in obj.LegProyeccionSocial)
                {
                    sb.AppendFormat(@"<tr>
                                            <td class='col-bd col-l col-t2'>{0}</td>
                                            <td class='col-bd col-c'>{1}</td>
                                            <td class='col-bd col-c'>{2}</td>
                                            <td class='col-bd col-l col-t2'>{3}</td>
                                            <td class='col-bd col-l col-t1'>{4}</td>
                                        </tr>", objx.CLegProyDescripcion.ToUpper(), 
                                        objx.DLegProyFechaInicio.ToShortDateString(), 
                                        objx.DLegProyFechaFin.ToShortDateString(), 
                                        objx.vTipo.CConDescripcion == null ? "": objx.vTipo.CConDescripcion.ToUpper(), 
                                        objx.CLegProyInstitucionNavigation == null ? objx.CLegProyOtraInst : objx.CLegProyInstitucionNavigation.CPerNombre == null ? "" : objx.CLegProyInstitucionNavigation.CPerNombre.ToUpper());
                }
                sb.Append(@"</table>");



                sb.Append(@"<h3 class='title-h3'>XVII. LICENCIA PROFESIONAL</h3>");
                sb.Append(@"<table>
                                <tr>
                                    <th class='col-hd'>COLEGIO PROFESIONAL</th>
                                    <th class='col-hd'>NRO COLEGIATURA</th>
                                    <th class='col-hd'>CONDICION</th>
                                    <th class='col-hd'>FECHA EMISION</th>
                                    <th class='col-hd'>FECHA EXPIRACION</th>
                                </tr>
                            ");

                foreach (LegLicenciaProfesional objx in obj.LegLicenciaProfesional)
                {
                    sb.AppendFormat(@"<tr>
                                        <td class='col-bd col-l col-t2'>{0}</td>
                                        <td class='col-bd col-c'>{1}</td>
                                        <td class='col-bd col-c'>{2}</td>
                                        <td class='col-bd col-c'>{3}</td>
                                        <td class='col-bd col-c'>{4}</td>
                                    </tr>", 
                                    objx.CLegLicInstitucionNavigation == null ? objx.CLegLicOtraInst : objx.CLegLicInstitucionNavigation.CPerNombre == null ? "" : objx.CLegLicInstitucionNavigation.CPerNombre.ToUpper(),
                                    objx.CLegLicNroRegistro.ToString(),
                                    objx.vCondicion.CIntDescripcion == null ? "" : objx.vCondicion.CIntDescripcion.ToUpper(),
                                    objx.DLegLicFechaEmision.ToShortDateString(),
                                    objx.DLegLicFechaExpiracion.ToShortDateString()
                                    );
                }
                sb.Append(@"</table>");

                sb.Append(@"<h3 class='title-h3'>XVIII. MEMBRESIA</h3>");
                sb.Append(@"<table>
                                <tr>
                                    <th class='col-hd'>ORGANIZACION PROFESIONAL</th>
                                    <th class='col-hd'>NRO MEMBRESIA</th>
                                    <th class='col-hd'>FECHA EMISION</th>
                                    <th class='col-hd'>FECHA EXPIRACION</th>
                                </tr>
                            ");

                foreach (LegMembresia objx in obj.LegMembresia)
                {
                    sb.AppendFormat(@"<tr>
                                        <td class='col-bd col-l col-t2'>{0}</td>
                                        <td class='col-bd col-c col-t2'>{1}</td>
                                        <td class='col-bd col-c'>{2}</td>
                                        <td class='col-bd col-c'>{3}</td>
                                    </tr>",
                                    objx.CLegMemInstitucionNavigation == null ? objx.CLegMemOtraInst : objx.CLegMemInstitucionNavigation.CPerNombre == null ? "" : objx.CLegMemInstitucionNavigation.CPerNombre.ToUpper(),
                                    objx.CLegMemNroRegistro.ToString(),
                                    objx.DLegMemFechaEmision.ToShortDateString(),
                                    objx.DLegMemFechaExpiracion.ToShortDateString()
                                    );
                }
                sb.Append(@"</table>");


            }




            sb.AppendFormat(@" </body>
                        </html>");
                               
            return sb.ToString();
        }
    }
}
