using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GYM_ITM.Models;

namespace GYM_ITM.Controllers
{
    public class EspaciosController : Controller
    {
        private readonly DbgymContext _context;

        public EspaciosController(DbgymContext context)
        {
            _context = context;
        }

        // GET: Espacios
        public async Task<IActionResult> Index(string buscar)
        {
            var espacios = from Espacio in _context.Espacios
                           select Espacio;
            if (!String.IsNullOrEmpty(buscar))
            {
                espacios= espacios.Where(s => s.NombreEspacio.Contains(buscar));
            }
            return View(await espacios.ToListAsync());
        }

        // GET: Espacios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var espacio = await _context.Espacios
                .FirstOrDefaultAsync(m => m.IdEspacio == id);
            if (espacio == null)
            {
                return NotFound();
            }

            return View(espacio);
        }

        // GET: Espacios/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Espacios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdEspacio,NombreEspacio,LimiteDisponibilidad")] Espacio espacio)
        {
            if (ModelState.IsValid)
            {
                _context.Add(espacio);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(espacio);
        }

        // GET: Espacios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var espacio = await _context.Espacios.FindAsync(id);
            if (espacio == null)
            {
                return NotFound();
            }
            return View(espacio);
        }

        // POST: Espacios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdEspacio,NombreEspacio,LimiteDisponibilidad")] Espacio espacio)
        {
            if (id != espacio.IdEspacio)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(espacio);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EspacioExists(espacio.IdEspacio))
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
            return View(espacio);
        }

        // GET: Espacios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var espacio = await _context.Espacios
                .FirstOrDefaultAsync(m => m.IdEspacio == id);
            if (espacio == null)
            {
                return NotFound();
            }

            return View(espacio);
        }

        // POST: Espacios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var espacio = await _context.Espacios.FindAsync(id);
            if (espacio != null)
            {
                _context.Espacios.Remove(espacio);
                //foreach (Horario horario in _context.Horarios)
                //{
                //    if (horario.IdEspacio == id)
                //    {
                //        _context.Remove(horario);
                //    }
                //    else
                //    {
                //        continue;
                //    }
                //}
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EspacioExists(int id)
        {
            return _context.Espacios.Any(e => e.IdEspacio == id);
        }
    }
}
