using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace wsSIGENotif
{
    public class Logger
    {
        //HttpContext.Current.Request.MapPath("~");
        //Info Puede ser un simple registro en la bitácora.
        //Error Cuando la aplicación truena.
        //Debug Ejecución de queries, autenticación de usuarios, sesión expirada
        //private string _tipoExcepcion;
        //private string _nombreClase;
        private string _ruta = "";
        private string _carpeta;
        private string _nombreArchivo;
        private string _extensionArchivo;
        private string _separadoColumn;
        protected List<string> _lLogs = new List<string>();

        public Logger(string sRuta, string sCarpeta)
        {
            this._carpeta = "Logs/";
            this._nombreArchivo = "_Log_";
            //this._nombreArchivo = "Log_" + DateTime.Now.ToString("yyyy_MM_dd"); //para que guarde el archivo con fecha actual
            this._extensionArchivo = "txt";
            this._separadoColumn = " | ";
            this._ruta = sRuta;
            this._carpeta = sCarpeta;
        }
        public string Ruta
        {
            get { return _ruta + "/" + _carpeta; }
        }
        public bool CrearDirectorio()
        {
            try
            {
                if (!Directory.Exists(Ruta))
                    Directory.CreateDirectory(Ruta);
                return true;
            }
            catch (DirectoryNotFoundException ex)
            {
                Registrar("Error al crear el directorio: " + Ruta + " - " + ex.Message, "Error", ex.TargetSite.DeclaringType.Name, ex.TargetSite.ToString());
                return false;
            }
            catch (Exception ex)
            {
                string msj = ex.ToString();
                return false;
            }
        }

        public bool Registrar(string excepcion, string tipoExcepcion, string nombreClase, string metodoClase)
        {
            try
            {
                limpiarLista();

                if (!CrearDirectorio()) return false;

                string registro = "";
                registro += "Fecha: " + DateTime.Now + this._separadoColumn + " Excepción: " + excepcion + this._separadoColumn + " Tipo: " + tipoExcepcion + this._separadoColumn + " Clase: " + nombreClase + this._separadoColumn + " Método: " + metodoClase + Environment.NewLine;

                StreamWriter sw = new StreamWriter(Ruta + "/" + _nombreArchivo + "." + _extensionArchivo, true);
                sw.Write(registro);
                sw.Close();
                return true;
            }
            catch (Exception ex)
            {
                //Registrar("Error cargar el directorio: " + Ruta + " - " + ex.Message, "Error", ex.TargetSite.DeclaringType.Name, ex.TargetSite.ToString());
                string msj = ex.ToString();
                return false;
            }
        }

        public void limpiarLista()
        {
            _lLogs.Clear();
        }
    }
}