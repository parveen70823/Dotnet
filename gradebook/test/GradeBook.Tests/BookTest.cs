namespace GradeBook.Tests;

public class BookTest
{
    [Fact]
    public void BookCalculatesAnAverageGrade()
    {
        /* 1. Arrange
        var x = 5; 
        var y = 2; 
        var expect = x*y;

        //2. Act
        var actual  = 10;

        //3. Assert
        Assert.Equal(expect,actual);
        */

        var book = new InMemoryBook("");
        book.AddGrade(70.6);
        book.AddGrade(80.3);
        book.AddGrade(91.6);

        var result = book.GetStatistics();

        Assert.Equal(80.83, result.Average, 2);
        Assert.Equal(70.6, result.Low,1);
        Assert.Equal(91.6, result.High,2);
        Assert.Equal( 'B', result.Letter);


        
    }
}
