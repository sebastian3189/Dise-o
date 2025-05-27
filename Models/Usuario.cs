using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace GYM_ITM.Models;

[Index(nameof(Correo), IsUnique = true)]
public partial class Usuario
{
    public int IdUsuario { get; set; }

    [Required(ErrorMessage = "El nombre es obligatorio.")]
    public string? Nombre { get; set; }

    [Required(ErrorMessage = "El apellido es obligatorio.")]
    public string? Apellido { get; set; }

    [Required(ErrorMessage = "El correo es obligatorio.")]
    [EmailAddress]
    
    public string? Correo { get; set; }

    [Required(ErrorMessage = "El telefono es obligatorio.")]
    public string? Telefono { get; set; }

    public virtual ICollection<Horario> IdHorarios { get; set; } = new List<Horario>();
}
