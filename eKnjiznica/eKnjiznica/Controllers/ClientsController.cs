using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using eKnjiznica.Data;
using eKnjiznica.Data.Entities;
using eKnjiznica.Data.Repositories;
using Microsoft.AspNetCore.Authorization;
using eKnjiznica.Models;
using Microsoft.AspNetCore.Identity;
using eKnjiznica.ViewModels;

namespace eKnjiznica.Controllers
{
    [Authorize]
    public class ClientsController : Controller
    {
        private readonly IClientRepository _repository;
        private readonly UserManager<ApplicationUser> _userManager;

        public ClientsController(IClientRepository repo, UserManager<ApplicationUser> userManager)
        {
            _repository = repo;
            _userManager = userManager;
        }

        // GET: Clients
        public async Task<IActionResult> Index()
        {
            return View(await _repository.FindAllAsync());
        }

        // GET: Clients/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                var user = await _userManager.FindByEmailAsync(User.Identity.Name);
                var currentClient = await _repository.FindByApplicationId(user.Id);
                return View(currentClient);
            }

            var client = await _repository.FindOne(id.Value);
            if (client == null)
            {
                return NotFound();
            }
            var vm = new ClientViewModel
            {
                Clients = client,
                IsAdmin = User.IsInRole("Admin")
            };

            return View(vm);

            
        }

        // GET: Clients/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Clients/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Credit")] Client client)
        {
            if (ModelState.IsValid)
            {
                _repository.Add(client);
                await _repository.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(client);
        }

        // GET: Clients/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = await _repository.FindOne(id.Value);
            if (client == null)
            {
                return NotFound();
            }
            return View(client);
        }

        // POST: Clients/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Credit,applicationUserId")] Client client)
        {
            if (id != client.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {             
                    _repository.Update(client);
                    await _repository.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClientExists(client.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(client);
        }

        // GET: Clients/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = await _repository.FindOne(id.Value);
            if (client == null)
            {
                return NotFound();
            }

            return View(client);
        }

        // POST: Clients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var client = await _repository.FindOne(id);
            _repository.Remove(client);
            await _repository.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ClientExists(int id)
        {
            return _repository.Any(id);
        }
    }
}
