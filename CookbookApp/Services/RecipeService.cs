using CookbookApp.Models;
using System.Text.Json;
using Microsoft.AspNetCore.Hosting;
using System.Linq;
using System.Text.Json.Serialization;

namespace CookbookApp.Services;

public class RecipeService : IRecipeService
{
    private readonly string _filePath;
    private static List<Recipe> _recipes = new();
    private static int _nextId = 1;

    public RecipeService(IWebHostEnvironment env)
    {
        _filePath = Path.Combine(env.WebRootPath, "data", "recipes.json");
    }

    public async Task<List<Recipe>> GetAllAsync()
    {
        await LoadFromFile();
        await SeedDataIfEmpty();
        return _recipes;
    }

    public async Task<Recipe?> GetByIdAsync(int id)
    {
        await LoadFromFile();
        return _recipes.FirstOrDefault(r => r.Id == id);
    }

    public async Task<int> AddAsync(Recipe recipe)
    {
        await LoadFromFile();
        recipe.Id = _nextId++;
        _recipes.Add(recipe);
        await SaveToFile();
        return recipe.Id;
    }

    public async Task UpdateAsync(Recipe recipe)
    {
        await LoadFromFile();
        var existing = _recipes.FirstOrDefault(r => r.Id == recipe.Id);
        if (existing != null)
        {
            existing.Title = recipe.Title ?? "";
            existing.Description = recipe.Description ?? "";
            existing.Category = recipe.Category ?? "";
            existing.CookingTimeMinutes = recipe.CookingTimeMinutes;
            existing.Portions = recipe.Portions;
            existing.Ingredients = recipe.Ingredients ?? new();
            await SaveToFile();
        }
    }

    public async Task DeleteAsync(int id)
    {
        await LoadFromFile();
        var recipe = _recipes.FirstOrDefault(r => r.Id == id);
        if (recipe != null)
        {
            _recipes.Remove(recipe);
            await SaveToFile();
        }
    }

    public async Task<List<Recipe>> SearchByIngredientsAsync(List<int> ingredientIds)
    {
        await LoadFromFile();
        if (!ingredientIds.Any())
            return _recipes;

        return _recipes.Where(r => r.Ingredients.Any(ing => ingredientIds.Contains(ing.IngredientId))).ToList();
    }

    private async Task LoadFromFile()
    {
        try
        {
            if (File.Exists(_filePath))
            {
                var json = await File.ReadAllTextAsync(_filePath);
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                _recipes = JsonSerializer.Deserialize<List<Recipe>>(json, options) ?? new();
                _nextId = _recipes.Any() ? _recipes.Max(r => r.Id) + 1 : 1;
            }
            else
            {
                _recipes = new();
                _nextId = 1;
            }
        }
        catch
        {
            _recipes = new();
            _nextId = 1;
        }
    }

    private async Task SaveToFile()
    {
        Directory.CreateDirectory(Path.GetDirectoryName(_filePath)!);
        var options = new JsonSerializerOptions
        {
            WriteIndented = true,
            ReferenceHandler = ReferenceHandler.IgnoreCycles
        };
        var json = JsonSerializer.Serialize(_recipes, options);
        await File.WriteAllTextAsync(_filePath, json);
    }

    private async Task SeedDataIfEmpty()
    {
        if (!_recipes.Any())
        {
            _recipes.Add(new Recipe
            {
                Id = _nextId++,
                Title = "Борщ",
                Description = "Классический борщ",
                Category = "Первое",
                CookingTimeMinutes = 120,
                Portions = 6,
                Ingredients = new()
                {
                    new RecipeIngredient { IngredientId = 1, Name = "Свёкла", Amount = "1 шт" },
                    new RecipeIngredient { IngredientId = 2, Name = "Картофель", Amount = "3 шт" },
                    new RecipeIngredient { IngredientId = 3, Name = "Морковь", Amount = "2 шт" },
                    new RecipeIngredient { IngredientId = 4, Name = "Лук", Amount = "1 шт" },
                    new RecipeIngredient { IngredientId = 5, Name = "Мясо", Amount = "500г" },
                    new RecipeIngredient { IngredientId = 6, Name = "Сметана", Amount = "200г" }
                }
            });
            await SaveToFile();
        }
    }
}