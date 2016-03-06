using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using AutoMapper;
using LendingLibrary.Core.Domain;
using LendingLibrary.Core.Interfaces.Repositories;
using LendingLibrary.Web.ViewModels;

namespace LendingLibrary.Web.Controllers
{
    public class ItemsController : Controller
    {
        private readonly IItemsRepository _itemsRepository;
        private readonly IMappingEngine _mappingEngine;

        public ItemsController(IItemsRepository itemsRepository, IMappingEngine mappingEngine)
        {
            if (itemsRepository == null) throw new ArgumentNullException(nameof(itemsRepository));
            if (mappingEngine == null) throw new ArgumentNullException(nameof(mappingEngine));
            _itemsRepository = itemsRepository;
            _mappingEngine = mappingEngine;
        }

        public ActionResult Index()
        {
            var allItems = _itemsRepository.GetAllItems();
            var itemViewModels = _mappingEngine.Map<List<Item>, List<ItemViewModel>>(allItems);
            return View(itemViewModels);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(ItemViewModel itemViewModel)
        {
            itemViewModel.Id = Guid.NewGuid();
            if (ModelState.IsValid)
            {
                var item = _mappingEngine.Map<ItemViewModel, Item>(itemViewModel);
                _itemsRepository.Save(item);
                return RedirectToAction("Index", "Items");
            }
            return View(itemViewModel);
        }

        public ActionResult Edit(Guid? id)
        {
            if (id == Guid.Empty)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var item = _itemsRepository.GetById(id);
            var itemViewModel = _mappingEngine.Map<Item, ItemViewModel>(item);
            if (itemViewModel == null)
            {
                return HttpNotFound();
            }
            return View(itemViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ItemViewModel itemViewModel)
        {
            if (ModelState.IsValid)
            {
                var existingItem = _itemsRepository.GetById(itemViewModel.Id);
                var newItem = _mappingEngine.Map<ItemViewModel, Item>(itemViewModel);
                _itemsRepository.Update(existingItem, newItem);

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
            var item = _itemsRepository.GetById(id);
            var itemViewModel = _mappingEngine.Map<Item, ItemViewModel>(item);
            if (itemViewModel == null)
            {
                return HttpNotFound();
            }
            return View(itemViewModel);
        }
        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            if (id != Guid.Empty)
            {
                var item = _itemsRepository.GetById(id);
                _itemsRepository.DeleteItem(item);
            }
            return View("Index");
        }
    }
}