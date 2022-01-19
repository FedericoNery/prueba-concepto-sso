using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace SSOConcepto.Data
{
    //public class ApplicationDbContext : IdentityDbContext<User> -> Si quiero agregarle mas properties
    // User debe heredar de IdentityUser
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions <ApplicationDbContext> options): base (options)
        {

        }
    }
}
