using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;

namespace PlasticBackupDB.SQLUtils
{
    public class SQLCommand
    {
        string sql;

        public class SQLParam
        {
            public string name;
            public Type paramType;

            public SQLParam(string _name, Type _paramType)
            {
                this.name = _name;
                this.paramType = _paramType;
            }
        }

        List<SQLParam> sqlParams;
        SQLiteCommand command { get; set; }

        public SQLCommand(string _sql, List<SQLParam> _sqlParams)
        {
            sql = _sql;
            sqlParams = _sqlParams;

            command = new SQLiteCommand(_sql);
            command.CreateParameter();
        }

        //public string getSQL(List<object> paramValues)
        //{
        //
        //}

    }
}
