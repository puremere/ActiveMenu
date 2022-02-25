using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        public restaurant restaurant { get; set; }

        public ICollection<Item> items { get; set; }
    }

    public class restaurant
    {
        [Key]
        
        public Guid restaurantID { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string imageAddress { get; set; }
        public string Address { get; set; }

        public ICollection<category> Categories { get; set; }
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
        public category category { get; set; }


    }

    public class order
    {
        [Key]
       
        public Guid orderID { get; set; }
        public string  items { get; set; }
        public string  counts { get; set; }
        public string paymentUrl { get; set; }
        public bool status { get; set; }
        public DateTime paymentDate { get; set; }
        public string peigiri { get; set; }

    }


}
