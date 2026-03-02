namespace MigraziiRabotaitePj.DTO
{
    public class ProductReadDTO
    {
        public Guid Id { get; set; }

        public string? Brand { get; set; }
        public string? Model { get; set; }
        public string? Description { get; set; }
        public string? FK_Salesman { get; set; }

        public decimal? Price { get; set; }
        public int Quantity { get; set; }
        public string? Currency { get; set; }

        public CharacteristicsDTO? Characteristics { get; set; }
    }
}
