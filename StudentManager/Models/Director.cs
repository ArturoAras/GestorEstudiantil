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

        private int SearchProfessor(int nDocument)
        {
            int claveProfessor = 0;

            NpgsqlConnection connection = new NpgsqlConnection();
            string query = "SELECT id_user, b_adult FROM users " +
                            "WHERE n_document = @document" +
                            "AND b_adult = true";

            NpgsqlCommand command = new NpgsqlCommand(query, connection);
            command.Parameters.AddWithValue("@document", nDocument);

            var reader = command.ExecuteReader();

            while (reader.Read())
            {
                claveProfessor = reader.GetInt32(0);
            }
            connection.Close();

            if (claveProfessor == 0)
            {
                Console.WriteLine("No se encontró un usuario con ese número de documento.");
                Console.WriteLine("Vuelva a ingresar el número de documento: ");
                claveProfessor = SearchProfessor(Convert.ToInt32(Console.ReadLine()));
                return claveProfessor;
            }
            else
            {
                Console.WriteLine("Usuario encontrado en la base de datos.");
                return claveProfessor;
            }

        }
        public void SaveProfessor(string stringConnection, User User)
        {
            int DbProfessor = SearchProfessor(User.Document);
            NpgsqlConnection connection = new NpgsqlConnection(stringConnection); //TODO
            string query = "INSERT INTO professor(n_user, n_course, n_subject) " +
                            "VALUES(@n_user, @n_course, @n_subject)";

            NpgsqlCommand command = new NpgsqlCommand(query, connection);

            command.Parameters.AddWithValue("@n_user", DbProfessor);
            command.Parameters.AddWithValue("@n_course"); //TODO:ID_COURSE);
            command.Parameters.AddWithValue("@n_subject"); //TODO: ID_SUBJECT   


        }
    }
}
