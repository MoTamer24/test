namespace WebApplication1
{
    public class BasketItem
    {
       
            public int Id { get; set; }
            public string Name { get; set; }
            public double Price { get; set; }

            public BasketItem(int id, string name, double price)
            {
                Id = id;
                Name = name;
                Price = price;
            }
        
    }
}
