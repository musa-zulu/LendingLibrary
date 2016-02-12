using System;
using System.Collections.Generic;
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
            if (ModelState.IsValid)
            {
                var item = _mappingEngine.Map<ItemViewModel, Item>(itemViewModel);
                _itemsRepository.Save(item);
                return RedirectToAction("Index", "Items");
            }
            return View(itemViewModel);
        }

        public ActionResult Edit(Guid id)
        {
            var item = _itemsRepository.GetById(id);
            var itemViewModel = _mappingEngine.Map<Item, ItemViewModel>(item);
            return View(itemViewModel);
        }

        [HttpPost]
        public ActionResult Edit(ItemViewModel itemViewModel)
        {
            if (!ModelState.IsValid) return View(itemViewModel);
            var item = _mappingEngine.Map<ItemViewModel, Item>(itemViewModel);
            _itemsRepository.Save(item);
            return RedirectToAction("Index", "Items");
        }

        public ActionResult Delete(Guid id)
        {
            if (id != Guid.Empty)
            {
                var item = _itemsRepository.GetById(id);//TODO: test this line
                _itemsRepository.DeleteItem(item);
            }
            return View("Index");
        }
    }
}