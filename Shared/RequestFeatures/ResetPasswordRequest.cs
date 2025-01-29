using System.ComponentModel.DataAnnotations;

namespace Shared.RequestFeatures;

public class ResetPasswordRequest
{
    [Required(ErrorMessage = "Required email Address")]
    public string Email { get; set; } = null!;
}
