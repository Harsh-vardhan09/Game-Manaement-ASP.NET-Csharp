using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Dtos;
using WebApplication1.Models;

namespace WebApplication1.Endpoints;

public static class GamesEndpoints
{
    const string GetGameEndpointName = "GetGame";
    private static readonly List<GameDto> games = [
        new (1, "street fighter 2","fighting",19.99M,new DateOnly(1992,7,15)),
        new (2, "super mario bros","platformer",14.99M,new DateOnly(1985,9,13)),
        new (3, "the legend of zelda","adventure",24.99M,new DateOnly(1986,2,21)),
        new (4, "pac-man","arcade",9.99M,new DateOnly(1980,5,22))
    ];
    public static void MapGamesEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/games");

        //GET /games
        group.MapGet("/", async (GameStoreContext dbContext) => await dbContext.Games.Include(game => game.Genre).Select(game => new GameDto(
                    game.Id,
                    game.Name,
                    game.Genre!.Name,
                    game.Price,
                    game.ReleaseDate
        ))
        .AsNoTracking()
        .ToListAsync()
        );

        //GET /games/1
        group.MapGet("/{id}", async (int id, GameStoreContext dbContext) =>
        {
            var game = await dbContext.Games.FindAsync(id);

            return game is null ? Results.NotFound() : Results.Ok(
                new GameDetailsDto(
                    game.Id,
                    game.Name,
                    game.GenreId,
                    game.Price,
                    game.ReleaseDate
                )
            );
        }).WithName(GetGameEndpointName);

        //POST /games
        group.MapPost("/", async (CreateGameDto newGame, GameStoreContext dbContext) =>
        {
            Game game = new()
            {
                Name = newGame.Name,
                GenreId = newGame.GenreId,
                Price = newGame.Price,
                ReleaseDate = newGame.ReleaseDate

            };

            dbContext.Games.Add(game);
            await dbContext.SaveChangesAsync();

            GameDetailsDto gameDto = new(
                game.Id,
                game.Name,
                game.GenreId,
                game.Price,
                game.ReleaseDate
            );
            return Results.CreatedAtRoute(GetGameEndpointName, new { id = gameDto.Id }, gameDto);
        });

        //Put /games/{id}
        group.MapPut("/{id}", async(int id, UpdateGameDto updatedGame,GameStoreContext dbContext) =>
        {
            var existingGame=await dbContext.Games.FindAsync(id);

            if (existingGame is null)
            {
                return Results.NotFound();
            }

            existingGame.Name=updatedGame.Name;
            existingGame.GenreId=updatedGame.GenreId;
            existingGame.Price=updatedGame.Price;
            existingGame.ReleaseDate=updatedGame.ReleaseDate;
            
            await dbContext.SaveChangesAsync();

            return Results.NoContent();
        });

        //Delete /games/{id}
        group.MapDelete("/{id}", async(int id,GameStoreContext dbContext) =>
        {
            await dbContext.Games.Where(Game=>Game.Id==id).ExecuteDeleteAsync();
            return Results.NoContent();
        });

        app.MapGet("/", () => "Hello World!");
    }
}
