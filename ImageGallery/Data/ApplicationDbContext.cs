using ImageGallery.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImageGallery.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        // передает в конструктор базового класса объекта DbContextOptions, который инкапсулирует параметры конфигурации.
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options){}

        //Для получения всех данных из таблицы, вы можете просто использовать свойство класса контекста, 
        //ссылающееся на класс модели и имеющее тип DbSet<T>. Entity Framework создаст запрос в базу данных 
        //для загрузки всех данных из таблицы, связанной с этим классом модели.
        public DbSet<ImageDetail> ImageDetail { get; set; }
        public DbSet<Category> Categories { get; set; }
    }
}
