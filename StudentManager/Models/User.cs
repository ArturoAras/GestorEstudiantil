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

        public int GetSubject(string stringconnection, string subjectName)
        {
            NpgsqlConnection connection = new NpgsqlConnection(stringconnection);
            string query = "SELECT * FROM subject " +
                            "WHERE s_name = @name";

            NpgsqlCommand command = new NpgsqlCommand(query, connection);
            command.Parameters.AddWithValue("@name", subjectName);
            connection.Open();

            var reader = command.ExecuteReader();
            int idSubject = 0;
            while (reader.Read())
            {
                idSubject = reader.GetInt32(0);
            }
            connection.Close();

            return idSubject;
        }
        //SetAge -> calcular edad
    }
}
