using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace wsSIGENotif
{
    public class libSQL
    {
        public libSQL()
        {
            //
            // TODO: Agregar aquí la lógica del constructor
            //
        }

        public SqlParameter CrearParametro(string sNombre, object oValor)
        {
            SqlParameter sqlParam = new SqlParameter();

            sqlParam.ParameterName = sNombre;
            sqlParam.Value = oValor;
            return sqlParam;
        }

        public SqlParameter CrearParametro(string sNombre, object oValor, SqlDbType tTipo)
        {
            SqlParameter sqlParam = new SqlParameter();

            sqlParam.ParameterName = sNombre;
            sqlParam.Value = oValor;
            sqlParam.SqlDbType = tTipo;
            return sqlParam;
        }

        public SqlParameter CrearParametro(string sNombre, object oValor, SqlDbType tTipo, int nTamanio, byte nEscala)
        {
            SqlParameter sqlParam = new SqlParameter();

            sqlParam.ParameterName = sNombre;
            sqlParam.Value = oValor;
            sqlParam.SqlDbType = tTipo;
            sqlParam.Size = nTamanio;
            sqlParam.Scale = nEscala;
            return sqlParam;
        }

        public SqlParameter CrearParametro(string sNombre, object oValor, SqlDbType tTipo, int nTamanio)
        {
            SqlParameter sqlParam = new SqlParameter();

            sqlParam.ParameterName = sNombre;
            sqlParam.Value = oValor;
            sqlParam.SqlDbType = tTipo;
            sqlParam.Size = nTamanio;
            return sqlParam;
        }

        public SqlParameter CrearParametro(string sNombre, SqlDbType tTipo, System.Data.ParameterDirection tDireccion)
        {
            SqlParameter sqlParam = new SqlParameter();

            sqlParam.ParameterName = sNombre;
            sqlParam.SqlDbType = tTipo;
            sqlParam.Direction = tDireccion;
            return sqlParam;
        }

        public SqlParameter CrearParametro(string sNombre, object oValor, SqlDbType tTipo, System.Data.ParameterDirection tDireccion)
        {
            SqlParameter sqlParam = new SqlParameter();

            sqlParam.ParameterName = sNombre;
            sqlParam.Value = oValor;
            sqlParam.SqlDbType = tTipo;
            sqlParam.Direction = tDireccion;
            return sqlParam;
        }

        public SqlParameter CrearParametro(string sNombre, object oValor, SqlDbType tTipo, int nTamanio, byte nEscala, System.Data.ParameterDirection tDireccion)
        {
            SqlParameter sqlParam = new SqlParameter();

            sqlParam.ParameterName = sNombre;
            sqlParam.Value = oValor;
            sqlParam.SqlDbType = tTipo;
            sqlParam.Size = nTamanio;
            sqlParam.Scale = nEscala;
            sqlParam.Direction = tDireccion;
            return sqlParam;
        }


        #region IDisposable Members
        /// <summary>
        /// Procedimiento para destruir objetos
        /// Autor: L.I. Erik José Enriquez Carmona
        /// </summary>
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}