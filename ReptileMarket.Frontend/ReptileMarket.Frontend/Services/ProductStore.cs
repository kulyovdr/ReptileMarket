namespace ReptileMarket.Frontend.Services
{
    public record DemoProduct(int Id, string Species, string Morph, int Price, string City, string Address, string Sex, string Age, string Description, DateTime HatchDate,
        string? Diet = null, string? Breeding = null, string? Shipping = null, string? Exchange = null, string? Weight = null, DateTime? Published = null);

    public class ProductStore
    {
        private static readonly string[] SpeciesList =
        [
            "Гемитеконникс", "Эублефар", "Бананоед", "Фельзума", "Королевский питон",
            "Бородатая агама", "Маисовый полоз", "Йеменский хамелеон", "Королевская змея",
            "Токи", "Нефрус", "Черепахи"
        ];

        private static readonly string[] CityList = ["Москва", "Санкт-Петербург", "Казань", "Екатеринбург", "Новосибирск", "Краснодар"];

        public List<DemoProduct> Products { get; } = [];

        private readonly HashSet<int> _favorites = [];

        public event Action? Changed;

        public ProductStore()
        {
            for (var i = 1; i <= 30; i++)
            {
                var species = SpeciesList[(i - 1) % SpeciesList.Length];
                var snake = IsSnake(species);

                Products.Add(new DemoProduct(
                    i,
                    species,
                    "Normal",
                    Price: 3000 + i * 250,
                    City: CityList[(i - 1) % CityList.Length],
                    Address: $"г. {CityList[(i - 1) % CityList.Length]}, ул. Пушкина, д. {i}",
                    Sex: i % 2 == 0 ? "Самка" : "Самец",
                    Age: $"{1 + i % 5} мес",
                    Description: "Уникальные гены от проверенных производителей. Питомник HansoGeckon",
                    HatchDate: DateTime.Today.AddDays(-(25 + i * 19)),
                    Diet: DietFor(species, i),
                    Breeding: i % 2 == 0 ? "Собственное" : "Несобственное",
                    Shipping: i % 3 == 0 ? "Только самовывоз" : "Есть отправка",
                    Exchange: i % 4 == 0 ? "Нет" : "Есть",
                    Weight: (snake ? 120 + i * 30 : 35 + i * 7).ToString(),
                    Published: DateTime.Today.AddDays(-(i % 21))));

                if (i <= 16)
                {
                    _favorites.Add(i);
                }
            }
        }

        private static readonly string[] SnakeSpeciesList = ["Королевский питон", "Маисовый полоз", "Королевская змея"];
        private static readonly string[] SnakeDiets = ["Крыса", "Мышь", "Птица", "Другое"];
        private static readonly string[] LizardDiets = ["Насекомые", "Пюре"];

        private static bool IsSnake(string species) => SnakeSpeciesList.Contains(species);

        private static string DietFor(string species, int i) => species switch
        {
            var s when IsSnake(s) => SnakeDiets[i % SnakeDiets.Length],
            "Черепахи" => "Другое",
            _ => LizardDiets[i % LizardDiets.Length]
        };

        public bool IsFavorite(int id) => _favorites.Contains(id);

        public List<DemoProduct> GetFavorites() => Products.Where(p => _favorites.Contains(p.Id)).ToList();

        public DemoProduct? Get(int id) => Products.FirstOrDefault(p => p.Id == id);

        public DemoProduct Add(DemoProduct product)
        {
            var id = Products.Count == 0 ? 1 : Products.Max(p => p.Id) + 1;
            var saved = product with { Id = id, Published = product.Published ?? DateTime.Today };
            Products.Add(saved);
            Changed?.Invoke();
            return saved;
        }

        public void Update(DemoProduct product)
        {
            var index = Products.FindIndex(p => p.Id == product.Id);
            if (index < 0)
            {
                return;
            }

            Products[index] = product;
            Changed?.Invoke();
        }

        public void ToggleFavorite(int id)
        {
            if (!_favorites.Add(id))
            {
                _favorites.Remove(id);
            }

            Changed?.Invoke();
        }

        public void Delete(int id)
        {
            Products.RemoveAll(p => p.Id == id);
            _favorites.Remove(id);
            Changed?.Invoke();
        }
    }
}
