namespace DotNetTrainingBatch0.JwtDynamicRbacWebApi.Features.Product;

public class ProductService
{
    private static List<ProductDto> _products = new()
    {
        new ProductDto { Id = 1, Name = "Laptop", Price = 1200.50m, Description = "High-end laptop" },
        new ProductDto { Id = 2, Name = "Mouse", Price = 25.00m, Description = "Wireless mouse" }
    };

    public ProductListResponse GetProducts(ProductListRequest request)
    {
        var products = _products
            .Where(x => string.IsNullOrEmpty(request.SearchTerm) || 
                        x.Name.Contains(request.SearchTerm, StringComparison.OrdinalIgnoreCase))
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToList();

        return new ProductListResponse
        {
            Products = products,
            TotalCount = _products.Count
        };
    }

    public ProductCreateResponse CreateProduct(ProductCreateRequest request)
    {
        var newProduct = new ProductDto
        {
            Id = _products.Any() ? _products.Max(x => x.Id) + 1 : 1,
            Name = request.Name,
            Price = request.Price,
            Description = request.Description
        };

        _products.Add(newProduct);

        return new ProductCreateResponse
        {
            IsSuccess = true,
            Message = "Product created successfully.",
            Data = newProduct
        };
    }

    public ProductUpdateResponse UpdateProduct(int id, ProductUpdateRequest request)
    {
        var product = _products.FirstOrDefault(x => x.Id == id);
        if (product == null) 
            return new ProductUpdateResponse { IsSuccess = false, Message = "Product not found." };

        product.Name = request.Name;
        product.Price = request.Price;
        product.Description = request.Description;

        return new ProductUpdateResponse 
        { 
            IsSuccess = true, 
            Message = $"Product {id} updated successfully.",
            Data = product
        };
    }

    public ProductDeleteResponse DeleteProduct(int id)
    {
        var product = _products.FirstOrDefault(x => x.Id == id);
        if (product == null) 
            return new ProductDeleteResponse { IsSuccess = false, Message = "Product not found." };

        _products.Remove(product);

        return new ProductDeleteResponse 
        { 
            IsSuccess = true, 
            Message = $"Product {id} deleted successfully." 
        };
    }
}
