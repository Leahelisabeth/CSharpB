using Microsoft.EntityFrameworkCore;

namespace BeltS.Models
{
    public class BeltSContext : DbContext
    {
        public BeltSContext(DbContextOptions<BeltSContext> options) : base(options) { }
        public DbSet<User> User { get; set; }
        public DbSet<Connection> Connection { get; set; }


    }
}
//Transaction myt = _context.Transactions.Where(t => t.id = 1)
                        // .Include(tr => true.User)
                        //.SingleorDefault()
                        //myt.User.FirstName
                        //cool cool
//User myUser = _context.User.Where(U => U.id = 1)
//                          .Include(tr => User.Transactions)
//                             .SingleOrDefault();
//foreach(var trans in User.transactions){
//    trans.AddTake
//}
// List<Comments> Allcomments = _context.Messages()
// .Include(m=> comments)
// .ThenInclude(c=> comments.user)