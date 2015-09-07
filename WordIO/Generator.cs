using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Novacode;

namespace WordIO
{
    public class Generator
    {
        public void CrearLibro(bool trabajos = false)
        {
            var db = new XIXCNPsicodiagnosticoEntities();
            var html = @"<!DOCTYPE html xmlns=""http://www.w3.org/1999/xhtml"" lang=""es-AR"" xml:lang=""es-AR""><head><meta http-equiv=""Content-Type"" content=""text/html; charset=UTF-8"" /></head><html><body>{0}</body></html>";

            var indice = "<h1>Índice</h1>";
            foreach (var area in db.WorkArea)
            {
                indice += string.Format("<h2>{0}</h2>", area.Nombre);
                var lista = "<ul>";
                foreach (var work in db.Work.OrderBy(x => x.Puntaje).ToList().Where(x => x.AreaID.Equals(area.Id) && x.EstadoID != 1 && x.EstadoID != 6 && x.TrabajoAprobado.GetValueOrDefault(false)).ToList())
                {
                    lista += "<li>" + HtmlRemoval.StripTagsRegex(work.Title) + "</li>";
                }
                lista += "</ul>";
                indice += lista;
            }

            var cuerpo = "<h1>Contenido</h1><hr />";

            foreach (var area in db.WorkArea)
            {
                cuerpo += "<h2>" + area.Nombre + "</h2>";

                if (area.Id == 6)
                {
                    var content = string.Empty;

                    foreach (var mesa in db.MesasDePonenciasLibres.Where(x => x.Work.Any()).ToList())
                    {
                        var innerContent = "<h3>" + HtmlRemoval.StripTagsCharArray(mesa.Titulo) + "</h3>";
                        innerContent += "<h3>Coordinador/es: " + HtmlRemoval.StripTagsCharArray(mesa.Coordinadores) + "</h3>";
                        mesa.Work = db.Work.Where(x => x.MesasDePonenciasLibres.Any() && x.MesasDePonenciasLibres.FirstOrDefault().id.Equals(mesa.id)).ToList();
                        foreach (var work in mesa.Work.ToList())
                        {
                            innerContent += "<h4>" + HtmlRemoval.StripTagsCharArray(work.Title) + "</h4>";
                            innerContent += trabajos ?
                                                (work.FullWorkBody)
                                            :   (work.Body);
                        }
                        cuerpo += innerContent;
                        //content += string.Format("<h3>{0}</h3><h3>Coordinador/es: {1}</h3>{2}",
                        //        mesa.Titulo,
                        //        mesa.Coordinadores,
                        //        innerContent
                        //    );
                        cuerpo += content;

                        //var innerContent = "<h3>" + HtmlRemoval.StripTagsCharArray(mesa.Titulo) + "</h3>";
                        //foreach (var work in mesa.Work.ToList())
                        //{
                        //    var trabajoresumen = trabajos
                        //        ? (work.FullWorkBody)
                        //        : (work.Body);

                        //    innerContent += string.Format("<h4>{2}</h4>{3} <hr />",
                        //        work.Title,
                        //        trabajoresumen
                        //        );
                        //}

                        //content += string.Format("<h3>{0}</h3><h3>Coordinador/es: {1}</h3>{2}",
                        //        mesa.Titulo,
                        //        mesa.Coordinadores,
                        //        innerContent
                        //    );

                        //cuerpo += content;
                    }
                    /*
                        TITULO DE PONENCIAS LIBRES (ya está afuera de este if)
                            titulo de la mesa | coordinador de la mesa
                                titulo del trabajo
                                autores
                                cuerpo del trabajo
                    */
                }
                else
                {
                    foreach (var work in db.Work.OrderBy(x => x.Puntaje).ToList().Where(x => x.AreaID.Equals(area.Id) && x.EstadoID != 1 && x.EstadoID != 6 && x.TrabajoAprobado.GetValueOrDefault(false)).ToList())
                    {
                        var autores = work.Autores; //HtmlRemoval.StripTagsRegex(work.Autores);
                        var modalidad = db.WorkArea.Find(work.AreaID);

                        //var trabajoresumen = trabajos
                        //    ? HtmlRemoval.StripTagsRegex(work.FullWorkBody)
                        //    : HtmlRemoval.StripTagsRegex(work.Body);

                        var trabajoresumen = trabajos
                                ? (work.FullWorkBody)
                                : (work.Body);



                        cuerpo += string.Format("<h3>{0}</h3><h4>Modalidad: {1}</h4><h4>Integrante/s: {2}</h4>{3} <hr />",
                            HtmlRemoval.StripTagsRegex(work.Title),
                            modalidad.Nombre,
                            autores,
                            trabajoresumen);
                    }
                }
            }

            var body = indice + cuerpo;
            html = string.Format(html, body);
            var path = @"C:\TEMP\LibroResumenes.doc";
            if (trabajos) path = @"C:\TEMP\LibroTrabajos.doc";

            File.WriteAllText(path, html);
        }

        //public void CreateSampleDocument()
        //{
        //    // Modify to suit your machine:
        //    string fileName = @"C:\TEMP\LibroResumenes.docx";

        //    string headlineText = "Constitution of the United States";
        //    string paraOne = ""
        //        + "We the People of the United States, in Order to form a more perfect Union, "
        //        + "establish Justice, insure domestic Tranquility, provide for the common defence, "
        //        + "promote the general Welfare, and secure the Blessings of Liberty to ourselves "
        //        + "and our Posterity, do ordain and establish this Constitution for the United "
        //        + "States of America.";

        //    // A formatting object for our headline:
        //    var Titulo1 = new Formatting
        //    {
        //        FontFamily = new System.Drawing.FontFamily("Arial Black"),
        //        Size = 18D,
        //        Position = 12
        //    };
        //    var Titulo2 = new Formatting
        //    {
        //        FontFamily = new System.Drawing.FontFamily("Arial Black"),
        //        Size = 12D,
        //        Position = 12
        //    };
        //    var Texto1 = new Formatting { FontFamily = new System.Drawing.FontFamily("Arial"), Size = 10D };

        //    // Create the document in memory:
        //    var doc = DocX.Create(fileName);

        //    var db = new XIXCNPsicodiagnosticoEntities();

        //    var workAreas = db.WorkArea.ToList();
        //    foreach (var workArea in workAreas)
        //    {
        //        doc.InsertParagraph(workArea.Nombre, false, Titulo1);
        //        var worksInArea = db.Work.Where(x => x.AreaID.Equals(workArea.Id) && x.EstadoID != 1 && x.EstadoID != 6).ToList();
        //        foreach (var work in worksInArea)
        //        {
        //            doc.InsertParagraph(HtmlRemoval.StripTagsRegex(work.Title), false, Titulo2);
        //            doc.InsertParagraph(HtmlRemoval.StripTagsRegex(work.Body), false, Texto1);
        //        }
        //    }


        //    // Insert the now text obejcts;
        //    //doc.InsertParagraph(headlineText, false, headLineFormat);
        //    //doc.InsertParagraph(paraOne, false, paraFormat);

        //    // Save to the output directory:
        //    doc.Save();
        //}
    }
}
