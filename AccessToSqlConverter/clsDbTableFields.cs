using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccessToSqlConverter
{
    public class clsDbTableFields
    {
        private string _fieldName;
        private int _fieldType;
        private OleDbType _oleFieldType;

        public string fieldName
        {
            get { return _fieldName; }
            set { _fieldName = value; }
        }

        public int fieldType
        {
            get { return _fieldType; }
            set { _fieldType = value;
            }
            
        }
        public OleDbType oleFieldType
        {
            get { return _oleFieldType; }
            set { _oleFieldType = (OleDbType)fieldType; }
        }
    }
}
