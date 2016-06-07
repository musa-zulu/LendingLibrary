using System;
using System.Collections.Generic;
using System.Net;
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

        public JsonResult Edit(Guid? id)
        {
            if (id == Guid.Empty) return Json(JsonRequestBehavior.AllowGet);
            var person = _personRepository.GetById(id);
            var personViewModel = _mappingEngine.Map<Person, PersonViewModel>(person);
            return Json(new { personViewModel }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Edit(PersonViewModel personViewModel)
        {
            if (ModelState.IsValid)
            {
                var existingPerson = _personRepository.GetById(personViewModel.Id);
                var newPerson = _mappingEngine.Map<PersonViewModel, Person>(personViewModel);
                _personRepository.Update(existingPerson, newPerson);

                return RedirectToAction("Index");
            }
            return View(personViewModel);
        }
        
        [HttpPost, ActionName("Delete")]
        public JsonResult DeleteConfirmed(Guid id)
        {
            if (id != Guid.Empty)
            {
                var person = _personRepository.GetById(id);
                _personRepository.DeletePerson(person);
            }
            return Json(JsonRequestBehavior.AllowGet);
        }
    }
}
