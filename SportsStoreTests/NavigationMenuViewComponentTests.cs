﻿using Microsoft.AspNetCore.Mvc.ViewComponents;
using Moq;
using SportsStore.Components;
using SportsStore.Models;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace SportsStoreTests
{
    public class NavigationMenuViewComponentTests
    {
        [Fact]
        public void Indicates_Selected_Category()
        {
            string categoryToSelect = "Cat2";

            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns((new Product[]
            {
                new Product { ProductID = 1, Name = "P1", Category = "Cat1"},
                new Product { ProductID = 2, Name = "P2", Category = "Cat2"},
                new Product { ProductID = 3, Name = "P3", Category = "Cat1"},
                new Product { ProductID = 4, Name = "P4", Category = "Cat2"},
                new Product { ProductID = 5, Name = "P5", Category = "Cat3"}
            }).AsQueryable());

            NavigationMenuViewComponent target = new NavigationMenuViewComponent(mock.Object);

            target.ViewComponentContext = new ViewComponentContext
            {
                ViewContext = new Microsoft.AspNetCore.Mvc.Rendering.ViewContext
                {
                    RouteData = new Microsoft.AspNetCore.Routing.RouteData()
                }
            };
            target.RouteData.Values["category"] = categoryToSelect;

            string result = (string)(target.Invoke() as ViewViewComponentResult).ViewData["SelectedCategory"];

            Assert.Equal(categoryToSelect, result);
        }
        [Fact]
        public void Can_Select_Categories()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns((new Product[]
            {
                new Product { ProductID = 1, Name = "P1", Category = "Cat1"},
                new Product { ProductID = 2, Name = "P2", Category = "Cat2"},
                new Product { ProductID = 3, Name = "P3", Category = "Cat1"},
                new Product { ProductID = 4, Name = "P4", Category = "Cat2"},
                new Product { ProductID = 5, Name = "P5", Category = "Cat3"}
            }).AsQueryable());

            NavigationMenuViewComponent target = new NavigationMenuViewComponent(mock.Object);

            string[] results = 
                ((IEnumerable<string>)
                    (target.Invoke() as ViewViewComponentResult).ViewData.Model).ToArray();

            Assert.True(Enumerable.SequenceEqual(new string[] { "Cat1", "Cat2", "Cat3" }, results));
        }
    }
}
