using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class ProductsRepository(StoreContext context) : IProductsRepository
{
    private readonly StoreContext _context = context;

    public void AddProduct(Product product)
    {
        _context.Products.Add(product);
    }

    public void DeleteProduct(Product product)
    {
        _context.Products.Remove(product);
    }

    public async Task<Product?> GetProductByIdAsync(int id)
    {
        return await _context.Products.FindAsync(id);
    }

    public async Task<IReadOnlyList<Product>> GetProductsAsync(string? brand, string? type, string? sort)
    {

        var query = _context.Products.AsQueryable();

        if (!string.IsNullOrEmpty(brand))
        {
            query = query.Where(p => p.Brand == brand);
        }

        if (!string.IsNullOrEmpty(type))
        {
            query = query.Where(p => p.Type == type);
        }

        if (!string.IsNullOrEmpty(sort))
        {
            query = sort.ToLower() switch
            {
                "priceasc" => query.OrderBy(p => p.Price),
                "pricedesc" => query.OrderByDescending(p => p.Price),
                _ => query.OrderBy(p => p.Name)
            };
        }

        return await query.Skip(5).Take(5).ToListAsync();
    }

    public bool ProductExists(int id)
    {
        return _context.Products.Any(p => p.Id == id);
    }

    public async Task<bool> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync() > 0;
    }

    public void UpdateProduct(Product product)
    {
        _context.Entry(product).State = EntityState.Modified;
    }

    public async Task<IReadOnlyList<string>> GetBrandsAsync()
    {
        return await _context.Products.Select(p => p.Brand).Distinct().ToListAsync();
    }

    public async Task<IReadOnlyList<string>> GetTypesAsync()
    {
        return await _context.Products.Select(p => p.Type).Distinct().ToListAsync();
    }
}
