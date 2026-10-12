using Npgsql;
using StudentManager.Models;

namespace ProgramacionOrientadaO.Data
{
    internal class Session
    {
        private bool Semester = false;
        public string MainMenu(string StringConnection)
        {
            Console.WriteLine("BIENVENIDO AL CAMPUS ACADÉMICO");
            Console.WriteLine("| Ingreso al campus  - Menú principal|");
            Console.WriteLine("------------------------------------- ");
            Console.WriteLine("A continuación ingrese el número opción que desea ejecutar:");
            Console.WriteLine("1) Iniciar sesión.");
            Console.WriteLine("2) Registrarse.");
            Console.WriteLine("3) Salir.");
            string UserChoice = Console.ReadLine();
            if (UserChoice == null) UserChoice = "4";
            return UserChoice;
        }

        public User? LogInValidation(string StringConnection, int Ndocument, string SPassword)
        {
            NpgsqlConnection Connection = new NpgsqlConnection(StringConnection);

            string query = "SELECT * FROM users WHERE n_document = @document AND s_password = @password";

            NpgsqlCommand Command = new NpgsqlCommand(query, Connection);
            Command.Parameters.AddWithValue("@document", Ndocument);
            Command.Parameters.AddWithValue("@password", SPassword);
            Connection.Open();

            NpgsqlDataReader DataReader = Command.ExecuteReader();

            while (DataReader.Read()) //Creo el objeto usuario que inicia sesión
            {
                if (DataReader.GetBoolean(2))
                {
                    Professor Professor = new Professor();
                    Professor.Name = DataReader.GetString(1);
                    Professor.Document = DataReader.GetInt32(3);
                    Professor.DateOfBirth = DataReader.GetFieldValue<DateOnly>(6);
                    Professor.Role = "Teacher";
                    Professor.Password = SPassword;

                    //DataReader.Close();
                    //Connection.Close();
                    Console.WriteLine("Inicio de sesión exitoso.");
                    Console.WriteLine("Usuario : " + Professor.Name);
                    Console.WriteLine("Rol: " + Professor.Role);
                    Connection.Close();
                    return Professor;
                }
                else
                {
                    Student Student = new Student();

                    Student.Name = DataReader.GetString(1);
                    Student.Document = DataReader.GetInt32(3);
                    Student.DateOfBirth = DataReader.GetFieldValue<DateOnly>(6);
                    Student.Role = "Student";
                    Student.Password = SPassword;

                    if (Student.Name != null)
                    {
                        Console.WriteLine("Inicio de sesión exitoso.");
                        Console.WriteLine("Usuario : " + Student.Name);
                        Console.WriteLine("Rol: " + Student.Role);
                        Connection.Close();
                        return Student;
                    }
                }                 
            }
            Connection.Close();
            return null;
        }

        //Arroja un menú en base al Role del usuario
        public string ContextualMenu(User User)
        {
            string UserOption;
            if (User.Role == "Teacher")
            {
                Console.WriteLine("Menú de opciones");
                Console.WriteLine("1) Ver curso.");
                Console.WriteLine("2) Ver calificaciones por alumno.");
                Console.WriteLine("3) Cargar calificaciones.");
                Console.WriteLine("4) Cerrar sesión.");
                UserOption = Console.ReadLine();

                return UserOption;
            }
            else
            {
                Console.WriteLine("1) Ver calificaciones.");
                Console.WriteLine("2) Ver materias inscriptas.");
                Console.WriteLine("3) Cerrar sesión.");
                UserOption = Console.ReadLine();

                return UserOption;
            }
        }

        //Busca el documento en la DB para evitar duplicados
        public int SignUpValidation(string StringConnection)
        {
            Console.WriteLine("Número de documento: ");
            int NDocument = Convert.ToInt32(Console.ReadLine());

            NpgsqlConnection Connection = new NpgsqlConnection(StringConnection);
            string query = "SELECT n_document FROM users WHERE n_document = @document";

            NpgsqlCommand Command = new NpgsqlCommand(query, Connection);
            Command.Parameters.AddWithValue("@document", NDocument);
            Connection.Open();

            var DataReader = Command.ExecuteReader();
            
            while (DataReader.Read())
            {
                if (DataReader.GetInt32(0) == NDocument)
                {
                    Console.WriteLine("El número de documento ingresado ya se encuentra registrado. \n" +
                                        "Por favor ingrese un documento válido.");

                    SignUpValidation(StringConnection);
                }
            }
            Connection.Close();

            return NDocument;

        }

        //Guarda el usuario en la DB
        public void SignUpUser(string StringConnection, int NDocument)
        {
            Console.WriteLine("Nombre completo: ");
            string Name = Console.ReadLine();
            Console.WriteLine("Correo electrónico: ");
            string Email = Console.ReadLine();
            Console.WriteLine("Fecha de nacimiento (DD/MM/AAAA): ");
            DateOnly Dob = DateOnly.Parse(Console.ReadLine());
            int Age = DateTime.Now.Year - Dob.Year;
            Console.WriteLine("Ingrese su contraseña: ");
            string Password = Console.ReadLine();

            NpgsqlConnection Connection = new NpgsqlConnection(StringConnection);
            string query = "INSERT INTO users(s_name, b_adult, n_document, s_password, s_email, d_dob) " +
                            "VALUES(@Name, @IsAdult, @Document, @Password, @Email, @dob)";
            NpgsqlCommand Command = new NpgsqlCommand(query, Connection);

            Command.Parameters.AddWithValue("@Name", Name);
            Command.Parameters.AddWithValue("@Document", NDocument);
            Command.Parameters.AddWithValue("@Password", Password);
            Command.Parameters.AddWithValue("@Email", Email);
            Command.Parameters.AddWithValue("@dob", Dob);

            if (Age > 18) Command.Parameters.AddWithValue("@IsAdult", true);
            else Command.Parameters.AddWithValue("@IsAdult", false);

            Connection.Open();
            Command.ExecuteNonQuery();
            Connection.Close();

            Console.WriteLine("Registro creado con éxito!");
        }

        public void StartSemester()
        {
            this.Semester = true;
        }

        public void EndSemester()
        {
            this.Semester = false;
        }
    }
}
