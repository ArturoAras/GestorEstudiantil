using Npgsql;
using StudentManager.Data;

namespace ProgramacionOrientadaO.Models
{
    internal class Director : User
    {
        public void OpenSemester(Session Session)
        {
            Session.StartSemester();
        }

        public void CloseSemester(Session Session)
        {
            Session.EndSemester();
        }

        private int SearchProfessor(int Ndocument)
        {
            int claveProfessor = 0;

            NpgsqlConnection Connection = new NpgsqlConnection();
            string query = "SELECT id_user, b_adult FROM users " +
                            "WHERE n_document = @document" +
                            "AND b_adult = true";

            NpgsqlCommand Command = new NpgsqlCommand(query, Connection);
            Command.Parameters.AddWithValue("@document", Ndocument);

            var Reader = Command.ExecuteReader();

            while (Reader.Read())
            {
                claveProfessor = Reader.GetInt32(0);
            }
            Connection.Close();

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
        public void SaveProfessor(string StringConnection, User User)
        {
            int DbProfessor = SearchProfessor(User.Document);
            NpgsqlConnection Connection = new NpgsqlConnection(StringConnection); //TODO
            string query = "INSERT INTO professor(n_user, n_course, n_subject) " +
                            "VALUES(@n_user, @n_course, @n_subject)";

            NpgsqlCommand Command = new NpgsqlCommand(query, Connection);

            Command.Parameters.AddWithValue("@n_user", DbProfessor);
            Command.Parameters.AddWithValue("@n_course"); //TODO:ID_COURSE);
            Command.Parameters.AddWithValue("@n_subject"); //TODO: ID_SUBJECT   


        }
    }
}
