using Collections;

var _ = new IEnumerableExamples();

Console.WriteLine("Printing names of Students");

foreach (Student student in _.studentsEnumerable)
{
    Console.WriteLine(student.FirstName + " " + student.LastName);
}

Console.ReadLine();