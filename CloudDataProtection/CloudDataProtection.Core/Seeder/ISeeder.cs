using System.Threading.Tasks;

namespace CloudDataProtection.Core.Seeder
{
    public interface ISeeder
    {
        Task Seed();
    }
}