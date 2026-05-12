using System.ComponentModel.DataAnnotations.Schema;

namespace DotNetTrainingBatch0.Database.AppDbContextModels;

[Table("Tbl_Permission")]
public class AppPermission
{
    public int Id { get; set; }
    public string PermissionName { get; set; } = "";
}
