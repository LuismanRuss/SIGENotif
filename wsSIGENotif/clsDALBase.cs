using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace wsSIGENotif
{
    public abstract class clsDALBase : IDisposable
    {
        protected DataSet _dtSet;
        protected DataTable _dtTable;
        protected String _sScalar;
        protected String _sErrMsg;
        protected ArrayList _aOutput;


        #region Metodos heredables de la clase base

        abstract public Boolean Conectar();
        abstract public Boolean Desconectar();
        abstract public Boolean Conectar(String sConexion);

        abstract public Boolean ExecQuery_SET(String sProcAlmacenado, ArrayList aParametros);
        abstract public Boolean ExecQuery_SET_OUT(String sProcAlmacenado, ArrayList aParametros);
        abstract public Boolean ExecQuery_TBL(String sProcAlmacenado, ArrayList aParametros, String sOrdenadoPor);
        abstract public Boolean ExecQuery_SCL(String sProcAlmacenado, ArrayList aParametros);
        abstract public Boolean ExecQuery_OUT(String sProcAlmacenado, ArrayList aParametros);
        abstract public Boolean ExecQuery(String sProcAlmacenado, ArrayList aParametros);

        #endregion


        #region "Propiedades públicas de la Clase Base"
        #endregion


        #region "Métodos públicos de la Clase Base"

        public DataSet Get_dtSet()
        {
            return _dtSet;
        }

        public DataTable Get_dtTable()
        {
            return _dtTable;
        }

        public String Get_sScalar()
        {
            return _sScalar;
        }

        public String Get_sError()
        {
            return _sErrMsg;
        }

        public ArrayList Get_aOutput()
        {
            return _aOutput;
        }


        #endregion


        #region IDisposable Members

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}