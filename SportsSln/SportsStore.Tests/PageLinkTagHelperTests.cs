using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Moq;
using SportsStore.Models.ViewModels;
using SportsStore.Infrastructure;
using Xunit;

namespace SportsStore.Tests;

public class PageLinkTagHelperTests
{
    [Fact]
    public void Can_Generate_Page_Links()
    {
        // Arrange.
        Mock<IUrlHelper> urlHelper = new();
        urlHelper.SetupSequence(x => x.Action(It.IsAny<UrlActionContext>()))
            .Returns("Test/Page1")
            .Returns("Test/Page2")
            .Returns("Test/Page3");
        Mock<IUrlHelperFactory> urlHelperFactory = new();
        urlHelperFactory.Setup(f => f.GetUrlHelper(It.IsAny<ActionContext>()))
            .Returns(urlHelper.Object);
        Mock<ViewContext> viewContext = new();
        PageLinkTagHelper helper = new(urlHelperFactory.Object)
        {
            PageModel = new PagingInfo 
            {
                CurrentPage = 2,
                TotalItems = 28,
                ItemsPerPage = 10
            },
            ViewContext = viewContext.Object,
            PageAction = "Test"
        };
        TagHelperContext ctx = new TagHelperContext(
            new TagHelperAttributeList(),
            new Dictionary<object, object>(), "");
        var content = new Mock<TagHelperContent>();
        TagHelperOutput output = new TagHelperOutput("div", 
            new TagHelperAttributeList(),
            (cache, encoder) => Task.FromResult(content.Object));


        // Act.
        helper.Process(ctx, output);

        // Assert.
        Assert.Equal(@"<a href=""Test/Page1"">1</a>"
            + @"<a href=""Test/Page2"">2</a>"
            + @"<a href=""Test/Page3"">3</a>",
            output.Content.GetContent());
    }
}