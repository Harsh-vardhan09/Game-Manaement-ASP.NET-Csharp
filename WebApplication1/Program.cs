using Microsoft.Extensions.ObjectPool;
using WebApplication1.Data;
using WebApplication1.Endpoints;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddValidation();

var connString="Data Source=GameStore.db";
builder.Services.AddSqlite<GameStoreContext>(connString);

var app = builder.Build();

app.MapGamesEndpoints();

app.MigrateDb();

app.Run();
