using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManager.Interfaces
{
    internal interface IProfessor
    {
        List<string> Courses { get; set; }

        void ShowGrade(string StringConnection, int GradeNumber);
        void PutNotes(string StringConnection, int Ndocument, int Note, string Subject); //Grabar en DB las notas
    }
}
