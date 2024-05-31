using CsvDataApp.Data;
using CsvDataApp.Models;
using Microsoft.EntityFrameworkCore;

namespace CsvDataApp.Repositories
{
    public class PersonRepository: IPersonRepository
    {
        private readonly AppDbContext _context;
        public PersonRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Person>> GetPersons()
        {
            return await _context.Persons.ToListAsync();
        }

        public async Task<Person> GetPersonByID(int id)
        {
            return await _context.Persons.FindAsync(id);
        }

        public async Task InsertPerson(Person person)
        {
            await _context.Persons.AddAsync(person);
            await Save();
        }

        public async Task DeletePerson(int personID)
        {
            Person person = await _context.Persons.FindAsync(personID);
            _context.Persons.Remove(person);
            await Save();
        }

        public async Task UpdatePerson(Person person)
        {
            _context.Update(person);
            await Save();
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }

    public interface IPersonRepository
    {
        Task<IEnumerable<Person>> GetPersons();
        Task<Person> GetPersonByID(int id);
        Task InsertPerson(Person person);
        Task DeletePerson(int personID);
        Task UpdatePerson(Person person);
        Task Save();
        void Dispose();
    }
}
