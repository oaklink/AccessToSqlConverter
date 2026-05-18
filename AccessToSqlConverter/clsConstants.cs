using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccessToSqlConverter
{
    class ClsConstants
    {
        //Database Constant declarations
        public const int DISCONNECT_FAILED = -1;
        public const int DISCONNECT_OK = 0;
        public const int DB_INSERT_FAILED = -1;
        public const int DB_UPDATE_FAILED = -1;
        public const int DB_INSERT = 2;
        public const int DB_UPDATE_MODE = 1;
        public const int DB_INSERT_MODE = 2;
        public const int DB_SUCCESS = 0;
        public const int DB_FAIL = -1;

        //Database table name constants
        public const string TBL_PROC_ID = "tblProcessId";

        //XML Constants
        public const string DATABASE = "//Definitions/Database";
        public const string DATA_TABLES = "//Definitions/Database/Tables";

        //Debug Constants
        public const string DBUG_FILENAME = "dbConverter";

        //Access database data types
        public const string oleDtInt32 = "System.Int32";
        public const string oleDtString = "System.String";
        public const string oleDtDouble = "System.Double";
        public const string oleDtBoolean = "System.Boolean";
        public const string oleDtDateTime = "System.DateTime";

        //SQLite database data types
        public const int sqlInteger = 3;
        public const int sqlDouble = 5;
        public const int sqlBoolean = 11;
        public const int sqlDate = 7;
        public const int sqlWChar = 130;

        //Length of date/time to capture only date portion
        public const int iDateLen = 10;
    }
}
