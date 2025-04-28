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
    public class ActividadesController : Controller
    {
        private readonly DbgymContext _context;

        public ActividadesController(DbgymContext context)
        {
            _context = context;
        }

        // GET: Actividades

        public async Task<IActionResult> Index(string buscar, string filtro)
        {
            var actividades = from Actividad in _context.Actividades
                           select Actividad;
            if (!String.IsNullOrEmpty(buscar))
            {
                actividades= actividades.Where(S => S.NombreActividad.Contains(buscar));
            }

      
            return View(await actividades.ToListAsync());
        }
       
        // GET: Actividades/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var actividade = await _context.Actividades
                .FirstOrDefaultAsync(m => m.IdActividad == id);
            if (actividade == null)
            {
                return NotFound();
            }

            return View(actividade);
        }

        // GET: Actividades/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Actividades/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdActividad,NombreActividad,DescripcionActividad")] Actividades actividade)
        {
            if (ModelState.IsValid)
            {
                _context.Add(actividade);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(actividade);
        }

        // GET: Actividades/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var actividade = await _context.Actividades.FindAsync(id);
            if (actividade == null)
            {
                return NotFound();
            }

            return View(actividade);
        }

        // POST: Actividades/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdActividad,NombreActividad,DescripcionActividad")] Actividades actividade)
        {
            if (id != actividade.IdActividad)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(actividade);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ActividadeExists(actividade.IdActividad))
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
            return View(actividade);
        }

        // GET: Actividades/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var actividade = await _context.Actividades
                .FirstOrDefaultAsync(m => m.IdActividad == id);
            if (actividade == null)
            {
                return NotFound();
            }

            return View(actividade);
        }

        // POST: Actividades/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var actividade = await _context.Actividades.FindAsync(id);
            if (actividade != null)
            {
                _context.Actividades.Remove(actividade);
                foreach (Horario horario in _context.Horarios) {
                    if (horario.IdActividad == id) { 
                        _context.Remove(horario);
                    } else { 
                        continue;
                    }
                }
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ActividadeExists(int id)
        {
            return _context.Actividades.Any(e => e.IdActividad == id);
        }
    }
}
