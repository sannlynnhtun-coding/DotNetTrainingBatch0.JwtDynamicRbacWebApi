using System.ComponentModel.DataAnnotations.Schema;

namespace DotNetTrainingBatch0.Database.AppDbContextModels;

[Table("Tbl_Role")]
public class AppRole
{
    public int Id { get; set; }
    public string RoleName { get; set; } = "";
}
