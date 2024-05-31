using CsvDataApp.Models;
using CsvDataApp.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CsvDataApp.Controllers
{
    public class PersonController : Controller
    {
        private readonly IPersonRepository _repository;

        public PersonController(IPersonRepository repository)
        {
            _repository = repository;
        }

        public async Task<IActionResult> Index()
        {
            var persons = await _repository.GetPersons();
            return View(persons);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var person = await _repository.GetPersonByID(id.Value);
            if (person == null)
            {
                return NotFound();
            }
            return View("Edit", person);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Identity,FirstName,Sirname,Age,Sex,Mobile,Active")] Person person)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _repository.UpdatePerson(person);
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(person);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var person = await _repository.GetPersonByID(id.Value);
            
            if (person == null)
            {
                return NotFound();
            }
            return View(person);
        }

        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _repository.DeletePerson(id);
            return RedirectToAction(nameof(Index));
        }


    }

   
}
