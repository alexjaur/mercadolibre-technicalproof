using MercadoLibre.TechnicalProof.ByAl3xJauregui.Core.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace MercadoLibre.TechnicalProof.ByAl3xJauregui.Core.Data.Contexts
{
    internal class TopSecretDbContext : DbContext
    {
        public TopSecretDbContext(DbContextOptions options) 
            : base(options)
        { }

        public DbSet<ReceivedMessages> ReceivedMessages { get; set; }
    }
}
