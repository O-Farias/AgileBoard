using AgileBoard.API.Data;
using AgileBoard.API.Models;
using Microsoft.EntityFrameworkCore;

namespace AgileBoard.API.Services
{
    public class UserService : IUserService
    {
        private readonly AgileBoardContext _context;

        public UserService(AgileBoardContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _context.Users
                .Include(u => u.AssignedCards)
                .ToListAsync();
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            var user = await _context.Users
                .Include(u => u.AssignedCards)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
            {
                throw new KeyNotFoundException($"Usuário com ID {id} não encontrado.");
            }

            return user;
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email);

            if (user == null)
            {
                throw new KeyNotFoundException($"Usuário com email {email} não encontrado.");
            }

            return user;
        }

        public async Task<User> CreateUserAsync(User user)
        {
            if (user == null)
            {
                throw new ArgumentException("Usuário não pode ser nulo.");
            }

            var emailExists = await _context.Users.AnyAsync(u => u.Email == user.Email);
            if (emailExists)
            {
                throw new InvalidOperationException($"Email {user.Email} já está em uso.");
            }

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task UpdateUserAsync(User user)
        {
            if (user == null)
            {
                throw new ArgumentException("Usuário não pode ser nulo.");
            }

            var existingUser = await _context.Users.FindAsync(user.Id);
            if (existingUser == null)
            {
                throw new KeyNotFoundException($"Usuário com ID {user.Id} não encontrado.");
            }

            if (await _context.Users.AnyAsync(u => u.Email == user.Email && u.Id != user.Id))
            {
                throw new InvalidOperationException($"Email {user.Email} já está em uso.");
            }

            user.UpdatedAt = DateTime.UtcNow;
            _context.Entry(existingUser).CurrentValues.SetValues(user);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteUserAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                throw new KeyNotFoundException($"Usuário com ID {id} não encontrado.");
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }
    }
}