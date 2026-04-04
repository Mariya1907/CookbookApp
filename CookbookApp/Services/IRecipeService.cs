using CookbookApp.Models;
using System.Collections.Generic;

namespace CookbookApp.Services;

public interface IRecipeService
{
    Task<List<Recipe>> GetAllAsync();
    Task<Recipe?> GetByIdAsync(int id);
    Task<int> AddAsync(Recipe recipe);
    Task UpdateAsync(Recipe recipe);
    Task DeleteAsync(int id);
    Task<List<Recipe>> SearchByIngredientsAsync(List<int> ingredientIds);
}