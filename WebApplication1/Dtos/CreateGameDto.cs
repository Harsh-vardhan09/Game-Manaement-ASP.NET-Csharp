using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Dtos;

public  record CreateGameDto(
    [Required][StringLength(50)] string Name,
    [Required][Range(1,50)] int GenreId,
    [Required][Range(1,100)] decimal Price,
    [Required] DateOnly ReleaseDate
);
