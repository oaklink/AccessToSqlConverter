using System.Collections.Generic;

namespace AccessToSqlConverter
{
    public class ClsdbFields
    {
        private string _fieldName;
        private string _dataType;

        public string FieldName
        {
            get { return _fieldName; }
            set { _fieldName = value; }
        }
        public string DataType
        {
            get { return _dataType; }
            set { _dataType = value; }
        }
    }

    class ClsDbTableDef
    {
        List<ClsdbFields> _dbFields = new List<ClsdbFields>();

        private string _tableName = string.Empty;
        //Store tablename
        public string TableName
        {
            get { return _tableName; }
            set { _tableName = value; }
        }

        //Store field name
        public void AddField(string fieldName, string dataType)
        {
            ClsdbFields temp = new ClsdbFields
            {
                FieldName = fieldName,
                DataType = dataType
            };

            _dbFields.Add(temp);
        }

        //Read field name
        public List<ClsdbFields> ReadFields
        {
            get { return _dbFields; }
        }
    }
}
