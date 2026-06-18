using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.SQLite;
using System.IO;
using System.Windows.Forms;

namespace AccessToSqlConverter
{
    public partial class Form1 : Form
    {
        //List definitions
        List<int> oldExcelIDs = new List<int>();
        List<clsDbTableFields> lstDbTableField = new List<clsDbTableFields>();

        //User directory must contain xml configuration file
        string myPath = string.Empty;

        //Instantiate classes
        ClsDebug dbug = new ClsDebug();

        ClsSQLiteDatabase sqlDb = new ClsSQLiteDatabase();
        ClsOleDatabase oleDb = new ClsOleDatabase();

        ClsConstants ClsConst = new ClsConstants();

        SQLiteConnection connSql = new SQLiteConnection();
        OleDbConnection connAccessDb = new OleDbConnection();

        string sqlDbFilePath = string.Empty;
        string oleDbFilePath = string.Empty;

        public Form1()
        {
            //Initialize Environment
            myPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string fullFileName = myPath + @"\" + ClsConstants.DBUG_FILENAME;
            GlbData.SetDbugFileName = fullFileName;
            File.WriteAllText(fullFileName, "");

            //Create SQLite database connection string
            sqlDbFilePath = System.Environment.CurrentDirectory + @"\dbSatDetails.db; Version = 3; New = True; Compress = True;";
            sqlDb.ConnectionString = "Data Source = " + sqlDbFilePath;

            InitializeComponent();
            lblSelectedDatabase.Text = "";
            tsLabel1.Text = "SQLite Db Connection String > " + sqlDbFilePath;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Terminate application
            Application.Exit();
        }
        private void TerminateApp(string msg)
        {
            //List definitions
            List<int> oldExcelIDs = new List<int>();
            List<clsDbTableFields> lstDbTableField = new List<clsDbTableFields>();

            //Delete all Excel records from DB
            sqlDb.SqlQry.SqlText = "Delete From " + ClsConstants.TBL_PROC_ID;
            if (sqlDb.Update() == ClsConstants.DB_FAIL)
            {
                dbug.AddToDebug("Delete Excel Process Information");
            }
            //Disconnect from database
            sqlDb.Disconnect();

            if (!string.IsNullOrEmpty(msg))
            {
                MessageBox.Show(msg, "Application Error", MessageBoxButtons.OK);
            }

            //Teminate application
            Environment.Exit(1);

        }

        private void btnSelectAccessDb_Click(object sender, EventArgs e)

        {
            //Initialise User Interface

            bool validFileName = true;

            string dbFullFileName = string.Empty;
            string dbSafeName = string.Empty;
            string attr = string.Empty;
            string dbPath = string.Empty;

            //Open channel to database
            OpenFileDialog accessDbFileLocation = new OpenFileDialog
            {
                InitialDirectory = @"V:\Documents\GitHub\C# Projects\AccessToSqlConverter",
                Filter = "Access DB (*.accdb)|*.accdb"
            };
            if (accessDbFileLocation.ShowDialog() == DialogResult.OK)
            {
                // Check for valid attraction CR1, VR1 or VR2
                dbFullFileName = accessDbFileLocation.FileName;
                dbSafeName = accessDbFileLocation.SafeFileName;
                attr = dbSafeName.Substring(0, 3);
                dbPath = Path.GetDirectoryName(dbFullFileName);

                switch (attr)
                {
                    case "CR1":
                        break;

                    case "VR1":
                        break;

                    case "VR2":
                        break;

                    default:
                        validFileName = false;
                        break;

                }
            }

            if (validFileName)
            {
                lblSelectedDatabase.Text = accessDbFileLocation.FileName;

                //Create Access database connection string
                string oleDbFilePath = dbFullFileName;
                oleDb.ConnectionString = "Provider = Microsoft.ACE.OLEDB.12.0; Data Source = " + oleDbFilePath + ";Persist Security Info=False;";
                connAccessDb.ConnectionString = oleDb.ConnectionString;
                connAccessDb.Open();
                btnDbImport.Enabled = true;
            }
            else
            {
                lblSelectedDatabase.Text = "Invalid Database File";
            }

        }

        private void btnDbImport_Click(object sender, EventArgs e)
        {
            if (lblSelectedDatabase.Text != string.Empty)
            {

                //Generate SQLite database based on contents of selected Access databse
                StoreAccesDbStructure(lblSelectedDatabase.Text);
            }
            else
            {
                MessageBox.Show("Invalid Database Name", "User Error", MessageBoxButtons.OK);
            }
        }
        private void StoreAccesDbStructure(string accessDb)
        {
            string tableName = string.Empty;
            string fieldName = string.Empty;

            string[] tableRestrictions = new string[4];
            tableRestrictions[3] = "TABLE";

            //Open stream to access db

            //Construct access db connection string
            string path = Environment.CurrentDirectory;
            path = lblSelectedDatabase.Text;
            connAccessDb = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path);

            //Get Access DB table definitions
            connAccessDb.Open();
            DataTable tables = connAccessDb.GetSchema("Tables", tableRestrictions);
            DataRow tblNameRow = tables.Rows[0];
            tableName = tblNameRow["TABLE_NAME"].ToString();

            //Get each of the field names, field types and enumurated OLE field type

            string[] colRestrictions = new string[] { null, null, tableName, null };
            DataTable columns = connAccessDb.GetSchema("Columns", colRestrictions);

            foreach (DataRow row in columns.Select("", "ORDINAL_POSITION"))
            {

                clsDbTableFields temp = new clsDbTableFields();
                temp.fieldName = (string)(row["COLUMN_NAME"]);
                temp.fieldType = Convert.ToInt32(row["DATA_TYPE"]);
                temp.oleFieldType = (OleDbType)temp.fieldType;

                if (temp.oleFieldType == OleDbType.Date)
                {
                    temp.fieldType = 130;
                    temp.oleFieldType = OleDbType.WChar;
                }

                //Add delimter to class
                switch (temp.fieldType)
                {
                    case ClsConstants.sqlInteger: // Integer
                    case ClsConstants.sqlDouble: // Double
                    case ClsConstants.sqlBoolean: // Boolean

                        //Null delimiter
                        temp.delimiter = string.Empty;
                        break;

                    case ClsConstants.sqlDate: //Date - handle as string
                    case ClsConstants.sqlWChar: //WChar

                        //delimiter = '
                        temp.delimiter = "'";
                        break;

                }

                lstDbTableField.Add(temp);
            }

            //Create SQlite DB based on contents of Access DB

            genSqlLiteDb(tableName, lstDbTableField);
        }

        //Generate SQLite database tables and associated fields based on selected Access database
        private void genSqlLiteDb(string tblName, List<clsDbTableFields> accessDbDefintions)
        {

            //Query database for table name
            sqlDb.SqlQry.SqlText = "SELECT name FROM sqlite_master WHERE type = 'table' AND name='" + tblName + "'";
            sqlDb.Select("sqlite_master");

            if (sqlDb.SqlQry.RecordCount == 0)
            {
                //Data table does not exist - create data table and associated fields
                sqlDb.SqlQry.SqlText = "CREATE table " + tblName + " (id integer primary key)";
                sqlDb.Update();
                if (sqlDb.SqlQry.OpStatus != ClsConstants.DB_SUCCESS)
                {
                    TerminateApp("Error Detected Creating Database Table " + tblName);
                }

                //Field definitions
                foreach (clsDbTableFields field in accessDbDefintions)
                {
                    //Check field exists by query
                    sqlDb.SqlQry.SqlText = "SELECT [" + field.fieldName + "] FROM " + tblName;

                    sqlDb.Select(tblName);

                    if (sqlDb.SqlQry.OpStatus != ClsConstants.DB_SUCCESS)
                    {
                        if (field.oleFieldType == OleDbType.Date)
                            field.oleFieldType = OleDbType.WChar;

                        //    field.oleFieldType = ClsConstants.oleDtString;
                        sqlDb.SqlQry.SqlText = "ALTER TABLE " + tblName + " ADD COLUMN " + field.fieldName + " " + field.oleFieldType + ";";
                        sqlDb.Update();
                    }

                }

                //SQLite database now created sequentially read Access db records and store in SQLite db

                //QueryAccess db for all records
                DataSet dsAccessRecords;

                oleDb.SqlQry.SqlText = "Select * From " + tblName + " Order By [ID]";
                dsAccessRecords = oleDb.Select(tblName);

                if (oleDb.SqlQry.OpStatus == ClsConstants.DB_SUCCESS)
                {
                    string updateQry = string.Empty;
                    string colNames = string.Empty;
                    string colValues = string.Empty;

                    int rowCnt = 0;

                    bool firstPass = true;

                    //Build field and field value strings
                    foreach (DataRow row in dsAccessRecords.Tables[0].Rows)
                    {
                        //Initialise column values part of SQL insert command
                        colValues = string.Empty;

                        rowCnt++;
                        tsLabel1.Text = "Processing Row " + rowCnt;
                        statusStrip1.Refresh();

                        //Build SQL UPDATE query command
                        for (int i = 0; i < dsAccessRecords.Tables[0].Columns.Count; i++)
                        {
                            object v = row[i];

                            string c = dsAccessRecords.Tables[0].Columns[i].ColumnName;
                            string dt = dsAccessRecords.Tables[0].Columns[i].DataType.ToString();
                            string delimiter = string.Empty;

                            //Add delimter to class
                            switch (dt)
                            {
                                case ClsConstants.oleDtBoolean:
                                case ClsConstants.oleDtDouble:
                                case ClsConstants.oleDtInt32:
                                    //Null delimiter
                                    delimiter = string.Empty;
                                    break;

                                case ClsConstants.oleDtString:
                                case ClsConstants.oleDtDateTime:
                                    //WChar - delimiter = '
                                    delimiter = "'";
                                    break;

                                default:
                                    MessageBox.Show("Invalid Data Type In Access DB Definition");
                                    break;

                            }
                            v = delimiter + v + delimiter;
                            colValues += v + ",";

                            if (firstPass)
                            {
                                colNames += c + ",";
                            }
                        }

                        //Only build column sql on first record
                        if (firstPass)
                        {
                            //Remove trailing "," delimter
                            colNames = colNames.Substring(0, colNames.Length - 1);
                        }
                        firstPass = false;

                        colValues = colValues.Substring(0, colValues.Length - 1);
                        //Complete SQL insert command
    
                        updateQry = "INSERT INTO " + tblName + " (" + colNames + ")";
                        updateQry += "VALUES (" + colValues + ");";

                        sqlDb.SqlQry.SqlText = updateQry;
                        sqlDb.Update();
                    }
                }
                else
                {
                    MessageBox.Show(oleDb.SqlQry.ErrorMsg);
                }

            }
            else
            {
                //If SQLite database exists check that current and new database schema are the same
                validateDbSchema(lstDbTableField, tblName);

                MessageBox.Show("SQLite Database Already Exists");
            }

        }

        private bool validateDbSchema(List<clsDbTableFields> accessDbFields, string tblName)
        {
            List<clsDbTableFields> lstOleDbDef = new List<clsDbTableFields>();
            List<clsDbTableFields> lstSqlDbDef = new List<clsDbTableFields>();

            bool retVal = false;

            //Compare each OLE DB field to the SQL field - note database fields do not need to be in the same order
            //If OLE db field does not exist in SQL db - create it
            //If SQL db field does not exist in OLE db request confirm deltion of field



            sqlDb.readTableFilelds(tblName);
            return retVal;
        }

    }
}
