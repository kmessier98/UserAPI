using Microsoft.AspNetCore.Identity;

namespace UserApi.Domain.Entities
{
    // Assurez-vous d'ajouter la référence au package NuGet :
    // Microsoft.AspNetCore.Identity.EntityFrameworkCore
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}
