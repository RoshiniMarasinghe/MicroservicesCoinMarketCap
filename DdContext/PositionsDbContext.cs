using Microsoft.EntityFrameworkCore;
using PositionsService.Models;

namespace PositionsService.Data
{
    public class PositionsDbContext : DbContext
    {
        public PositionsDbContext(DbContextOptions<PositionsDbContext> options) : base(options) { }

        public DbSet<FinancialPosition> FinancialPositions { get; set; }
    }
}
