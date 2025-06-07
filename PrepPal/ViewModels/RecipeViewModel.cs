namespace PrepPal.ViewModels
{
    public class RecipeViewModel : INotifyPropertyChanged
    {
        private Recipe _selectedRecipe;
        private readonly PrepPalDbContext _dbContext;
        private bool _isAllRecipesSelected = true;

        // Observable collections for the RecipeSelectionPage
        public ObservableCollection<Recipe> AllRecipes { get; set; }
        public ObservableCollection<Recipe> FilteredRecipes { get; set; }

        public bool IsFavoriteRecipesSelected => !IsAllRecipesSelected;
        public bool IsAllRecipesSelected
        {
            get => _isAllRecipesSelected;
            set
            {
                _isAllRecipesSelected = value;
                OnPropertyChanged();
                ApplyFilter();
            }
        }
        public ObservableCollection<Recipe> Recipes { get; set; }

        public ObservableCollection<Recipe> FavoriteRecipes
        {
            get
            {
                return new ObservableCollection<Recipe>(Recipes.Where(r => r.IsFavorite));
            }
        }

        public ICommand IncreaseServingsCommand { get; set; }
        public ICommand DecreaseServingsCommand { get; set; }
        public ICommand ToggleFavoriteCommand { get; set; }
        public ICommand SwitchToAllRecipesCommand { get; }
        public ICommand SwitchToFavoriteRecipesCommand { get; }
        public ICommand NavigateBackCommand { get; }
        public ICommand OpenSourceUrlCommand { get; }

        public Recipe SelectedRecipe
        {
            get => _selectedRecipe;
            set
            {
                if (_selectedRecipe != value)
                {
                    _selectedRecipe = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(SelectedRecipe));
                }
            }
        }
        public RecipeViewModel(PrepPalDbContext dbContext)
        {
            _dbContext = dbContext;

            ToggleFavoriteCommand = new Command<Recipe>(ToggleFavorite);
            IncreaseServingsCommand = new Command(IncreaseServings);
            DecreaseServingsCommand = new Command(DecreaseServings);
            SwitchToAllRecipesCommand = new Command(SwitchToAllRecipes);
            SwitchToFavoriteRecipesCommand = new Command(SwitchToFavoriteRecipes);
            NavigateBackCommand = new Command(NavigateBack);
            OpenSourceUrlCommand = new Command(OpenSourceUrl);

            Recipes = new ObservableCollection<Recipe>();
            AllRecipes = new ObservableCollection<Recipe>();
            FilteredRecipes = new ObservableCollection<Recipe>();

            LoadRecipes();
        }

        private async void LoadRecipes()
        {
            try
            {
                AllRecipes.Clear();

                var hardcodedRecipes = new ObservableCollection<Recipe>
                {

new Recipe
{
    Name = "Spaghetti Carbonara",
    RecipeIngredients = new List<RecipeIngredient>
    {
        new RecipeIngredient { IngredientName = "Spaghetti", Quantity = 12, Unit = "oz", Aisle = "Pasta", StorageLocation = "Pantry" },
        new RecipeIngredient { IngredientName = "Pancetta or guanciale, diced", Quantity = 4, Unit = "oz", Aisle = "Meat", StorageLocation = "Fridge" },
        new RecipeIngredient { IngredientName = "Egg yolks", Quantity = 4, Unit = "pcs", Aisle = "Dairy", StorageLocation = "Fridge" },
        new RecipeIngredient { IngredientName = "Parmesan cheese, grated", Quantity = 0.75m, Unit = "cup", Aisle = "Dairy", StorageLocation = "Fridge" },
        new RecipeIngredient { IngredientName = "Black pepper", Quantity = 0, Unit = "to taste", Aisle = "Spices", StorageLocation = "Pantry" },
        new RecipeIngredient { IngredientName = "Salt", Quantity = 0, Unit = "to taste", Aisle = "Spices", StorageLocation = "Pantry" }
    },
    Instructions = new List<Instruction>
    {
        new Instruction { StepNumber = 1, Description = "Cook spaghetti in salted boiling water until al dente." },
        new Instruction { StepNumber = 2, Description = "Sauté pancetta in a skillet until crisp." },
        new Instruction { StepNumber = 3, Description = "Whisk egg yolks and Parmesan in a bowl." },
        new Instruction { StepNumber = 4, Description = "Drain pasta, reserving some water. Toss pasta with pancetta." },
        new Instruction { StepNumber = 5, Description = "Remove from heat, add egg mixture, and toss quickly. Add pasta water if needed." },
        new Instruction { StepNumber = 6, Description = "Season with black pepper and serve immediately." }
    },
    Category = "Main Dish",
    Servings = 4,
    PrepTime = 10,
    CookTime = 20,
    TotalTime = 30,
    Source = "Italian Classic",
    SourceURL = "https://www.giallozafferano.com/recipes/Spaghetti-Carbonara.html",
    ImageURL = "https://www.savoryexperiments.com/wp-content/uploads/2019/01/Carbonara-3.jpg"
},

// Italian Recipe 2: Margherita Pizza
new Recipe
{
    Name = "Pizza Margherita",
    RecipeIngredients = new List<RecipeIngredient>
    {
        new RecipeIngredient { IngredientName = "Pizza dough", Quantity = 1, Unit = "lb", Aisle = "Bakery", StorageLocation = "Pantry" },
        new RecipeIngredient { IngredientName = "Tomato sauce", Quantity = 0.5m, Unit = "cup", Aisle = "Canned Goods", StorageLocation = "Pantry" },
        new RecipeIngredient { IngredientName = "Fresh mozzarella, sliced", Quantity = 8, Unit = "oz", Aisle = "Dairy", StorageLocation = "Fridge" },
        new RecipeIngredient { IngredientName = "Fresh basil leaves", Quantity = 0, Unit = "handful", Aisle = "Produce", StorageLocation = "Fridge" },
        new RecipeIngredient { IngredientName = "Olive oil", Quantity = 1, Unit = "tbsp", Aisle = "Oils", StorageLocation = "Pantry" },
        new RecipeIngredient { IngredientName = "Salt", Quantity = 0, Unit = "to taste", Aisle = "Spices", StorageLocation = "Pantry" }
    },
    Instructions = new List<Instruction>
    {
        new Instruction { StepNumber = 1, Description = "Preheat oven to 500°F (260°C)." },
        new Instruction { StepNumber = 2, Description = "Stretch dough onto a pizza stone or baking sheet." },
        new Instruction { StepNumber = 3, Description = "Spread tomato sauce over dough. Top with mozzarella slices." },
        new Instruction { StepNumber = 4, Description = "Bake for 10-12 minutes until crust is golden and cheese is bubbly." },
        new Instruction { StepNumber = 5, Description = "Top with fresh basil and drizzle with olive oil before serving." }
    },
    Category = "Main Dish",
    Servings = 2,
    PrepTime = 15,
    CookTime = 12,
    TotalTime = 27,
    Source = "Italian Classic",
    SourceURL = "https://www.giallozafferano.com/recipes/Pizza-Margherita.html",
    ImageURL = "https://images.ricardocuisine.com/services/recipes/pizza.jpg"
},

// Italian Recipe 3: Tiramisu
new Recipe
{
    Name = "Tiramisu",
    RecipeIngredients = new List<RecipeIngredient>
    {
        new RecipeIngredient { IngredientName = "Ladyfingers", Quantity = 24, Unit = "pcs", Aisle = "Bakery", StorageLocation = "Pantry" },
        new RecipeIngredient { IngredientName = "Mascarpone cheese", Quantity = 8, Unit = "oz", Aisle = "Dairy", StorageLocation = "Fridge" },
        new RecipeIngredient { IngredientName = "Egg yolks", Quantity = 4, Unit = "pcs", Aisle = "Dairy", StorageLocation = "Fridge" },
        new RecipeIngredient { IngredientName = "Sugar", Quantity = 0.5m, Unit = "cup", Aisle = "Baking", StorageLocation = "Pantry" },
        new RecipeIngredient { IngredientName = "Espresso, cooled", Quantity = 1, Unit = "cup", Aisle = "Beverages", StorageLocation = "Pantry" },
        new RecipeIngredient { IngredientName = "Cocoa powder", Quantity = 0, Unit = "for dusting", Aisle = "Baking", StorageLocation = "Pantry" }
    },
    Instructions = new List<Instruction>
    {
        new Instruction { StepNumber = 1, Description = "Beat egg yolks with sugar until thick and pale." },
        new Instruction { StepNumber = 2, Description = "Fold in mascarpone until smooth." },
        new Instruction { StepNumber = 3, Description = "Dip ladyfingers in espresso and layer in a dish." },
        new Instruction { StepNumber = 4, Description = "Spread half the mascarpone mixture over ladyfingers. Repeat layers." },
        new Instruction { StepNumber = 5, Description = "Dust with cocoa powder and chill for at least 4 hours before serving." }
    },
    Category = "Dessert",
    Servings = 6,
    PrepTime = 25,
    CookTime = 0,
    TotalTime = 25,
    Source = "Italian Classic",
    SourceURL = "https://www.giallozafferano.com/recipes/Tiramisu.html",
    ImageURL = "https://www.gimmesomeoven.com/wp-content/uploads/2020/07/Tiramisu-Recipe-Cover.jpg"
},

// Armenian Recipe 1: Khorovats (Armenian BBQ)
new Recipe
{
    Name = "Khorovats (Armenian BBQ)",
    RecipeIngredients = new List<RecipeIngredient>
    {
        new RecipeIngredient { IngredientName = "Pork shoulder, cubed", Quantity = 2, Unit = "lb", Aisle = "Meat", StorageLocation = "Fridge" },
        new RecipeIngredient { IngredientName = "Onion, sliced", Quantity = 2, Unit = "pcs", Aisle = "Produce", StorageLocation = "Pantry" },
        new RecipeIngredient { IngredientName = "Bell pepper, sliced", Quantity = 2, Unit = "pcs", Aisle = "Produce", StorageLocation = "Fridge" },
        new RecipeIngredient { IngredientName = "Tomato, sliced", Quantity = 2, Unit = "pcs", Aisle = "Produce", StorageLocation = "Fridge" },
        new RecipeIngredient { IngredientName = "Salt", Quantity = 0, Unit = "to taste", Aisle = "Spices", StorageLocation = "Pantry" },
        new RecipeIngredient { IngredientName = "Black pepper", Quantity = 0, Unit = "to taste", Aisle = "Spices", StorageLocation = "Pantry" }
    },
    Instructions = new List<Instruction>
    {
        new Instruction { StepNumber = 1, Description = "Marinate pork with onions, salt, and pepper for several hours." },
        new Instruction { StepNumber = 2, Description = "Thread pork and vegetables onto skewers." },
        new Instruction { StepNumber = 3, Description = "Grill over hot coals, turning occasionally, until meat is cooked through." },
        new Instruction { StepNumber = 4, Description = "Serve hot with lavash and fresh herbs." }
    },
    Category = "Main Dish",
    Servings = 6,
    PrepTime = 30,
    CookTime = 30,
    TotalTime = 60,
    Source = "Armenian Family Recipe",
    SourceURL = "https://www.armeniandish.com/khorovats-armenian-bbq/",
    ImageURL = "https://phoenixtour.org/wp-content/uploads/2021/12/10-KHOROVATS.jpg"
},

// Armenian Recipe 2: Lavash (Armenian Flatbread)
new Recipe
{
    Name = "Lavash (Armenian Flatbread)",
    RecipeIngredients = new List<RecipeIngredient>
    {
        new RecipeIngredient { IngredientName = "All-purpose flour", Quantity = 3, Unit = "cups", Aisle = "Baking", StorageLocation = "Pantry" },
        new RecipeIngredient { IngredientName = "Water", Quantity = 1, Unit = "cup", Aisle = "Beverages", StorageLocation = "Pantry" },
        new RecipeIngredient { IngredientName = "Salt", Quantity = 1, Unit = "tsp", Aisle = "Spices", StorageLocation = "Pantry" }
    },
    Instructions = new List<Instruction>
    {
        new Instruction { StepNumber = 1, Description = "Mix flour, water, and salt to form a soft dough. Knead until smooth." },
        new Instruction { StepNumber = 2, Description = "Let dough rest for 30 minutes." },
        new Instruction { StepNumber = 3, Description = "Divide dough, roll into thin sheets." },
        new Instruction { StepNumber = 4, Description = "Bake on a hot griddle or in a tandoor until lightly browned." }
    },
    Category = "Bread",
    Servings = 8,
    PrepTime = 20,
    CookTime = 10,
    TotalTime = 30,
    Source = "Armenian Family Recipe",
    SourceURL = "https://www.armeniandish.com/lavash-armenian-flatbread/",
    ImageURL = "https://vidarbergum.com/wp-content/uploads/2023/08/armenian-lavash-wrap-bread-4.jpg"
},

// Armenian Recipe 3: Harissa (Armenian Wheat & Chicken Porridge)
new Recipe
{
    Name = "Harissa (Armenian Wheat & Chicken Porridge)",
    RecipeIngredients = new List<RecipeIngredient>
    {
        new RecipeIngredient { IngredientName = "Chicken, whole", Quantity = 1, Unit = "pcs", Aisle = "Meat", StorageLocation = "Fridge" },
        new RecipeIngredient { IngredientName = "Shelled wheat", Quantity = 2, Unit = "cups", Aisle = "Grains", StorageLocation = "Pantry" },
        new RecipeIngredient { IngredientName = "Butter", Quantity = 2, Unit = "tbsp", Aisle = "Dairy", StorageLocation = "Fridge" },
        new RecipeIngredient { IngredientName = "Salt", Quantity = 0, Unit = "to taste", Aisle = "Spices", StorageLocation = "Pantry" }
    },
    Instructions = new List<Instruction>
    {
        new Instruction { StepNumber = 1, Description = "Boil chicken in water until tender. Remove bones and shred meat." },
        new Instruction { StepNumber = 2, Description = "Add shelled wheat to broth and cook until very soft, stirring often." },
        new Instruction { StepNumber = 3, Description = "Return chicken to pot, add butter and salt. Cook until thick and creamy." },
        new Instruction { StepNumber = 4, Description = "Serve hot, drizzled with extra butter if desired." }
    },
    Category = "Main Dish",
    Servings = 8,
    PrepTime = 20,
    CookTime = 120,
    TotalTime = 140,
    Source = "Armenian Family Recipe",
    SourceURL = "https://www.armeniandish.com/harissa-armenian-wheat-chicken-porridge/",
    ImageURL = "https://soulandstreusel.com/wp-content/uploads/2020/05/CgWNma9RmW7ynwRS6LNbg_thumb_2eb7-735x747.jpg"
},

// Armenian Recipe 4: Gata (Armenian Sweet Bread)
new Recipe
{
    Name = "Gata (Armenian Sweet Bread)",
    RecipeIngredients = new List<RecipeIngredient>
    {
        new RecipeIngredient { IngredientName = "All-purpose flour", Quantity = 3, Unit = "cups", Aisle = "Baking", StorageLocation = "Pantry" },
        new RecipeIngredient { IngredientName = "Butter, softened", Quantity = 1, Unit = "cup", Aisle = "Dairy", StorageLocation = "Fridge" },
        new RecipeIngredient { IngredientName = "Sugar", Quantity = 1, Unit = "cup", Aisle = "Baking", StorageLocation = "Pantry" },
        new RecipeIngredient { IngredientName = "Eggs", Quantity = 2, Unit = "pcs", Aisle = "Dairy", StorageLocation = "Fridge" },
        new RecipeIngredient { IngredientName = "Baking powder", Quantity = 1, Unit = "tsp", Aisle = "Baking", StorageLocation = "Pantry" },
        new RecipeIngredient { IngredientName = "Vanilla extract", Quantity = 1, Unit = "tsp", Aisle = "Baking", StorageLocation = "Pantry" }
    },
    Instructions = new List<Instruction>
    {
        new Instruction { StepNumber = 1, Description = "Mix flour and baking powder. Beat butter and sugar until fluffy." },
        new Instruction { StepNumber = 2, Description = "Add eggs and vanilla to butter mixture. Gradually add flour mixture to form dough." },
        new Instruction { StepNumber = 3, Description = "Roll dough, fill with sweet filling if desired, and shape into rounds." },
        new Instruction { StepNumber = 4, Description = "Bake at 350°F (175°C) for 25-30 minutes until golden." }
    },
    Category = "Dessert",
    Servings = 10,
    PrepTime = 30,
    CookTime = 30,
    TotalTime = 60,
    Source = "Armenian Family Recipe",
    SourceURL = "https://www.armeniandish.com/gata-armenian-sweet-bread/",
    ImageURL = "https://mission-food.com/wp-content/uploads/2015/12/Gata-17-1024x683.jpg"
},
                    

                    // Ukrainian Recipe 1: Borscht
                    new Recipe
                    {
                        Name = "Borscht (Ukrainian Beet Soup)",
                        RecipeIngredients = new List<RecipeIngredient>
                        {
                            new RecipeIngredient { IngredientName = "Beets, peeled and grated", Quantity = 3, Unit = "pcs", Aisle = "Produce", StorageLocation = "Fridge" },
                            new RecipeIngredient { IngredientName = "Carrot, grated", Quantity = 1, Unit = "pcs", Aisle = "Produce", StorageLocation = "Fridge" },
                            new RecipeIngredient { IngredientName = "Potatoes, diced", Quantity = 3, Unit = "pcs", Aisle = "Produce", StorageLocation = "Pantry" },
                            new RecipeIngredient { IngredientName = "Cabbage, shredded", Quantity = 0.5m, Unit = "head", Aisle = "Produce", StorageLocation = "Fridge" },
                            new RecipeIngredient { IngredientName = "Onion, chopped", Quantity = 1, Unit = "pcs", Aisle = "Produce", StorageLocation = "Pantry" },
                            new RecipeIngredient { IngredientName = "Tomato paste", Quantity = 2, Unit = "tbsp", Aisle = "Canned Goods", StorageLocation = "Pantry" },
                            new RecipeIngredient { IngredientName = "Garlic, minced", Quantity = 2, Unit = "cloves", Aisle = "Produce", StorageLocation = "Pantry" },
                            new RecipeIngredient { IngredientName = "Vegetable oil", Quantity = 2, Unit = "tbsp", Aisle = "Oils", StorageLocation = "Pantry" },
                            new RecipeIngredient { IngredientName = "Beef broth", Quantity = 6, Unit = "cups", Aisle = "Canned Goods", StorageLocation = "Pantry" },
                            new RecipeIngredient { IngredientName = "Salt", Quantity = 0, Unit = "to taste", Aisle = "Spices", StorageLocation = "Pantry" },
                            new RecipeIngredient { IngredientName = "Black pepper", Quantity = 0, Unit = "to taste", Aisle = "Spices", StorageLocation = "Pantry" },
                            new RecipeIngredient { IngredientName = "Dill, chopped", Quantity = 2, Unit = "tbsp", Aisle = "Produce", StorageLocation = "Fridge" },
                            new RecipeIngredient { IngredientName = "Sour cream", Quantity = 0, Unit = "for serving", Aisle = "Dairy", StorageLocation = "Fridge" }
                        },
                        Instructions = new List<Instruction>
                        {
                            new Instruction { StepNumber = 1, Description = "Heat oil in a large pot. Sauté onion, carrot, and beets until soft." },
                            new Instruction { StepNumber = 2, Description = "Add tomato paste and garlic, cook for 2 minutes." },
                            new Instruction { StepNumber = 3, Description = "Pour in beef broth, add potatoes and cabbage. Bring to a boil." },
                            new Instruction { StepNumber = 4, Description = "Reduce heat and simmer until vegetables are tender, about 20 minutes." },
                            new Instruction { StepNumber = 5, Description = "Season with salt, pepper, and dill. Serve hot with sour cream." }
                        },
                        Category = "Soup",
                        Servings = 6,
                        PrepTime = 20,
                        CookTime = 40,
                        TotalTime = 60,
                        Source = "Ukrainian Family Recipe",
                        SourceURL = "https://ukrainian-recipes.com/borscht.html",
                        ImageURL = "https://cdn.pickuplimes.com/cache/aa/5f/aa5f79401e4e89308b3c35b18fa23cbf.jpg"
                    },

                    // Ukrainian Recipe 2: Varenyky (Pierogi)
                    new Recipe
                    {
                        Name = "Varenyky (Ukrainian Dumplings)",
                        RecipeIngredients = new List<RecipeIngredient>
                        {
                            new RecipeIngredient { IngredientName = "All-purpose flour", Quantity = 2, Unit = "cups", Aisle = "Baking", StorageLocation = "Pantry" },
                            new RecipeIngredient { IngredientName = "Egg", Quantity = 1, Unit = "pcs", Aisle = "Dairy", StorageLocation = "Fridge" },
                            new RecipeIngredient { IngredientName = "Water", Quantity = 0.5m, Unit = "cup", Aisle = "Beverages", StorageLocation = "Pantry" },
                            new RecipeIngredient { IngredientName = "Salt", Quantity = 0.5m, Unit = "tsp", Aisle = "Spices", StorageLocation = "Pantry" },
                            new RecipeIngredient { IngredientName = "Potatoes, peeled and diced", Quantity = 3, Unit = "pcs", Aisle = "Produce", StorageLocation = "Pantry" },
                            new RecipeIngredient { IngredientName = "Onion, chopped", Quantity = 1, Unit = "pcs", Aisle = "Produce", StorageLocation = "Pantry" },
                            new RecipeIngredient { IngredientName = "Butter", Quantity = 2, Unit = "tbsp", Aisle = "Dairy", StorageLocation = "Fridge" },
                            new RecipeIngredient { IngredientName = "Farmer's cheese", Quantity = 1, Unit = "cup", Aisle = "Dairy", StorageLocation = "Fridge" },
                            new RecipeIngredient { IngredientName = "Black pepper", Quantity = 0, Unit = "to taste", Aisle = "Spices", StorageLocation = "Pantry" }
                        },
                        Instructions = new List<Instruction>
                        {
                            new Instruction { StepNumber = 1, Description = "Mix flour, egg, water, and salt to form a soft dough. Let rest 30 minutes." },
                            new Instruction { StepNumber = 2, Description = "Boil potatoes until tender. Mash with butter, cheese, salt, and pepper." },
                            new Instruction { StepNumber = 3, Description = "Roll dough thin, cut circles. Place filling in center, fold and seal edges." },
                            new Instruction { StepNumber = 4, Description = "Boil varenyky in salted water until they float, about 3-4 minutes." },
                            new Instruction { StepNumber = 5, Description = "Serve with sautéed onions and sour cream." }
                        },
                        Category = "Main Dish",
                        Servings = 4,
                        PrepTime = 40,
                        CookTime = 20,
                        TotalTime = 60,
                        Source = "Ukrainian Family Recipe",
                        SourceURL = "https://ukrainian-recipes.com/varenyky.html",
                        ImageURL = "https://i2.wp.com/thesuburbansoapbox.com/wp-content/uploads/2016/12/Potato-Pierogies-6.jpg"
                    },

                    // Ukrainian Recipe 3: Holubtsi (Stuffed Cabbage Rolls)
                    new Recipe
                    {
                        Name = "Holubtsi (Stuffed Cabbage Rolls)",
                        RecipeIngredients = new List<RecipeIngredient>
                        {
                            new RecipeIngredient { IngredientName = "Green cabbage", Quantity = 1, Unit = "head", Aisle = "Produce", StorageLocation = "Fridge" },
                            new RecipeIngredient { IngredientName = "Ground pork", Quantity = 0.5m, Unit = "lb", Aisle = "Meat", StorageLocation = "Fridge" },
                            new RecipeIngredient { IngredientName = "Ground beef", Quantity = 0.5m, Unit = "lb", Aisle = "Meat", StorageLocation = "Fridge" },
                            new RecipeIngredient { IngredientName = "Cooked rice", Quantity = 1, Unit = "cup", Aisle = "Grains", StorageLocation = "Pantry" },
                            new RecipeIngredient { IngredientName = "Onion, chopped", Quantity = 1, Unit = "pcs", Aisle = "Produce", StorageLocation = "Pantry" },
                            new RecipeIngredient { IngredientName = "Carrot, grated", Quantity = 1, Unit = "pcs", Aisle = "Produce", StorageLocation = "Fridge" },
                            new RecipeIngredient { IngredientName = "Tomato sauce", Quantity = 2, Unit = "cups", Aisle = "Canned Goods", StorageLocation = "Pantry" },
                            new RecipeIngredient { IngredientName = "Salt", Quantity = 0, Unit = "to taste", Aisle = "Spices", StorageLocation = "Pantry" },
                            new RecipeIngredient { IngredientName = "Black pepper", Quantity = 0, Unit = "to taste", Aisle = "Spices", StorageLocation = "Pantry" }
                        },
                        Instructions = new List<Instruction>
                        {
                            new Instruction { StepNumber = 1, Description = "Boil cabbage, separate leaves. Trim thick veins." },
                            new Instruction { StepNumber = 2, Description = "Mix pork, beef, rice, onion, carrot, salt, and pepper." },
                            new Instruction { StepNumber = 3, Description = "Place filling on each leaf, roll up, tucking in sides." },
                            new Instruction { StepNumber = 4, Description = "Arrange rolls in a baking dish, pour tomato sauce over." },
                            new Instruction { StepNumber = 5, Description = "Cover and bake at 350°F for 1 hour. Serve hot." }
                        },
                        Category = "Main Dish",
                        Servings = 6,
                        PrepTime = 40,
                        CookTime = 60,
                        TotalTime = 100,
                        Source = "Ukrainian Family Recipe",
                        SourceURL = "https://ukrainian-recipes.com/holubtsi.html",
                        ImageURL = "https://tacataca.prosport.ro/wp-content/uploads/2022/06/Ukrainian-Cabbage-Rolls-Recipe-This-Traditional-Holubtsi-R-40439-56316253e5-1646770653.jpg"
                    },

                    // Ukrainian Recipe 4: Deruny (Potato Pancakes)
                    new Recipe
                    {
                        Name = "Deruny (Ukrainian Potato Pancakes)",
                        RecipeIngredients = new List<RecipeIngredient>
                        {
                            new RecipeIngredient { IngredientName = "Potatoes, peeled and grated", Quantity = 4, Unit = "pcs", Aisle = "Produce", StorageLocation = "Pantry" },
                            new RecipeIngredient { IngredientName = "Egg", Quantity = 1, Unit = "pcs", Aisle = "Dairy", StorageLocation = "Fridge" },
                            new RecipeIngredient { IngredientName = "Onion, grated", Quantity = 1, Unit = "pcs", Aisle = "Produce", StorageLocation = "Pantry" },
                            new RecipeIngredient { IngredientName = "All-purpose flour", Quantity = 2, Unit = "tbsp", Aisle = "Baking", StorageLocation = "Pantry" },
                            new RecipeIngredient { IngredientName = "Salt", Quantity = 0.5m, Unit = "tsp", Aisle = "Spices", StorageLocation = "Pantry" },
                            new RecipeIngredient { IngredientName = "Black pepper", Quantity = 0, Unit = "to taste", Aisle = "Spices", StorageLocation = "Pantry" },
                            new RecipeIngredient { IngredientName = "Vegetable oil", Quantity = 0, Unit = "for frying", Aisle = "Oils", StorageLocation = "Pantry" },
                            new RecipeIngredient { IngredientName = "Sour cream", Quantity = 0, Unit = "for serving", Aisle = "Dairy", StorageLocation = "Fridge" }
                        },
                        Instructions = new List<Instruction>
                        {
                            new Instruction { StepNumber = 1, Description = "Mix grated potatoes, onion, egg, flour, salt, and pepper." },
                            new Instruction { StepNumber = 2, Description = "Heat oil in skillet. Drop spoonfuls of batter, flatten slightly." },
                            new Instruction { StepNumber = 3, Description = "Fry until golden brown on both sides, about 3-4 minutes per side." },
                            new Instruction { StepNumber = 4, Description = "Drain on paper towels. Serve hot with sour cream." }
                        },
                        Category = "Breakfast",
                        Servings = 4,
                        PrepTime = 15,
                        CookTime = 20,
                        TotalTime = 35,
                        Source = "Ukrainian Family Recipe",
                        SourceURL = "https://ukrainian-recipes.com/deruny.html",
                        ImageURL = "https://www.heathrow.com/content/dam/heathrow/web/common/images/aspect-ratio-16-9/image/blog/pancake-day/Pancake-Day-497277474.jpg/jcr:content/renditions/cq5dam.web.1680.945.jpeg"
                    }
                };



                foreach (var recipe in hardcodedRecipes)
                {
                    Recipes.Add(recipe);
                    AllRecipes.Add(recipe);
                }
                ApplyFilter();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading recipes: {ex.Message}");
            }
        }

        private void ApplyFilter()
        {
            FilteredRecipes.Clear();

            if (IsAllRecipesSelected)
            {
                foreach (var recipe in AllRecipes)
                {
                    FilteredRecipes.Add(recipe);
                }
            }
            else
            {
                foreach (var recipe in Recipes.Where(r => r.IsFavorite))
                {
                    FilteredRecipes.Add(recipe);
                }
            }
            OnPropertyChanged(nameof(FilteredRecipes));
        }

        public void UpdatedFilteredRecipes()
        {
            ApplyFilter();
        }

        private void ToggleFavorite(Recipe recipe)
        {
            if (recipe != null)
            {
                recipe.IsFavorite = !recipe.IsFavorite;

                ApplyFilter();

                OnPropertyChanged(nameof(Recipes));
                OnPropertyChanged(nameof(FavoriteRecipes));
            }
        }
        private void SwitchToAllRecipes()
        {
            IsAllRecipesSelected = true;
            ApplyFilter();
        }

        private void SwitchToFavoriteRecipes()
        {
            IsAllRecipesSelected = false;
            ApplyFilter();
        }

        private void IncreaseServings()
        {
            if (SelectedRecipe != null)
            {
                SelectedRecipe.Servings++;
                OnPropertyChanged(nameof(SelectedRecipe));
            }
        }
        private void DecreaseServings()
        {
            if (SelectedRecipe != null && SelectedRecipe.Servings > 1)
            {
                SelectedRecipe.Servings--;
                OnPropertyChanged(nameof(SelectedRecipe));
            }
        }
        private void OpenSourceUrl()
        {
            if (!string.IsNullOrEmpty(SelectedRecipe?.SourceURL))
            {
                try
                {
                    Uri uri = new Uri(SelectedRecipe.SourceURL);
                    Browser.OpenAsync(uri, BrowserLaunchMode.SystemPreferred);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error opening URL: {ex.Message}");
                }
            }
        }

        private async void NavigateBack()
        {
            try
            {
                // Check the current page route
                var currentRoute = Shell.Current.CurrentState.Location.ToString();

                // If the current route is RecipeSelectionPage, go back to MealPlanPage
                if (currentRoute.Contains("RecipeSelectionPage"))
                {
                    await Shell.Current.GoToAsync("//MealPlanPage");
                }
                // If the current route is RecipeDetailPage, go back to RecipePage
                else if (currentRoute.Contains("RecipeDetailPage"))
                {
                    await Shell.Current.GoToAsync("//RecipePage");
                }
                // If the current route is AddRecipePage, go back to RecipePage
                else if (currentRoute.Contains("AddRecipePage"))
                {
                    await Shell.Current.GoToAsync("//RecipePage");
                }
                // Fallback to default (RecipePage) if no conditions are met
                else
                {
                    await Shell.Current.GoToAsync("//RecipePage");
                }
            }
            catch (Exception ex)
            {
                // Log the exception for debugging purposes
                Console.WriteLine($"NavigateBack exception: {ex.Message}");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
