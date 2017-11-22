using System;
using System.Collections.Generic;
using System.Data.Common;
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

            public enum sqliteType
            {
                NULL,
                INTEGER,
                REAL,
                TEXT,
                BLOB
            }

            public sqliteType paramType;

            public SQLParam(string _name, sqliteType _paramType)
            {
                this.name = _name;
                this.paramType = _paramType;
            }
        }

        List<SQLParam> sqlParams;
        SQLiteCommand command { get; set; }
        public SQLConnection myConnection { get; set; }

        System.Data.DbType getDBType(SQLParam.sqliteType type)
        {
            if (type.Equals(SQLParam.sqliteType.BLOB)) return System.Data.DbType.Binary;
            if (type.Equals(SQLParam.sqliteType.INTEGER)) return System.Data.DbType.Int32;
            if (type.Equals(SQLParam.sqliteType.REAL)) return System.Data.DbType.Double;
            if (type.Equals(SQLParam.sqliteType.TEXT)) return System.Data.DbType.String;

            return System.Data.DbType.Object;
        }

        public SQLCommand(string _sql, List<SQLParam> _sqlParams)
        {
            sql = _sql;
            sqlParams = _sqlParams;


            command = new SQLiteCommand(_sql);
            command.CreateParameter();

            foreach (SQLParam p in sqlParams)
            {
                SQLiteParameter param = command.CreateParameter();
                param.ParameterName = p.name;
                param.DbType = getDBType(p.paramType);
                command.Parameters.Add(param);
            }
        }

        void updateParamsAndConnection(List<object> paramValues)
        {
            for(int i=0;
                i<Math.Min(paramValues.Count, command.Parameters.Count);
                i++)
            {
                command.Parameters[i].Value = paramValues[i];
            }
            command.Connection = myConnection.myConnection;
        }

        public int ExecuteNonScalar(List<object> paramValues)
        {
            updateParamsAndConnection(paramValues);
            return command.ExecuteNonQuery();
        }

        public List<T> ExecuteReadAll<T>(List<object> paramValues, Func<DbDataReader, T> readFunc)
        {
            List<T> result = new List<T>();
            updateParamsAndConnection(paramValues);

            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                result.Add(readFunc(reader));
            }

            return result;
        }

        public object ExecuteScalar(List<object> paramValues)
        {
            updateParamsAndConnection(paramValues);
            return command.ExecuteScalar();
        }

    }
}
