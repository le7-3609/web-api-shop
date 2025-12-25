using AutoMapper;
using DTO;
using Entities;
using Microsoft.EntityFrameworkCore;
using Repositories;
using System.Text.Json;

namespace Services
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;
        private readonly IMapper _mapper;

        public CartService(ICartRepository cartRepository, IMapper mapper)
        {
            _cartRepository = cartRepository;
            _mapper = mapper;
        }
        public async Task<CartItemDTO?> GetByIdAsync(int id)
        {
            var cartItem = await _cartRepository.GetByIdAsync(id);

            if (cartItem == null)
                return null;

            return _mapper.Map<CartItemDTO>(cartItem);
        }
        public async Task<IEnumerable<CartItemDTO>?> GetUserCartAsync(int userId)
        {
            var cartItems = await _cartRepository.GetUserCartAsync(userId);

            if (cartItems == null)
                return null;

            return _mapper.Map<IEnumerable<CartItemDTO>>(cartItems);
        }

        public async Task<CartItemDTO> CreateCartItemAsync(AddCartItemDTO dto)
        {
            var existing = await _cartRepository.GetByCartAndProductIdAsync(dto.CartId, dto.ProductId);
            if (existing != null)
                throw new Exception("Cart item already exists for this user and product.");

            var cartItem = _mapper.Map<CartItem>(dto);
            var created = await _cartRepository.CreateUserCartAsync(cartItem);

            return _mapper.Map<CartItemDTO>(created);
        }

        //לא עשיתי אותו עדיין זה רק מעטפת כדי שלא יעשה טעות בקולנטרולר
        public async Task<CartItemDTO> UpdateUserCartAsync(CartItemDTO dto)
        {
            return dto;
        }

        public async Task<bool> DeleteUserCartAsync(int cartItemId)
        {
            bool succeeded = await _cartRepository.DeleteUserCartAsync(cartItemId);
            if (!succeeded)
                return false;
            return true;
        }
    }
}
