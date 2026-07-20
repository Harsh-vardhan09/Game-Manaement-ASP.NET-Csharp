using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Data;

public class GameStoreContext(DbContextOptions<GameStoreContext> options):DbContext(options)
{
    
}
