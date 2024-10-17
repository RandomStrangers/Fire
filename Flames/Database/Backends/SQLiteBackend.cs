﻿/********************************************************
* ADO.NET 2.0 Data Provider for SQLite Version 3.X
* Written by Robert Simpson (robert@blackcastlesoft.com)
*
* Released to the public domain, use at your own risk!
********************************************************/
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading;
using SQLiteErrorCode = System.Int32;

namespace Flames.SQL
{
    public enum SqlType
    {
        Single, Double, Decimal,
        SByte, Int16, Int32, Int64,
        Byte, UInt16, UInt32, UInt64,
        Boolean, DateTime,
        Binary, String, Object,
    }

    [SuppressUnmanagedCodeSecurity]
    public static class Interop
    {
        public const string lib = "sqlite3";

        [DllImport(lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern SQLiteErrorCode sqlite3_open_v2(byte[] utf8Filename, ref IntPtr db, int flags, IntPtr vfs);
        [DllImport(lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern SQLiteErrorCode sqlite3_close_v2(IntPtr db); /* 3.7.14+ */
        [DllImport(lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern SQLiteErrorCode sqlite3_exec(IntPtr db, byte[] strSql, IntPtr pvCallback, IntPtr pvParam, ref IntPtr errMsg);
        [DllImport(lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern SQLiteErrorCode sqlite3_prepare_v2(IntPtr db, byte[] strSql, int nBytes, ref IntPtr stmt, ref IntPtr ptrRemain);

        [DllImport(lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern SQLiteErrorCode sqlite3_busy_timeout(IntPtr db, int ms);
        [DllImport(lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern long sqlite3_last_insert_rowid(IntPtr db);
        [DllImport(lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern int sqlite3_changes(IntPtr db);
        [DllImport(lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern int sqlite3_get_autocommit(IntPtr db);

        [DllImport(lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern SQLiteErrorCode sqlite3_bind_double(IntPtr stmt, int index, double value);
        [DllImport(lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern SQLiteErrorCode sqlite3_bind_int(IntPtr stmt, int index, int value);
        [DllImport(lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern SQLiteErrorCode sqlite3_bind_int64(IntPtr stmt, int index, long value);

        [DllImport(lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern SQLiteErrorCode sqlite3_bind_null(IntPtr stmt, int index);
        [DllImport(lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern SQLiteErrorCode sqlite3_bind_blob(IntPtr stmt, int index, byte[] value, int nSize, IntPtr nTransient);
        [DllImport(lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern SQLiteErrorCode sqlite3_bind_text(IntPtr stmt, int index, byte[] value, int nlen, IntPtr pvReserved);

        [DllImport(lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern int sqlite3_bind_parameter_count(IntPtr stmt);
        [DllImport(lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr sqlite3_bind_parameter_name(IntPtr stmt, int index);

        [DllImport(lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern double sqlite3_column_double(IntPtr stmt, int index);
        [DllImport(lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern int sqlite3_column_int(IntPtr stmt, int index);
        [DllImport(lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern long sqlite3_column_int64(IntPtr stmt, int index);

        [DllImport(lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern int sqlite3_column_bytes(IntPtr stmt, int index);
        [DllImport(lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr sqlite3_column_blob(IntPtr stmt, int index);
        [DllImport(lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr sqlite3_column_text(IntPtr stmt, int index);

        [DllImport(lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern int sqlite3_column_count(IntPtr stmt);
        [DllImport(lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern TypeAffinity sqlite3_column_type(IntPtr stmt, int index);
        [DllImport(lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr sqlite3_column_name(IntPtr stmt, int index);

        [DllImport(lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr sqlite3_next_stmt(IntPtr db, IntPtr stmt);
        [DllImport(lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern SQLiteErrorCode sqlite3_step(IntPtr stmt);
        [DllImport(lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern SQLiteErrorCode sqlite3_reset(IntPtr stmt);
        [DllImport(lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern SQLiteErrorCode sqlite3_finalize(IntPtr stmt);

        [DllImport(lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr sqlite3_errmsg(IntPtr db);
        [DllImport(lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern SQLiteErrorCode sqlite3_errcode(IntPtr db);
        [DllImport(lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern SQLiteErrorCode sqlite3_extended_errcode(IntPtr db);
        [DllImport(lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr sqlite3_errstr(SQLiteErrorCode rc); /* 3.7.15+ */
    }

    public abstract class SQLiteConnection : ISqlConnection
    {
        public int _transactionLevel;
        public IntPtr handle;

        public abstract bool ConnectionPooling { get; }
        public abstract string DBPath { get; }

        public override ISqlTransaction BeginTransaction()
        {
            return new SQLiteTransaction(this);
        }

        public override void ChangeDatabase(string databaseName) 
        { 
        }

        public override ISqlCommand CreateCommand(string sql) 
        { 
            return new SQLiteCommand(sql, this); 
        }

        public long LastInsertRowId
        {
            get
            {
                if (handle == IntPtr.Zero) throw new InvalidOperationException("Database connection closed");
                return Interop.sqlite3_last_insert_rowid(handle);
            }
        }

        public int Changes
        {
            get
            {
                if (handle == IntPtr.Zero) throw new InvalidOperationException("Database connection closed");
                return Interop.sqlite3_changes(handle);
            }
        }

        public bool AutoCommit
        {
            get
            {
                if (handle == IntPtr.Zero) throw new InvalidOperationException("Database connection closed");
                return Interop.sqlite3_get_autocommit(handle) == 1;
            }
        }

        public SQLiteErrorCode ResultCode()
        {
            if (handle == IntPtr.Zero) throw new InvalidOperationException("Database connection closed");
            return Interop.sqlite3_errcode(handle);
        }

        public SQLiteErrorCode ExtendedResultCode()
        {
            if (handle == IntPtr.Zero) throw new InvalidOperationException("Database connection closed");
            return Interop.sqlite3_extended_errcode(handle);
        }

        public string GetLastError()
        {
            if (handle == IntPtr.Zero) return "database connection closed";
            return SQLiteConvert.FromUTF8(Interop.sqlite3_errmsg(handle), -1);
        }

        public SQLiteStatement Prepare(string strSql, ref string strRemain)
        {
            byte[] b = SQLiteConvert.ToUTF8(strSql);
            uint start = (uint)Environment.TickCount;
            while (true)
            {
                IntPtr stmt = IntPtr.Zero, ptr = IntPtr.Zero;
                SQLiteErrorCode n = Interop.sqlite3_prepare_v2(handle, b, b.Length - 1, ref stmt, ref ptr);

                if (n == SQLiteErrorCodes.Ok)
                {
                    strRemain = SQLiteConvert.FromUTF8(ptr, -1);
                    if (stmt != IntPtr.Zero) return new SQLiteStatement(this, stmt);
                    return null;
                }
                else if (n == SQLiteErrorCodes.Locked || n == SQLiteErrorCodes.Busy)
                {
                    // Locked -- delay a small amount before retrying
                    SQLiteConvert.TrySleep(this, n, start);
                    if (stmt != IntPtr.Zero) Interop.sqlite3_finalize(stmt);
                }
                else
                {
                    throw new SQLiteException(n, GetLastError());
                }
            }
        }

        public override void Open()
        {
            if (handle != IntPtr.Zero) throw new InvalidOperationException();

            try
            {
                if (ConnectionPooling) handle = RemoveFromPool();

                if (handle == IntPtr.Zero)
                {
                    IntPtr db = IntPtr.Zero;

                    const int flags = 4 | 2; // CREATE(4) | READ_WRITE(2)
                    SQLiteErrorCode n = Interop.sqlite3_open_v2(SQLiteConvert.ToUTF8(DBPath), ref db, flags, IntPtr.Zero);

                    if (n != SQLiteErrorCodes.Ok) throw new SQLiteException(n, null);
                    handle = db;
                }

                SetTimeout(0);
            }
            catch (SQLiteException)
            {
                Close();
                throw;
            }
        }

        public void SetTimeout(int timeoutMS)
        {
            if (handle == IntPtr.Zero) throw new SQLiteException("no connection handle available");
            SQLiteErrorCode n = Interop.sqlite3_busy_timeout(handle, timeoutMS);
            if (n != SQLiteErrorCodes.Ok) throw new SQLiteException(n, GetLastError());
        }

        public static void Check(SQLiteConnection connection)
        {
            if (connection == null)
                throw new ArgumentNullException("connection");
            if (connection.handle == IntPtr.Zero)
                throw new InvalidOperationException("The connection is not open.");
        }

        public bool Reset(bool canThrow)
        {
            if (handle == IntPtr.Zero) return false;
            IntPtr stmt = IntPtr.Zero;

            do
            {
                stmt = Interop.sqlite3_next_stmt(handle, stmt);
                if (stmt != IntPtr.Zero) Interop.sqlite3_finalize(stmt);
            } while (stmt != IntPtr.Zero);

            // NOTE: Is a transaction NOT pending on the connection?
            if (AutoCommit) return true;

            SQLiteErrorCode n = Interop.sqlite3_exec(handle, SQLiteConvert.ToUTF8("ROLLBACK"),
                                                     IntPtr.Zero, IntPtr.Zero, ref stmt);
            if (n == SQLiteErrorCodes.Ok) return true;

            if (canThrow) throw new SQLiteException(n, GetLastError());
            return false;
        }

        public override void Dispose() 
        { 
            Close(false); 
        }
        public override void Close() 
        { 
            Close(true); 
        }

        public void Close(bool canThrow)
        {
            if (handle == IntPtr.Zero) return;

            // TODO: handle leak here??
            if (ConnectionPooling)
            {
                if (Reset(canThrow)) AddToPool(handle);
            }
            else
            {
                LimitPool(0);
                Reset(canThrow);
                Interop.sqlite3_close_v2(handle);
            }

            handle = IntPtr.Zero;
            _transactionLevel = 0;
        }

        public static Queue<IntPtr> pool = new Queue<IntPtr>();
        public const int MaxPoolSize = 300;
        public static object poolLocker = new object();

        public static void LimitPool(int max)
        {
            lock (poolLocker)
            {
                while (pool.Count > max)
                {
                    IntPtr handle = pool.Dequeue();
                    Interop.sqlite3_close_v2(handle);
                }
            }
        }

        public static void AddToPool(IntPtr handle)
        {
            lock (poolLocker)
            {
                LimitPool(MaxPoolSize - 1);
                pool.Enqueue(handle);
            }
        }

        public static IntPtr RemoveFromPool()
        {
            lock (poolLocker)
            {
                if (pool.Count > 0) return pool.Dequeue();
                return IntPtr.Zero;
            }
        }
    }

    public sealed class SQLiteCommand : ISqlCommand
    {
        public string sqlCmd;
        public SQLiteConnection conn;
        public SQLiteStatement stmt;
        public List<string> param_names = new List<string>();
        public List<object> param_values = new List<object>();

        public SQLiteCommand(string sql, SQLiteConnection connection)
        {
            sqlCmd = sql;
            conn = connection;
        }

        public void DisposeStatement()
        {
            stmt?.Dispose();
            stmt = null;
        }

        public override void Dispose()
        {
            conn = null;
            param_names.Clear();
            param_values.Clear();
            sqlCmd = null;
            DisposeStatement();
        }

        public SQLiteStatement NextStatement()
        {
            if (stmt != null) DisposeStatement();
            if (string.IsNullOrEmpty(sqlCmd)) return null;

            try
            {
                stmt = conn.Prepare(sqlCmd, ref sqlCmd);
            }
            catch (Exception)
            {
                DisposeStatement();
                // Cannot continue on, so set the remaining text to null.
                sqlCmd = null;
                throw;
            }

            stmt?.BindAll(param_names, param_values);
            return stmt;
        }

        public override void Prepare() 
        { 
        }

        public override void ClearParameters()
        {
            param_names.Clear();
            param_values.Clear();
        }

        public override void AddParameter(string name, object value)
        {
            param_names.Add(name);
            param_values.Add(value);
        }

        public override ISqlReader ExecuteReader()
        {
            SQLiteConnection.Check(conn);

            SQLiteDataReader reader = new SQLiteDataReader(this);
            reader.NextResult();
            return reader;
        }

        public override int ExecuteNonQuery()
        {
            using (ISqlReader reader = ExecuteReader())
            {
                while (reader.Read()) 
                { 
                }
                return reader.RowsAffected;
            }
        }
    }

    public static class SQLiteConvert
    {
        public static string[] _datetimeFormats = new string[] {
            DATEFORMAT_UTC, DATEFORMAT_LOCAL
        };

        public const string DATEFORMAT_UTC = "yyyy-MM-dd HH:mm:ssK";
        public const string DATEFORMAT_LOCAL = "yyyy-MM-dd HH:mm:ss";
        public static Encoding utf8 = new UTF8Encoding();

        public static byte[] ToUTF8(string text)
        {
            int count = utf8.GetByteCount(text) + 1;
            byte[] data = new byte[count];

            count = utf8.GetBytes(text, 0, text.Length, data, 0);
            data[count] = 0;

            return data;
        }

        public static string FromUTF8(IntPtr ptr, int len)
        {
            if (ptr == IntPtr.Zero) return "";

            if (len < 0)
            {
                len = 0;
                while (Marshal.ReadByte(ptr, len) != 0) 
                { 
                    len++; 
                }
            }
            if (len == 0) return "";

            byte[] data = new byte[len];
            Marshal.Copy(ptr, data, 0, len);
            return utf8.GetString(data, 0, len);
        }

        public static DateTime ToDateTime(string text)
        {
            return DateTime.SpecifyKind(DateTime.ParseExact(
                text, _datetimeFormats, DateTimeFormatInfo.InvariantInfo,
                DateTimeStyles.None), DateTimeKind.Unspecified);
        }

        public static string ToString(DateTime value)
        {
            string format = (value.Kind == DateTimeKind.Utc) ? DATEFORMAT_UTC : DATEFORMAT_LOCAL;
            return value.ToString(format, CultureInfo.InvariantCulture);
        }


        public const int Timeout = 30;
        public static uint seed = 123456789;

        public static void TrySleep(SQLiteConnection conn, SQLiteErrorCode n, uint start)
        {
            if ((uint)Environment.TickCount > start + (Timeout * 1000))
            {
                throw new SQLiteException(n, conn.GetLastError());
            }
            else
            {
                seed = seed * 1103515245 + 12345;
                Thread.Sleep((int)(seed % 150) + 1);
            }
        }


        public static SqlType TypeToDbType(Type typ)
        {
            TypeCode tc = Type.GetTypeCode(typ);
            if (tc == TypeCode.Object)
            {
                if (typ == typeof(byte[])) return SqlType.Binary;
                return SqlType.String;
            }
            return type_to_dbtype[(int)tc];
        }

        public static SqlType[] type_to_dbtype = {
            SqlType.Object,   // Empty (0)
            SqlType.Binary,   // Object (1)
            SqlType.Object,   // DBNull (2)
            SqlType.Boolean,  // Boolean (3)
            SqlType.SByte,    // Char (4)
            SqlType.SByte,    // SByte (5)
            SqlType.Byte,     // Byte (6)
            SqlType.Int16,    // Int16 (7)
            SqlType.UInt16,   // UInt16 (8)
            SqlType.Int32,    // Int32 (9)
            SqlType.UInt32,   // UInt32 (10)
            SqlType.Int64,    // Int64 (11)
            SqlType.UInt64,   // UInt64 (12)
            SqlType.Single,   // Single (13)
            SqlType.Double,   // Double (14)
            SqlType.Decimal,  // Decimal (15)
            SqlType.DateTime, // DateTime (16)
            SqlType.Object,   // ?? (17)
            SqlType.String    // String (18)
        };
    }

    public enum TypeAffinity
    {
        Uninitialized = 0,
        Int64 = 1,
        Double = 2,
        Text = 3,
        Blob = 4,
        Null = 5,
    }

    public sealed class SQLiteDataReader : ISqlReader
    {
        public SQLiteCommand _command;
        public SQLiteStatement stmt;
        public int readState, rowsAffected, columns;
        public string[] fieldNames;

        public SQLiteDataReader(SQLiteCommand cmd)
        {
            _command = cmd;
        }

        public override void Dispose() 
        { 
            Close(); 
        }
        public override void Close()
        {
            _command = null;
            stmt = null;
            fieldNames = null;
        }

        public void CheckClosed()
        {
            if (_command == null)
                throw new InvalidOperationException("DataReader has been closed");

            SQLiteConnection.Check(_command.conn);
        }

        public void VerifyForGet()
        {
            CheckClosed();
            if (readState != 0) throw new InvalidOperationException("No current row");
        }

        public TypeAffinity GetAffinity(int i)
        {
            VerifyForGet();
            return stmt.ColumnAffinity(i);
        }

        public override bool GetBoolean(int i) 
        { 
            return GetInt32(i) != 0; 
        }

        public override byte[] GetBytes(int i)
        {
            if (GetAffinity(i) == TypeAffinity.Blob)
                return stmt.GetBytes(i);
            throw new InvalidCastException();
        }

        public override DateTime GetDateTime(int i)
        {
            if (GetAffinity(i) == TypeAffinity.Text)
                return stmt.GetDateTime(i);
            throw new NotSupportedException();
        }

        public override double GetDouble(int i)
        {
            TypeAffinity aff = GetAffinity(i);
            if (aff == TypeAffinity.Int64 || aff == TypeAffinity.Double)
                return stmt.GetDouble(i);
            throw new NotSupportedException();
        }

        public override int GetInt32(int i)
        {
            if (GetAffinity(i) == TypeAffinity.Int64)
                return stmt.GetInt32(i);
            throw new InvalidCastException();
        }

        public override long GetInt64(int i)
        {
            if (GetAffinity(i) == TypeAffinity.Int64)
                return stmt.GetInt64(i);
            throw new InvalidCastException();
        }

        public override string GetString(int i) 
        { 
            return stmt.GetText(i); 
        }

        public override bool IsDBNull(int i)
        {
            return GetAffinity(i) == TypeAffinity.Null;
        }


        public override object GetValue(int i)
        {
            TypeAffinity affinity = GetAffinity(i);
            return stmt.GetValue(i, affinity);
        }

        public override string GetStringValue(int col)
        {
            return GetString(col);
        }

        public override string DumpValue(int col)
        {
            TypeAffinity affinity = GetAffinity(col);
            if (affinity == TypeAffinity.Null) return "NULL";

            string value = GetString(col);
            if (affinity == TypeAffinity.Text || affinity == TypeAffinity.Blob)
                return Quote(value);

            // TODO doubles not exact? probably doesn't matter
            return value;
        }


        public override string GetName(int i) 
        { 
            return stmt.ColumnName(i); 
        }

        public override int GetOrdinal(string name)
        {
            VerifyForGet();
            if (fieldNames == null) fieldNames = new string[columns];

            for (int i = 0; i < columns; i++)
            {
                string field = fieldNames[i];
                if (field == null)
                {
                    field = stmt.ColumnName(i);
                    fieldNames[i] = field;
                }
                if (name.Equals(field, StringComparison.OrdinalIgnoreCase)) return i;
            }
            return -1;
        }


        public override int FieldCount { get { return columns; } }

        public override int RowsAffected { get { return rowsAffected; } }

        public bool NextResult()
        {
            CheckClosed();

            while (true)
            {
                stmt = _command.NextStatement(); // next statement to execute
                readState = 1; // set the state to "done reading"

                // reached the end of the statements, no more resultsets
                if (stmt == null) return false;

                columns = stmt.ColumnCount();
                if (stmt.Step())
                {
                    readState = -1;
                }
                else if (columns == 0)
                {
                    // No rows or columns returned, move to the next statement
                    rowsAffected += stmt.conn.Changes;
                    continue;
                }
                else
                {
                    // This statement returned columns but no rows
                }

                // Found a row-returning resultset eligible to be returned!
                fieldNames = null;
                return true;
            }
        }

        public override bool Read()
        {
            CheckClosed();

            // First Row was already read at NextResult() level, so don't step again here
            if (readState == -1)
            {
                readState = 0; 
                return true;
            }
            else if (readState == 0)
            { // Actively reading rows
                if (stmt.Step()) return true;
                readState = 1; // Finished reading rows
            }
            return false;
        }
    }

    public sealed class SQLiteException : ExternalException
    {
        public SQLiteException(SQLiteErrorCode code, string message) : base(FormatError(code, message)) 
        { 
        }

        public SQLiteException(string message) : this(SQLiteErrorCodes.Unknown, message) 
        { 
        }

        public static string FormatError(SQLiteErrorCode code, string message)
        {
            string msg = GetErrorString(code) + Environment.NewLine + message;
            return msg.Trim();
        }

        public static string[] errors = new string[] {
            /* SQLITE_OK          */ "not an error",
            /* SQLITE_ERROR       */ "SQL logic error or missing database",
            /* SQLITE_INTERNAL    */ "internal logic error",
            /* SQLITE_PERM        */ "access permission denied",
            /* SQLITE_ABORT       */ "callback requested query abort",
            /* SQLITE_BUSY        */ "database is locked",
            /* SQLITE_LOCKED      */ "database table is locked",
            /* SQLITE_NOMEM       */ "out of memory",
            /* SQLITE_READONLY    */ "attempt to write a readonly database",
            /* SQLITE_INTERRUPT   */ "interrupted",
            /* SQLITE_IOERR       */ "disk I/O error",
            /* SQLITE_CORRUPT     */ "database disk image is malformed",
            /* SQLITE_NOTFOUND    */ "unknown operation",
            /* SQLITE_FULL        */ "database or disk is full",
            /* SQLITE_CANTOPEN    */ "unable to open database file",
            /* SQLITE_PROTOCOL    */ "locking protocol",
            /* SQLITE_EMPTY       */ "table contains no data",
            /* SQLITE_SCHEMA      */ "database schema has changed",
            /* SQLITE_TOOBIG      */ "string or blob too big",
            /* SQLITE_CONSTRAINT  */ "constraint failed",
            /* SQLITE_MISMATCH    */ "datatype mismatch",
            /* SQLITE_MISUSE      */ "library routine called out of sequence",
            /* SQLITE_NOLFS       */ "large file support is disabled",
            /* SQLITE_AUTH        */ "authorization denied",
            /* SQLITE_FORMAT      */ "auxiliary database format error",
            /* SQLITE_RANGE       */ "bind or column index out of range",
            /* SQLITE_NOTADB      */ "file is encrypted or is not a database",
            /* SQLITE_NOTICE      */ "notification message",
            /* SQLITE_WARNING     */ "warning message"
        };

        public static string GetErrorString(SQLiteErrorCode rc)
        {
            try
            {
                IntPtr ptr = Interop.sqlite3_errstr(rc);
                if (ptr != IntPtr.Zero)
                {
                    return Marshal.PtrToStringAnsi(ptr);
                }
            }
            catch (EntryPointNotFoundException)
            {
                // do nothing.
            }

            if (rc < 0 || rc >= errors.Length)
                rc = SQLiteErrorCodes.Error;
            return errors[rc];
        }
    }

    public static class SQLiteErrorCodes
    {
        public const int Unknown = -1;
        public const int Ok = 0;
        public const int Error = 1;
        public const int Busy = 5;
        public const int Locked = 6;
        public const int Row = 100;
        public const int Done = 101;
    }

    public sealed class SQLiteStatement : IDisposable
    {
        public IntPtr handle;
        public SQLiteConnection conn;
        public string[] paramNames;

        public SQLiteStatement(SQLiteConnection connection, IntPtr handle)
        {
            conn = connection;
            this.handle = handle;

            // Determine parameters for this statement (if any) and prepare space for them.
            int count = Interop.sqlite3_bind_parameter_count(handle);
            if (count <= 0) return;

            paramNames = new string[count];
            for (int i = 0; i < count; i++)
            {
                IntPtr p = Interop.sqlite3_bind_parameter_name(handle, i + 1);
                paramNames[i] = SQLiteConvert.FromUTF8(p, -1);
            }
        }

        public void Dispose()
        {
            if (handle != IntPtr.Zero)
            {
                Interop.sqlite3_finalize(handle);
                handle = IntPtr.Zero;
            }

            paramNames = null;
            conn = null;
        }

        public bool Step()
        {
            uint start = (uint)Environment.TickCount;
            while (true)
            {
                SQLiteErrorCode n = Interop.sqlite3_step(handle);
                if (n == SQLiteErrorCodes.Row) return true;
                if (n == SQLiteErrorCodes.Done) return false;
                if (n == SQLiteErrorCodes.Ok) continue;

                // An error occurred, attempt to reset the statement. If it errored because
                // the database is locked, then keep retrying until the command timeout occurs.
                n = Interop.sqlite3_reset(handle);
                if (n == SQLiteErrorCodes.Locked || n == SQLiteErrorCodes.Busy)
                {
                    SQLiteConvert.TrySleep(conn, n, start);
                }
                else if (n != SQLiteErrorCodes.Ok)
                {
                    throw new SQLiteException(n, conn.GetLastError());
                }
            }
        }

        public int ColumnCount() 
        { 
            return Interop.sqlite3_column_count(handle); 
        }

        public string ColumnName(int index)
        {
            IntPtr p = Interop.sqlite3_column_name(handle, index);
            return SQLiteConvert.FromUTF8(p, -1);
        }

        public TypeAffinity ColumnAffinity(int index)
        {
            return Interop.sqlite3_column_type(handle, index);
        }

        public void BindAll(List<string> names, List<object> values)
        {
            if (paramNames == null || names.Count == 0) return;

            for (int idx = 0; idx < names.Count; idx++)
            {
                int i = FindParameter(names[idx]);
                if (i == -1) continue;

                SQLiteErrorCode n = BindParameter(i + 1, values[idx]);
                if (n != SQLiteErrorCodes.Ok) throw new SQLiteException(n, conn.GetLastError());
            }
        }

        public int FindParameter(string name)
        {
            int count = paramNames.Length;
            for (int i = 0; i < count; i++)
            {
                if (name.Equals(paramNames[i], StringComparison.OrdinalIgnoreCase)) return i;
            }
            return -1;
        }

        public SQLiteErrorCode BindParameter(int i, object obj)
        {
            if (obj == null || obj == DBNull.Value)
            {
                return Interop.sqlite3_bind_null(handle, i);
            }

            SqlType type = SQLiteConvert.TypeToDbType(obj.GetType());
            switch (type)
            {
                case SqlType.DateTime:
                    return Bind_DateTime(i, Convert.ToDateTime(obj, CultureInfo.InvariantCulture));
                case SqlType.Boolean:
                    return Bind_Int32(i, Convert.ToBoolean(obj) ? 1 : 0);
                case SqlType.SByte:
                    return Bind_Int32(i, Convert.ToSByte(obj));
                case SqlType.Int16:
                    return Bind_Int32(i, Convert.ToInt16(obj));
                case SqlType.Int32:
                    return Bind_Int32(i, Convert.ToInt32(obj));
                case SqlType.Int64:
                    return Bind_Int64(i, Convert.ToInt64(obj));
                case SqlType.Byte:
                    return Bind_Int32(i, Convert.ToByte(obj));
                case SqlType.UInt16:
                    return Bind_Int32(i, Convert.ToUInt16(obj));
                case SqlType.UInt32:
                    return Bind_Int32(i, (int)Convert.ToUInt32(obj));
                case SqlType.UInt64:
                    return Bind_Int64(i, (long)Convert.ToUInt64(obj));
                case SqlType.Single:
                case SqlType.Double:
                case SqlType.Decimal:
                    return Interop.sqlite3_bind_double(handle, i, Convert.ToDouble(obj));
                case SqlType.Binary:
                    byte[] b = (byte[])obj;
                    return Interop.sqlite3_bind_blob(handle, i, b, b.Length, (IntPtr)(-1));
                default:
                    return Bind_Text(i, obj.ToString());
            }
        }

        public SQLiteErrorCode Bind_Int32(int index, int value)
        {
            return Interop.sqlite3_bind_int(handle, index, value);
        }

        public SQLiteErrorCode Bind_Int64(int index, long value)
        {
            return Interop.sqlite3_bind_int64(handle, index, value);
        }

        public SQLiteErrorCode Bind_Text(int index, string value)
        {
            byte[] b = SQLiteConvert.ToUTF8(value);
            return Interop.sqlite3_bind_text(handle, index, b, b.Length - 1, (IntPtr)(-1));
        }

        public SQLiteErrorCode Bind_DateTime(int index, DateTime dt)
        {
            return Bind_Text(index, SQLiteConvert.ToString(dt));
        }

        public object GetValue(int index, TypeAffinity affinity)
        {
            switch (affinity)
            {
                case TypeAffinity.Blob:
                    return GetBytes(index);
                case TypeAffinity.Double:
                    return GetDouble(index);
                case TypeAffinity.Int64:
                    return GetInt64(index);
                case TypeAffinity.Null:
                    return DBNull.Value;
            }
            return GetText(index);
        }

        public double GetDouble(int index)
        {
            return Interop.sqlite3_column_double(handle, index);
        }

        public int GetInt32(int index)
        {
            return Interop.sqlite3_column_int(handle, index);
        }

        public long GetInt64(int index)
        {
            return Interop.sqlite3_column_int64(handle, index);
        }

        public string GetText(int index)
        {
            return SQLiteConvert.FromUTF8(Interop.sqlite3_column_text(handle, index),
                                          Interop.sqlite3_column_bytes(handle, index));
        }

        public DateTime GetDateTime(int index)
        {
            return SQLiteConvert.ToDateTime(GetText(index));
        }

        public byte[] GetBytes(int index)
        {
            int srcLen = Interop.sqlite3_column_bytes(handle, index);
            if (srcLen <= 0) return null;
            byte[] dst = new byte[srcLen];

            IntPtr src = Interop.sqlite3_column_blob(handle, index);
            Marshal.Copy(src, dst, 0, srcLen);
            return dst;
        }
    }

    public sealed class SQLiteTransaction : ISqlTransaction
    {
        public SQLiteConnection conn;

        public SQLiteTransaction(SQLiteConnection connection)
        {
            conn = connection;
            if (conn._transactionLevel++ == 0)
            {
                try
                {
                    using (ISqlCommand cmd = conn.CreateCommand("BEGIN IMMEDIATE"))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (SQLiteException)
                {
                    conn._transactionLevel--;
                    conn = null;
                    throw;
                }
            }
        }

        public bool disposed;
        public override void Dispose()
        {
            if (disposed) return;
            if (IsValid(false)) IssueRollback(false);
            disposed = true;
        }

        public override void Commit()
        {
            SQLiteConnection.Check(conn);
            IsValid(true);

            if (--conn._transactionLevel == 0)
            {
                using (ISqlCommand cmd = conn.CreateCommand("COMMIT"))
                {
                    cmd.ExecuteNonQuery();
                }
            }
            conn = null;
        }

        public override void Rollback()
        {
            SQLiteConnection.Check(conn);
            IsValid(true);
            IssueRollback(true);
        }

        public void IssueRollback(bool throwError)
        {
            if (conn == null) return;

            try
            {
                using (ISqlCommand cmd = conn.CreateCommand("ROLLBACK"))
                {
                    cmd.ExecuteNonQuery();
                }
            }
            catch
            {
                if (throwError) throw;
            }
            conn._transactionLevel = 0;
        }

        public bool IsValid(bool throwError)
        {
            if (conn == null)
            {
                if (throwError) throw new ArgumentNullException("No connection associated with this transaction");
                return false;
            }

            if (conn.handle == IntPtr.Zero)
            {
                if (throwError) throw new SQLiteException("Connection was closed");
                return false;
            }

            if (conn._transactionLevel == 0 || conn.AutoCommit)
            {
                conn._transactionLevel = 0; // Make sure the transaction level is reset before returning
                if (throwError) throw new SQLiteException("No transaction is active on this connection");
                return false;
            }
            return true;
        }
    }
}