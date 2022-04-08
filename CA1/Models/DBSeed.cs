using System.Security.Cryptography;
using System.Text;

namespace CA1.Models
{
    public class DBSeed
    {
        private DBContext dbContext;

        public DBSeed(DBContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void Seed()
        {
            dbContext.Add(new Product
            {

                Name = ".NET charts",
                Desc = "blah blah blah",
                Img = "./img/chart.png",
                Price = 12
            });

            dbContext.Add(new Product
            {

                Name = ".NET Tops",
                Desc = "blah blah blah",
                Img = "./img/top.png",
                Price = 123
            });

            dbContext.Add(new Product
            {

                Name = ".NET beds",
                Desc = "blah blah blah",
                Img = "./img/bed.png",
                Price = 1234
            });

            dbContext.Add(new Product
            {

                Name = ".NET chart2",
                Desc = "blah blah blah",
                Img = "./img/chart.png",
                Price = 12345
            });

            dbContext.Add(new Product
            {

                Name = ".NET tops2",
                Desc = "blah blah blah",
                Img = "./img/top.png",
                Price = 12346
            });

            // get a hash algorithm object
            //HashAlgorithm sha = SHA256.Create();
            //byte[] hash = sha.ComputeHash(Encoding.UTF8.GetBytes("password1"));

            dbContext.Add(new User
            {

                UserName = "user1",
                PassHash = "password",
            });


            dbContext.SaveChanges();
        }

    }
}
