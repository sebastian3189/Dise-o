using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GYM_ITM.Models;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using GYM_ITM.Models.Observer;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace GYM_ITM.Controllers
{
    public class UsuariosController : Controller, ISuscriber
    {
        private readonly DbgymContext _context;

        public UsuariosController(DbgymContext context)
        {
            _context = context;
        }

        //Ejecución en NotificarSuscriptores() de la interfaz INotifier Observer en HorariosController
        public async Task<string> NotificarUsuario(int id)
        {
            Usuario? usuario = await _context.Usuarios.FindAsync(id);
            if (usuario != null)
            {
                return $"Hola, soy {usuario.Nombre} {usuario.Apellido}, ya me enteré del cambio de horario o lugar. Gracias.";
            } else
            {
                return $"La persona con ID {id} no está registrado en nuestra base de datos, por ese motivo no tenemos datos para efectuar su notificación. Debe registrarse para recibir próximas notificaciones.";
            }
        }

        // GET: Usuarios
        public async Task<IActionResult> Index(string buscar, string filtro )
        {
            var usuarios = from Usuario in _context.Usuarios
                          select Usuario;
            if (!String.IsNullOrEmpty(buscar))
            {
             usuarios = usuarios.Where(s => s.Nombre.Contains(buscar) || s.Apellido.Contains(buscar) || s.Correo.Contains(buscar));
            }

            ViewData["FiltroNombre"]=string.IsNullOrEmpty(filtro)? "NombreDescendente" : "";
            ViewData["FiltroNombre"] = string.IsNullOrEmpty(filtro) ? "NombreAscendente" : "";
            ViewData["FiltroApellido"] = string.IsNullOrEmpty(filtro) ? "ApellidoDescendente" : "";
            ViewData["FiltroApellido"] = string.IsNullOrEmpty(filtro) ? "ApellidoAscendente" : "";
            ViewData["FiltroCorreo"] = string.IsNullOrEmpty(filtro) ? "CorreoDescendente" : "";
            ViewData["FiltroCorreo"] = string.IsNullOrEmpty(filtro) ? "CorreoAscendente" : "";

            switch (filtro)
            {
                case "NombreDescendente":
                    usuarios = usuarios.OrderByDescending(usuario => usuario.Nombre);
                    break;
                case "ApellidoDescendente":
                    usuarios = usuarios.OrderByDescending(usuario => usuario.Apellido);
                    break;
                case "CorreoDescendente":
                    usuarios = usuarios.OrderByDescending(usuario => usuario.Correo);
                    break;
                case "NombreAscendente":
                    usuarios = usuarios.OrderBy(usuario => usuario.Nombre);
                    break;
                case "ApellidoAscendente":
                    usuarios = usuarios.OrderBy(usuario => usuario.Apellido);
                    break;
                case "CorreoAscendente":
                    usuarios = usuarios.OrderBy(usuario => usuario.Correo);
                    break;
                default:
                    usuarios = usuarios.OrderBy(usuario => usuario.IdUsuario);
                    break;
            }



            return View(await usuarios.ToListAsync());
        }

        

        // GET: Usuarios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(m => m.IdUsuario == id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // GET: Usuarios/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Usuarios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdUsuario,Nombre,Apellido,Correo,Telefono")] Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                _context.Add(usuario);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(usuario);
        }

        // GET: Usuarios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }
            return View(usuario);
        }

        // POST: Usuarios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdUsuario,Nombre,Apellido,Correo,Telefono")] Usuario usuario)
        {
            if (id != usuario.IdUsuario)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(usuario);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsuarioExists(usuario.IdUsuario))
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
            return View(usuario);
        }

        // GET: Usuarios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(m => m.IdUsuario == id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // POST: Usuarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario != null)
            {
                _context.Usuarios.Remove(usuario);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UsuarioExists(int id)
        {
            return _context.Usuarios.Any(e => e.IdUsuario == id);
        }

    }
}
