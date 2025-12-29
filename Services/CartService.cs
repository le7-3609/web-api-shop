using AutoMapper;
using DTO;
using Entities;
using Repositories;

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

        public async Task<CartItemDTO?> GetCartItemByIdAsync(int id)
        {
            var cartItem = await _cartRepository.GetCartItemByIdAsync(id);

            if (cartItem == null)
                return null;

            return _mapper.Map<CartItemDTO>(cartItem);
        }

        public async Task<IEnumerable<CartItemDTO>?> GetCartItemsByCartIdAsync(int cartId)
        {
            var cartItems = await _cartRepository.GetCartItemsByCartIdAsync(cartId);

            if (cartItems == null)
                return null;

            return _mapper.Map<IEnumerable<CartItemDTO>>(cartItems);
        }

        public async Task<CartItemDTO?> AddCartItemAsync(AddCartItemDTO dto)
        {
            var existingItem = await _cartRepository.GetByCartAndProductIdAsync(dto.CartId, dto.ProductId);

            if (existingItem != null)
            {
                return null;
            }
            var cartItem = _mapper.Map<CartItem>(dto);
            var created = await _cartRepository.AddCartItemAsync(cartItem);

            return _mapper.Map<CartItemDTO>(created);
        }

        public async Task<CartItemDTO> UpdateCartItemAsync(CartItemDTO dto)
        {
            var cartItem = _mapper.Map<CartItem>(dto);
            var updated = await _cartRepository.UpdateCartItemAsync(cartItem);

            return _mapper.Map<CartItemDTO>(updated);
        }

        public async Task<bool> DeleteCartItemAsync(int cartItemId)
        {
            return await _cartRepository.DeleteCartItemAsync(cartItemId);
        }

        public async Task<bool> ClearCartAsync(int cartId)
        {
            return await _cartRepository.ClearCartItemsAsync(cartId);
        }
    }
}