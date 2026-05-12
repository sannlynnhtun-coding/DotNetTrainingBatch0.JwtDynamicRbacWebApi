using System.ComponentModel.DataAnnotations.Schema;

namespace DotNetTrainingBatch0.Database.AppDbContextModels;

[Table("Tbl_RolePermission")]
public class AppRolePermission
{
    public int Id { get; set; }
    public int RoleId { get; set; }
    public int PermissionId { get; set; }
}
