using ActiveMenu.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace ActiveMenu.Classes
{
    public class addItem
    {
        Context dbcontext = new Context();
        private static Random random = new Random();
        public string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        public void addGhasr()
        {


            Guid newGUid = Guid.NewGuid();
            restaurant rest = new restaurant()
            {
                Address = "کنار گذر بزرگراه یادگار امام جنوب، نبش  دستغیب، کافه قصرالشمس",
                 pardakht = 1,
                  sabad = 1,
                description = "یک محیط آرام و دلنشین برای وقت گذرانی با دوستان و ساختن یه روز خوب در کافه رستوران قصر الشمس",
                imageAddress = "/images/Restaurant/kafeGhasr.png",
                instagram = "",
                logo = "ghasr.png",
                mobile = "09300023700",
                orderNumberBegin = 100,
                password = "password",
                phone = "02122931806",
                restaurantID = newGUid,
                title = "کافه فصر اشمس",
                totem = "لحظه های شما برای ما مهم است",
                username = "ghasr",
                whatsapp = "09300023700",
                zarin = ""
            };
            dbcontext.restaurants.Add(rest);

            string catName = RandomString(10);
            category cat = new category()
            {
                categoryID = Guid.NewGuid(),
                imageAddress = "/images/Categories/" + catName + ".png",
                restaurantID = newGUid,
                title = "اسموتی ها",
                culomn = "1",

            };
            dbcontext.categories.Add(cat);

            catName = RandomString(10);
            cat = new category()
            {
                categoryID = Guid.NewGuid(),
                imageAddress = "/images/Categories/" + catName + ".png",
                restaurantID = newGUid,
                title = "دمنوش و چایی",
                culomn = "1",

            };
            dbcontext.categories.Add(cat);

            catName = RandomString(10);
            cat = new category()
            {
                categoryID = Guid.NewGuid(),
                imageAddress = "/images/Categories/" + catName + ".png",
                restaurantID = newGUid,
                title = "دسرها",
                culomn = "1",

            };
            dbcontext.categories.Add(cat);
            catName = RandomString(10);
            cat = new category()
            {
                categoryID = Guid.NewGuid(),
                imageAddress = "/images/Categories/" + catName + ".png",
                restaurantID = newGUid,
                title = " کیک ها",
                culomn = "1",

            };
            dbcontext.categories.Add(cat);
            catName = RandomString(10);
            cat = new category()
            {
                categoryID = Guid.NewGuid(),
                imageAddress = "/images/Categories/" + catName + ".png",
                restaurantID = newGUid,
                title = " بار گرم",
                culomn = "1",

            };
            dbcontext.categories.Add(cat);
            catName = RandomString(10);
            cat = new category()
            {
                categoryID = Guid.NewGuid(),
                imageAddress = "/images/Categories/" + catName + ".png",
                restaurantID = newGUid,
                title = " بار سرد",
                culomn = "1",

            };
            dbcontext.categories.Add(cat);
            catName = RandomString(10);
            cat = new category()
            {
                categoryID = Guid.NewGuid(),
                imageAddress = "/images/Categories/" + catName + ".png",
                restaurantID = newGUid,
                title = " شیک ها",
                culomn = "1",

            };
            dbcontext.categories.Add(cat);
            catName = RandomString(10);
            cat = new category()
            {
                categoryID = Guid.NewGuid(),
                imageAddress = "/images/Categories/" + catName + ".png",
                restaurantID = newGUid,
                title = "هوکا",
                culomn = "1",

            };
            dbcontext.categories.Add(cat);
            catName = RandomString(10);
            cat = new category()
            {
                categoryID = Guid.NewGuid(),
                imageAddress = "/images/Categories/" + catName + ".png",
                restaurantID = newGUid,
                title = "صبحانه",
                culomn = "1",

            };
            dbcontext.categories.Add(cat);
            catName = RandomString(10);
            cat = new category()
            {
                categoryID = Guid.NewGuid(),
                imageAddress = "/images/Categories/" + catName + ".png",
                restaurantID = newGUid,
                title = "پیش غذا",
                culomn = "1",

            };
            dbcontext.categories.Add(cat);
            catName = RandomString(10);
            cat = new category()
            {
                categoryID = Guid.NewGuid(),
                imageAddress = "/images/Categories/" + catName + ".png",
                restaurantID = newGUid,
                title = "غذا",
                culomn = "1",

            };
            dbcontext.categories.Add(cat);


            dbcontext.SaveChanges();
        }


        public void addCat()
        {
            Guid resid = new Guid("1c0b9a5d-f5ec-4224-a1ac-a67a0144027e");
            string catName = RandomString(10);
            category cat = new category()
            {
                categoryID = Guid.NewGuid(),
                imageAddress = "/images/Categories/" + catName + ".png",
                restaurantID = resid,
                title = "SALAD",
                culomn = "1",

            };
            dbcontext.categories.Add(cat);
            catName = RandomString(10);
            cat = new category()
            {
                categoryID = Guid.NewGuid(),
                imageAddress = "/images/Categories/" + catName + ".png",
                restaurantID = resid,
                title = "CREAMY MILKSHAKES",
                culomn = "1",

            };
            dbcontext.categories.Add(cat);
            catName = RandomString(10);
            cat = new category()
            {
                categoryID = Guid.NewGuid(),
                imageAddress = "/images/Categories/" + catName + ".png",
                restaurantID = resid,
                title = "MOJITOS",
                culomn = "1",

            };
            dbcontext.categories.Add(cat);
            catName = RandomString(10);
            cat = new category()
            {
                categoryID = Guid.NewGuid(),
                imageAddress = "/images/Categories/" + catName + ".png",
                restaurantID = resid,
                title = "OMELETTES",
                culomn = "1",

            };
            dbcontext.categories.Add(cat);
            catName = RandomString(10);
            cat = new category()
            {
                categoryID = Guid.NewGuid(),
                imageAddress = "/images/Categories/" + catName + ".png",
                restaurantID = resid,
                title = "CHEESECAKES",
                culomn = "1",

            };
            dbcontext.categories.Add(cat);
            catName = RandomString(10);
            cat = new category()
            {
                categoryID = Guid.NewGuid(),
                imageAddress = "/images/Categories/" + catName + ".png",
                restaurantID = resid,
                title = "KIDS",
                culomn = "1",

            };
            dbcontext.categories.Add(cat);

            dbcontext.SaveChanges();

        }
        public void addDiez()
        {


            Guid newGUid = Guid.NewGuid();
            restaurant rest = new restaurant()
            {
                Address = "میدان صنعت، بلوار فرحزادی، مجتمع تجاری میلاد نور، همکف دوم ، کافه دییز",
                pardakht = 1,
                sabad = 1,
                description = "یک محیط آرام و دلنشین برای وقت گذرانی با دوستان و ساختن یه روز خوب در کافه رستوران قصر الشمس",
                imageAddress = "/images/Restaurant/kafeDiez.png",
                instagram = "",
                logo = "diez.png",
                mobile = "09300023700",
                orderNumberBegin = 100,
                password = "password",
                phone = "02122931806",
                restaurantID = newGUid,
                title = "کافه دییز",
                totem = "لحظه های شما برای ما مهم است",
                username = "diez",
                whatsapp = "09300023700",
                zarin = ""
            };
            dbcontext.restaurants.Add(rest);

            string catName = RandomString(10);
            category cat = new category()
            {
                categoryID = Guid.NewGuid(),
                imageAddress = "/images/Categories/" + catName + ".png",
                restaurantID = newGUid,
                title = "قهوه",
                culomn = "1",

            };
            dbcontext.categories.Add(cat);

            catName = RandomString(10);
            cat = new category()
            {
                categoryID = Guid.NewGuid(),
                imageAddress = "/images/Categories/" + catName + ".png",
                restaurantID = newGUid,
                title = "نوشیدنی های گرم",
                culomn = "2",

            };
            dbcontext.categories.Add(cat);

            catName = RandomString(10);
            cat = new category()
            {
                categoryID = Guid.NewGuid(),
                imageAddress = "/images/Categories/" + catName + ".png",
                restaurantID = newGUid,
                title = "چای و دمنوش",
                culomn = "1",

            };
            dbcontext.categories.Add(cat);
            catName = RandomString(10);
            cat = new category()
            {
                categoryID = Guid.NewGuid(),
                imageAddress = "/images/Categories/" + catName + ".png",
                restaurantID = newGUid,
                title = " اسموتی و آبیموه",
                culomn = "2",

            };
            dbcontext.categories.Add(cat);
            catName = RandomString(10);
            cat = new category()
            {
                categoryID = Guid.NewGuid(),
                imageAddress = "/images/Categories/" + catName + ".png",
                restaurantID = newGUid,
                title = " موکتل",
                culomn = "1",

            };
            dbcontext.categories.Add(cat);
            catName = RandomString(10);
            cat = new category()
            {
                categoryID = Guid.NewGuid(),
                imageAddress = "/images/Categories/" + catName + ".png",
                restaurantID = newGUid,
                title = " شیک",
                culomn = "2",

            };
            dbcontext.categories.Add(cat);
            catName = RandomString(10);
            cat = new category()
            {
                categoryID = Guid.NewGuid(),
                imageAddress = "/images/Categories/" + catName + ".png",
                restaurantID = newGUid,
                title = " نوشیدنی های سرد",
                culomn = "1",

            };
            dbcontext.categories.Add(cat);
            catName = RandomString(10);
            cat = new category()
            {
                categoryID = Guid.NewGuid(),
                imageAddress = "/images/Categories/" + catName + ".png",
                restaurantID = newGUid,
                title = "کیک",
                culomn = "1",

            };
            dbcontext.categories.Add(cat);
            catName = RandomString(10);
            cat = new category()
            {
                categoryID = Guid.NewGuid(),
                imageAddress = "/images/Categories/" + catName + ".png",
                restaurantID = newGUid,
                title = "دسر",
                culomn = "1",

            };
            dbcontext.categories.Add(cat);
            catName = RandomString(10);
            cat = new category()
            {
                categoryID = Guid.NewGuid(),
                imageAddress = "/images/Categories/" + catName + ".png",
                restaurantID = newGUid,
                title = "سالاد و میان وعده",
                culomn = "1",

            };
            dbcontext.categories.Add(cat);
            catName = RandomString(10);
            cat = new category()
            {
                categoryID = Guid.NewGuid(),
                imageAddress = "/images/Categories/" + catName + ".png",
                restaurantID = newGUid,
                title = "صبحانه و میان وعده",
                culomn = "1",

            };
            dbcontext.categories.Add(cat);


            dbcontext.SaveChanges();
        }
    }
}