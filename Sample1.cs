using Microsoft.EntityFrameworkCore;

namespace EFCoreMultiThread
{
    public class Sample1
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
            // Consulta com rastreramento de entidades
            var students = await context.Students.ToListAsync();            

            var stepProcessors = new List<Task>();

            foreach (var student in students)
            {
                stepProcessors.Add(Task.Run(() =>
                {
                    // Para evitar o problema de concorrência, não utilizamos o contexto nesse escopo, pois ele é compartilhado entre várias threads
                    // Apenas atualizamos o objeto
                    student.Name = "Updated " + student.Name;
                }));
            }

            await Task.WhenAll(stepProcessors);

            // Salva as alterações no contexto principal
            await context.SaveChangesAsync();

            Console.WriteLine("Students updated. Press any key to continue...");
            Console.ReadKey();
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