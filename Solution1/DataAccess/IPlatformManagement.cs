using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccess
{
    public interface IPlatformManagement
    {
        DbSet<Player> Players { get; set; }
        DbSet<Course> Courses { get; set; }
        DbSet<Module> Modules { get; set; }
        DbSet<Mark> Marks { get; set; }
        DbSet<Homework> Homeworks { get; set; }
        DbSet<Anouncement> Anouncements { get; set; }

        DbSet<TEntity> Set<TEntity>() where TEntity : class;
        int SaveChanges();
        EntityEntry Remove(object entity);
        EntityEntry<TEntity> Add<TEntity>(TEntity entity) where TEntity : class;
        EntityEntry Update(object entity);
    }
}
