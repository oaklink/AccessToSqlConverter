using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Data.SQLite;
using System.IO;
using System.Windows.Forms;

namespace AccessToSqlConverter
{
    public partial class Form1 : Form
    {
        //List definitions
        List<ClsDbTableDef> dbTableDefinitions = new List<ClsDbTableDef>();
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
            InitializeComponent();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Terminate application
            Application.Exit();
        }
        private void TerminateApp(string msg)
        {
            //List definitions
            List<ClsDbTableDef> dbTableDefinitions = new List<ClsDbTableDef>();
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

        private void SelectAccessDb(object sender, EventArgs e)
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
                InitialDirectory = @"V:\Documents\GitHub\C# Projects\accessDb",
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
    }
}
