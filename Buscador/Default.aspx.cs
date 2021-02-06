using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web;
using System.Web.UI;
using iTextSharp.text;
using iTextSharp.text.pdf;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;
using System.Net.Mail;
using System.Net;

namespace Buscador
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargaDatosAlumno();
            }
        }
        public void CargaDatosAlumno()
        {

            gvdAlumnos.DataSource = dtAlumnos();
            gvdAlumnos.DataBind();

        }
        public DataTable dtAlumnos()
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["connDB"].ConnectionString))
            {

                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SPSalumnos";
                cmd.Connection = conn;
                conn.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                // this will query your database and return the result to your datatable
                da.Fill(dt);
                conn.Close();
                da.Dispose();

            }
            return dt;
        }

        public void BusquedaAlumno()
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["connDB"].ConnectionString))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SPSalumnosPorNombre";
                cmd.Parameters.Add("@nombre", SqlDbType.VarChar).Value = txtNombreBus.Text.Trim();
                cmd.Connection = conn;
                conn.Open();
                gvdAlumnos.DataSource = cmd.ExecuteReader();
                gvdAlumnos.DataBind();
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            BusquedaAlumno();
        }

        protected void btnDescarga_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            Document document = new Document();
            PdfWriter writer = PdfWriter.GetInstance(document, HttpContext.Current.Response.OutputStream);

            dt = dtAlumnos();

            if (dt.Rows.Count > 0)
            {
                document.Open();
                Font fontTitle = FontFactory.GetFont(FontFactory.COURIER_BOLD, 25);
                Font font9 = FontFactory.GetFont(FontFactory.TIMES, 9);


                PdfPTable table = new PdfPTable(dt.Columns.Count);
                document.Add(new Paragraph(20, "Reporte de Alumnos 2020", fontTitle));
                document.Add(new Chunk("\n"));

                float[] widths = new float[dt.Columns.Count];
                for (int i = 0; i < dt.Columns.Count; i++)
                    widths[i] = 4f;

                table.SetWidths(widths);
                table.WidthPercentage = 90;

                PdfPCell cell = new PdfPCell(new Phrase("columns"));
                cell.Colspan = dt.Columns.Count;

                foreach (DataColumn c in dt.Columns)
                {
                    table.AddCell(new Phrase(c.ColumnName, font9));
                }

                foreach (DataRow r in dt.Rows)
                {
                    if (dt.Rows.Count > 0)
                    {
                        for (int h = 0; h < dt.Columns.Count; h++)
                        {
                            table.AddCell(new Phrase(r[h].ToString(), font9));
                        }
                    }
                }
                document.Add(table);
            }
            document.Close();
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "attachment; filename=alumnos2020" + ".pdf");
            HttpContext.Current.Response.Write(document);
            Response.Flush();
            Response.End();
        }

        protected void btnExcel_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            dt = dtAlumnos();

            Stream s = DataTableToExcel(dt);
            if (s != null)
            {
                MemoryStream ms = s as MemoryStream;
                Response.AddHeader("Content-Disposition", string.Format("attachment;filename=" + HttpUtility.UrlEncode("alumnos202014") + ".xlsx"));
                Response.ContentType = "application/vnd.ms-excel";
                Response.AddHeader("Content-Length", ms.ToArray().Length.ToString());
                Response.BinaryWrite(ms.ToArray());
                Response.Flush();
                ms.Close();
                ms.Dispose();
            }
            else
                Response.Write("Error!Connot Download");
        }

        private Stream DataTableToExcel(DataTable dt)
        {
            XSSFWorkbook workbook = new XSSFWorkbook();
            MemoryStream ms = new MemoryStream();
            ISheet sheet = workbook.CreateSheet("Alumnoxxx");      
            XSSFRow headerRow = headerRow = (XSSFRow)sheet.CreateRow(0);

            try
            {
                foreach (DataColumn column in dt.Columns)
                    headerRow.CreateCell(column.Ordinal).SetCellValue(column.ColumnName);
                int rowIndex = 1;
                foreach (DataRow row in dt.Rows)
                {
                    XSSFRow dataRow = (XSSFRow)sheet.CreateRow(rowIndex);
                    foreach (DataColumn column in dt.Columns)
                        dataRow.CreateCell(column.Ordinal).SetCellValue(row[column].ToString());
                    ++rowIndex;
                }
                for (int i = 0; i <= dt.Columns.Count; ++i)
                    sheet.AutoSizeColumn(i);
                workbook.Write(ms);
                ms.Flush();
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                ms.Close();
                sheet = null;
                headerRow = null;
                workbook = null;
            }
            return ms;
        }

        protected void btnEnvioCorreo_Click(object sender, EventArgs e)
        {

         SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
            smtpClient.Credentials = new NetworkCredential("aguilarsystems@hotmail.com", "xhrae30921140h");
           // smtpClient.Credentials = new NetworkCredential("aguilar.systems@gmail.com", "xhrae30921140g");
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.EnableSsl = true;
            smtpClient.UseDefaultCredentials = false;

            MailMessage mail = new MailMessage();        
            mail.From = new MailAddress("aguilarsystems@hotmail.com", "Escuela PRO");
            mail.To.Add(new MailAddress("aguilar.systems@gmail.com"));
            mail.Subject = "Mensaje de Bienvenida";
            mail.IsBodyHtml = true;
            mail.Body= body ;

     

            smtpClient.Send(mail);
        }
    }
}