using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GYM_ITM.Models;

public partial class Espacio
{
    public int IdEspacio { get; set; }

    [Required(ErrorMessage = "El Espacio es obligatorio.")]
    public string? NombreEspacio { get; set; }


      [Required]
    [Range(1, int.MaxValue, ErrorMessage = "El límite de disponibilidad debe ser mayor que 0.")]
    public int? LimiteDisponibilidad { get; set; }

    public virtual ICollection<Horario> Horarios { get; set; } = new List<Horario>();

}
