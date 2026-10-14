using Npgsql;
using StudentManager.Interfaces;

namespace StudentManager.Models
{
    internal class Professor : User, IProfessor
    {
        public List<string> courses { get; set; }

        public int GetStudent(string stringconnection, int nDocument)
        {
            NpgsqlConnection connection = new NpgsqlConnection(stringconnection);
            string query = "SELECT id_student FROM student " +
                            "INNER JOIN users ON users.id_user = student.n_user " +
                            "WHERE users.n_document = @document";

            NpgsqlCommand command = new NpgsqlCommand(query, connection);
            command.Parameters.AddWithValue("@document", nDocument);
            connection.Open();

            var reader = command.ExecuteReader();
            int idStudent = 0;
            while (reader.Read())
            {
                idStudent = reader.GetInt32(0);
            }
            connection.Close();
            return idStudent;
        }
        public void PutNotes(string stringconnection, int nDocument, int note, string subject) //Registra notas de cada alumno en la DB
        {
            int idsubject = GetSubject(stringconnection, subject);
            int idStudent = GetStudent(stringconnection, nDocument);

            NpgsqlConnection connection = new NpgsqlConnection(stringconnection);
            string query = "INSERT INTO student_subject_notes(n_student, n_subject, n_note, d_datenote) " +
                            "VALUES(@student, @subject, @note, @sysdate)";

            NpgsqlCommand command = new NpgsqlCommand(query, connection);
            command.Parameters.AddWithValue("@student", idStudent);
            command.Parameters.AddWithValue("@subject", idsubject);
            command.Parameters.AddWithValue("@note", note);
            command.Parameters.AddWithValue("@sysdate", DateTime.Now.Date);

            connection.Open();

            int rowsAffected = command.ExecuteNonQuery();

            if (rowsAffected == 1) Console.WriteLine("Se insertó " + rowsAffected + " nota.");
            connection.Close();
        }

        public void ShowGrade(string stringconnection, int gradeNumber) //TODO: query
        {
            NpgsqlConnection connection = new NpgsqlConnection(stringconnection);
            string query = "SELECT s_name FROM student " +
                            "INNER JOIN users ON users.id_user = student.n_user " +
                            "INNER JOIN course ON course.id_course = student.n_course " +
                            "WHERE course.n_course = @course";

            NpgsqlCommand command = new NpgsqlCommand(query, connection);
            command.Parameters.AddWithValue("@course", gradeNumber);
            connection.Open();

            var reader = command.ExecuteReader();
            Console.WriteLine("Alumnos del curso " + gradeNumber + ":");
            while (reader.Read())
            {
                //TODO: debe imprimir en pantalla los nombres de los alumnos que posean el curso pasado como parámetro
                Console.WriteLine(reader.GetString(0));
                Console.WriteLine("--------------------");
            }
            connection.Close();
        }

        public void NotePerStudent(string stringconnection, int document)
        {
            NpgsqlConnection connection = new NpgsqlConnection(stringconnection);
            string query = "SELECT u.s_name, u.n_document, subject.s_name, n_note FROM subject " +
                            "INNER JOIN student_subject_notes ssn ON ssn.n_subject = subject.id_subject " +
                            "INNER JOIN student s ON s.id_student = ssn.n_student " +
                            "INNER JOIN users u ON u.id_user = s.n_user " +
                            "WHERE u.n_document = @document";

            NpgsqlCommand command = new NpgsqlCommand(query, connection);
            command.Parameters.AddWithValue("@document", document);

            connection.Open();

            int notes = 0;
            var reader = command.ExecuteReader();

            while (reader.Read())
            {
                if (notes == 0)
                {
                    Console.WriteLine("ESTUDIANTE   |   DOCUMENTO   |   ASIGNATURA   |   CALIFICACIÓN");
                    Console.WriteLine("----------------------------------------------");
                }
                Console.Write(reader.GetString(0) + " | " + reader.GetInt32(1) + " | " + reader.GetString(2) + " | " + reader.GetInt32(3));
                Console.WriteLine();
                notes++;
            }

            if (notes == 0) Console.WriteLine("No se encontraron notas para el alumno");
            connection.Close();
        } 

    }
}
