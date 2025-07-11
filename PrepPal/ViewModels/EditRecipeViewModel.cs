using System.Windows.Input;
using PrepPal.Models;
using System.Collections.Generic;
using Microsoft.Maui.Controls;

namespace PrepPal.ViewModels;

public class EditRecipeViewModel : BindableObject
{
    private string _recipeName;
    private string _category;
    private string _source;
    private string _sourceURL;
    private string _prepTime;
    private string _cookTime;
    private string _imageURL;

    public string RecipeName
    {
        get => _recipeName;
        set
        {
            _recipeName = value;
            OnPropertyChanged();
        }
    }

    public string Category
    {
        get => _category;
        set
        {
            _category = value;
            OnPropertyChanged();
        }
    }

    public string Source
    {
        get => _source;
        set
        {
            _source = value;
            OnPropertyChanged();
        }
    }

    public string SourceURL
    {
        get => _sourceURL;
        set
        {
            _sourceURL = value;
            OnPropertyChanged();
        }
    }

    public string PrepTime
    {
        get => _prepTime;
        set
        {
            _prepTime = value;
            OnPropertyChanged();
        }
    }

    public string CookTime
    {
        get => _cookTime;
        set
        {
            _cookTime = value;
            OnPropertyChanged();
        }
    }

    public string ImageURL
    {
        get => _imageURL;
        set
        {
            _imageURL = value;
            OnPropertyChanged();
        }
    }
    
    public List<RecipeIngredient> RecipeIngredients { get; set; } = new List<RecipeIngredient>();
    public List<Instruction> Instructions { get; set; } = new List<Instruction>();
    
    public ICommand EditRecipeNameCommand { get; }
    public ICommand EditRecipeCategoryCommand { get; }
    public ICommand EditRecipeSourceCommand { get; }
    public ICommand EditRecipeSourceURLCommand { get; }
    public ICommand EditRecipePrepTimeCommand { get; }
    public ICommand EditRecipeCookTimeCommand { get; }
    public ICommand EditRecipeImageURLCommand { get; }
    public ICommand EditRecipeIngredientsCommand { get; }
    public ICommand EditRecipeInstructionsCommand { get; }

    public EditRecipeViewModel()
    {
        EditRecipeNameCommand = new Command(EditRecipeName);
        EditRecipeCategoryCommand = new Command(EditCategory);
        EditRecipeSourceCommand = new Command(EditSource);
        EditRecipeSourceURLCommand = new Command(EditSourceURL);
        EditRecipePrepTimeCommand = new Command(EditPrepTime);
        EditRecipeCookTimeCommand = new Command(EditCookTime);
        EditRecipeImageURLCommand = new Command(EditImageURL);
        EditRecipeIngredientsCommand = new Command(EditIngredients);
        EditRecipeInstructionsCommand = new Command(EditInstructions);
    }

    private async void EditRecipeName()
    {
        string result = await Application.Current.MainPage.DisplayPromptAsync("Edit Recipe Name", "Enter new recipe name:", initialValue: RecipeName);
        if (!string.IsNullOrWhiteSpace(result))
        {
            RecipeName = result;
        }
    }

    private async void EditCategory()
    {
        string result = await Application.Current.MainPage.DisplayPromptAsync("Edit Recipe Category", "Enter new category:", initialValue: Category);
        if (!string.IsNullOrWhiteSpace(result))
        {
            Category = result;
        }
    }

    private async void EditSource()
    {
        string result = await Application.Current.MainPage.DisplayPromptAsync("Edit Recipe Source", "Enter new source:", initialValue: Source);
        if (!string.IsNullOrWhiteSpace(result))
        {
            Source = result;
        }
    }

    private async void EditSourceURL()
    {
        string result = await Application.Current.MainPage.DisplayPromptAsync("Edit Source URL", "Enter new source URL:", initialValue: SourceURL);
        if (!string.IsNullOrWhiteSpace(result))
        {
            SourceURL = result;
        }
    }

    private async void EditPrepTime()
    {
        string result = await Application.Current.MainPage.DisplayPromptAsync("Edit Prep Time", "Enter new prep time (in minutes):", initialValue: PrepTime);
        if (!string.IsNullOrWhiteSpace(result))
        {
            PrepTime = result;
        }
    }

    private async void EditCookTime()
    {
        string result = await Application.Current.MainPage.DisplayPromptAsync("Edit Cook Time", "Enter new cook time (in minutes):", initialValue: CookTime);
        if (!string.IsNullOrWhiteSpace(result))
        {
            CookTime = result;
        }
    }

    private async void EditImageURL()
    {
        string result = await Application.Current.MainPage.DisplayPromptAsync("Edit Image URL", "Enter new image URL:", initialValue: ImageURL);
        if (!string.IsNullOrWhiteSpace(result))
        {
            ImageURL = result;
        }
    }

    private async void EditIngredients()
    {
        string result = await Application.Current.MainPage.DisplayPromptAsync(
            "Edit Ingredients",
            "Enter new ingredients (comma-separated):",
            initialValue: string.Join(", ", RecipeIngredients.Select(i => $"{i.Quantity} {i.Unit} {i.IngredientName}"))
        );

        if (!string.IsNullOrWhiteSpace(result))
        {
            var updatedIngredients = result.Split(',')
                                           .Select(ingredient => ingredient.Trim())
                                           .Where(ingredient => !string.IsNullOrWhiteSpace(ingredient))
                                           .Select(ingredient =>
                                           {
                                               var parts = ingredient.Split(' ', 3);
                                               if (parts.Length == 3)
                                               {
                                                   return new RecipeIngredient
                                                   {
                                                       Quantity = decimal.TryParse(parts[0], out var quantity) ? quantity : 0,
                                                       Unit = parts[1],
                                                       IngredientName = parts[2]
                                                   };
                                               }
                                               return null;
                                           })
                                           .Where(ingredient => ingredient != null)
                                           .ToList();

            RecipeIngredients = updatedIngredients;
            OnPropertyChanged(nameof(RecipeIngredients));
        }
    }

    private async void EditInstructions()
    {
        string result = await Application.Current.MainPage.DisplayPromptAsync(
            "Edit Instructions",
            "Enter new instructions (semicolon-separated):",
            initialValue: string.Join("; ", Instructions.Select(i => $"{i.StepNumber}. {i.Description}"))
        );

        if (!string.IsNullOrWhiteSpace(result))
        {
            var updatedInstructions = result.Split(';')
                                             .Select((instruction, index) => new Instruction
                                             {
                                                 StepNumber = index + 1,
                                                 Description = instruction.Trim()
                                             })
                                             .Where(instruction => !string.IsNullOrWhiteSpace(instruction.Description))
                                             .ToList();

            Instructions = updatedInstructions;
            OnPropertyChanged(nameof(Instructions));
        }
    }
}