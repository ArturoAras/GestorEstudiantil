using Npgsql;
using StudentManager.Models;

namespace StudentManager.Data
{
    internal class Session
    {
        private bool semester = false;
        public string MainMenu(string stringconnection)
        {
            Console.WriteLine("BIENVENIDO AL CAMPUS ACADÉMICO");
            Console.WriteLine("| Ingreso al campus  - Menú principal|");
            Console.WriteLine("------------------------------------- ");
            Console.WriteLine("A continuación ingrese el número opción que desea ejecutar:");
            Console.WriteLine("1) Iniciar sesión.");
            Console.WriteLine("2) Registrarse.");
            Console.WriteLine("3) Salir.");
            string userChoice = Console.ReadLine();
            if (userChoice == null) userChoice = "4";
            return userChoice;
        }

        public User? LogInValidation(string stringconnection, int nDocument, string sPassword)
        {
            NpgsqlConnection connection = new NpgsqlConnection(stringconnection);

            string query = "SELECT * FROM users WHERE n_document = @document AND s_password = @password";

            NpgsqlCommand command = new NpgsqlCommand(query, connection);
            command.Parameters.AddWithValue("@document", nDocument);
            command.Parameters.AddWithValue("@password", sPassword);
            connection.Open();

            NpgsqlDataReader dataReader = command.ExecuteReader();

            while (dataReader.Read()) //Creo el objeto usuario que inicia sesión
            {
                if (dataReader.GetBoolean(2))
                {
                    Professor Professor = new Professor();
                    Professor.Name = dataReader.GetString(1);
                    Professor.Document = dataReader.GetInt32(3);
                    Professor.DateOfBirth = dataReader.GetFieldValue<DateOnly>(6);
                    Professor.Role = "Teacher";
                    Professor.Password = sPassword;

                    //dataReader.Close();
                    //connection.Close();
                    Console.WriteLine("Inicio de sesión exitoso.");
                    Console.WriteLine("Usuario : " + Professor.Name);
                    Console.WriteLine("Rol: " + Professor.Role);
                    connection.Close();
                    return Professor;
                }
                else
                {
                    Student Student = new Student();

                    Student.Name = dataReader.GetString(1);
                    Student.Document = dataReader.GetInt32(3);
                    Student.DateOfBirth = dataReader.GetFieldValue<DateOnly>(6);
                    Student.Role = "Student";
                    Student.Password = sPassword;

                    if (Student.Name != null)
                    {
                        Console.WriteLine("Inicio de sesión exitoso.");
                        Console.WriteLine("Usuario : " + Student.Name);
                        Console.WriteLine("Rol: " + Student.Role);
                        connection.Close();
                        return Student;
                    }
                }                 
            }
            connection.Close();
            return null;
        }

        //Arroja un menú en base al Role del usuario
        public string ContextualMenu(User user)
        {
            string userOption;
            if (user.Role == "Teacher")
            {
                Console.WriteLine("Menú de opciones");
                Console.WriteLine("1) Ver curso.");
                Console.WriteLine("2) Ver calificaciones por alumno.");
                Console.WriteLine("3) Cargar calificaciones.");
                Console.WriteLine("4) Cerrar sesión.");
                userOption = Console.ReadLine();

                return userOption;
            }
            else
            {
                Console.WriteLine("1) Ver calificaciones.");
                Console.WriteLine("2) Ver materias inscriptas.");
                Console.WriteLine("3) Cerrar sesión.");
                userOption = Console.ReadLine();

                return userOption;
            }
        }

        //Busca el documento en la DB para evitar duplicados
        public int SignUpValidation(string stringconnection)
        {
            Console.WriteLine("Número de documento: ");
            int nDocument = Convert.ToInt32(Console.ReadLine());

            NpgsqlConnection connection = new NpgsqlConnection(stringconnection);
            string query = "SELECT n_document FROM users WHERE n_document = @document";

            NpgsqlCommand command = new NpgsqlCommand(query, connection);
            command.Parameters.AddWithValue("@document", nDocument);
            connection.Open();

            var dataReader = command.ExecuteReader();
            
            while (dataReader.Read())
            {
                if (dataReader.GetInt32(0) == nDocument)
                {
                    Console.WriteLine("El número de documento ingresado ya se encuentra registrado. \n" +
                                        "Por favor ingrese un documento válido.");

                    SignUpValidation(stringconnection);
                }
            }
            connection.Close();

            return nDocument;

        }

        //Guarda el usuario en la DB
        public void SignUpUser(string stringconnection, int nDocument)
        {
            Console.WriteLine("Nombre completo: ");
            string name = Console.ReadLine();
            Console.WriteLine("Correo electrónico: ");
            string email = Console.ReadLine();
            Console.WriteLine("Fecha de nacimiento (DD/MM/AAAA): ");
            DateOnly dob = DateOnly.Parse(Console.ReadLine());
            int age = DateTime.Now.Year - dob.Year;
            Console.WriteLine("Ingrese su contraseña: ");
            string password = Console.ReadLine();

            NpgsqlConnection connection = new NpgsqlConnection(stringconnection);
            string query = "INSERT INTO users(s_name, b_adult, n_document, s_password, s_email, d_dob) " +
                            "VALUES(@Name, @IsAdult, @Document, @Password, @Email, @dob)";
            NpgsqlCommand command = new NpgsqlCommand(query, connection);

            command.Parameters.AddWithValue("@Name", name);
            command.Parameters.AddWithValue("@Document", nDocument);
            command.Parameters.AddWithValue("@Password", password);
            command.Parameters.AddWithValue("@Email", email);
            command.Parameters.AddWithValue("@dob", dob);

            if (age > 18) command.Parameters.AddWithValue("@IsAdult", true);
            else command.Parameters.AddWithValue("@IsAdult", false);

            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();

            Console.WriteLine("Registro creado con éxito!");
        }

        public void StartSemester()
        {
            this.semester = true;
        }

        public void EndSemester()
        {
            this.semester = false;
        }
    }
}
