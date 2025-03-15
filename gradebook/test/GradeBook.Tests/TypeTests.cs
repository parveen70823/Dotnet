
using Microsoft.VisualStudio.TestPlatform.ObjectModel;

namespace GradeBook.Tests;

public delegate string WriteLogDelegate(string logMessage);
public class TypeTests
{
    int count = 0;
    [Fact]
    public void WriteLogDelegateCanPointToMethod()
    {
        // WriteLogDelegate  log;
        // log = ReturnMessage;
        // var result= log("Hello");
        // Assert.Equal("Hello", result);

        WriteLogDelegate  log= ReturnMessage;
        log += ReturnMessage;
        log += IncrementCount;
        var result= log("Hello");
        Assert.Equal(3, count);

    }

    string IncrementCount(string message)
    {
        count ++;
        return message.ToLower();
    }
    string ReturnMessage(string message)
    {
        count++;
        return message;
    }
    // <---------------------------->
    [Fact]
    public void StringBehavesLikeValueType()
    {
        string name = "cott";
        var upper= MakeUpperCase(name);
        Assert.Equal("cott",name);
        Assert.Equal("COTT",upper);
    }

    private string MakeUpperCase(string par)
    {
        return par.ToUpper();
    }

    // <------------------------------>
    [Fact]
    public void ValueTypeAlsoPassByValue()
    {
        var x = GetInt();
        SetInt(ref x);
        Assert.Equal(50,x);
    }

    private void SetInt(ref int x)
    {
        x = 50;
    }

    private int GetInt()
    {
        return 3;
    }

    // <------------------------------>
    [Fact]
    public void CSharpCanPassByRef()
    {
        var book1 = GetBook("Book 1");
        GetBookSetName2(ref book1, "New Book");
        // GetBookSetName2(out book1, "New Book");
        // with out parametere we always have to initailize out object because it assumes it is not benn initailized

        Assert.Equal("New Book", book1.Name);
    }

    private void GetBookSetName2(ref InMemoryBook book, string name)
    {
        book = new InMemoryBook(name);
    }
    // <------------------------------>
    [Fact]
    public void CSharpPassByVal()
    {
        var book1 = GetBook("Book 1");
        GetBookSetName(book1, "New Book");
        Assert.Equal("Book 1", book1.Name);
    }

    private void GetBookSetName(InMemoryBook book, string name)
    {
        book = new InMemoryBook(name);
    }
    // <------------------------------>
    [Fact]
    public void CanSetNameFromReference()
    {
        var book1 = GetBook("Book 1");
        SetName(book1, "New Book");
        Assert.Equal("New Book", book1.Name);
    }

    private void SetName(InMemoryBook book, string name)
    {
        book.Name = name;
    }
    // <------------------------------>

    [Fact]
    public void GetBookReturnsDifferentObjects()
    {
        var book1 = GetBook("Book 1"); 
        var book2 = GetBook("Book 2"); 

        Assert.Equal("Book 1", book1.Name);
        Assert.Equal("Book 2", book2.Name);
        Assert.NotSame(book1, book2);
    }

    // <------------------------------>
    [Fact]
    public void TwoVarCanReferSameObject()
    {
        var book1 = GetBook("Book 1");
        var book2 = book1;
        Assert.Same(book1, book2);
        Assert.True(Object.ReferenceEquals(book1, book2));
    }

    InMemoryBook GetBook(string name)
    {
        return new InMemoryBook(name);
    }
    // <------------------------------>
}
