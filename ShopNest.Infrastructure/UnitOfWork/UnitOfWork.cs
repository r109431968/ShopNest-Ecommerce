using ShopNest.Application.Interfaces;
using ShopNest.Domain.Entities;
using ShopNest.Infrastructure.Persistence;
using ShopNest.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopNest.Infrastructure.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ShopNestDbContext _context;
        private IGenericRepository<Product>? _products;
        private IGenericRepository<Category>? _categories;   
        private IGenericRepository<User>? _users;    
        private IGenericRepository<Order>? _orders;
        private IGenericRepository<OrderItem>? _orderItems;
        private IGenericRepository<Cart>? _carts;
        private IGenericRepository<CartItem>? _cartItems;
        private IGenericRepository<Review>? _reviews;
        private IGenericRepository<RefreshToken>? _refreshTokens;

        public UnitOfWork(ShopNestDbContext context)
        {
            _context = context;
        }

        public IGenericRepository<Product> Products =>
            _products ??= new GenericRepository<Product>(_context);

        public IGenericRepository<Category> Categories =>
            _categories ??= new GenericRepository<Category>(_context);

        public IGenericRepository<User> Users =>
            _users ??= new GenericRepository<User>(_context);

        public IGenericRepository<Order> Orders =>
            _orders ??= new GenericRepository<Order>(_context);

        public IGenericRepository<OrderItem> OrderItems =>
            _orderItems ??= new GenericRepository<OrderItem>(_context);

        public IGenericRepository<Cart> Carts =>
            _carts ??= new GenericRepository<Cart>(_context);

        public IGenericRepository<CartItem> CartItems =>
            _cartItems ??= new GenericRepository<CartItem>(_context);

        public IGenericRepository<Review> Reviews =>
            _reviews ??= new GenericRepository<Review>(_context);

        public IGenericRepository<RefreshToken> RefreshTokens =>
            _refreshTokens ??= new GenericRepository<RefreshToken>(_context);

        public void Dispose()
        {
            _context.Dispose();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
