using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace wsSIGENotif
{
    public class clsDALSQL : clsDALBase
    {
        private SqlConnection _sqlCnn;
        private Boolean _bMultiQuery;

        #region "Métdos públicos de la Clase"

        public clsDALSQL(Boolean bMultiQuery, string sAmbiente)
        {
            _sqlCnn = new SqlConnection(DES.funDES_FromBase64(ConfigurationManager.AppSettings[sAmbiente]));
            //DES.funDES_FromBase64
            _bMultiQuery = bMultiQuery;
        }

        public clsDALSQL()
        {
            // TODO: Complete member initialization
        }
        #endregion


        #region "Métodos privados de la Clase"
        #endregion


        #region Métodos heredados de la Clase clsDALBase

        override public Boolean Conectar()
        {
            Boolean blnStatus = false;

            if (_bMultiQuery)
            {
                _sErrMsg = String.Empty;
                try
                {
                    _sqlCnn.Open();
                    blnStatus = true;
                }
                catch (SqlException sqlExcep)
                {
                    _sErrMsg = sqlExcep.Number.ToString() + " - " + sqlExcep.Message;
                    _sqlCnn.Close();
                    SqlConnection.ClearAllPools();
                }
            }
            return blnStatus;
        }

        override public Boolean Desconectar()
        {
            Boolean blnStatus = false;

            if (_bMultiQuery)
            {
                _sErrMsg = String.Empty;
                try
                {
                    _sqlCnn.Close();
                    blnStatus = true;
                }
                catch (SqlException sqlExcep)
                {
                    _sErrMsg = sqlExcep.Number.ToString() + " - " + sqlExcep.Message;
                }
                finally
                {
                    SqlConnection.ClearAllPools();
                }
            }
            return blnStatus;
        }

        override public Boolean Conectar(String sConexion)
        {
            Boolean blnStatus = false;

            if (_bMultiQuery)
            {
                Desconectar();
                _sqlCnn = new SqlConnection(sConexion);
                _sErrMsg = String.Empty;
                try
                {
                    _sqlCnn.Open();
                    blnStatus = true;
                }
                catch (SqlException sqlExcep)
                {
                    _sErrMsg = sqlExcep.Number.ToString() + " - " + sqlExcep.Message;
                    _sqlCnn.Close();
                    SqlConnection.ClearAllPools();
                }
            }
            return blnStatus;
        }

        override public Boolean ExecQuery_SET_OUT(String sProcAlmacenado, ArrayList aParametros)
        {
            Boolean blnStatus = false;
            SqlDataAdapter sqlAdp = new SqlDataAdapter(sProcAlmacenado, _sqlCnn);
            int nPO = 0;
            _dtSet = new DataSet();
            _sErrMsg = String.Empty;
            _aOutput = new ArrayList();
            try
            {
                sqlAdp.SelectCommand.CommandTimeout = 360;
                sqlAdp.SelectCommand.CommandType = CommandType.StoredProcedure;
                if (aParametros != null)
                {
                    for (int i = 0; i < aParametros.Count; i++)
                    {
                        sqlAdp.SelectCommand.Parameters.Add(aParametros[i]);
                        if (sqlAdp.SelectCommand.Parameters[i].Direction == ParameterDirection.Output)
                        {
                            _aOutput.Add(sqlAdp.SelectCommand.Parameters[i].Value);
                            nPO++;
                        }
                    }
                    aParametros.Clear();
                }

                if (!_bMultiQuery)
                {
                    _sqlCnn.Open();
                }
                sqlAdp.Fill(_dtSet);

                if (nPO > 0)
                {
                    _aOutput = new ArrayList(nPO);
                    foreach (SqlParameter sqlP in sqlAdp.SelectCommand.Parameters)
                    {
                        if (sqlP.Direction == ParameterDirection.Output)
                        {
                            _aOutput.Add(sqlP.Value);
                        }
                    }
                }
                blnStatus = true;
            }
            catch (SqlException sqlExcep)
            {
                _sErrMsg = sqlExcep.Number.ToString() + " - " + sqlExcep.Message;
            }
            finally
            {
                if (!_bMultiQuery)
                {
                    _sqlCnn.Close();
                    SqlConnection.ClearAllPools();
                }
            }
            return blnStatus;
        }

        override public Boolean ExecQuery_SET(String sProcAlmacenado, ArrayList aParametros)
        {
            Boolean blnStatus = false;
            SqlDataAdapter sqlAdp = new SqlDataAdapter(sProcAlmacenado, _sqlCnn);

            _dtSet = new DataSet();
            _sErrMsg = String.Empty;
            try
            {
                sqlAdp.SelectCommand.CommandTimeout = 360;
                sqlAdp.SelectCommand.CommandType = CommandType.StoredProcedure;
                if (aParametros != null)
                {
                    for (int i = 0; i < aParametros.Count; i++)
                    {
                        sqlAdp.SelectCommand.Parameters.Add(aParametros[i]);
                    }
                    aParametros.Clear();
                }
                if (!_bMultiQuery)
                {
                    _sqlCnn.Open();
                }
                sqlAdp.Fill(_dtSet);
                blnStatus = true;
            }
            catch (SqlException sqlExcep)
            {
                _sErrMsg = sqlExcep.Number.ToString() + " - " + sqlExcep.Message;
            }
            finally
            {
                if (!_bMultiQuery)
                {
                    _sqlCnn.Close();
                    SqlConnection.ClearAllPools();
                }
            }
            return blnStatus;
        }

        override public Boolean ExecQuery_TBL(String sProcAlmacenado, ArrayList aParametros, String sOrdenadoPor)
        {
            Boolean blnStatus = false;
            SqlDataAdapter sqlAdp = new SqlDataAdapter(sProcAlmacenado, _sqlCnn);

            _dtTable = new DataTable();
            _sErrMsg = String.Empty;
            try
            {
                sqlAdp.SelectCommand.CommandTimeout = 360;
                sqlAdp.SelectCommand.CommandType = CommandType.StoredProcedure;
                if (aParametros != null)
                {
                    for (int i = 0; i < aParametros.Count; i++)
                    {
                        sqlAdp.SelectCommand.Parameters.Add(aParametros[i]);
                    }
                    aParametros.Clear();
                }
                if (!_bMultiQuery)
                {
                    _sqlCnn.Open();
                }
                sqlAdp.Fill(_dtTable);
                _dtTable.DefaultView.Sort = sOrdenadoPor;
                blnStatus = true;
            }
            catch (SqlException sqlExcep)
            {
                _sErrMsg = sqlExcep.Number.ToString() + " - " + sqlExcep.Message;
            }
            finally
            {
                if (!_bMultiQuery)
                {
                    _sqlCnn.Close();
                    SqlConnection.ClearAllPools();
                }
            }
            return blnStatus;
        }

        override public Boolean ExecQuery_SCL(String sProcAlmacenado, ArrayList aParametros)
        {
            Boolean blnStatus = false;
            SqlCommand sqlCmd = new SqlCommand(sProcAlmacenado, _sqlCnn);

            _sScalar = String.Empty;
            _sErrMsg = String.Empty;
            try
            {
                sqlCmd.CommandTimeout = 360;
                sqlCmd.CommandType = CommandType.StoredProcedure;
                if (aParametros != null)
                {
                    for (int i = 0; i < aParametros.Count; i++)
                    {
                        sqlCmd.Parameters.Add(aParametros[i]);
                    }
                    aParametros.Clear();
                }
                if (!_bMultiQuery)
                {
                    _sqlCnn.Open();
                }
                _sScalar = (string)sqlCmd.ExecuteScalar();
                if (_sScalar == null) { _sScalar = String.Empty; }
                blnStatus = true;
            }
            catch (SqlException sqlExcep)
            {
                _sErrMsg = sqlExcep.Number.ToString() + " - " + sqlExcep.Message;
            }
            finally
            {
                if (!_bMultiQuery)
                {
                    _sqlCnn.Close();
                    SqlConnection.ClearAllPools();
                }
            }
            return blnStatus;
        }

        override public Boolean ExecQuery_OUT(String sProcAlmacenado, ArrayList aParametros)
        {
            Boolean blnStatus = false;
            SqlCommand sqlCmd = new SqlCommand(sProcAlmacenado, _sqlCnn);

            _aOutput = new ArrayList();
            _sErrMsg = String.Empty;
            try
            {
                sqlCmd.CommandTimeout = 360;
                sqlCmd.CommandType = CommandType.StoredProcedure;
                if (aParametros != null)
                {
                    for (int i = 0; i < aParametros.Count; i++)
                    {
                        sqlCmd.Parameters.Add(aParametros[i]);
                    }
                    aParametros.Clear();
                }
                if (!_bMultiQuery)
                {
                    _sqlCnn.Open();
                }

                for (int i = 0; i < sqlCmd.Parameters.Count; i++)
                {
                    if (sqlCmd.Parameters[i].Direction == ParameterDirection.Output)
                    {
                        _aOutput.Add(sqlCmd.Parameters[i].Value);
                    }
                }
                sqlCmd.ExecuteNonQuery();
                if (_aOutput.Count > 0 && sqlCmd.Parameters.Count > 0)
                {
                    _aOutput = new ArrayList(sqlCmd.Parameters.Count);
                    foreach (SqlParameter sqlP in sqlCmd.Parameters)
                    {
                        if (sqlP.Direction == ParameterDirection.Output)
                            _aOutput.Add(sqlP.Value);
                    }
                }
                blnStatus = true;
            }
            catch (SqlException sqlExcep)
            {
                _sErrMsg = sqlExcep.Number.ToString() + " - " + sqlExcep.Message;
            }
            finally
            {
                if (!_bMultiQuery)
                {
                    _sqlCnn.Close();
                    SqlConnection.ClearAllPools();
                }
            }
            return blnStatus;
        }

        override public Boolean ExecQuery(String sProcAlmacenado, ArrayList aParametros)
        {
            Boolean blnStatus = false;
            SqlCommand sqlCmd = new SqlCommand(sProcAlmacenado, _sqlCnn);

            _sErrMsg = String.Empty;
            try
            {
                sqlCmd.CommandTimeout = 360;
                sqlCmd.CommandType = CommandType.StoredProcedure;
                if (aParametros != null)
                {
                    for (int i = 0; i < aParametros.Count; i++)
                    {
                        sqlCmd.Parameters.Add(aParametros[i]);
                    }
                    aParametros.Clear();
                }
                if (!_bMultiQuery)
                {
                    _sqlCnn.Open();
                }
                sqlCmd.ExecuteNonQuery();
                blnStatus = true;
            }
            catch (SqlException sqlExcep)
            {
                _sErrMsg = sqlExcep.Number.ToString() + " - " + sqlExcep.Message;
            }
            finally
            {
                if (!_bMultiQuery)
                {
                    _sqlCnn.Close();
                    SqlConnection.ClearAllPools();
                }
            }
            return blnStatus;
        }

        #endregion
    }
}