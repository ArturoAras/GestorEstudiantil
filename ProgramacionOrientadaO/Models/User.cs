using Npgsql;
using ProgramacionOrientadaO.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgramacionOrientadaO.Models
{
    internal class User : IUser
    {
        public string Name { get; set; }
        public string Password { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public string Role { get; set; }
        public int Age { get; set; }
        public int Document { get; set; }


        /*public User(string Name, string Password, DateOnly Dob, string Role, int Age, int NDocument)
        {
            this.Name = Name;
            this.Password = Password;
            this.DateOfBirth = Dob;
            this.Role = Role;
            this.Age = Age;
            this.Document = NDocument;
        }*/


        public void ConsultNotes()
        {
            Console.WriteLine("Devolver notas"); 
        }

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
            
            return IdSubject;
        }
        //SetAge -> calcular edad
    }
}
