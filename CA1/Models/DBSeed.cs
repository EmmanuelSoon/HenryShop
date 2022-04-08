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
                prodId = 1,
                Name = ".NET charts",
                Desc = "blah blah blah",
                Img = "./img/chart.png",
                Price = 12
            });

            dbContext.Add(new Product
            {
                prodId = 2,
                Name = ".NET Tops",
                Desc = "blah blah blah",
                Img = "./img/top.png",
                Price = 123
            });

            dbContext.Add(new Product
            {
                prodId = 3,
                Name = ".NET beds",
                Desc = "blah blah blah",
                Img = "./img/bed.png",
                Price = 1234
            });

            dbContext.Add(new Product
            {
                prodId = 4,
                Name = ".NET chart2",
                Desc = "blah blah blah",
                Img = "./img/chart.png",
                Price = 12345
            });

            dbContext.Add(new Product
            {
                prodId = 5,
                Name = ".NET tops2",
                Desc = "blah blah blah",
                Img = "./img/top.png",
                Price = 12346
            });


            dbContext.SaveChanges();
        }

    }
}
