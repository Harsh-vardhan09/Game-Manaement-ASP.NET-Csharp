namespace WebApplication1.Dtos;

// A DTO is an contract between client and server since it represents 
// a shared agreement about how data will be transferred and used.

public record GameDetailsDto(
    int Id,
    string Name,
    int GenreId,
    decimal Price,
    DateOnly ReleaseDate
);