using System.ComponentModel.DataAnnotations;

namespace CookbookApp.Models;

public class Recipe
{
    public int Id { get; set; }
    
    [Required]
    public string Title { get; set; } = "";
    
    public string Description { get; set; } = "";
    
    [Required]
    public string Category { get; set; } = "";
    
    public int CookingTimeMinutes { get; set; }
    
    public int Portions { get; set; }
    
    public List<RecipeIngredient> Ingredients { get; set; } = new();
}

// ✅ ВСЁ В ОДНОМ ФАЙЛЕ!
public class RecipeIngredient
{
    public int IngredientId { get; set; }
    public string Name { get; set; } = "";
    public string Amount { get; set; } = "";
}