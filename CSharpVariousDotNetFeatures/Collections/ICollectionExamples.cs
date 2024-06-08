namespace Collections
{
    public class ICollectionExamples
    {
        //Extends 'IEnumerable', and adds additional functionality to work with Collections.

        //Ideal for cases where Enumeration and access to the 'Count' of elements is required.

        public ICollection<Student> studentsCollection = Student.students.ToList();

        public ICollection<Student> extraStudentsCollection = Student.extraStudents.ToList();

        public Student primaryStudentsSingular = Student.students.ToList().First();

        public Student extraStudentsSingular = Student.extraStudents.ToList().First();

        public int GetCountOfElements() => studentsCollection.Count;

        public bool IsReadOnly() => studentsCollection.IsReadOnly;

        public void Add() => studentsCollection.Add(extraStudentsSingular);

        public void Clear() => studentsCollection.Clear();

        public bool Contains() => studentsCollection.Contains(primaryStudentsSingular);

        public void CopyTo() => extraStudentsCollection.CopyTo([.. studentsCollection], 12);

        public bool Remove() => studentsCollection.Remove(Student.students.ToList().Last());
    }
}
