namespace Library.Models.ViewModel;

public class ChangePasswordViewModel
{
    public Guid UserId { get; set; }
    public string OldPassword { get; set; }
    public string NewPassword { get; set; }
}