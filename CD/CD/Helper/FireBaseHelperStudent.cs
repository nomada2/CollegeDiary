﻿using CD.Models;
using Firebase.Database;
using Firebase.Database.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CD.Helper
{
	class FireBaseHelperStudent
	{
		private readonly string Student_Name = "Students";
		private readonly string UserUID = App.UserUID;
		readonly FirebaseClient firebase = new FirebaseClient(App.conf.firebase);

		public async Task<List<Student>> GetAllStudents()
		{
			return (await firebase.Child(UserUID).Child(Student_Name).OnceAsync<Student>()).Select(item => new Student
			{
				StudentName = item.Object.StudentName,
				StudentID = item.Object.StudentID,
				StudentEmail = item.Object.StudentEmail,
			}).ToList();
		}

		public async Task AddStudent(string UID, string studentName, string UC, string studentEmail)
		{
			await firebase.Child(UID).Child(Student_Name).PostAsync(new Student()
			{
				StudentID = Guid.NewGuid(),
				StudentName = studentName,
				StudentEmail = studentEmail,
				College_University = UC
			}) ;
		}

		public async Task<Student> GetStudent(Guid studentID)
		{
			var allStudents = await GetAllStudents();
			await firebase.Child(UserUID).Child(Student_Name).OnceAsync<Student>();
			return allStudents.FirstOrDefault(a => a.StudentID == studentID);
		}

		public async Task<Student> GetStudent(string studentName)
		{
			var allStudents = await GetAllStudents();
			await firebase.Child(UserUID).Child(Student_Name).OnceAsync<Student>();
			return allStudents.FirstOrDefault(a => a.StudentName == studentName);
		}

		public async Task DeleteStudent(Guid StudentID)
		{
			var toDeleteStudent = (await firebase.Child(Student_Name).OnceAsync<Subject>()).FirstOrDefault
				(a => a.Object.SubjectID == StudentID);
			await firebase.Child(UserUID).Child(Student_Name).Child(toDeleteStudent.Key).DeleteAsync();
		}
	}
}
