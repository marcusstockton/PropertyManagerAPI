using System;

namespace PropertyManagerApi.Models.DTOs.Auth
{
    public class UserDetailsDto
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
    }
}