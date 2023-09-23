﻿using Domain.Abstractions;

namespace Domain.Products;

public static class ProductErrors
{
    public static Error NotFound = new(
        "Product.Found",
        "The product with the specified identifier was not found");
}