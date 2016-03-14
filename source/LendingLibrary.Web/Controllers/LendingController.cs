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
    public class LendingController : Controller
    {
        private IMappingEngine _mappingEngine;
        private ILendingRepository _lendingRepository;

        public LendingController(ILendingRepository lendingRepository)
        {
            if (lendingRepository == null) throw new ArgumentNullException(nameof(lendingRepository));
            this._lendingRepository = lendingRepository;
        }

        public LendingController(ILendingRepository lendingRepository, IMappingEngine mappingEngine) : this(lendingRepository)
        {
            if (mappingEngine == null) throw new ArgumentNullException(nameof(mappingEngine));
            this._mappingEngine = mappingEngine;
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
            return View();
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
    }
}