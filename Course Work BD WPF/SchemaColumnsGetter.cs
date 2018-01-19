using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Course_Work_BD_WPF
{
    class SchemaColumnsGetter
    {
        String tableName;
        SqlConnector sqlConnector;

        public SchemaColumnsGetter(String tableName, SqlConnector sqlConnector)
        {
            this.tableName = tableName;
            this.sqlConnector = sqlConnector;
        }

        public IReadOnlyList <String> GetColumns()
        {
            string[] restrictions = new string[4] { null, null, tableName, null };
            return sqlConnector.SqlConnection.GetSchema("Columns", restrictions).AsEnumerable().Select(s => s.Field<String>("Column_Name")).ToList();
        }
    }
}
