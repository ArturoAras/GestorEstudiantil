using Npgsql;
using ProgramacionOrientadaO.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ProgramacionOrientadaO.Models
{
    internal class Professor : User, IProfessor
    {
        public List<string> Courses { get; set; }

        public int GetStudent(string StringConnection, int Ndocument)
        {
            NpgsqlConnection Connection = new NpgsqlConnection(StringConnection);
            string query = "SELECT id_student FROM student " +
                            "INNER JOIN users ON users.id_user = student.n_user " +
                            "WHERE users.n_document = @document";

            NpgsqlCommand Command = new NpgsqlCommand(query, Connection);
            Command.Parameters.AddWithValue("@document", Ndocument);
            Connection.Open();

            var Reader = Command.ExecuteReader();
            int IdStudent = 0;
            while (Reader.Read())
            {
                IdStudent = Reader.GetInt32(0);
            }
            return IdStudent;
        }
        public void PutNotes(string StringConnection, int Ndocument, int Note, string Subject) //Registra notas de cada alumno en la DB
        {
            int IdSubject = GetSubject(StringConnection, Subject);
            int IdStudent = GetStudent(StringConnection, Ndocument);

            NpgsqlConnection Connection = new NpgsqlConnection(StringConnection);
            string query = "INSERT INTO student_subject_notes(n_student, n_subject, n_note, d_datenote) " +
                            "VALUES(@student, @subject, @note, @sysdate)";

            NpgsqlCommand Command = new NpgsqlCommand(query, Connection);
            Command.Parameters.AddWithValue("@student", IdStudent);
            Command.Parameters.AddWithValue("@subject", IdSubject);
            Command.Parameters.AddWithValue("@note", Note);
            Command.Parameters.AddWithValue("@sysdate", DateTime.Now.Date);

            Connection.Open();

            int RowsAffected = Command.ExecuteNonQuery();

            if (RowsAffected == 1) Console.WriteLine("Se insertó " + RowsAffected + " nota.");
        }

        public void ShowGrade(string StringConnection, int GradeNumber) //TODO: query
        {
            NpgsqlConnection Connection = new NpgsqlConnection(StringConnection);
            string query = "SELECT s_name FROM student " +
                            "INNER JOIN users ON users.id_user = student.n_user " +
                            "INNER JOIN course ON course.id_course = student.n_course " +
                            "WHERE course.n_course = @course";

            NpgsqlCommand Command = new NpgsqlCommand(query, Connection);
            Command.Parameters.AddWithValue("@course", GradeNumber);
            Connection.Open();

            var Reader = Command.ExecuteReader();
            Console.WriteLine("Alumnos del curso " + GradeNumber + ":");
            while (Reader.Read())
            {
                //TODO: debe imprimir en pantalla los nombres de los alumnos que posean el curso pasado como parámetro
                Console.WriteLine(Reader.GetString(0));
                Console.WriteLine("--------------------");
            }
        }

        public void NotePerStudent(string StringConnection, int Document)
        {
            NpgsqlConnection Connection = new NpgsqlConnection(StringConnection);
            string query = "SELECT u.s_name, u.n_document, subject.s_name, n_note FROM subject " +
                            "INNER JOIN student_subject_notes ssn ON ssn.n_subject = subject.id_subject " +
                            "INNER JOIN student s ON s.id_student = ssn.n_student " +
                            "INNER JOIN users u ON u.id_user = s.n_user " +
                            "WHERE u.n_document = @document";

            NpgsqlCommand Command = new NpgsqlCommand(query, Connection);
            Command.Parameters.AddWithValue("@document", Document);

            Connection.Open();

            int notes = 0;
            var Reader = Command.ExecuteReader();

            while (Reader.Read())
            {
                if (notes == 0)
                {
                    Console.WriteLine("ESTUDIANTE   |   DOCUMENTO   |   ASIGNATURA   |   CALIFICACIÓN");
                    Console.WriteLine("----------------------------------------------");
                }
                Console.Write(Reader.GetString(0) + " | " + Reader.GetInt32(1) + " | " + Reader.GetString(2) + " | " + Reader.GetInt32(3));
                Console.WriteLine();
                notes++;
            }

            if (notes == 0) Console.WriteLine("No se encontraron notas para el alumno");
            Connection.Close();
        } 

    }
}
