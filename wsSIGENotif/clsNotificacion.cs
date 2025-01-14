using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Text;
using System.Web;

namespace wsSIGENotif
{
    public class clsNotificacion
    {
        #region Propiedades privadas
        private List<System.Net.Mail.MailMessage> _correos;

        private int _idCorreo;              //id del correo a enviar
        private string _strAsunto;          //Asunto de la notificación
        private string _strMensaje;         //Cuerpo de la notificación
        private string _correoTo;           //Indica las direccion de correo a quien se enviara el mensaje
        private string _subject;            //Asunto del mensaje
        private string _message;            // Cuerpo del mensaje
        private string _strAmbiente;        // Ambiente desde el cual se enviarán las notificaciones
        private string _strMailsTo;         //Llave que indica si las notificaciones se enviarán a los destinatarios correctos o a un correo pretederminado

        System.Collections.ArrayList _lstParametros = new System.Collections.ArrayList(); // lista de parametros
        System.Collections.ArrayList _Mensajes = new System.Collections.ArrayList(); //Lista de mensajes
        private static string ruta = System.Web.Hosting.HostingEnvironment.MapPath("~");//Ruta en la raíz el proyecto
        private static Logger _log = new Logger(ruta, "Logs/");
        #endregion

        #region Contructores
        public clsNotificacion()
        {
            this._correos = new List<System.Net.Mail.MailMessage>();
        }

        #endregion

        #region Procedimientos de la Clase

        #region public void SendNotificacion()
        /// <summary>
        /// Procedimiento que enviará notificaciones vía correo electrónico según la opción que se paso en el constructor.
        /// Autor: L.I. Emmanuel Méndez Flores
        /// </summary>
        ///<param name="nidCorreo">id del correo a enviar</param>
        ///<param name="strAmbiente">Ambiente al que se conectará</param>
        ///<param name="strMailsTo">Cuentas de correos a las que se les enviará el correo para no enviarlo a los destinatarios del correo</param>
        /// <returns>devuelve una lista de objeto correos y una lista de objeto mensaje</returns>
        public string SendNotificacion(int nidCorreo, string strAmbiente, string strMailsTo)
        {
            _strAmbiente = strAmbiente.ToUpper();
            _strMailsTo = strMailsTo;
            _idCorreo = nidCorreo;
            string strRespuesta = "";

            pObtiene_Correo();
            strRespuesta = SendMails();
            if (strRespuesta.ToUpper() == "HECHO")
            {
                pActualiza_EstadoEnviado();
            }
            return strRespuesta;
        }
        #endregion

        private void pObtiene_Correo()
        {
            try
            {
                using (clsDALSQL objDALSQL = new clsDALSQL(false, _strAmbiente))
                {
                    string strAdvertenciaPrueba = "[NOTIFICACIÓN DE PRUEBA, HAGA CASO OMISO]";
                    string strBody = "";
                    libSQL lSQL = new libSQL();
                    _lstParametros.Add(lSQL.CrearParametro("@strACCION", "SELECCIONA_CORREO"));
                    _lstParametros.Add(lSQL.CrearParametro("@intIDCORREO", _idCorreo));
                    if (objDALSQL.ExecQuery_TBL("SP_SEL_CORREO", _lstParametros, ""))
                    {
                        DataTable dtCorreo = new DataTable();
                        dtCorreo = objDALSQL.Get_dtTable();

                        if (dtCorreo != null)
                        {
                            foreach (DataRow row in dtCorreo.Rows)
                            {
                                clsNotificacion objMensaje = new clsNotificacion(); // Objeto que almacenara los mensajes
                                System.Net.Mail.MailMessage correo = new System.Net.Mail.MailMessage();
                                correo.From = new System.Net.Mail.MailAddress(ConfigurationManager.AppSettings["AppMail"]); // Correo desde el Cual se enviaran las notificaciones
                                if (_strMailsTo == "")
                                {
                                    correo.To.Add(row["sDestinatarios"].ToString());
                                }
                                else
                                {
                                    correo.To.Add(_strMailsTo);
                                }

                                if (_strAmbiente != "PROD" && _strAmbiente != "SIGE")
                                {
                                    strBody = "<div align='center' style='color:#003366; font-family:Arial, Helvetica, sans-serif; font-size:20px; background-color:yellow;'>" + strAdvertenciaPrueba + "</div><br/>";
                                    strBody += row["sMensaje"].ToString();
                                    strBody += "<br/><div align='center' style='color:#003366; font-family:Arial, Helvetica, sans-serif; font-size:20px; background-color:yellow;' > " + strAdvertenciaPrueba + " </div>";
                                }
                                else
                                {
                                    strBody += row["sMensaje"].ToString();
                                }

                                correo.Subject = row["sAsunto"].ToString();
                                correo.SubjectEncoding = Encoding.UTF8;
                                correo.Body = strBody;
                                // USO DE HTML
                                correo.BodyEncoding = Encoding.UTF8;
                                correo.IsBodyHtml = true;
                                //
                                this._correos.Add(correo);

                                objMensaje.strAsunto = correo.Subject;
                                objMensaje.strMensaje = correo.Body;
                                objMensaje.correoTo = correo.To.ToString();
                                objMensaje.subject = correo.Subject.ToString();
                                this._Mensajes.Add(objMensaje);
                            }
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                _log.Registrar(ex.ToString(), "Debug", ex.TargetSite.DeclaringType.Name, ex.TargetSite.ToString());
            }
            finally
            {
                //_ds.Dispose();
            }
        }

        private void pActualiza_EstadoEnviado()
        {
            try
            {
                using (clsDALSQL objDALSQL = new clsDALSQL(false, _strAmbiente))
                {
                    libSQL lSQL = new libSQL();
                    _lstParametros.Add(lSQL.CrearParametro("@strACCION", "ACTUALIZA_ESTADO_ENV"));
                    _lstParametros.Add(lSQL.CrearParametro("@intIDCORREO", _idCorreo));
                    objDALSQL.ExecQuery("PA_IDUS_CORREO", _lstParametros);
                }
            }
            catch(Exception ex)
            {
                _log.Registrar("Error al actualizar el indicador de envio: " + ex.ToString(), "Debug", ex.TargetSite.DeclaringType.Name, ex.TargetSite.ToString());
            }
        }

        #region void SendMails()
        /// <summary>
        /// Función que se encarga de enviar los correos a los destinatarios correspondientes
        /// Autor: L.I. Emmanuel Méndez Flores
        /// </summary>
        /// <returns>devuelve un string si envio o no el correo</returns>
        public string SendMails()
        {
            string strRespuesta = "Hecho";
            clsNotificacion objNotifica = new clsNotificacion();
            //objNotifica.MailSplit();            // Obtiene la cuenta desde la cual se enviaran las notificaciones
            if (this._correos != null)
            {
                string strCuerpoMensaje = "";

                foreach (System.Net.Mail.MailMessage correo in this._correos)
                {
                    System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient();
                    smtp.Timeout = 200000;
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["AppMail"], ConfigurationManager.AppSettings["AppMailPwd"]);
                    smtp.Port = 587;
                    smtp.Host = ConfigurationManager.AppSettings["SmtpClient"];
                    smtp.EnableSsl = true;
                    smtp.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;

                    correoTo = correo.To.ToString();
                    subject = correo.Subject.ToString();
                    message = correo.Body.ToString();

                    if (message != strCuerpoMensaje)
                    {
                        strCuerpoMensaje = message;
                        try
                        {
                            smtp.Send(correo);
                        }
                        catch (Exception ex)
                        {
                            _log.Registrar(ex.ToString(), "Debug", ex.TargetSite.DeclaringType.Name, ex.TargetSite.ToString());
                            strRespuesta = ex.Message;
                            correo.Dispose();
                        }
                        finally
                        {
                            correo.Dispose();
                        }
                    }
                }
            }
            return strRespuesta;
        }
        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
        #endregion

        #endregion

        #region Getters y Setters
        public string strAsunto { get { return _strAsunto; } set { _strAsunto = value; } }                  //Asunto de la notificación
        public string strMensaje { get { return _strMensaje; } set { _strMensaje = value; } }               //Cuerpo de la notificación
        public string correoTo { get { return _correoTo; } set { _correoTo = value; } }                      //Indica las direccion de correo a quien se enviara el mensaje
        public string subject { get { return _subject; } set { _subject = value; } }                        //Asunto del mensaje
        public string message { get { return _message; } set { _message = value; } }                         // Cuerpo del mensaje
        public string strAmbiente { get { return _strAmbiente; } set { _strAmbiente = value; } }             // Ambiente desde el cual se enviarán las notificaciones
        public string strMailsTo { get { return _strMailsTo; } set { _strMailsTo = value; } }                 //Llave que indica si las notificaciones se enviarán a los destinatarios correctos o a un correo pretederminado
        #endregion
    }
}