using WebApplication1.Dtos;

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
        var group=app.MapGroup("/games");

        //GET /games
        group.MapGet("/", () => games);

        //GET /games/1
        group.MapGet("/{id}", (int id) =>
        {
            var game = games.Find(game => game.Id == id);

            return game is null ? Results.NotFound() : Results.Ok(game);
        });

        //POST /games
        group.MapPost("/", (CreateGameDto newGame) =>
        {
            if (string.IsNullOrEmpty(newGame.Name))
            {
                return Results.BadRequest("Name is required");
            }
            GameDto game = new(
                games.Count + 1,
                newGame.Name,
                newGame.Genre,
                newGame.Price,
                newGame.ReleaseDate
            );
            games.Add(game);
            return Results.CreatedAtRoute(GetGameEndpointName, new { id = game.Id }, game);
        });

        //Put /games/{id}
        group.MapPut("/{id}", (int id, UpdateGameDto updatedGame) =>
        {
            var index = games.FindIndex(game => game.Id == id);

            if (index == -1)
            {
                return Results.NotFound();
            }

            games[index] = new GameDto(
                id,
                updatedGame.Name,
                updatedGame.Genre,
                updatedGame.Price,
                updatedGame.ReleaseDate
            );
            return Results.NoContent();
        });

        //Delete /games/{id}
        group.MapDelete("/{id}", (int id) =>
        {
            games.RemoveAll(game => game.Id == id);
            return Results.NoContent();
        });

        


        app.MapGet("/", () => "Hello World!");
    }
}
