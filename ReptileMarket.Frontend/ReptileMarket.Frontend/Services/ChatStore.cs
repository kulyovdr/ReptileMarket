namespace ReptileMarket.Frontend.Services
{
    public record ChatMessage(bool FromMe, string Text, DateTime Time);

    public record Chat(int Id, string SellerName, string Species, List<ChatMessage> Messages, int ProductId);

    public class ChatStore
    {
        public List<Chat> Chats { get; } = [];

        public event Action? Changed;

        public ChatStore()
        {
            Chats.Add(new Chat(1, "Игорь", "Гемитеконникс",
            [
                new(false, "Здравствуйте! Гемитеконникс ещё в продаже?", DateTime.Today.AddDays(-2).AddHours(-5)),
                new(true, "Добрый день! Да, в наличии.", DateTime.Today.AddDays(-2).AddHours(-4)),
                new(false, "Какого он возраста?", DateTime.Today.AddDays(-2).AddHours(-3)),
                new(true, "Примерно 4 месяца, ест охотно.", DateTime.Today.AddDays(-2).AddHours(-2)),
                new(false, "Отправите в другой город?", DateTime.Today.AddHours(-20))
            ], 1));

            Chats.Add(new Chat(2, "Мария", "Эублефар",
            [
                new(true, "Здравствуйте! Можно забронировать эублефара?", DateTime.Today.AddDays(-1).AddHours(-6)),
                new(false, "Добрый день! Да, бронь на три дня.", DateTime.Today.AddDays(-1).AddHours(-5)),
                new(true, "Отлично, заберу в выходные.", DateTime.Today.AddDays(-1).AddHours(-4))
            ], 2));

            Chats.Add(new Chat(3, "Питомник HansoGecko", "Бананоед",
            [
                new(false, "Добрый день! У нас как раз свежие бананоеды, подсказать по ценам?", DateTime.Today.AddDays(-4))
            ], 3));

            Chats.Add(new Chat(4, "Алексей", "Королевский питон",
            [
                new(false, "Привет! Питон уже уехал, но есть похожий морф.", DateTime.Today.AddDays(-6)),
                new(true, "Скиньте фото, пожалуйста.", DateTime.Today.AddDays(-6).AddHours(2))
            ], 5));

            Chats.Add(new Chat(5, "Ольга", "Фельзума",
            [
                new(true, "Фельзума самец или самка?", DateTime.Today.AddDays(-8)),
                new(false, "Самка, уже откладывала яйца.", DateTime.Today.AddDays(-8).AddHours(1))
            ], 4));
        }

        public Chat? Get(int id) => Chats.FirstOrDefault(c => c.Id == id);

        public Chat GetOrCreate(int id, string sellerName, string species, int productId = 0)
        {
            var existing = Get(id);
            if (existing is not null)
            {
                return existing;
            }

            var chat = new Chat(id, sellerName, species, [], productId);
            Chats.Add(chat);
            Changed?.Invoke();
            return chat;
        }

        public void SendMessage(int id, string text)
        {
            Get(id)?.Messages.Add(new ChatMessage(true, text, DateTime.Now));
            Changed?.Invoke();
        }
    }

    public static class ChatFormat
    {
        public static string Time(DateTime time)
        {
            if (time.Date == DateTime.Today)
            {
                return time.ToString("HH:mm");
            }

            if (time.Date == DateTime.Today.AddDays(-1))
            {
                return "Вчера";
            }

            return time.ToString("dd.MM.yyyy");
        }
    }
}
