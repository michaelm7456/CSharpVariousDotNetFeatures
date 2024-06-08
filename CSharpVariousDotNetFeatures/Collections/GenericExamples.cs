namespace Collections
{
    public class GenericExamples
    {
        public IEnumerable<Student> studentsEnumerable = Student.students;

        public IEnumerable<Student> extraStudentsEnumerable = Student.extraStudents;

        public IEnumerator<Student> studentsEnumerator = Student.students.GetEnumerator();

        public Student GetFirstItemInCollection() => studentsEnumerable.First();

        public Student GetFirstItemInCollectionElseReturnEmpty() => studentsEnumerable.FirstOrDefault();

        public Student GetLastItemInCollection() => studentsEnumerable.Last();

        public Student GetLastItemInCollectionElseReturnEmpty() => studentsEnumerable.LastOrDefault();

        //Use '.Any()' to check if any elements meet a specific condition.
        public bool CheckIfAnyElementMeetsConditionInCollection(string surname) => studentsEnumerable.Any(_ => _.LastName == surname);

        //Use '.Contains()' when you need to check if a specific element exists in a collection.
        public bool CheckIfSpecificElementExistsInCollection(string name) => studentsEnumerable.Contains(studentsEnumerable.First());
    }
}
