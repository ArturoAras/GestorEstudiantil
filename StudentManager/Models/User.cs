using Npgsql;

namespace StudentManager.Models
{
    internal class User
    {
        public int IdUser { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public string Role { get; set; }
        public int Age { get; set; }
        public int Document { get; set; }

        public int GetSubject(string StringConnection, string SubjectName)
        {
            NpgsqlConnection Connection = new NpgsqlConnection(StringConnection);
            string query = "SELECT * FROM subject " +
                            "WHERE s_name = @name";

            NpgsqlCommand Command = new NpgsqlCommand(query, Connection);
            Command.Parameters.AddWithValue("@name", SubjectName);
            Connection.Open();

            var Reader = Command.ExecuteReader();
            int IdSubject = 0;
            while (Reader.Read())
            {
                IdSubject = Reader.GetInt32(0);
            }
            Connection.Close();

            return IdSubject;
        }
        //SetAge -> calcular edad
    }
}
