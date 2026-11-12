namespace ReptileMarket.Frontend.Services
{
    public class Seller
    {
        public Seller(int id, string name, string city, string bio, int joined)
        {
            Id = id;
            Name = name;
            City = city;
            Bio = bio;
            Joined = joined;
        }

        public int Id { get; init; }
        public string Name { get; init; }
        public string City { get; init; }
        public string Bio { get; set; }
        public int Joined { get; init; }
        public string? PhotoUrl { get; set; }
    }

    public record DemoProduct(int Id, string Species, string Morph, int Price, string City, string Address, string Sex, string Age, string Description, DateTime HatchDate,
        string? Diet = null, string? Breeding = null, string? Shipping = null, string? Exchange = null, string? Weight = null, DateTime? Published = null, int SellerId = 0);

    public class ProductStore
    {
        private static readonly string[] SpeciesList =
        [
            "Гемитеконникс", "Эублефар", "Бананоед", "Фельзума", "Королевский питон",
            "Бородатая агама", "Маисовый полоз", "Йеменский хамелеон", "Королевская змея",
            "Токи", "Нефрус", "Черепахи"
        ];

        private static readonly string[] CityList = ["Москва", "Санкт-Петербург", "Казань", "Екатеринбург", "Новосибирск", "Краснодар"];

        public List<Seller> Sellers { get; } =
        [
            new(1, "Игорь", "Москва", "Развожу гемитеконниксов и эублефаров седьмой год. Все животные выращены у меня, кормлю качественными кормами, ветеринар рядом. Помогу с выбором и советом по содержанию.", 2020),
            new(2, "Мария", "Санкт-Петербург", "Небольшой домашний питомник эублефаров и бананоедов. Каждый малыш с паспортом и фото с рождения. Отправляю по всей России надёжными перевозчиками.", 2021),
            new(3, "Питомник HansoGecko", "Казань", "Профессиональный питомник гекконов: фельзумы, токи, нефрус. Работаем с 2018 года, более 200 успешных сделок. Даём гарантию здоровья на 30 дней и сопровождаем новичков.", 2018),
            new(4, "Ольга", "Екатеринбург", "Развожу хамелеонов и агам уже пять лет. Йеменские хамелеоны моего разведения, приручены к рукам. Подскажу по обустройству террариума под ключ.", 2021),
            new(5, "Алексей", "Новосибирск", "Коллекционный развод питонов и полозов. Специализируюсь на редких морфах королевского питона. Обмен с доплатой рассмотрю, отправка только с термосумкой.", 2019),
            new(6, "Дмитрий", "Краснодар", "Агамы и агамоводство: бородатые, круглоголовые, водяные. Свой инсектарий, свежие корма круглый год. Возможна доставка по югу России.", 2022),
            new(7, "Светлана", "Москва", "Черепахи — моя страсть. Среднеазиатские, бирманские, водяные. Помогу с оформлением документов и первичным осмотром у герпетолога.", 2020),
            new(8, "Питомник GeckoHouse", "Санкт-Петербург", "GeckoHouse — семейный питомник. Эублефары, маисовые полозы, королевские змеи. Ведро с малышами каждый сезон. Отзывы и фото родителей по запросу.", 2019)
        ];

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
                    Published: DateTime.Today.AddDays(-(i % 21)),
                    SellerId: 1 + (i - 1) % Sellers.Count));

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

        public Seller? GetSeller(int id) => Sellers.FirstOrDefault(s => s.Id == id);

        public List<DemoProduct> GetBySeller(int sellerId) => Products.Where(p => p.SellerId == sellerId).ToList();

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

        public void UpdateSellerProfile(int sellerId, string bio, string? photoUrl, bool removePhoto = false)
        {
            var seller = GetSeller(sellerId);
            if (seller is null)
            {
                return;
            }

            seller.Bio = bio;
            if (removePhoto)
            {
                seller.PhotoUrl = null;
            }
            else if (photoUrl is not null)
            {
                seller.PhotoUrl = photoUrl;
            }

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
