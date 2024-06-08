namespace Collections
{
    public class IListExamples
    {
        //Extends 'ICollection', and adds indexing capabilities, which allows direct access to elements by Index.

        //Ideal for accessing elements by Index, or random access to elements.

        public IList<Student> studentsIList = Student.students.ToList();

        public List<Student> studentsList = Student.students.ToList();

        public Student student = Student.students.ToList().First();

        public void InsertElementIntoListAtSpecifiedIndex() => Student.students.ToList().Insert(0, Student.extraStudents.First());

        public void RemoveElementFromListAtSpecifiedIndex() => Student.students.ToList().RemoveAt(0);

        public int GetIndexOfList() => studentsList.IndexOf(student, 5);

        public int GetCapacityOfList() => studentsList.Capacity;
    }
}
