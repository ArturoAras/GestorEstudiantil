#Student Manager

Console application to manage students and personal. Built with C# and .NET

##FEATURES

As a user you can take three main roles: Director, Professor or Student.

 - As a Director, you can open or close the semester and register new professors based on the list of users.

 - As a Professor, you can put notes to students and search students history.

 - As a Student, you can sign in new subjects and consult your notes history.

 - All movements are saved in a PostgreSQL DB.


##Tech Stack

 - C#
 - .NET 9.0
 - Npgsql 10.0.3


###Status
	- In progress. Next steps
		- A Subject class, where it saves the duration and the minimum notes per student.
		- Semester validation: if the semester is closed, no one can save notes, register subjects or any other movement.

#Author
	**Arturo Ariel Aras**
	**www.linkedin.com/in/arturo-aras**