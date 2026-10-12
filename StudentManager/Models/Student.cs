using Npgsql;

namespace StudentManager.Models
{
    internal class Student : User
    {
        private Dictionary<string, int> Subject { get; set; }

        public void ShowNotes(string StringConnection)
        {
            NpgsqlConnection Connection = new NpgsqlConnection(StringConnection);
            string query = "SELECT u.s_name, subject.s_name, n_note FROM subject " +
                            "INNER JOIN student_subject_notes ssn ON ssn.n_subject = subject.id_subject " +
                            "INNER JOIN student s ON s.id_student = ssn.n_student " +
                            "INNER JOIN users u ON u.id_user = s.n_user " +
                            "WHERE u.n_document = @document " +
                            "ORDER BY 2";

            NpgsqlCommand Command = new NpgsqlCommand(query, Connection);
            Command.Parameters.AddWithValue("@document", this.Document);

            Connection.Open();

            int notes = 0;
            var Reader = Command.ExecuteReader();

            while (Reader.Read())
            {
                if (notes == 0)
                {
                    Console.WriteLine("ESTUDIANTE   |   ASIGNATURA   |   CALIFICACIÓN");
                    Console.WriteLine("----------------------------------------------");
                }
                Console.Write(Reader.GetString(0) + " | " + Reader.GetString(1) + " | " + Reader.GetInt32(2));
                Console.WriteLine();
                notes++;
            }

            if (notes == 0) Console.WriteLine("No se encontraron notas para el alumno");
            Connection.Close();
        }

        public void ShowAsignature(string StringConnection)
        {
            NpgsqlConnection Connection = new NpgsqlConnection(StringConnection);
            string query = "SELECT DISTINCT subject.s_name FROM subject " +
                            "INNER JOIN student_subject_notes ssn ON ssn.n_subject = subject.id_subject " +
                            "INNER JOIN student s ON s.id_student = ssn.n_student " +
                            "INNER JOIN users u ON u.id_user = s.n_user " +
                            "WHERE u.n_document = @document";

            NpgsqlCommand Command = new NpgsqlCommand(query, Connection);
            Command.Parameters.AddWithValue("@document", this.Document);

            Connection.Open();

            int notes = 0;
            var Reader = Command.ExecuteReader();

            while (Reader.Read())
            {
                if (notes == 0)
                {
                    Console.WriteLine("------------------------------------------------");
                    Console.WriteLine("ASIGNATURAS INSCRIPTAS POR " + this.Name);
                    Console.WriteLine("------------------------------------------------");
                }
                Console.Write(Reader.GetString(0));
                Console.WriteLine();
                notes++;
            }

            if (notes == 0) Console.WriteLine("No se encontraron asignaturas para el alumno");
            Connection.Close();
        }
    }
}
