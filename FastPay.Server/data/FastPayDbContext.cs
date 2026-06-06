using Microsoft.EntityFrameworkCore;

namespace FastPay.Server.Data;

public class FastPayDbContext(DbContextOptions<FastPayDbContext> options)
: DbContext (options) 
{}

