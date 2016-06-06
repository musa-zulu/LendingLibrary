using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using AutoMapper;
using LendingLibrary.Core.Domain;
using LendingLibrary.Core.Interfaces.Repositories;
using LendingLibrary.Web.ViewModels;

namespace LendingLibrary.Web.Controllers
{
    public class LendingController : Controller
    {
        private readonly IMappingEngine _mappingEngine;
        private readonly ILendingRepository _lendingRepository;
        private readonly IPersonRepository _personRepository;
        private readonly IItemsRepository _itemsRepository;

        public LendingController(ILendingRepository lendingRepository)
        {
            if (lendingRepository == null) throw new ArgumentNullException(nameof(lendingRepository));
            this._lendingRepository = lendingRepository;
        }

        public LendingController(ILendingRepository lendingRepository, IMappingEngine mappingEngine, IPersonRepository personRepository, IItemsRepository itemsRepository) : this(lendingRepository)
        {
            if (mappingEngine == null) throw new ArgumentNullException(nameof(mappingEngine));
            if (personRepository == null) throw new ArgumentNullException(nameof(personRepository));
            if (itemsRepository == null) throw new ArgumentNullException(nameof(itemsRepository));
            this._mappingEngine = mappingEngine;
            _personRepository = personRepository;
            _itemsRepository = itemsRepository;
        }
        
        public ActionResult Index()
        {
            var viewModel = new List<LendingViewModel>();
            var lendings = _lendingRepository.GetAll();
            if (lendings != null)
            {
                viewModel = _mappingEngine.Map<List<Lending>, List<LendingViewModel>>(lendings);
            }
            
            return View(viewModel);
        }

        public ActionResult Create()
        {
            var lendingViewModel = new LendingViewModel();
            var people = GetPersonSelectList(lendingViewModel);
            var items = GetItemsSelectList(lendingViewModel);
            lendingViewModel.PeopleSelectList = people;
            lendingViewModel.ItemsSelectList = items;
            return View(lendingViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(LendingViewModel lendingViewModel)
        {
            if (!ModelState.IsValid) return View(lendingViewModel);
            var lendingEntry = _mappingEngine.Map<LendingViewModel, Lending>(lendingViewModel);
            SetItemOn(lendingViewModel, lendingEntry);
            SetPersonOn(lendingViewModel, lendingEntry);
            _lendingRepository.Save(lendingEntry);
            return RedirectToAction("Index");
        }

        public ActionResult Edit(Guid? id)
        {
            if (id == Guid.Empty)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var item = _lendingRepository.GetById(id);
            var lendingItemViewModel = _mappingEngine.Map<Lending, LendingViewModel>(item);
            if (lendingItemViewModel == null)
            {
                return HttpNotFound();
            }
            var people = GetPersonSelectList(lendingItemViewModel);
            var items = GetItemsSelectList(lendingItemViewModel);
            lendingItemViewModel.PeopleSelectList = people;
            lendingItemViewModel.ItemsSelectList = items;
            return View(lendingItemViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(LendingViewModel lendingViewModel)
        {
            if (ModelState.IsValid)
            {
                var existingItem = _lendingRepository.GetById(lendingViewModel.Id);
                var newItem = _mappingEngine.Map<LendingViewModel, Lending>(lendingViewModel);
                SetItemOn(lendingViewModel, newItem);
                SetPersonOn(lendingViewModel, newItem);
                _lendingRepository.Update(existingItem, newItem);

                return RedirectToAction("Index");
            }
            return View(lendingViewModel);
        }
        
        public JsonResult Delete(Guid id)
        {
            if (id != Guid.Empty)
            {
                var lending = _lendingRepository.GetById(id);
                _lendingRepository.DeleteLending(lending);
            }
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        private SelectList GetPersonSelectList(LendingViewModel viewModel)
        {
            var people = _personRepository.GetAllPeople();
            people = people ?? new List<Person>();
            var listItems = people.Select(p => new SelectListItem
            {
                Text = p.FirstName,
                Value = p.PersonId.ToString()
            });
            return new SelectList(listItems, "Value", "Text", viewModel.PersonId);
        }

        private SelectList GetItemsSelectList(LendingViewModel viewModel)
        {
            var list = _itemsRepository.GetAllItems();
            list = list ?? new List<Item>();
            var listItems = list.Select(p => new SelectListItem
            {
                Text = p.ItemName,
                Value = p.ItemId.ToString()

            });
            return new SelectList(listItems, "Value", "Text", viewModel.ItemId);
        }
       
        private void SetItemOn(LendingViewModel viewModel, Lending lending)
        {
            if (viewModel.ItemId == Guid.Empty) return;
            var item = _itemsRepository.GetById(viewModel.ItemId);
            lending.ItemName = item.ItemName;
        }

        private void SetPersonOn(LendingViewModel viewModel, Lending lending)
        {
            if (viewModel.PersonId != Guid.Empty)
            {
                var person = _personRepository.GetById(viewModel.PersonId);
                lending.PersonName = person.FirstName;
            }
        }
    }
}