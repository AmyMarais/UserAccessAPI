using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserAccessAPI.Data;
using UserAccessAPI.Models;

namespace UserAccessAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserAccessDbContext _context;

        public UserController(UserAccessDbContext context) => _context = context;

        [HttpGet]
        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        [ProducesResponseType(typeof(UserDetail), StatusCodes.Status200OK)]
        public async Task<IEnumerable<UserDetail>> GetUsers()   
           => await _context.UserDetails.ToListAsync();
   
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(UserDetail), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _context.UserDetails.FindAsync(id);

            if (user == null)
            {
                return NotFound("User not found.");
            };

            return Ok(user);
        }

        [HttpPost]
        [ProducesResponseType(typeof(UserDetail), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateUser(UserDetail user)
        {

            if (user == null)
            {
                return BadRequest("Invalid user data.");
            };

            await _context.UserDetails.AddAsync(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, user);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(UserDetail), StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public async Task<IActionResult> EditUser(int id, UserDetail user)
        {
            if (id != user.Id)
            {
                return BadRequest("User ID mismatch.");
            }

            var existingUser = await _context.UserDetails.FindAsync(user.Id);

            if(existingUser == null)
            {
                return NotFound("User not found.");
            }

            // Update the existing user entity with the new data
            existingUser.Username = user.Username;
            existingUser.AccessLevel = user.AccessLevel;
            existingUser.Permission = user.Permission;
           

            await _context.SaveChangesAsync();

            // Re-query the database to fetch the updated user data
            var updatedUser = await _context.UserDetails.FindAsync(user.Id);

            return Ok(updatedUser);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(UserDetail), StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public async Task<IActionResult> DeleteUser(int id) 
        {
            var userToDelete = await _context.UserDetails.FindAsync(id);
            if(userToDelete == null)
            {
                return NotFound("User not found.");
            }

            _context.UserDetails.Remove(userToDelete);
            await _context.SaveChangesAsync();

            return NoContent();
        }

    }
}
