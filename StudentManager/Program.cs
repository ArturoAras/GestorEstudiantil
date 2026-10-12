using StudentManager.Data;
using StudentManager.Models;

Session Session = new Session();
List<User> UserSession = new List<User>();
User? CurrentUser = null;

string StringConnection = "Host=localhost;" +
                                  "Database=campus_academic_db;" +
                                  "Port=5432;" +
                                  "Username=postgres;" +
                                  "Password=1234;";



string UserOption = Session.MainMenu(StringConnection);
while (UserOption != "0")
{
    //MENÚ PRINCIPAL POR DEFECTO
    switch (UserOption)
    {
        case "1":
            //Valida que se haya iniciado sesión correctamente
            while (CurrentUser == null)
            {
                Console.WriteLine("Ingrese su número de documento: ");
                int Ndocument;
                try
                {
                    Ndocument = Convert.ToInt32(Console.ReadLine());

                }catch(FormatException e)
                {
                    Console.WriteLine(e.Message);
                    Ndocument = 0;
                }
                Console.Write("Ingrese su contraseña: ");
                string Spassword = Console.ReadLine();

                CurrentUser = Session.LogInValidation(StringConnection, Ndocument, Spassword);

                if(CurrentUser == null) Console.WriteLine("Usuario y/o contraseña incorrectos.");
            }

            UserSession.Add(CurrentUser);
            UserOption = Session.ContextualMenu(CurrentUser);


            //MENÚ CONTEXTUAL DE USUARIO
            while (UserOption != "0")
            {
                switch (UserOption)
                {
                    case "1":
                        {
                            if (CurrentUser is Professor professor) //Si es profesor devuelve curso por #
                            {
                                Console.WriteLine("¿Qué curso desea ver?");
                                int Grade = Convert.ToInt32(Console.ReadLine());

                                professor.ShowGrade(StringConnection, Grade);
                            }

                            if (CurrentUser is Student student) //Si es estudiante devuelve calificaciones 
                            {
                                student.ShowNotes(StringConnection);
                            }
                            Console.WriteLine("-------------------------------");
                            Console.WriteLine("Qué otra acción desea realizar?");
                            Console.WriteLine("-------------------------------");
                            UserOption = Session.ContextualMenu(CurrentUser);
                            break;
                        }

                    case "2":
                        {
                            //Si es PROFESOR ver calificaciones del curso
                            if (CurrentUser is Professor Professor)
                            {
                                Console.WriteLine("Ingrese el número de documento del estudiante: ");
                                int Document = Convert.ToInt32(Console.ReadLine());
                                Professor.NotePerStudent(StringConnection, Document);
                            }
                            //Si es ALUMNO ver asignaturas del alumno
                            if (CurrentUser is Student Student)
                            {
                                Student.ShowAsignature(StringConnection);
                            }
                        }

                        Console.WriteLine("-------------------------------");
                        Console.WriteLine("Qué otra acción desea realizar?");
                        Console.WriteLine("-------------------------------");
                        UserOption = Session.ContextualMenu(CurrentUser);
                        break;

                    case "3":
                        {
                            if (CurrentUser is Professor professor) //Si es PROFESOR cargar calificaciones a alumno
                            {
                                string NextNote = "si", Subject = "";
                                int Note, Document;
                                while (NextNote == "si")
                                {
                                    Console.WriteLine("Ingrese el número de documento del alumno: ");
                                    try
                                    {
                                        Document = Convert.ToInt32(Console.ReadLine());
                                        Console.Write("Ingrese el nombre de la materia a la que corresponde la calificación: ");
                                        Subject = Console.ReadLine();
                                        Console.WriteLine("Ingrese la nota: ");
                                        Note = Convert.ToInt32(Console.ReadLine());

                                    }catch(FormatException e)
                                    {
                                        Note = 0;
                                        Document = 0;
                                    }
                                    while (Note < 1 || Note > 10)
                                    {
                                        Console.WriteLine("La calificación no puede ser menor a 1 o mayor a 10");
                                        Console.WriteLine("Ingrese la calificación: ");
                                        Note = Convert.ToInt32(Console.ReadLine());
                                    }

                                    professor.PutNotes(StringConnection, Document, Note, Subject);

                                    Console.WriteLine("Desea ingresar otra calificación? ");
                                    NextNote = Console.ReadLine().ToLower();
                                }

                                Console.WriteLine("-------------------------------");
                                Console.WriteLine("Qué otra acción desea realizar?");
                                Console.WriteLine("-------------------------------");
                                UserOption = Session.ContextualMenu(CurrentUser);
                            }

                            if (CurrentUser is Student) //Si es ALUMNO sale del menú contextual
                            {
                                Console.WriteLine("Sesión finalizada");
                                UserOption = "0";
                            }
                        }
                        
                        break;
                    case "4":  
                        {
                            if (CurrentUser is Professor Professor)
                            {
                                Console.WriteLine("Sesión finalizada");
                                UserOption = "0";
                            }
                            else
                            {
                                Console.WriteLine("Opción del menú no disponible.");
                                UserOption = Session.ContextualMenu(CurrentUser);
                            }
                        }
                        break;
                    default:
                        Console.WriteLine("Opción no válida. Sesión finalizada.");
                        UserOption = "3";
                        break;
                }
            }
            //Si el alumno cierra sesión debe volver a lanzar el menú principal
            break;

        case "2":

            Console.WriteLine("FORMULARIO DE REGISTRO - CAMPUS ACADÉMICO");
            Console.WriteLine("-----------------------------------------");
            int NDocument = Session.SignUpValidation(StringConnection);//Valida que el documento no haya sido registrado anteriormente
            Session.SignUpUser(StringConnection, NDocument);//Graba la información en la DB
            UserOption = Session.MainMenu(StringConnection); //Luego del registro arroja el menú nuevamente
            break;

        case "3":
            Console.WriteLine("Gracias por utilizar nuestra plataforma");
            Console.WriteLine("Cantidad de usuarios que accedieron durante la sesión: " + UserSession.Count());
            if (UserSession.Count() > 0)
            {
                Console.WriteLine("Detalles de los usuarios:");
                Console.WriteLine("--------------------------");
                Console.WriteLine("   USUARIO   |     ROL     ");
                Console.WriteLine("--------------------------");
                for (int i = 0; i < UserSession.Count(); i++)
                {
                    Console.WriteLine(UserSession[i].Name + " | " + UserSession[i].Role);
                }
                Console.WriteLine("--------------------------");
            }
            UserOption = "0";
            break;

        default:
            if(UserOption == "0" && CurrentUser != null)
            {
                UserOption = Session.MainMenu(StringConnection);
                CurrentUser = null;
            }
            else
            {
                Console.WriteLine("No se ingresó una opción correcta.");
                UserOption = Session.MainMenu(StringConnection);
            }
            break;
    }
    //if (UserOption == "0") Session.MainMenu(StringConnection);
}
