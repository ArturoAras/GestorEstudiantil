using Npgsql;

namespace StudentManager.Models
{
    internal class Student : User
    {
        private Dictionary<string, int> subject { get; set; }

        public void ShowNotes(string stringConnection)
        {
            NpgsqlConnection connection = new NpgsqlConnection(stringConnection);
            string query = "SELECT u.s_name, subject.s_name, n_note FROM subject " +
                            "INNER JOIN student_subject_notes ssn ON ssn.n_subject = subject.id_subject " +
                            "INNER JOIN student s ON s.id_student = ssn.n_student " +
                            "INNER JOIN users u ON u.id_user = s.n_user " +
                            "WHERE u.n_document = @document " +
                            "ORDER BY 2";

            NpgsqlCommand command = new NpgsqlCommand(query, connection);
            command.Parameters.AddWithValue("@document", this.Document);

            connection.Open();

            int notes = 0;
            var reader = command.ExecuteReader();

            while (reader.Read())
            {
                if (notes == 0)
                {
                    Console.WriteLine("ESTUDIANTE   |   ASIGNATURA   |   CALIFICACIÓN");
                    Console.WriteLine("----------------------------------------------");
                }
                Console.Write(reader.GetString(0) + " | " + reader.GetString(1) + " | " + reader.GetInt32(2));
                Console.WriteLine();
                notes++;
            }

            if (notes == 0) Console.WriteLine("No se encontraron notas para el alumno");
            connection.Close();
        }

        public void ShowAsignature(string stringConnection)
        {
            NpgsqlConnection connection = new NpgsqlConnection(stringConnection);
            string query = "SELECT DISTINCT subject.s_name FROM subject " +
                            "INNER JOIN student_subject_notes ssn ON ssn.n_subject = subject.id_subject " +
                            "INNER JOIN student s ON s.id_student = ssn.n_student " +
                            "INNER JOIN users u ON u.id_user = s.n_user " +
                            "WHERE u.n_document = @document";

            NpgsqlCommand command = new NpgsqlCommand(query, connection);
            command.Parameters.AddWithValue("@document", this.Document);

            connection.Open();

            int notes = 0;
            var reader = command.ExecuteReader();

            while (reader.Read())
            {
                if (notes == 0)
                {
                    Console.WriteLine("------------------------------------------------");
                    Console.WriteLine("ASIGNATURAS INSCRIPTAS POR " + this.Name);
                    Console.WriteLine("------------------------------------------------");
                }
                Console.Write(reader.GetString(0));
                Console.WriteLine();
                notes++;
            }

            if (notes == 0) Console.WriteLine("No se encontraron asignaturas para el alumno");
            connection.Close();
        }
    }
}
