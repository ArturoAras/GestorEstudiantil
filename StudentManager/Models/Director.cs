using Npgsql;
using StudentManager.Data;

namespace StudentManager.Models
{
    internal class Director : User
    {
        public void OpenSemester(Session session)
        {
            session.StartSemester();
        }

        public void CloseSemester(Session session)
        {
            session.EndSemester();
        }

        private int SearchProfessor(int document)
        {
            int professorId = 0;

            NpgsqlConnection connection = new NpgsqlConnection();
            string query = "SELECT id_user, b_adult FROM users " +
                            "WHERE n_document = @document" +
                            "AND b_adult = true";

            NpgsqlCommand command = new NpgsqlCommand(query, connection);
            command.Parameters.AddWithValue("@document", document);

            var reader = command.ExecuteReader();

            while (reader.Read())
            {
                professorId = reader.GetInt32(0);
            }
            connection.Close();

            if (professorId == 0)
            {
                Console.WriteLine("No se encontró un usuario con ese número de documento.");
                Console.WriteLine("Vuelva a ingresar el número de documento: ");
                professorId = SearchProfessor(Convert.ToInt32(Console.ReadLine()));
                return professorId;
            }
            else
            {
                Console.WriteLine("Usuario encontrado en la base de datos.");
                return professorId;
            }

        }
        public void SaveProfessor(string stringConnection, User user)
        {
            int dbProfessor = SearchProfessor(user.Document);
            NpgsqlConnection connection = new NpgsqlConnection(stringConnection); //TODO
            string query = "INSERT INTO professor(n_user, n_course, n_subject) " +
                            "VALUES(@n_user, @n_course, @n_subject)";

            NpgsqlCommand command = new NpgsqlCommand(query, connection);

            command.Parameters.AddWithValue("@n_user", dbProfessor);
            command.Parameters.AddWithValue("@n_course"); //TODO:ID_COURSE);
            command.Parameters.AddWithValue("@n_subject"); //TODO: ID_SUBJECT   


        }
    }
}
