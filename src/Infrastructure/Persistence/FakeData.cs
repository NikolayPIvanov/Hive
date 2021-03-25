// using System.Collections.Generic;
// using Bogus;
// using Hive.Domain.Entities.Categories;
//
// namespace Hive.Infrastructure.Persistence
// {
//     public static class FakeData
//     {
//         public static List<Category> Categories = new();
//         
//         public static void Init(int count)
//         {
//             var baseCategoriesFaker = new Faker<Category>()
//                 .RuleFor(p => p.Title, f => f.Name.JobTitle());
//
//             var baseCategories = baseCategoriesFaker.Generate(count);
//             
//             Categories.AddRange(baseCategories);
//         }
//     }
// }