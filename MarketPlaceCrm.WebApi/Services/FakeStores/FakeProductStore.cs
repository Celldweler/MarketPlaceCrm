using System;
using System.Linq;
using System.Collections.Generic;
using Bogus;
using MarketPlaceCrm.WebApi.ViewModels.ProductsDtos;

namespace MarketPlaceCrm.WebApi.Services.FakeStores
{
    public class FakeProductStore
    {
        private readonly List<ProductDto> _fakeProducts;

        public FakeProductStore()
        {
            var fakeId = 1;
            _fakeProducts = new Faker<ProductDto>().RuleFor(o => o.Id, x => fakeId++)
                .RuleFor(o => o.Name, x => x.Lorem.Sentence(5))
                .RuleFor(o => o.Description, x => x.Lorem.Sentence(10, 15))
                .RuleFor(o => o.Price, GetRoundPrice)
                .RuleFor(o => o.Qty, x => x.Random.Int(1, 10))
                .Generate(10);
        }
        private static decimal GetRoundPrice(Faker value) =>
            decimal.Round(value.Random.Decimal(10.00m, 100.00m), 2, MidpointRounding.AwayFromZero);

        public List<ProductDto> All { get => _fakeProducts; }

        private int GenerateUniqueId() => _fakeProducts.Count > 0 ? _fakeProducts.Select(x => x.Id).Max() + 1 : 1;
       
        public bool Add(CreateProductForm productForm)
        {
            _fakeProducts.Add(new ProductDto
            {
                Id = GenerateUniqueId(),
                Name = productForm.Name,
                Description = productForm.Description,
                Price = productForm.Price,
            });

            return true;
        }

        public bool Remove(int id)
        {
            return true;
        }

        public bool Update(EditProductForm editProductForm)
        {

            return true;
        }
    }
}