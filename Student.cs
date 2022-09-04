namespace EFCoreMultiThread
{
    public class Student
    {
        public Student()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}