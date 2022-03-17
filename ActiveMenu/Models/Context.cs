using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace ActiveMenu.Model
{

     class Context : DbContext
    {
      
        public Context() : base("ActiveMenu")
        {
            Database.SetInitializer<Context>(new MigrateDatabaseToLatestVersion<Context, ActiveMenu.Migrations.Configuration>());
            //Database.SetInitializer<Context>(new DropCreateDatabaseIfModelChanges<Context>());
        }
     

        public DbSet<category> categories { get; set; }
        public DbSet<restaurant> restaurants { get; set; }
        public DbSet<Item> items { get; set; }
        public DbSet<order> orders { get; set; }
        public DbSet<orderDetail> orderDetails { get; set; }

        
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

    }


    public class category
    {
        [Key]
       
        public Guid categoryID { get; set; }
        public string title { get; set; }
        public string culomn { get; set;  }
        public string parentID { get; set; }
        public string imageAddress { get; set; }

        [ForeignKey("restaurant")]
        public Guid restaurantID { get; set; }
        [JsonIgnore]
        public virtual restaurant restaurant { get; set; }

        public ICollection<Item> items { get; set; }
    }

    public class restaurant
    {
        [Key]
        
        public Guid restaurantID { get; set; }
        public string logo { get; set; }
        public int orderNumberBegin { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string totem { get; set; }
        public string phone { get; set; }
        public string mobile { get; set; }
        public string instagram { get; set; }
        public string whatsapp { get; set; }
        public string imageAddress { get; set; }
        public string Address { get; set; }

        public ICollection<category> Categories { get; set; }

        public ICollection<order> orders { get; set; }
    }

    public class order
    {
        [Key]
        public Guid orderID { get; set; }
        public int orderNumber { get; set; }
        public int isRefine { get; set; }
        public int minutToAdd { get; set; }
        public double minutPassed { get; set; }
        public string us { get; set; }
        public string peigiry { get; set; }
        public DateTime orderTime { get; set; }
        public int payment { get; set; }
        public int status { get; set; }

        [ForeignKey("restaurant")]
        public Guid restaurantID { get; set; }
        [JsonIgnore]
        public virtual restaurant restaurant { get; set; }
        

    }
    public class orderDetail
    {
        [Key]
        public Guid orderDetailID { get; set; }
        public int count { get; set; }
        public Guid orderID { get; set; }

        [ForeignKey("item")]
        public Guid itemID { get; set; }
        public virtual Item item { get; set; }

        
    }
    public class Item
    {
        [Key]
       
        public Guid itemID { get; set; }
        public string imageList { get; set; }
        public string title { get; set; }
        public string desctiption { get; set; }
        public string ingredient { get; set; }
        public long price { get; set; }

        [ForeignKey("category")]
        public Guid categoryID { get; set; }
        [JsonIgnore]
        public virtual category category { get; set; }


        public ICollection<orderDetail> orderDetails { get; set; }
    }

    


}
