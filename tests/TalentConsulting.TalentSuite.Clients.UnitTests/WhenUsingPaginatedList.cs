using FluentAssertions;
using TalentConsulting.TalentSuite.Clients.Common;

namespace TalentConsulting.TalentSuite.Clients.UnitTests;

public class WhenUsingPaginatedList
{
    [Fact]
    public void PaginatedList_Constructor_Sets_Properties_Correctly()
    {
        // Arrange
        var items = new List<int> { 1, 2, 3 };
        var pageNumber = 1;
        var count = 10;
        var pageSize = 3;

        // Act
        var paginatedList = new PaginatedList<int>(items, count, pageNumber, pageSize);

        // Assert
        items.Should().BeEquivalentTo(paginatedList.Items);
        pageNumber.Should().Be(paginatedList.PageNumber);
        Math.Ceiling(count / (double)pageSize).Should().Be(paginatedList.TotalPages);
        count.Should().Be(paginatedList.TotalCount);
    }

    [Theory]
    [InlineData(1, 3, 3, false, false)] // First page, 3 items, page size 3
    [InlineData(2, 3, 3, true, false)] // Second page, 3 items, page size 3
    [InlineData(1, 10, 3, false, true)] // First page, 10 items, page size 3
    [InlineData(2, 10, 3, true, true)] // Second page, 10 items, page size 3
    [InlineData(1, 2, 3, false, false)] // First page, 2 items, page size 3
    public void PaginatedList_HasPreviousPage_And_HasNextPage_Returns_Correct_Values(int pageNumber, int count, int pageSize, bool expectedHasPreviousPage, bool expectedHasNextPage)
    {
        // Arrange
        var items = Enumerable.Range(1, count).ToList();
        var paginatedList = new PaginatedList<int>(items, count, pageNumber, pageSize);

        // Act
        var hasPreviousPage = paginatedList.HasPreviousPage;
        var hasNextPage = paginatedList.HasNextPage;

        // Assert
        expectedHasPreviousPage.Should().Be(hasPreviousPage);
        expectedHasNextPage.Should().Be(hasNextPage);
    }
}
