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
        public DbSet<place> places { get; set; }
        public DbSet<userMessage> userMessages { get; set; }
        public DbSet<emailBlog> emailBlogs { get; set; }
        public DbSet<mavad> mavads { get; set; }
        public DbSet<mavadItem> mavadItems { get; set; }
        public DbSet<user> users { get; set; }
        public DbSet<userRole> userRoles { get; set; }
        public DbSet<orderDetailMavad> orderDetailMavads { get; set; }
        



        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

    }


    public class category
    {
        [Key]
       
        public Guid categoryID { get; set; }
        public int isVisible { get; set; }
        public int priority { get; set; }
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
    

    public class mavad
    {
        [Key]

        public Guid mavadID { get; set; }
        public Guid restaurantID { get; set; }
        public string title { get; set; }
        public decimal mojoodi { get; set; }
        public string type { get; set; }
        public decimal Calories { get; set; }
        public decimal Proteins { get; set; }
        public decimal Carbohydrates { get; set; }
        public decimal Fats { get; set; }
    }

    public class mavadItem
    {
        [Key]

        public Guid mavaditemID { get; set; }
        public Guid mavadID { get; set; }
        public Guid itemID { get; set; }
        public decimal amount { get; set; }
      
        public string  description { get; set; }
    }

    public class user
    {
        [Key]

        public Guid userID { get; set; }
        public string   password { get; set; }
        public string    username { get; set; }
        public string token { get; set; }
    }
    public class userRole
    {
        [Key]

        public Guid useroleID { get; set; }
        public Guid restuantID { get; set; }
        public Guid userID { get; set; }
        public string roleID { get; set; }
       
    }
   



    public class restaurant
    {
        [Key]
        
        public Guid restaurantID { get; set; }
        public string meyar { get; set; }
        public int vat { get; set; }
        public string  garson { get; set; }
        public int pardakht { get; set; }
        public int sabad { get; set; }
        public string logo { get; set; }
        public int orderNumberBegin { get; set; }
        public string title { get; set; }
        public string zarin { get; set; }
        public string description { get; set; }
        public string totem { get; set; }
        public string phone { get; set; }
        public string mobile { get; set; }
        public string instagram { get; set; }
        public string whatsapp { get; set; }
        public string imageAddress { get; set; }
        public string Address { get; set; }
        public string whatsappPhone { get; set; }
        public string email { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public DateTime  oppening { get; set; }
        public DateTime colsing { get; set; }
        public string brandName { get; set; }

        public ICollection<category> Categories { get; set; }

        public ICollection<order> orders { get; set; }
        public ICollection<place> places { get; set; }
        
    }


    public class place
    {
        [Key]
        public Guid placeID { get; set; }
        public string title { get; set; }
        public int calling { get; set; }
        public string message { get; set; }
        public string url { get; set; }
        public DateTime date { get; set; }
        public string image { get; set; }


        [ForeignKey("restaurant")]
        public Guid restaurantID { get; set; }
        [JsonIgnore]
        public virtual restaurant restaurant { get; set; }

    }

    public class promoCode
    {
        [Key]
        public Guid promoCodeID { get; set; }
        public string title { get; set; }
        public decimal price { get; set; }
        public int percent { get; set; }
    }
    public class order
    {
        [Key]
        public Guid orderID { get; set; }
        public string auth { get; set; }
        public int orderNumber { get; set; }
       
        public int final { get; set; }
        public int isRefine { get; set; }
        public int minutToAdd { get; set; }
        public double minutPassed { get; set; }
        public double price { get; set; }
        public double paymentLink { get; set; }
        public string us { get; set; }
        public string peigiry { get; set; }
        public DateTime orderTime { get; set; }
        public int payment { get; set; }
        public int status { get; set; }

      
        public Guid placeID { get; set; }
      
        public virtual string place { get; set; }


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
        public double price { get; set; }
        public string description { get; set; }
        [ForeignKey("item")]
        public Guid itemID { get; set; }
        public int final { get; set; }
        public virtual Item item { get; set; }

        
    }
    public class orderDetailMavad
    {
        [Key]
        public Guid odmID { get; set; }
        public Guid mavaID { get; set; }
        public Guid orderDetailID { get; set; }
    }
    public class Item
    {
        [Key]
       
        public Guid itemID { get; set; }
        public string imageList { get; set; }
        public decimal calory { get; set; }
        public string title { get; set; }
        public string desctiption { get; set; }
        public string ingredient { get; set; }
        public double price { get; set; }
        public int isDisabled { get; set; }
        public decimal Calories { get; set; }
        public decimal Proteins { get; set; }
        public string Instruction { get; set; }
        public decimal Carbohydrates { get; set; }
        public decimal Fats { get; set; }
        public int time { get; set; }

        [ForeignKey("category")]
        public Guid categoryID { get; set; }
        [JsonIgnore]
        public virtual category category { get; set; }


        public ICollection<orderDetail> orderDetails { get; set; }

    }
    public class userMessage
    {
        [Key]

        public Guid messageID { get; set; }
        public string email { get; set; }
        public string name { get; set; }
        public string text { get; set; }
    }

    public class emailBlog
    {
        [Key]

        public Guid messageID { get; set; }
        public string email { get; set; }
      
    }




}
