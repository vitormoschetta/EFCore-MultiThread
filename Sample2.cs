using Microsoft.EntityFrameworkCore;

namespace EFCoreMultiThread
{
    public class Sample2
    {
        public static async Task Execute()
        {
            using (var context = new AppDbContext())
            {
                await CreateStudentsAsync(context);

                await UpdateStudentsAsync(context);

                await DeleteStudentsAsync(context);
            }
        }


        static async Task CreateStudentsAsync(AppDbContext context)
        {
            for (int i = 0; i < 3; i++)
            {
                var student = new Student { Name = "Studant " + i };
                context.Students.Add(student);
            }
            await context.SaveChangesAsync();

            Console.WriteLine("Students added. Press any key to continue...");
            Console.ReadKey();
        }


        static async Task UpdateStudentsAsync(AppDbContext context)
        {
            // Independe de rastreamento, pois não iremos atualizar o objeto no contexto principal
            var students = await context.Students.AsNoTracking().ToListAsync();

            var stepProcessors = new List<Task>();

            foreach (var student in students)
            {
                stepProcessors.Add(UpdateStudentAsync(student));
            }

            await Task.WhenAll(stepProcessors);

            Console.WriteLine("Students updated. Press any key to continue...");
            Console.ReadKey();
        }

        static async Task UpdateStudentAsync(Student student)
        {
            // É possível utilizar o contexto nesse escopo, desde que ele seja criado dentro do escopo da thread, 
            // em vez de ser compartilhado entre várias threads
            using (var context = new AppDbContext())
            {
                student.Name = "Updated " + student.Name;
                context.Update(student); // Como criamos um novo contexto, precisamos atualizar o objeto no contexto
                await context.SaveChangesAsync(); // Salva as alterações no contexto

                // Contras: Muitas conexões ao banco de dados
            }
        }


        static async Task DeleteStudentsAsync(AppDbContext context)
        {
            var students = await context.Students.ToListAsync();

            context.Students.RemoveRange(students);

            await context.SaveChangesAsync();

            Console.WriteLine("Students deleted.");
        }
    }
}