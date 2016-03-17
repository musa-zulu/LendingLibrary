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
        private IMappingEngine _mappingEngine;
        private ILendingRepository _lendingRepository;
        private IPersonRepository _personRepository;
        private readonly IItemsRepository _itemsRepository;
        private IMappingEngine mappingEngine;

        public LendingController(ILendingRepository lendingRepository)
        {
            if (lendingRepository == null) throw new ArgumentNullException(nameof(lendingRepository));
            this._lendingRepository = lendingRepository;
        }

        public LendingController(ILendingRepository lendingRepository, IMappingEngine mappingEngine, IPersonRepository personRepository, IItemsRepository itemsRepository) : this(lendingRepository)
        {
            if (mappingEngine == null) throw new ArgumentNullException(nameof(mappingEngine));
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
        public ActionResult Edit(LendingViewModel itemViewModel)
        {
            if (ModelState.IsValid)
            {
                var existingItem = _lendingRepository.GetById(itemViewModel.Id);
                var newItem = _mappingEngine.Map<LendingViewModel, Lending>(itemViewModel);
                _lendingRepository.Update(existingItem, newItem);

                return RedirectToAction("Index");
            }
            return View(itemViewModel);
        }

        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var item = _lendingRepository.GetById(id);
            var lendingItemViewModel = _mappingEngine.Map<Lending, LendingViewModel>(item);
            if (lendingItemViewModel == null)
            {
                return HttpNotFound();
            }
            return View(lendingItemViewModel);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            if (id != Guid.Empty)
            {
                var item = _lendingRepository.GetById(id);
                _lendingRepository.DeleteLending(item);
            }
            return View("Index");
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
    }
}