using System.ComponentModel.DataAnnotations.Schema;

namespace DotNetTrainingBatch0.Database.AppDbContextModels;

[Table("Tbl_User")]
public class AppUser
{
    public int Id { get; set; }
    public string Username { get; set; } = "";
    public string Password { get; set; } = "";
    public int RoleId { get; set; }
}
