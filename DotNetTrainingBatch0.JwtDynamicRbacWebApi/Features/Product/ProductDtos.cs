namespace DotNetTrainingBatch0.JwtDynamicRbacWebApi.Features.Product;

public class ProductDto
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public decimal Price { get; set; }
    public string Description { get; set; } = "";
}

public class ProductCreateRequest
{
    public string Name { get; set; } = "";
    public decimal Price { get; set; }
    public string Description { get; set; } = "";
}

public class ProductCreateResponse
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; } = "";
    public ProductDto? Data { get; set; }
}

public class ProductListRequest
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? SearchTerm { get; set; }
}

public class ProductListResponse
{
    public List<ProductDto> Products { get; set; } = new();
    public int TotalCount { get; set; }
}

public class ProductUpdateRequest
{
    public string Name { get; set; } = "";
    public decimal Price { get; set; }
    public string Description { get; set; } = "";
}

public class ProductUpdateResponse
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; } = "";
    public ProductDto? Data { get; set; }
}

public class ProductDeleteResponse
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; } = "";
}

public class AdminOnlyResponse
{
    public string Message { get; set; } = "";
}
