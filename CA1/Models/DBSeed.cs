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
                Img = "/img/Charts.jpg",
                Price = 99,
                DownLoadLink = "https://dotnetcharting.com/"
            });

            dbContext.Add(new Product
            {

                Name = ".NET PayPal",
                Desc = "Integrate your .NET apps with PayPal the easy way!",
                Img = "/img/Paypal.jpg",
                Price = 69,
                DownLoadLink = "https://paypal.github.io/PayPal-NET-SDK/"
            });

            dbContext.Add(new Product
            {

                Name = ".NET ML",
                Desc = "Supercharged .NET machine learning libraries",
                Img = "/img/Machine.jpg",
                Price = 299,
                DownLoadLink = "https://dotnet.microsoft.com/en-us/learn/ml-dotnet/get-started-tutorial/intro"
            });

            dbContext.Add(new Product
            {

                Name = ".NET Analytics",
                Desc = "Performs data mining and analytics easily in .NET",
                Img = "/img/Analytics.jpg",
                Price = 299,
                DownLoadLink = "https://segment.com/docs/connections/sources/catalog/libraries/server/net/"
            });

            dbContext.Add(new Product
            {

                Name = ".NET Logger",
                Desc = "Logs and aggregates events easily in your .NET apps",
                Img = "/img/Logger.jpg",
                Price = 49,
                DownLoadLink = "https://www.nuget.org/packages/Microsoft.Extensions.Logging/"
            });

            dbContext.Add(new Product
            {

                Name = ".NET Numerics",
                Desc = "Powerful numerical methods for your .NET simulations",
                Img = "/img/Numerics.png",
                Price = 199,
                DownLoadLink = "https://numerics.mathdotnet.com/Packages.html"
            });

            dbContext.Add(new Product
            {

                Name = ".NET Forms",
                Desc = "Create forms in a click, simple and quick",
                Img = "/img/Forms.jpg",
                Price = 99,
                DownLoadLink = "https://docs.microsoft.com/en-us/dotnet/desktop/winforms/?view=netdesktop-6.0"
            });

            dbContext.Add(new Product
            {

                Name = ".NET Library",
                Desc = "Need to know more about .NET? here is your one stop solution",
                Img = "/img/Library.jpg",
                Price = 99,
                DownLoadLink = "https://www.nlb.gov.sg/"
            });

            dbContext.Add(new Product
            {

                Name = ".NET UI",
                Desc = "Set of controls and features for your platform",
                Img = "/img/UI.png",
                Price = 9,
                DownLoadLink = "https://en.wikipedia.org/wiki/User_interface"
            });

            dbContext.Add(new Product
            {

                Name = ".NET Runtime",
                Desc = "Analytical tool to check on your app's performance",
                Img = "/img/RunTime.jpg",
                Price = 69,
                DownLoadLink = "https://dotnet.microsoft.com/en-us/download/dotnet-framework"
            });

            dbContext.Add(new Product
            {

                Name = ".NET Image Processing",
                Desc = "A computer and artificial intelligence library for image processing",
                Img = "/img/Images.jpg",
                Price = 119,
                DownLoadLink = "https://en.wikipedia.org/wiki/Artificial_intelligence"
            });

            dbContext.Add(new Product
            {

                Name = ".NET Unity",
                Desc = "A development tool to create games for consoles and PCs",
                Img = "/img/unityweb.png",
                Price = 39,
                DownLoadLink = "https://docs.unity3d.com/Manual/index.html"
            });

            dbContext.Add(new Product
            {

                Name = ".NET Blazor",
                Desc = "A framework to enable developers to create web apps",
                Img = "/img/Blazorweb.png",
                Price = 19,
                DownLoadLink = "https://dotnet.microsoft.com/en-us/apps/aspnet/web-apps/blazor"
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
            //Updated to add wishlist
            WishList wishList = new WishList();

            dbContext.Add(new User
            {
                UserName = username,
                PassHash = hash,
                Firstname = "Mary",
                Lastname = "Lamb",
                shopcart = cart,
                wishlist = wishList
            });

            string username2 = "user2";
            string password2 = "password2";
            string combo2 = username2 + password2;
            byte[] hash2 = sha.ComputeHash(Encoding.UTF8.GetBytes(combo2));

            ShopCart cart2 = new ShopCart();
            WishList wishList2 = new WishList();
            dbContext.Add(new User
            {
                UserName = username2,
                PassHash = hash2,
                Firstname = "Henry",
                Lastname = "Pig",
                shopcart = cart2,
                wishlist = wishList2
            });

            AddUser("user3", "password3", sha, "John", "Doe");
            AddUser("user4", "password4", sha, "Alyssa", "Lim");
            AddUser("user5", "password5", sha, "Gavin", "Gaw");
            AddUser("user6", "password6", sha, "Youcheng", "Li");
            AddUser("user7", "password7", sha, "Anandeeswaran", "Venkatachalam");
            AddUser("user8", "password8", sha, "Hein", "Lin Zaw");
            AddUser("user9", "password9", sha, "Yoon", "Mie Mie Aung");
            AddUser("user10", "password10", sha, "Emmanuel", "Soon");

            dbContext.SaveChanges();

        }

        public void AddUser(string username, string password, HashAlgorithm sha, string firstname, string lastname)
        {
            ShopCart cart = new ShopCart();
            WishList wishlist = new WishList();
            string combo = username + password;
            byte[] hash = sha.ComputeHash(Encoding.UTF8.GetBytes(combo));
            dbContext.Add(new User
            {
                UserName = username,
                PassHash = hash,
                Firstname = firstname,
                Lastname = lastname,
                shopcart = cart,
                wishlist = wishlist
            });
        }

        public void SeedInventory()
        {
            //id, activation code, and product id
            AddInventory(".NET Charts", 10);
            AddInventory(".NET PayPal", 10);
            AddInventory(".NET ML", 2);
            AddInventory(".NET Analytics", 10);
            AddInventory(".NET Logger", 12);
            AddInventory(".NET Numerics", 5);
            AddInventory(".NET Forms", 5);
            AddInventory(".NET Library", 5);
            AddInventory(".NET UI", 5);
            AddInventory(".NET Runtime", 5);
            AddInventory(".NET Image Processing", 5);
            AddInventory(".NET Unity", 5);
            AddInventory(".NET Blazor", 5);

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