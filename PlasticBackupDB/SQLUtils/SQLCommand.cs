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
        // =======================================================

        public enum ResultCode
        {
             SQLITE_OK        =   0,   /* Successful result */
                 /* beginning-=of-e,rror-codes */
             SQLITE_ERROR     =   1,   /* Generic error */
             SQLITE_INTERNAL  =   2,   /* Internal logic error in SQLite */
             SQLITE_PERM      =   3,   /* Access permission denied */
             SQLITE_ABORT     =   4,   /* Callback routine requested an abort */
             SQLITE_BUSY      =   5,   /* The database file is locked */
             SQLITE_LOCKED    =   6,   /* A table in the database is locked */
             SQLITE_NOMEM     =   7,   /* A malloc() failed */
             SQLITE_READONLY  =   8,   /* Attempt to write a readonly database */
             SQLITE_INTERRUPT =   9,   /* Operation terminated by sqlite3_interrupt()*/
             SQLITE_IOERR     =  10,   /* Some kind of disk I/O error occurred */
             SQLITE_CORRUPT   =  11,   /* The database disk image is malformed */
             SQLITE_NOTFOUND  =  12,   /* Unknown opcode in sqlite3_file_control() */
             SQLITE_FULL      =  13,   /* Insertion failed because database is full */
             SQLITE_CANTOPEN  =  14,   /* Unable to open the database file */
             SQLITE_PROTOCOL  =  15,   /* Database lock protocol error */
             SQLITE_EMPTY     =  16,   /* Internal use only */
             SQLITE_SCHEMA    =  17,   /* The database schema changed */
             SQLITE_TOOBIG    =  18,   /* String or BLOB exceeds size limit */
             SQLITE_CONSTRAINT=  19,   /* Abort due to constraint violation */
             SQLITE_MISMATCH  =  20,   /* Data type mismatch */
             SQLITE_MISUSE    =  21,   /* Library used incorrectly */
             SQLITE_NOLFS     =  22,   /* Uses OS features not supported on host */
             SQLITE_AUTH      =  23,   /* Authorization denied */
             SQLITE_FORMAT    =  24,   /* Not used */
             SQLITE_RANGE     =  25,   /* 2nd parameter to sqlite3_bind out of range */
             SQLITE_NOTADB    =  26,   /* File opened that is not a database file */
             SQLITE_NOTICE    =  27,   /* Notifications from sqlite3_log() */
             SQLITE_WARNING   =  28,   /* Warnings from sqlite3_log() */
             SQLITE_ROW       =  100,  /* sqlite3_step() has another row ready */
             SQLITE_DONE      =  101  /* sqlite3_step() has finished executing */
        }

        public int ExecuteNonScalar(List<object> paramValues, SQLConnection conn)
        {
            if (conn == null)
                throw new Exception("Connection is null in command.");
                
            using (command = new SQLiteCommand(sql, conn.myConnection))
            {
                updateParamsAndConnection(paramValues);

                command.Connection.Open();
                int result = command.ExecuteNonQuery();
                command.Connection.Close(); 

                return result;
            }
        }

        public List<T> ExecuteReadAll<T>(List<object> paramValues, Func<DbDataReader, T> readFunc, SQLConnection conn)
        {
            if (conn == null)
                throw new Exception("Connection is null in command.");

            using (command = new SQLiteCommand(sql, conn.myConnection))
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

        public object ExecuteScalar(List<object> paramValues, SQLConnection conn)
        {
            if (conn == null)
                throw new Exception("Connection is null in command.");

            using (command = new SQLiteCommand(sql, conn.myConnection))
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
