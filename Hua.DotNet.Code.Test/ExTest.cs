using Hua.DotNet.Code.Ex;
using Hua.DotNet.Code.Test.Ex.Model;
using Xunit;

namespace Hua.DotNet.Code.Test
{
    public class ExTest
    {
        [Theory]
        [InlineData("男", Gender.Man)]
        [InlineData("女", Gender.WoMan)]
        public void DisplayNameTest(string result, object obj)
        {
            Assert.Equal(result, obj.DisplayName());

            foreach (var propertyInfo in typeof(Person).GetProperties())
            {
                switch (propertyInfo.Name)
                {
                    case "PersonId":
                        Assert.Equal("唯一标识", propertyInfo.DisplayName());
                        break;
                    case "Name":
                        Assert.Equal("姓名", propertyInfo.DisplayName());
                        break;
                    case "Tel":
                        Assert.Equal("电话", propertyInfo.DisplayName());
                        break;
                    default:
                        break;
                }
            }
        }

        [Fact]
        public void DescriptionTest()
        {
            Assert.Equal("男", Gender.Man.DisplayName());
            Assert.Equal("人", new Person().Description());
            foreach (var propertyInfo in typeof(Person).GetProperties())
            {
                switch (propertyInfo.Name)
                {
                    case "PersonId":
                        Assert.Equal("唯一标识", propertyInfo.Description());
                        break;
                    case "Name":
                        Assert.Equal("姓名", propertyInfo.Description());
                        break;
                    case "Tel":
                        Assert.Equal("电话", propertyInfo.Description());
                        break;
                    default:
                        break;
                }
            }
        }


        [Theory]
        [InlineData("TrimStart", "测试TrimStart", "测试")]
        public void TrimStartTest(string result, string src, string searchStr)
        {
            Assert.Equal(result, src.TrimStartStr(searchStr));
        }

        [Theory]
        [InlineData("TrimEndStr", "TrimEndStr测试", "测试")]
        public void TrimEndStrTest(string result, string src, string searchStr)
        {
            Assert.Equal(result, src.TrimEndStr(searchStr));
        }

        [Fact]
        public void TypeTest()
        {
            var type = typeof(Person);
            foreach (var propertyInfo in type.GetProperties())
            {
                switch (propertyInfo.Name)
                {
                    case "PersonId":
                        Assert.Equal("唯一标识", propertyInfo.Description());
                        break;
                    case "Name":
                        Assert.Equal("姓名", propertyInfo.Description());
                        break;
                    case "Tel":
                        Assert.Equal("电话", propertyInfo.Description());
                        break;
                    default:
                        break;
                }
            }
        }
    }
}