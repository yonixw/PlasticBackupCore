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


            

        }

        // Setup before running:

        void updateParamsAndConnection(List<object> paramValues)
        {
            if (sqlParams != null && paramValues != null)
            {
                for (int i = 0;
                        i < Math.Min(paramValues.Count, sqlParams.Count);
                        i++)
                {
                    SQLiteParameter param = command.CreateParameter();
                    param.ParameterName = sqlParams[i].name;
                    param.DbType = getDBType(sqlParams[i].paramType);
                    param.Value = paramValues[i];
                    command.Parameters.Add(param);
                } 
            }
        }

        // Running the Command:

        public int ExecuteNonScalar(List<object> paramValues)
        {
            if (myConnection == null)
                throw new Exception("Connection is null in command.");
                
            using (command = new SQLiteCommand(sql, myConnection.myConnection))
            {
                updateParamsAndConnection(paramValues);

                command.Connection.Open();
                int result = command.ExecuteNonQuery();
                command.Connection.Close(); 

                return result;
            }
        }

        public List<T> ExecuteReadAll<T>(List<object> paramValues, Func<DbDataReader, T> readFunc)
        {
            if (myConnection == null)
                throw new Exception("Connection is null in command.");

            using (command = new SQLiteCommand(sql, myConnection.myConnection))
            {
                List<T> result = new List<T>();
                updateParamsAndConnection(paramValues);

                command.Connection.Open();
                SQLiteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    result.Add(readFunc(reader));
                }
                command.Connection.Close();

                return result; 
            }
        }

        public object ExecuteScalar(List<object> paramValues)
        {
            if (myConnection == null)
                throw new Exception("Connection is null in command.");

            using (command = new SQLiteCommand(sql, myConnection.myConnection))
            {
                updateParamsAndConnection(paramValues);

                command.Connection.Open();
                object result = command.ExecuteScalar();
                command.Connection.Close();

                return result; 
            }
        }

    }
}
