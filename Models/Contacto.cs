using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Models;

public class Contacto
{
  [Required]
  public string Name { get; set; } = "";

  [Required]
  public string Lastname { get; set; } = "";

  [Required]
  public string Email { get; set; } = "";


  [Required]
  public string Comments { get; set; } = "";

  [Required]
  [StringLength(10, MinimumLength = 10)]
  [RegularExpression(@"^\d{10}$")]
  public string PhoneNumber { get; set; } = "";
}
