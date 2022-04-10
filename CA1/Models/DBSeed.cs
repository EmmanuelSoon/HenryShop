using System;
using System.Security.Cryptography;
using System.Text;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;


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
            SeedProduct();
            SeedUser();
            SeedInventory();

        }

        public void SeedProduct()
        {
            dbContext.Add(new Product
            {

                Name = ".NET Charts",
                Desc = "Brings powerful charting capabilities to your .NET applications",
                Img = "./img/chart.png",
                Price = 99,
                DownLoadLink = "www.12345.com"
            });

            dbContext.Add(new Product
            {

                Name = ".NET PayPal",
                Desc = "Integrate your .NET apps with PayPal the easy way!",
                Img = "./img/top.png",
                Price = 69,
                DownLoadLink = "www.23456.com"
            });

            dbContext.Add(new Product
            {

                Name = ".NET ML",
                Desc = "Supercharged .NET machine learning libraries",
                Img = "./img/bed.png",
                Price = 299,
                DownLoadLink = "www.34567.com"
            });

            dbContext.Add(new Product
            {

                Name = ".NET Analytics",
                Desc = "Performs data mining and analytics easily in .NET",
                Img = "./img/chart.png",
                Price = 299,
                DownLoadLink = "45678.com"
            });

            dbContext.Add(new Product
            {

                Name = ".NET Logger",
                Desc = "Logs and aggregates events easily in your .NET apps",
                Img = "./img/top.png",
                Price = 49,
                DownLoadLink = "56789.com"
            });

            dbContext.Add(new Product
            {

                Name = ".NET Numerics",
                Desc = "Powerful numerical methods for your .NET simulations",
                Img = "./img/top.png",
                Price = 199,
                DownLoadLink = "56789.com"
            });

            dbContext.Add(new Product
            {

                Name = ".NET forms",
                Desc = "Create forms in a click, simple and quick",
                Img = "./img/top.png",
                Price = 99,
                DownLoadLink = "56789.com"
            });

            dbContext.Add(new Product
            {

                Name = ".NET library",
                Desc = "Need to know more about .NET? here is your one stop solution",
                Img = "./img/top.png",
                Price = 99,
                DownLoadLink = "56789.com"
            });

            dbContext.Add(new Product
            {

                Name = ".NET UI",
                Desc = "Set of controls and features for your platform",
                Img = "./img/top.png",
                Price = 9,
                DownLoadLink = "56789.com"
            });

            dbContext.Add(new Product
            {

                Name = ".NET Runtime",
                Desc = "Analytical tool to check on your app's performance",
                Img = "./img/top.png",
                Price = 69,
                DownLoadLink = "56789.com"
            });

            dbContext.Add(new Product
            {

                Name = ".NET Image Processing",
                Desc = "a computer and artificial intelligence library for image processing",
                Img = "./img/top.png",
                Price = 119,
                DownLoadLink = "56789.com"
            });

            dbContext.Add(new Product
            {

                Name = ".NET Unity",
                Desc = "a development tool to create games for consoles and PCs",
                Img = "./img/top.png",
                Price = 39,
                DownLoadLink = "56789.com"
            });

            dbContext.Add(new Product
            {

                Name = ".NET Blazor",
                Desc = "a framework to enable developers to create web apps",
                Img = "./img/top.png",
                Price = 19,
                DownLoadLink = "56789.com"
            });




            dbContext.SaveChanges();
        }

        public void SeedUser()
        {
            // get a hash algorithm object
            HashAlgorithm sha = SHA256.Create();

            string username = "user1";
            string password = "password";
            string combo = username + password;
            byte[] hash = sha.ComputeHash(Encoding.UTF8.GetBytes(combo));

            ShopCart cart = new ShopCart();
            dbContext.Add(new User
            {
                UserName = username,
                PassHash = hash,
                Firstname = "Mary",
                Lastname = "Lamb",
                shopcart = cart
            });

            string username2 = "user2";
            string password2 = "password2";
            string combo2 = username2 + password2;
            byte[] hash2 = sha.ComputeHash(Encoding.UTF8.GetBytes(combo2));

            ShopCart cart2 = new ShopCart();
            dbContext.Add(new User
            {
                UserName = username2,
                PassHash = hash2,
                Firstname = "Henry",
                Lastname = "Pig",
                shopcart = cart2
            });

            dbContext.SaveChanges();

        }

        public void SeedInventory()
        {
            //id, activation code, and product id

            Product product = dbContext.Products.FirstOrDefault(x => x.Name == ".NET Charts");
            if (product != null)
            {
                for (int i = 0; i < 10; i++)
                {
                    InventoryRecord record = new InventoryRecord
                    {
                        ActivationId = Guid.NewGuid() //using static method to call instead of default constructor because if not the GUID reverts to default all 0s.
                    };

                    product.InventoryRecords.Add(record);

                }
            }

            Product product2 = dbContext.Products.FirstOrDefault(x => x.Name == ".NET PayPal");
            if (product != null)
            {
                for (int i = 0; i < 5; i++)
                {
                    InventoryRecord record2 = new InventoryRecord
                    {
                        ActivationId = Guid.NewGuid() //using static method to call instead of default constructor because if not the GUID reverts to default all 0s.
                    };

                    product2.InventoryRecords.Add(record2);

                }
            }

            Product product3 = dbContext.Products.FirstOrDefault(x => x.Name == ".NET ML");
            if (product3 != null)
            {
                for (int i = 0; i < 1; i++)
                {
                    InventoryRecord record3 = new InventoryRecord
                    {
                        ActivationId = Guid.NewGuid() //using static method to call instead of default constructor because if not the GUID reverts to default all 0s.
                    };

                    product3.InventoryRecords.Add(record3);

                }
            }

            Product product4 = dbContext.Products.FirstOrDefault(x => x.Name == ".NET Analytics");
            if (product4 != null)
            {
                for (int i = 0; i < 2; i++)
                {
                    InventoryRecord record4 = new InventoryRecord
                    {
                        ActivationId = Guid.NewGuid() //using static method to call instead of default constructor because if not the GUID reverts to default all 0s.
                    };

                    product4.InventoryRecords.Add(record4);

                }
            }

            Product product5 = dbContext.Products.FirstOrDefault(x => x.Name == ".NET Logger");
            if (product5 != null)
            {
                for (int i = 0; i < 10; i++)
                {
                    InventoryRecord record5 = new InventoryRecord
                    {
                        ActivationId = Guid.NewGuid() //using static method to call instead of default constructor because if not the GUID reverts to default all 0s.
                    };

                    product5.InventoryRecords.Add(record5);

                }
            }

            Product product6 = dbContext.Products.FirstOrDefault(x => x.Name == ".NET Numerics");
            if (product6 != null)
            {
                for (int i = 0; i < 4; i++)
                {
                    InventoryRecord record6 = new InventoryRecord
                    {
                        ActivationId = Guid.NewGuid() //using static method to call instead of default constructor because if not the GUID reverts to default all 0s.
                    };

                    product6.InventoryRecords.Add(record6);

                }
            }




            dbContext.SaveChanges();
        }
        public void AddInventory(string productName, int qty)
        {
            Product product = dbContext.Products.FirstOrDefault(x => x.Name == productName);
            if (product != null)
            {
                for (int i = 0; i < qty; i++)
                {
                    InventoryRecord record = new InventoryRecord()
                    {
                        ActivationId = Guid.NewGuid()
                    };



                    product.InventoryRecords.Add(record);
                }
                dbContext.SaveChanges();
            }
            else
            {
                Debug.WriteLine("{0} is not in the Product list", productName);
            }
        }
    }


}