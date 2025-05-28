using API.Dtos;
using API.Errors;
using API.Extensions;
using API.Helpers;
using API.Middleware;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddAutoMapper(typeof(MappingProfiles));
builder.Services.AddDbContext<StoreContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddApplicationServices();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var loggerFactory = services.GetRequiredService<ILoggerFactory>();
    try 
    {
        var context = services.GetRequiredService<StoreContext>();
        await context.Database.MigrateAsync();
        await StoreContextSeed.SeedAsync(context, loggerFactory);
    }
    catch (Exception ex)
    {
        var logger = loggerFactory.CreateLogger<Program>();
        logger.LogError(ex, "An error occured during database migration");
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionMiddleware>();
app.UseStatusCodePagesWithReExecute("/errors/{0}");

app.UseHttpsRedirection();
app.UseStaticFiles();

app.MapGet("/api/products", async (IGenericRepository<Product> productsRepo, IMapper mapper, string sort, int? brandId, int? typeId) =>
{
    var spec = new ProductWithTypesAndBrandsSpecification(sort, brandId, typeId);

    var products = await productsRepo.ListAsync(spec);
    
    return Results.Ok(mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(products));
})
.WithName("GetProducts")
.WithTags("Products")
.WithOpenApi();

app.MapGet("/api/products/{id}", async (int id, IGenericRepository<Product> productsRepo, IMapper mapper) =>
{
    var spec = new ProductWithTypesAndBrandsSpecification(id);

    var product = await productsRepo.GetEntityWithSpec(spec);

    if (product == null)
    {
        return Results.NotFound(new ApiResponse(404));
    } 

    return Results.Ok(mapper.Map<Product, ProductToReturnDto>(product));
})
.WithName("GetProductsById")
.WithTags("Products")
.Produces<ProductToReturnDto>(StatusCodes.Status200OK)
.Produces<ApiResponse>(StatusCodes.Status404NotFound)
.WithOpenApi();

app.MapGet("/api/products/brands", async (IGenericRepository<ProductBrand> productBrandRepo) =>
{
    var brands = await productBrandRepo.ListAllAsync();
    return Results.Ok(brands);
})
.WithName("GetProductBrands")
.WithTags("Products")
.WithOpenApi();

app.MapGet("/api/products/types", async (IGenericRepository<ProductType> productTypeRepo) =>
{
    var brands = await productTypeRepo.ListAllAsync();
    return Results.Ok(brands);
})
.WithName("GetProductTypes")
.WithTags("Products")
.WithOpenApi();

app.MapGet("/api/buggy/notfound", async (StoreContext context) => 
{
    var thing = await context.Products.FindAsync(42);
    return thing == null ? Results.NotFound(new ApiResponse(404)) : Results.Ok(thing);
})
.WithTags("Buggy")
.WithOpenApi();

app.MapGet("/api/buggy/servererror", async (StoreContext context) => 
{
    var thing = await context.Products.FindAsync(42);
    var thingToReturn = thing.ToString();
    return Results.Ok(thingToReturn);
})
.WithTags("Buggy")
.WithOpenApi();

app.MapGet("/api/buggy/badrequest", () => 
{
    return Results.BadRequest(new ApiResponse(400));
})
.WithTags("Buggy")
.WithOpenApi();
 
app.MapGet("/api/buggy/badrequest/{id}", (int id) => 
{
    return Results.Ok(id);
})
.WithTags("Buggy")
.WithOpenApi();

app.MapGet("/errors/{code:int}", (int code) => 
{
    return Results.Json(new ApiResponse(code), statusCode: code);
})
.ExcludeFromDescription()
.WithTags("Buggy")
.WithOpenApi();

app.Run();
