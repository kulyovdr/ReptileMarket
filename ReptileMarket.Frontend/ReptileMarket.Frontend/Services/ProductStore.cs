namespace ReptileMarket.Frontend.Services
{
    public record DemoProduct(int Id, string Species, string Morph, int Price, string City, string Sex, string Age, string Description);

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
                Products.Add(new DemoProduct(
                    i,
                    SpeciesList[(i - 1) % SpeciesList.Length],
                    "Normal",
                    3000 + i * 250,
                    CityList[(i - 1) % CityList.Length],
                    i % 2 == 0 ? "Самка" : "Самец",
                    $"{1 + i % 5} мес",
                    "Уникальные гены от проверенных производителей. Питомник HansoGeckon"));

                if (i <= 16)
                {
                    _favorites.Add(i);
                }
            }
        }

        public bool IsFavorite(int id) => _favorites.Contains(id);

        public List<DemoProduct> GetFavorites() => Products.Where(p => _favorites.Contains(p.Id)).ToList();

        public DemoProduct? Get(int id) => Products.FirstOrDefault(p => p.Id == id);

        public void ToggleFavorite(int id)
        {
            if (!_favorites.Add(id))
            {
                _favorites.Remove(id);
            }

            Changed?.Invoke();
        }
    }
}
