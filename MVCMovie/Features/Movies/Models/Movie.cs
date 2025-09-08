using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MvcMovie.Features.Movies.Models;

public class Movie
{
    public int          Id { get; set; }
    [Required]
    [StringLength(60, MinimumLength = 2)] // validation rules for title
    // NOTE, attribute can be in a single line: [StringLength(60, MinimumLength = 2), Required]
    public string?      Title { get; set; } // ? ambiguous because string can be null

    [Display(Name = "Release Date")]
    [DataType(DataType.Date)] // declare datatype of date
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime     ReleaseDate { get; set; }

    // set validations using RegEx
    [Required]
    [RegularExpression(@"^[A-Z]+[a-zA-Z\s]*$")]
    [StringLength(30)]
    public string?      Genre { get; set; } // ? potential null operator
    [Column(TypeName = "decimal(18, 2)")]

    [Range(1, 100)]
    [DataType(DataType.Currency)]
    public decimal      Price { get; set; }
    
    // add Rating property
    [Required]
    [RegularExpression(@"^[A-Z]+[a-zA-Z0-9""'\s-]*$")]
    [StringLength(5)]

    public string?      Rating { get; set; }
}