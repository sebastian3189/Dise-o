using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GYM_ITM.Models;
using GYM_ITM.Models.Observer;

namespace GYM_ITM.Controllers
{
    public class HorariosController : Controller, INotifier
    {
        private readonly DbgymContext _context;
        private readonly ISuscriber _suscriber;
        //private List<Usuario> suscriptores;

        public HorariosController(DbgymContext context, ISuscriber suscriber)
        {
            _context = context;
            _suscriber = suscriber;
            //suscriptores = new List<Usuario>();
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmarAsistencia(int idHorario, int idUsuario)
        {
            Horario? horario = await _context.Horarios.FindAsync(idHorario);
            Usuario? usuario = await _context.Usuarios.FindAsync(idUsuario);

            if (usuario != null && horario != null)
            {
                usuario.HorariosConfirmados.Add(horario);
                horario.UsuariosConfirmados.Add(usuario);
                await _context.SaveChangesAsync();
                //suscriptores.Add(usuario);
                return RedirectToAction("Index");
            }
            else {
                return RedirectToAction("Index");
            }
        }

        public async Task<string> NotificarSuscriptores(int idHorario)
        {
            string respuestaSuscriptores = "SUSCRIPTORES NOTIFICADOS\n\n";
            List<Usuario> listaSuscriptores = await _context.Usuarios.Where(u => u.HorariosConfirmados.Any(h => h.IdHorario == idHorario)).Include(u => u.HorariosConfirmados).ToListAsync();

            if (listaSuscriptores.Any())
            {
                foreach (Usuario suscriptor in listaSuscriptores)
                {
                    respuestaSuscriptores += $"{await _suscriber.NotificarUsuario(suscriptor.IdUsuario)}\n";
                }
                return respuestaSuscriptores;
            }
            else {
                respuestaSuscriptores += "Ningún usuario confirmo su asistencia a este horario, por lo tanto, ningún usuario fue notificado.";
                return respuestaSuscriptores;
            }
        }

        //Siempre que un usuario confirma asistencia a un horario se añade como suscriptor al horario.
        //Es decir, se añade a Lista 'suscriptores'.
        public void AñadirSuscriptor(Usuario suscriptor)
        {
            //suscriptores.Add(suscriptor);
        }

        //Siempre que un usuario cancela asistencia a un horario se elimina como suscriptor del horario
        //Es decir, se elimina de Lista 'suscriptores'.
        public void ElimminarSuscriptor(Usuario suscriptor)
        {
            //suscriptores.Remove(suscriptor);
        }

        // GET: Horarios
        public async Task<IActionResult> Index(DateTime? fechaInicio, DateTime? fechaFin, int? idActividad, int? idEspacio)
        {
            var horarios = _context.Horarios.Include(h => h.IdActividadNavigation).Include(h => h.IdEspacioNavigation).AsQueryable();

            // Filtro por actividad
            if (idActividad.HasValue)
            {
                horarios = horarios.Where(h => h.IdActividad == idActividad);
            }

            // Filtro por espacio
            if (idEspacio.HasValue)
            {
                horarios = horarios.Where(h => h.IdEspacio == idEspacio);
            }

            // Filtro por fecha de inicio
            if (fechaInicio.HasValue)
            {
                horarios = horarios.Where(h => h.FechaInicio >= fechaInicio);
            }

            // Filtro por fecha de fin
            if (fechaFin.HasValue)
            {
                horarios = horarios.Where(h => h.FechaFin <= fechaFin);
            }

            // Pasar los datos de las actividades y los espacios para el filtro en la vista
            ViewData["IdActividad"] = new SelectList(_context.Actividades, "IdActividad", "NombreActividad", idActividad);
            ViewData["IdEspacio"] = new SelectList(_context.Espacios, "IdEspacio", "NombreEspacio", idEspacio);

            // Pasar las fechas de inicio y fin a la vista para mostrar los filtros seleccionados
            ViewData["FechaInicio"] = fechaInicio?.ToString("yyyy-MM-ddTHH:mm");
            ViewData["FechaFin"] = fechaFin?.ToString("yyyy-MM-ddTHH:mm");

            //Pasar datos de Usuarios para modal de confirmar asistencia
            var listaUsuarios = await _context.Usuarios.ToListAsync();
            ViewBag.ListaUsuarios = new SelectList(listaUsuarios, "IdUsuario", "Nombre");

            return View(await horarios.ToListAsync());
        }

        // GET: Horarios/Create
        public IActionResult Create()
        {
            ViewData["IdActividad"] = new SelectList(_context.Actividades, "IdActividad", "NombreActividad");
            ViewData["IdEspacio"] = new SelectList(_context.Espacios, "IdEspacio", "NombreEspacio");
            return View();
        }

        // POST: Horarios/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdHorario,FechaInicio,FechaFin,IdActividad,IdEspacio")] Horario horario)
        {
            // Verificar que todos los campos necesarios están completos
            if (horario.IdActividad == null || horario.IdEspacio == null)
            {
                TempData["ErrorMessage"] = "Todos los campos deben estar completos. Seleccione una actividad y un espacio.";
                ViewData["IdActividad"] = new SelectList(_context.Actividades, "IdActividad", "NombreActividad", horario.IdActividad);
                ViewData["IdEspacio"] = new SelectList(_context.Espacios, "IdEspacio", "NombreEspacio", horario.IdEspacio);
                return View(horario);
            }

            // Validar fechas (asegurar que la fecha de inicio es antes que la fecha de fin)
            if (horario.FechaInicio >= horario.FechaFin)
            {
                TempData["ErrorMessage"] = "La fecha de inicio debe ser menor que la fecha de fin.";
                ViewData["IdActividad"] = new SelectList(_context.Actividades, "IdActividad", "NombreActividad", horario.IdActividad);
                ViewData["IdEspacio"] = new SelectList(_context.Espacios, "IdEspacio", "NombreEspacio", horario.IdEspacio);
                return View(horario);
            }

            // Verificar si el espacio ya está ocupado en el rango de horas seleccionado
            var espacioOcupado = await _context.Horarios
                .Where(h => h.IdEspacio == horario.IdEspacio)
                .AnyAsync(h =>
                    // La hora de inicio de la nueva actividad no se solapa con la de otras
                    (horario.FechaInicio.HasValue && h.FechaFin.HasValue && horario.FechaInicio.Value.TimeOfDay < h.FechaFin.Value.TimeOfDay) &&
                    // La hora de fin de la nueva actividad no se solapa con la de otras
                    (horario.FechaFin.HasValue && h.FechaInicio.HasValue && horario.FechaFin.Value.TimeOfDay > h.FechaInicio.Value.TimeOfDay));

            if (espacioOcupado)
            {
                TempData["ErrorMessage"] = "El espacio seleccionado ya está ocupado en el rango de tiempo seleccionado. Por favor, seleccione otro espacio.";
                ViewData["IdActividad"] = new SelectList(_context.Actividades, "IdActividad", "NombreActividad", horario.IdActividad);
                ViewData["IdEspacio"] = new SelectList(_context.Espacios, "IdEspacio", "NombreEspacio", horario.IdEspacio);
                return View(horario);
            }

            // Si todo es válido, guardar el nuevo horario
            _context.Add(horario);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        // GET: Horarios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var horario = await _context.Horarios.FindAsync(id);
            if (horario == null)
            {
                return NotFound();
            }

            ViewData["IdActividad"] = new SelectList(_context.Actividades, "IdActividad", "NombreActividad", horario.IdActividad);
            ViewData["IdEspacio"] = new SelectList(_context.Espacios, "IdEspacio", "NombreEspacio", horario.IdEspacio);

            return View(horario);
        }

        // POST: Horarios/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdHorario,FechaInicio,FechaFin,IdActividad,IdEspacio")] Horario horario)
        {
            if (id != horario.IdHorario)
            {
                return NotFound();
            }

            // Validación 1: Campos requeridos
            if (horario.IdActividad == null || horario.IdEspacio == null)
            {
                TempData["ErrorMessage"] = "Todos los campos deben estar completos. Seleccione una actividad y un espacio.";
                return await LoadEditView(horario); // Método auxiliar (definido abajo)
            }

            // Validación 2: FechaInicio < FechaFin
            if (horario.FechaInicio >= horario.FechaFin)
            {
                TempData["ErrorMessage"] = "La fecha de inicio debe ser menor que la fecha de fin.";
                return await LoadEditView(horario);
            }

            // Validación 3: Espacio no solapado (excluyendo el horario actual)
            var espacioOcupado = await _context.Horarios
                .Where(h => h.IdEspacio == horario.IdEspacio && h.IdHorario != horario.IdHorario) // 
                .AnyAsync(h =>
                    (horario.FechaInicio < h.FechaFin && horario.FechaFin > h.FechaInicio) || // 
                    (horario.FechaInicio == h.FechaInicio && horario.FechaFin == h.FechaFin)  // 
                );

            if (espacioOcupado)
            {
                TempData["ErrorMessage"] = "El espacio seleccionado ya está ocupado en el rango de tiempo seleccionado. Por favor, seleccione otro espacio o ajuste las fechas.";
                return await LoadEditView(horario);
            }

            // Si todo es válido, guardar cambios
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(horario);
                    await _context.SaveChangesAsync();
                    ViewBag.RespuestasSuscriptores = await NotificarSuscriptores(id);
                    Console.WriteLine(await NotificarSuscriptores(id));
                    return RedirectToAction("Index");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HorarioExists(horario.IdHorario))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            return await LoadEditView(horario);
        }

        // Método auxiliar para cargar ViewData (evita repetir código)
        private async Task<IActionResult> LoadEditView(Horario horario)
        {
            ViewData["IdActividad"] = new SelectList(_context.Actividades, "IdActividad", "NombreActividad", horario.IdActividad);
            ViewData["IdEspacio"] = new SelectList(_context.Espacios, "IdEspacio", "NombreEspacio", horario.IdEspacio);
            return View(horario);
        }

        // GET: Horarios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var horario = await _context.Horarios
                .Include(h => h.IdActividadNavigation)
                .Include(h => h.IdEspacioNavigation)
                .FirstOrDefaultAsync(m => m.IdHorario == id);
            if (horario == null)
            {
                return NotFound();
            }

            return View(horario);
        }

        // POST: Horarios/Delete/5
        // POST: Horarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var horario = await _context.Horarios.FindAsync(id);
            if (horario != null)
            {
                await _context.Entry(horario).Collection(h => h.UsuariosConfirmados).LoadAsync();
                horario.UsuariosConfirmados.Clear();
                _context.Horarios.Remove(horario);
                await _context.SaveChangesAsync();
                Console.WriteLine(await NotificarSuscriptores(id));
            }
            return RedirectToAction(nameof(Index));
        }


        private bool HorarioExists(int id)
        {
            return _context.Horarios.Any(e => e.IdHorario == id);
        }

    }
}