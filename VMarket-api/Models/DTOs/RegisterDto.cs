using System.ComponentModel.DataAnnotations;

namespace VMarket_api.Models.DTOs;

public class RegisterDto
{
    [Required(ErrorMessage = "Pseudo requis")]
    [MinLength(3, ErrorMessage = "Pseudo trop court (min 3)")]
    public string Username { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email requis")]
    [EmailAddress(ErrorMessage = "Email invalide")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password requis")]
    [MinLength(6, ErrorMessage = "Password trop court (min 6)")]
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "Confirmation requise")]
    [Compare("Password", ErrorMessage = "Passwords ne correspondent pas")]
    public string ConfirmPassword { get; set; } = string.Empty;
}