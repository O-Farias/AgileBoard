using Microsoft.EntityFrameworkCore;
using AgileBoard.API.Models;

namespace AgileBoard.API.Data
{
    public class AgileBoardContext : DbContext
    {
        public AgileBoardContext(DbContextOptions<AgileBoardContext> options)
            : base(options)
        {
        }

        public DbSet<Board> Boards { get; set; }
    }
}