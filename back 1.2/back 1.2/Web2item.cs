namespace back_1._2
{
    public class Web2item
    {
        public int id { get; set; }
        public string name { get; set; }
        public string price { get; set; }

        public string image { get; set; }
        public string description { get; set; }
        public Web2item(int id, string name, string price, string description, string image)
        {
            this.id = id;
            this.name = name;
            this.price = price;
            this.image = image;
            this.description = description;
        }
    }
}
