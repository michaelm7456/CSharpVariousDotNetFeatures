namespace Collections
{
    public class IEnumerableExamples
    {
        //Most basic of 'IEnumerable', 'ICollection' and 'IList'
        
        //Intended as a read-only Collection of objects.

        //Ideal for iterating over a Collection without modifying it.

        public IEnumerable<Student> studentsEnumerable = Student.students;

        public IEnumerable<Student> extraStudentsEnumerable = Student.extraStudents;

        public IEnumerator<Student> studentsEnumerator = Student.students.GetEnumerator();

        public IEnumerable<Student> CombineMultipleListsIntoOne() => studentsEnumerable.Concat(extraStudentsEnumerable);

        public IEnumerable<string> SelectFirstNamesInCollection() => studentsEnumerable.Select(x => x.FirstName);
    }
}