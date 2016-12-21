using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Dapper;
using System.Data.SQLite;

namespace LevelsBeyondNotes.Models
{
    public class NotesRepo
    {
        public static string _databasePath;
        public static string _connectionString;

        // Retrieve the database name and path from configuration 
        // If called from Application_Start, this should be updated whenever the web.config is altered due to AppPool reset
        public static bool CreateDbIfNotExists()
        {
            try
            {
                // Need to override the default behavior of running files from /Program Files/IIS Express...
                var appDataPath = HttpContext.Current.Server.MapPath("~/App_Data/");
                var databaseName = System.Configuration.ConfigurationManager.AppSettings["database_filename"];
                _databasePath = appDataPath + databaseName;

                // The connection string should contain a replace spot for the db path
                _connectionString = string.Format(System.Configuration.ConfigurationManager.ConnectionStrings["notes"].ConnectionString, _databasePath);
                if (!System.IO.File.Exists(_databasePath))
                {
                    SQLiteConnection.CreateFile(_databasePath);
                    return NonQuery("create table notes(id INTEGER PRIMARY KEY, body TEXT)");
                }
                return true;
            }
            catch
            { /* This would be a filesystem issue, possibly permissions */ }
            return false;
        }

        public static IEnumerable<Note> GetNotes(string search)
        {
            var sql = "SELECT * FROM notes";
            if (!string.IsNullOrEmpty(search))
            {
                // Only implement the search sql if there's something to search for
                search = SanitizeBody(search);
                sql += $" WHERE body like '%{search}%'";
            }
            return GetEntities<Note>(sql, null);
        }

        public static Note GetNoteById(int noteId)
        {
            return GetEntities<Note>("SELECT * FROM notes WHERE id = @Id", new { Id = noteId }).FirstOrDefault();
        }

        public static void CreateNote(Note newNote)
        {
            newNote.body = SanitizeBody(newNote.body);
            var sql = $"INSERT INTO notes (body) VALUES ('{newNote.body}'); SELECT last_insert_rowid();";
            var noteId = Insert(sql);
            newNote.id = noteId;
        }

        private static string SanitizeBody(string body)
        {
            // Other things can be added here to ensure data integrity
            return body.Replace("''", "'").Replace("'", "''");
        }

        private static IEnumerable<T> GetEntities<T>(string query, object parameters)
        {
            // Implemented as a separate method for extensibility sake
            try
            {
                IEnumerable<T> rv;
                using (var connection = new SQLiteConnection(_connectionString))
                {

                    connection.Open();
                    // I've been looking for a good reason to try out Dapper. This was pretty much the extent of it...
                    rv = connection.Query<T>(query, parameters);
                    connection.Close();
                }
                return rv;
            }
            catch { /* Log to EventLog */ }
            return null;
        }

        private static int Insert(string sql)
        {
            try
            {
                object rv;
                using (var command = new SQLiteCommand(new SQLiteConnection(_connectionString)))
                {
                    command.CommandText = sql;
                    command.Connection.Open();
                    rv = command.ExecuteScalar();
                    command.Connection.Close();
                }
                return Convert.ToInt32(rv);
            }
            catch { /* Log to EventLog */ }
            return -1;
        }

        private static bool NonQuery(string sql)
        {
            try
            {
                using (var command = new SQLiteCommand(new SQLiteConnection(_connectionString)))
                {
                    command.CommandText = sql;
                    command.Connection.Open();
                    command.ExecuteNonQuery();
                    command.Connection.Close();
                }
                return true;
            }
            catch { /* Log to EventLog */ }
            return false;
        }
    }
}