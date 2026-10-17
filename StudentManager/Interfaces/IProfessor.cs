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

        void ShowGrade(string stringConnection, int gradeNumber);
        void PutNotes(string stringConnection, int nDocument, int note, string subject); //Grabar en DB las notas
    }
}
