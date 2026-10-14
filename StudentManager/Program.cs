using StudentManager.Data;
using StudentManager.Models;

Session session = new Session();
List<User> userSession = new List<User>();
User? currentUser = null;

string stringConnection = "Host=localhost;" +
                                  "Database=campus_academic_db;" +
                                  "Port=5432;" +
                                  "Username=postgres;" +
                                  "Password=1234;";



string userOption = session.MainMenu(stringConnection);
while (userOption != "0")
{
    //MENÚ PRINCIPAL POR DEFECTO
    switch (userOption)
    {
        case "1":
            //Valida que se haya iniciado sesión correctamente
            while (currentUser == null)
            {
                Console.WriteLine("Ingrese su número de documento: ");
                int nDocument;
                try
                {
                    nDocument = Convert.ToInt32(Console.ReadLine());

                }catch(FormatException e)
                {
                    Console.WriteLine(e.Message);
                    nDocument = 0;
                }
                Console.Write("Ingrese su contraseña: ");
                string sPassword = Console.ReadLine();

                currentUser = session.LogInValidation(stringConnection, nDocument, sPassword);

                if(currentUser == null) Console.WriteLine("Usuario y/o contraseña incorrectos.");
            }

            userSession.Add(currentUser);
            userOption = session.ContextualMenu(currentUser);


            //MENÚ CONTEXTUAL DE USUARIO
            while (userOption != "0")
            {
                switch (userOption)
                {
                    case "1":
                        {
                            if (currentUser is Professor professor) //Si es profesor devuelve curso por #
                            {
                                Console.WriteLine("¿Qué curso desea ver?");
                                int grade = Convert.ToInt32(Console.ReadLine());

                                professor.ShowGrade(stringConnection, grade);
                            }

                            if (currentUser is Student student) //Si es estudiante devuelve calificaciones 
                            {
                                student.ShowNotes(stringConnection);
                            }
                            Console.WriteLine("-------------------------------");
                            Console.WriteLine("Qué otra acción desea realizar?");
                            Console.WriteLine("-------------------------------");
                            userOption = session.ContextualMenu(currentUser);
                            break;
                        }

                    case "2":
                        {
                            //Si es PROFESOR ver calificaciones del curso
                            if (currentUser is Professor professor)
                            {
                                Console.WriteLine("Ingrese el número de documento del estudiante: ");
                                int document = Convert.ToInt32(Console.ReadLine());
                                professor.NotePerStudent(stringConnection, document);
                            }
                            //Si es ALUMNO ver asignaturas del alumno
                            if (currentUser is Student student)
                            {
                                student.ShowAsignature(stringConnection);
                            }
                        }

                        Console.WriteLine("-------------------------------");
                        Console.WriteLine("Qué otra acción desea realizar?");
                        Console.WriteLine("-------------------------------");
                        userOption = session.ContextualMenu(currentUser);
                        break;

                    case "3":
                        {
                            if (currentUser is Professor professor) //Si es PROFESOR cargar calificaciones a alumno
                            {
                                string nextNote = "si", subject = "";
                                int note, document;
                                while (nextNote == "si")
                                {
                                    Console.WriteLine("Ingrese el número de documento del alumno: ");
                                    try
                                    {
                                        document = Convert.ToInt32(Console.ReadLine());
                                        Console.Write("Ingrese el nombre de la materia a la que corresponde la calificación: ");
                                        subject = Console.ReadLine();
                                        Console.WriteLine("Ingrese la nota: ");
                                        note = Convert.ToInt32(Console.ReadLine());

                                    }catch(FormatException e)
                                    {
                                        note = 0;
                                        document = 0;
                                    }
                                    while (note < 1 || note > 10)
                                    {
                                        Console.WriteLine("La calificación no puede ser menor a 1 o mayor a 10");
                                        Console.WriteLine("Ingrese la calificación: ");
                                        note = Convert.ToInt32(Console.ReadLine());
                                    }

                                    professor.PutNotes(stringConnection, document, note, subject);

                                    Console.WriteLine("Desea ingresar otra calificación? ");
                                    nextNote = Console.ReadLine().ToLower();
                                }

                                Console.WriteLine("-------------------------------");
                                Console.WriteLine("Qué otra acción desea realizar?");
                                Console.WriteLine("-------------------------------");
                                userOption = session.ContextualMenu(currentUser);
                            }

                            if (currentUser is Student) //Si es ALUMNO sale del menú contextual
                            {
                                Console.WriteLine("Sesión finalizada");
                                userOption = "0";
                            }
                        }
                        
                        break;
                    case "4":  
                        {
                            if (currentUser is Professor)
                            {
                                Console.WriteLine("Sesión finalizada");
                                userOption = "0";
                            }
                            else
                            {
                                Console.WriteLine("Opción del menú no disponible.");
                                userOption = session.ContextualMenu(currentUser);
                            }
                        }
                        break;
                    default:
                        Console.WriteLine("Opción no válida. Sesión finalizada.");
                        userOption = "3";
                        break;
                }
            }
            //Si el alumno cierra sesión debe volver a lanzar el menú principal
            break;

        case "2":

            Console.WriteLine("FORMULARIO DE REGISTRO - CAMPUS ACADÉMICO");
            Console.WriteLine("-----------------------------------------");
            int nDocument = session.SignUpValidation(stringConnection);//Valida que el documento no haya sido registrado anteriormente
            session.SignUpUser(stringConnection, nDocument);//Graba la información en la DB
            userOption = session.MainMenu(stringConnection); //Luego del registro arroja el menú nuevamente
            break;

        case "3":
            Console.WriteLine("Gracias por utilizar nuestra plataforma");
            Console.WriteLine("Cantidad de usuarios que accedieron durante la sesión: " + userSession.Count());
            if (userSession.Count() > 0)
            {
                Console.WriteLine("Detalles de los usuarios:");
                Console.WriteLine("--------------------------");
                Console.WriteLine("   USUARIO   |     ROL     ");
                Console.WriteLine("--------------------------");
                for (int i = 0; i < userSession.Count(); i++)
                {
                    Console.WriteLine(userSession[i].Name + " | " + userSession[i].Role);
                }
                Console.WriteLine("--------------------------");
            }
            userOption = "0";
            break;

        default:
            if(userOption == "0" && currentUser != null)
            {
                userOption = session.MainMenu(stringConnection);
                currentUser = null;
            }
            else
            {
                Console.WriteLine("No se ingresó una opción correcta.");
                userOption = session.MainMenu(stringConnection);
            }
            break;
    }
}
