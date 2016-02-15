using System;
using System.Collections.Generic;
using System.Web.Mvc;
using AutoMapper;
using LendingLibrary.Core.Domain;
using LendingLibrary.Core.Interfaces.Repositories;
using LendingLibrary.Web.ViewModels;

namespace LendingLibrary.Web.Controllers
{
    public class PersonController : Controller
    {
        private readonly IPersonRepository _personRepository;
        private readonly IMappingEngine _mappingEngine;

        public PersonController(IPersonRepository personRepository, IMappingEngine mappingEngine)
        {
            if (personRepository == null) throw new ArgumentNullException(nameof(personRepository));
            if (mappingEngine == null) throw new ArgumentNullException(nameof(mappingEngine));
            this._personRepository = personRepository;
            this._mappingEngine = mappingEngine;
        }

        // GET: Person
        public ActionResult Index()
        {
            var allPeople = _personRepository.GetAllPeople();
            var personViewModels = _mappingEngine.Map<List<Person>, List<PersonViewModel>>(allPeople);
            return View(personViewModels);
        }

        // GET: Person/Create
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PersonViewModel personViewModel)
        {
            if (!ModelState.IsValid) return View(personViewModel);
            var person = _mappingEngine.Map<PersonViewModel, Person>(personViewModel);
            _personRepository.Save(person);
            return RedirectToAction("Index");
        }

        // GET: Person/Edit/5
        /*       public ActionResult Edit(Guid? id)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Person person = db.People.Find(id);
                if (person == null)
                {
                    return HttpNotFound();
                }
                return View(person);
            }

            [HttpPost]
            [ValidateAntiForgeryToken]
            public ActionResult Edit(Person personViewModel)
            {
                /*  if (ModelState.IsValid)
                  {
                      db.Entry(person).State = EntityState.Modified;
                      db.SaveChanges();
                      return RedirectToAction("Index");
                  }#1#
                return View(personViewModel);
            }

            // GET: Person/Delete/5
            public ActionResult Delete(Guid? id)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
              //  Person person = db.People.Find(id);
              //  if (person == null)
                {
                    return HttpNotFound();
                }
                return View(personViewModel);
            }

            // POST: Person/Delete/5
            [HttpPost, ActionName("Delete")]
            [ValidateAntiForgeryToken]
            public ActionResult DeleteConfirmed(Guid id)
            {
              //  Person person = db.People.Find(id);
            //    db.People.Remove(person);
            //    db.SaveChanges();
                return RedirectToAction("Index");
            }S*/
    }
}
