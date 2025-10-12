using Marten.Schema;

namespace Catalog.API.Data
{
    public class CatalogInitialData : IInitialData
    {
        public async Task Populate(IDocumentStore store, CancellationToken cancellation)
        {
            using var session = store.LightweightSession();

            if (await session.Query<Product>().AnyAsync(cancellation)) return;

            // Marten upsert will carter for existing records
            session.Store<Product>(GetPreconfiguredProducts);
            await session.SaveChangesAsync(cancellation);
        }

        private static List<Product> GetPreconfiguredProducts =>
        [
            new()
            {
                Id = Guid.Parse("0f3a2b1c-4d5e-6a7b-8c9d-0e1f2a3b4c5d"),
                Name = "Wireless Noise-Canceling Headphones",
                Category = new() { "Electronics", "Audio" },
                Description = "Over-ear Bluetooth headphones with ANC and 30-hour battery.",
                ImageFile = "https://example.com/images/headphones-wnc.jpg",
                Price = 199.99m
            },
            new()
            {
                Id = Guid.Parse("1a2b3c4d-5e6f-7081-92a3-b4c5d6e7f801"),
                Name = "Stainless Steel Water Bottle 32oz",
                Category = new() { "Sports", "Outdoors" },
                Description = "Double-wall insulated bottle keeps drinks cold for 24h.",
                ImageFile = "https://example.com/images/bottle-32oz.jpg",
                Price = 24.50m
            },
            new()
            {
                Id = Guid.Parse("2b3c4d5e-6f70-8192-a3b4-c5d6e7f8090a"),
                Name = "Mechanical Keyboard (TKL)",
                Category = new() { "Electronics", "Peripherals" },
                Description = "Tenkeyless mechanical keyboard with hot-swappable switches.",
                ImageFile = "https://example.com/images/keyboard-tkl.jpg",
                Price = 89.00m
            },
            new()
            {
                Id = Guid.Parse("3c4d5e6f-7081-92a3-b4c5-d6e7f8090a1b"),
                Name = "Smart LED Desk Lamp",
                Category = new() { "Home", "Office" },
                Description = "Adjustable color temperature, USB-C charging port.",
                ImageFile = "https://example.com/images/desk-lamp-smart.jpg",
                Price = 39.99m
            },
            new()
            {
                Id = Guid.Parse("4d5e6f70-8192-a3b4-c5d6-e7f8090a1b2c"),
                Name = "Nonstick Frying Pan 12\"",
                Category = new() { "Home", "Kitchen" },
                Description = "PFOA-free nonstick aluminum pan with ergonomic handle.",
                ImageFile = "https://example.com/images/frying-pan-12.jpg",
                Price = 34.95m
            },
            new()
            {
                Id = Guid.Parse("5e6f7081-92a3-b4c5-d6e7-f8090a1b2c3d"),
                Name = "Premium Whole Bean Coffee 2lb",
                Category = new() { "Grocery", "Beverages" },
                Description = "Medium roast, notes of cocoa and citrus, ethically sourced.",
                ImageFile = "https://example.com/images/coffee-2lb.jpg",
                Price = 22.99m
            },
            new()
            {
                Id = Guid.Parse("6f708192-a3b4-c5d6-e7f8-090a1b2c3d4e"),
                Name = "All-Weather Running Shoes",
                Category = new() { "Fashion", "Men" },
                Description = "Lightweight, breathable upper with grippy outsole.",
                ImageFile = "https://example.com/images/running-shoes-men.jpg",
                Price = 74.90m
            },
            new()
            {
                Id = Guid.Parse("708192a3-b4c5-d6e7-f809-0a1b2c3d4e5f"),
                Name = "Ceramic Planter Set (3-pack)",
                Category = new() { "Home", "Garden" },
                Description = "Matte finish planters with drainage trays.",
                ImageFile = "https://example.com/images/planters-3pack.jpg",
                Price = 29.99m
            },
            new()
            {
                Id = Guid.Parse("8192a3b4-c5d6-e7f8-090a-1b2c3d4e5f60"),
                Name = "Board Game: Expedition Quest",
                Category = new() { "Toys & Games", "Board Games" },
                Description = "Cooperative strategy game for 2–5 players, 60–90 min.",
                ImageFile = "https://example.com/images/boardgame-expedition.jpg",
                Price = 49.95m
            },
            new()
            {
                Id = Guid.Parse("92a3b4c5-d6e7-f809-0a1b-2c3d4e5f6071"),
                Name = "Vitamin C Serum 30ml",
                Category = new() { "Beauty", "Skincare" },
                Description = "Brightening serum with hyaluronic acid and vitamin E.",
                ImageFile = "https://example.com/images/serum-vitamin-c.jpg",
                Price = 18.75m
            },
            new()
            {
                Id = Guid.Parse("a3b4c5d6-e7f8-090a-1b2c-3d4e5f607182"),
                Name = "USB-C GaN Charger 65W",
                Category = new() { "Electronics", "Charging" },
                Description = "Compact PD charger with two USB-C ports.",
                ImageFile = null, // optional image
                Price = 39.00m
            },
            new()
            {
                Id = Guid.Parse("b4c5d6e7-f809-0a1b-2c3d-4e5f60718293"),
                Name = "Leather Notebook (A5)",
                Category = new() { "Office", "Stationery" },
                Description = "192 pages, dot grid, lay-flat binding.",
                ImageFile = null, // optional image
                Price = 16.49m
            }
        ];
    }
}
