using IdeaGenerator;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Logic.UnitTests
{
    public class StringFunctions_Test
    {
        [Theory]
        [InlineData("Example text with a tag <tag>text between tags</tag>.", "<tag>", "</tag>", "text between tags")]
        [InlineData("<random tag> <startTag>data</endTag>", "<startTag>", "</endTag>", "data")]
        [InlineData("random string @important data@ ", "@", "@", "important data")]
        [InlineData("<12>4<6789>", "<12>", "<6789>", "4")]
        public void ReadTagInfo_ShouldRun(string text, string startTag, string endTag, string expected)
        {
            // Arrange

            // Act
            var result = text.ReadTagInfo(startTag, endTag);

            // Assert
            Assert.Equal(result.tag, expected);
        }
    }
}