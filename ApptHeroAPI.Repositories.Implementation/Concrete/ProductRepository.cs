using ApptHeroAPI.Repositories.Abstraction.Contracts;
using ApptHeroAPI.Repositories.Context.Context;
using ApptHeroAPI.Repositories.Context.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ApptHeroAPI.Repositories.Implementation.Concrete
{
    public class ProductRepository : IRepository<Product>
    {
        private readonly SqlDbContext _dbContext;
        public ProductRepository(IDbContextFactory<SqlDbContext> dbContextFactory)
        {
            this._dbContext = dbContextFactory.CreateDbContext();
        }
        public async Task<Product> Add(Product entity)
        {
            try
            {
                EntityEntry<Product> result = this._dbContext.Product.Add(entity);

                if (await this.SaveChanges())
                {
                    return result.Entity;
                }
                return null;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> AddList(List<Product> entities)
        {
            try
            {
                this._dbContext.Product.AddRange(entities);
                return await this.SaveChanges();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> Delete(long id)
        {
            try
            {
                Product product = this._dbContext.Product.Where(s => s.ProductId == id).FirstOrDefault();
                this._dbContext.Product.Remove(product);
                return await this.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Product> Get(Expression<Func<Product, bool>> expression)
        {
            try
            {
                Product product = await this._dbContext.Product.Where(expression).FirstOrDefaultAsync();
                return product;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<Product>> GetAll(Expression<Func<Product, bool>> expression = null)
        {
            try
            {
                List<Product> Product = await this._dbContext.Product.Where(expression).ToListAsync();
                return Product;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task<List<Product>> GetAllIncluding(Expression<Func<Product, bool>> predicate, params Expression<Func<Product, object>>[] includeProperties)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Update(Product entity)
        {
            try
            {
                this._dbContext.Product.Update(entity);
                return await this.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }

        private async Task<bool> SaveChanges()
        {
            int rowsEffected = await this._dbContext.SaveChangesAsync();
            if (rowsEffected == 0) return false;
            return true;
        }
    }
}
