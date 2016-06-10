using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using LendingLibrary.Core;
using LendingLibrary.Core.Domain;
using LendingLibrary.Core.Interfaces.Repositories;
using LendingLibrary.Web.ViewModels;
using Microsoft.AspNet.Identity;

namespace LendingLibrary.Web.Controllers
{
    public class LendingController : Controller
    {
        private readonly IMappingEngine _mappingEngine;
        private readonly ILendingRepository _lendingRepository;
        private readonly IPersonRepository _personRepository;
        private readonly IItemsRepository _itemsRepository;
        private IDateTimeProvider _dateTimeProvider;

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
            _mappingEngine = mappingEngine;
            _personRepository = personRepository;
            _itemsRepository = itemsRepository;
        }

        public IDateTimeProvider DateTimeProvider
        {
            get { return _dateTimeProvider ?? (_dateTimeProvider = new DefaultDateTimeProvider()); }
            set
            {
                if (_dateTimeProvider != null) throw new InvalidOperationException("DateTimeProvider is already set");
                _dateTimeProvider = value;
            }
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
            var username = GetUserName();
            var lendingViewModel = new LendingViewModel();
            SetBaseFieldsOn(lendingViewModel, username);
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

        public JsonResult Edit(Guid? id)
        {
            if (id == Guid.Empty)
            {
                return Json(JsonRequestBehavior.AllowGet);
            }
            var item = _lendingRepository.GetById(id);
            var lendingViewModel = _mappingEngine.Map<Lending, LendingViewModel>(item);
            if (lendingViewModel == null)
            {
                return Json(JsonRequestBehavior.AllowGet);
            }
            lendingViewModel.PeopleSelectList = GetPersonSelectList(lendingViewModel);
            lendingViewModel.ItemsSelectList = GetItemsSelectList(lendingViewModel);
            return Json(new { lendingViewModel }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Edit(LendingViewModel lendingViewModel)
        {
            if (ModelState.IsValid)
            {
                var existingItem = _lendingRepository.GetById(lendingViewModel.Id);
                UpdateBaseFieldsOn(lendingViewModel);
                var newItem = _mappingEngine.Map<LendingViewModel, Lending>(lendingViewModel);
                SetItemOn(lendingViewModel, newItem);
                SetPersonOn(lendingViewModel, newItem);
                _lendingRepository.Update(existingItem, newItem);

                return RedirectToAction("Index");
            }
            return View(lendingViewModel);
        }

        public JsonResult Delete(Guid? id)
        {
            if (id != Guid.Empty)
            {
                var lending = _lendingRepository.GetById(id);
                _lendingRepository.DeleteLending(lending);
            }
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        private void SetBaseFieldsOn(LendingViewModel lendingViewModel, string username)
        {
            lendingViewModel.PeopleSelectList = GetPersonSelectList(lendingViewModel);
            lendingViewModel.ItemsSelectList = GetItemsSelectList(lendingViewModel);
            lendingViewModel.CreatedUsername = username;
            lendingViewModel.DateCreated = DateTimeProvider.Now;
            lendingViewModel.LastModifiedUsername = username;
            lendingViewModel.DateLastModified = DateTimeProvider.Now;
        }

        private void UpdateBaseFieldsOn(LendingViewModel lendingViewModel)
        {
            lendingViewModel.DateLastModified = DateTimeProvider.Now;
            lendingViewModel.LastModifiedUsername = GetUserName();
        }

        private string GetUserName()
        {
            var username = "";
            if (User?.Identity != null)
                username = User.Identity.GetUserName();
            return username;
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