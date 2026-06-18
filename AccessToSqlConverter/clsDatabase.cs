using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.SQLite;


namespace AccessToSqlConverter
{

    public class ClsSQLiteDatabase
    {
        ClsUtil util = new ClsUtil();
        ClsDebug dbug = new ClsDebug();
        private string _dbFileLocation;
        public string DbFileLocation
        {
            get { return _dbFileLocation; }
            set { _dbFileLocation = value; }
        }
        private string _templateFileLocation;
        public string TemplateFileLocation
        {
            get { return _templateFileLocation; }
            set { _templateFileLocation = value; }
        }
        public class ClsSqlSelect
        {
            //Constructor
            public ClsSqlSelect()
            {
            }
            private string _sqlText;
            public string SqlText
            {
                get { return _sqlText; }
                set { _sqlText = value; }
            }
            private string _tableName;
            public string TableName
            {
                get { return _tableName; }
                set { _tableName = value; }
            }
            private int _recordCount;
            public int RecordCount
            {
                get { return _recordCount; }
                set { _recordCount = value; }
            }
            private int _opStatus;
            public int OpStatus
            {
                get { return _opStatus; }
                set { _opStatus = value; }
            }
            private string _errorMsg;
            public string ErrorMsg
            {
                get { return _errorMsg; }
                set { _errorMsg = value; }
            }
            private string _fullErrorMsg;
            public string FullErrorMsg
            {
                get { return _fullErrorMsg; }
                set { _fullErrorMsg = value; }
            }
        }

        //Static declarations
        public SQLiteConnection dbConnection = new SQLiteConnection();
        public SQLiteCommand command = new SQLiteCommand();
        private static SQLiteDataAdapter selectAdapter = new SQLiteDataAdapter();
        SQLiteCommandBuilder dbCommandBuilder = new SQLiteCommandBuilder(selectAdapter);
        public DataSet dbDataSet = new DataSet();

        //Properties
        private string _connectionString;
        private string _errMessage;

        //Class instantiation
        public ClsSqlSelect SqlQry = new ClsSqlSelect();

        //Connection string property
        public string ConnectionString
        {
            get { return _connectionString; }
            set { _connectionString = value; }
        }
        public string ErrMessage
        {
            get { return _errMessage; }
            set { _errMessage = value; }
        }
        //Connect to database operation
        public SQLiteCommand Connect()
        {
            dbConnection.ConnectionString = _connectionString;

            command.Connection = new SQLiteConnection();
            try
            {
                command.Connection = dbConnection;
                command.Connection.Open();
                return command;
            }
            catch (SQLiteException)
            {
                return null;
            }
        }

        //Disconnect from database operation
        public int Disconnect()
        {
            try
            {
                dbConnection.Close();
                return ClsConstants.DISCONNECT_OK;
            }
            catch (SQLiteException)
            {
                return ClsConstants.DISCONNECT_FAILED;
            }
        }

        //Database Select Query
        public DataSet Select(string tbl)
        {
            DataSet ds;
            ds = new DataSet();
            ds.Clear();

            command.CommandText = SqlQry.SqlText;
            selectAdapter.SelectCommand = command;

            //Query database
            try
            {
                Connect();
                SqlQry.TableName = tbl;
                selectAdapter.Fill(ds, SqlQry.TableName);
                SqlQry.RecordCount = ds.Tables[SqlQry.TableName].Rows.Count;
                SqlQry.ErrorMsg = "Success";
                SqlQry.OpStatus = ClsConstants.DB_SUCCESS;
                SqlQry.TableName = "";
                Disconnect();
                return ds;
            }
            catch (SQLiteException err)
            {
                SqlQry.OpStatus = -1;
                SqlQry.ErrorMsg = err.Message;
                ErrMessage = err.Message;
                SqlQry.FullErrorMsg = Environment.NewLine + SqlQry.SqlText + Environment.NewLine + SqlQry.ErrorMsg;
                dbug.AddToDebug(SqlQry.FullErrorMsg);
                SqlQry.TableName = "";
                Disconnect();
                return null;
            }
        }

        //Database Update Query
        public int Update()
        {
            int retVal = 0;
            //Update databse
            try
            {
                Connect();
                command.CommandText = SqlQry.SqlText;
                command.Connection = dbConnection;
                retVal = command.ExecuteNonQuery();
                SqlQry.RecordCount = retVal;
                SqlQry.OpStatus = ClsConstants.DB_SUCCESS;
                Disconnect();
            }
            catch (Exception err)
            {
                SqlQry.OpStatus = -1;
                SqlQry.ErrorMsg = err.Message;
                ErrMessage = err.Message;
                dbug.AddToDebug(SqlQry.SqlText + Environment.NewLine + ErrMessage);
                retVal = ClsConstants.DB_UPDATE_FAILED;
                Disconnect();
            }

            return retVal;

        }

        //Database Update Query - Transactional
        public int Update(System.Collections.Generic.List<String> qry)
        {
            int retVal = 0;

            Connect();
            command.Connection = dbConnection;

            SQLiteTransaction myTransAction;
            myTransAction = dbConnection.BeginTransaction();
            command.Transaction = myTransAction;
            //Update databse
            try
            {
                //Loop through each sql query in list

                foreach (string query in qry)
                {
                    command.CommandText = query;
                    retVal = command.ExecuteNonQuery();
                }

                myTransAction.Commit();
                SqlQry.RecordCount = retVal;
                SqlQry.OpStatus = ClsConstants.DB_SUCCESS;
            }
            catch (Exception err)
            {
                myTransAction.Rollback();
                SqlQry.OpStatus = -1;
                SqlQry.ErrorMsg = err.Message;
                ErrMessage = err.Message;
                dbug.AddToDebug(SqlQry.SqlText + Environment.NewLine + ErrMessage);
                retVal = ClsConstants.DB_UPDATE_FAILED;
            }
            finally
            {
                Disconnect();
            }

            return retVal;

        }

        //Database List Table Fields
        public List<string> readTableFilelds(string tblName)
        {
            var retList = new List<string>();

            //Update databse
            try
            {
                Connect();

                //Initialise restricitions
                var sqlSchemaTable = dbConnection.GetSchema("Columns", new[] { null, null, tblName, null });

                //Populate return variable
                foreach (DataRow row in sqlSchemaTable.Rows)
                {
                    retList.Add(row["COLUMN_NAME"].ToString());
                }
                Disconnect();
            }
            catch (Exception err)
            {
                SqlQry.OpStatus = -1;
                SqlQry.ErrorMsg = err.Message;
                ErrMessage = err.Message;
                dbug.AddToDebug(SqlQry.SqlText + Environment.NewLine + ErrMessage);
                retList.Add(ClsConstants.DB_UPDATE_FAILED.ToString());
                Disconnect();
            }

            return retList;

        }

        //Destructor
        ~ClsSQLiteDatabase()
        {

        }
    }

    public class ClsOleDatabase
    {
        ClsUtil util = new ClsUtil();
        ClsDebug dbug = new ClsDebug();
        private string _dbFileLocation;
        public string DbFileLocation
        {
            get { return _dbFileLocation; }
            set { _dbFileLocation = value; }
        }
        private string _templateFileLocation;
        public string TemplateFileLocation
        {
            get { return _templateFileLocation; }
            set { _templateFileLocation = value; }
        }
        public class ClsSqlSelect
        {
            //Constructor
            public ClsSqlSelect()
            {
            }
            private string _sqlText;
            public string SqlText
            {
                get { return _sqlText; }
                set { _sqlText = value; }
            }
            private string _tableName;
            public string TableName
            {
                get { return _tableName; }
                set { _tableName = value; }
            }
            private int _recordCount;
            public int RecordCount
            {
                get { return _recordCount; }
                set { _recordCount = value; }
            }
            private int _opStatus;
            public int OpStatus
            {
                get { return _opStatus; }
                set { _opStatus = value; }
            }
            private string _errorMsg;
            public string ErrorMsg
            {
                get { return _errorMsg; }
                set { _errorMsg = value; }
            }
            private string _fullErrorMsg;
            public string FullErrorMsg
            {
                get { return _fullErrorMsg; }
                set { _fullErrorMsg = value; }
            }
        }

        //Static declarations
        public OleDbConnection dbConnection = new OleDbConnection();
        public OleDbCommand command = new OleDbCommand();
        private static OleDbDataAdapter selectAdapter = new OleDbDataAdapter();
        OleDbCommandBuilder dbCommandBuilder = new OleDbCommandBuilder(selectAdapter);
        public DataSet dbDataSet = new DataSet();

        //Properties
        private string _connectionString;
        private string _errMessage;

        //Class instantiation
        public ClsSqlSelect SqlQry = new ClsSqlSelect();

        //Connection string property
        public string ConnectionString
        {
            get { return _connectionString; }
            set { _connectionString = value; }
        }
        public string ErrMessage
        {
            get { return _errMessage; }
            set { _errMessage = value; }
        }
        //Connect to database operation
        public OleDbCommand Connect()
        {
            dbConnection.ConnectionString = _connectionString;

            command.Connection = new OleDbConnection();
            try
            {
                command.Connection = dbConnection;
                command.Connection.Open();
                return command;
            }
            catch (SQLiteException)
            {
                return null;
            }
        }

        //Disconnect from database operation
        public int Disconnect()
        {
            try
            {
                dbConnection.Close();
                return ClsConstants.DISCONNECT_OK;
            }
            catch (SQLiteException)
            {
                return ClsConstants.DISCONNECT_FAILED;
            }
        }

        //Database Select Query
        public DataSet Select(string tbl)
        {
            DataSet ds;
            ds = new DataSet();
            ds.Clear();

            command.CommandText = SqlQry.SqlText;
            selectAdapter.SelectCommand = command;

            //Query database
            try
            {
                Connect();
                SqlQry.TableName = tbl;
                selectAdapter.Fill(ds, SqlQry.TableName);
                SqlQry.RecordCount = ds.Tables[SqlQry.TableName].Rows.Count;
                SqlQry.ErrorMsg = "Success";
                SqlQry.OpStatus = ClsConstants.DB_SUCCESS;
                SqlQry.TableName = "";
                Disconnect();
                return ds;
            }
            catch (SQLiteException err)
            {
                SqlQry.OpStatus = -1;
                SqlQry.ErrorMsg = err.Message;
                ErrMessage = err.Message;
                SqlQry.FullErrorMsg = Environment.NewLine + SqlQry.SqlText + Environment.NewLine + SqlQry.ErrorMsg;
                dbug.AddToDebug(SqlQry.FullErrorMsg);
                SqlQry.TableName = "";
                Disconnect();
                return null;
            }
        }

        //Database Update Query
        public int Update()
        {
            int retVal = 0;
            //Update databse
            try
            {
                Connect();
                command.CommandText = SqlQry.SqlText;
                command.Connection = dbConnection;
                retVal = command.ExecuteNonQuery();
                SqlQry.RecordCount = retVal;
                SqlQry.OpStatus = ClsConstants.DB_SUCCESS;
                Disconnect();
            }
            catch (Exception err)
            {
                SqlQry.OpStatus = -1;
                SqlQry.ErrorMsg = err.Message;
                ErrMessage = err.Message;
                dbug.AddToDebug(SqlQry.SqlText + Environment.NewLine + ErrMessage);
                retVal = ClsConstants.DB_UPDATE_FAILED;
                Disconnect();
            }

            return retVal;

        }

        //Database Update Query - Transactional
        public int Update(System.Collections.Generic.List<String> qry)
        {
            int retVal = 0;

            Connect();
            command.Connection = dbConnection;

            OleDbTransaction myTransAction;
            myTransAction = dbConnection.BeginTransaction();
            command.Transaction = myTransAction;
            //Update databse
            try
            {
                //Loop through each sql query in list

                foreach (string query in qry)
                {
                    command.CommandText = query;
                    retVal = command.ExecuteNonQuery();
                }

                myTransAction.Commit();
                SqlQry.RecordCount = retVal;
                SqlQry.OpStatus = ClsConstants.DB_SUCCESS;
            }
            catch (Exception err)
            {
                myTransAction.Rollback();
                SqlQry.OpStatus = -1;
                SqlQry.ErrorMsg = err.Message;
                ErrMessage = err.Message;
                dbug.AddToDebug(SqlQry.SqlText + Environment.NewLine + ErrMessage);
                retVal = ClsConstants.DB_UPDATE_FAILED;
            }
            finally
            {
                Disconnect();
            }

            return retVal;

        }

        //Destructor
        ~ClsOleDatabase()
        {

        }
    }
}
