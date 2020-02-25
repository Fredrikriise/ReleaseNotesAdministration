using Services;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace test.Services
{
    public class UtilsTests
    {
        [Fact]
        public void TestStuff()
        {
            var utils = new Utils();
            var name = "Fredrik";
            //Assert.Equal(name, utils.DoStuff("Fredrik"));
        }
    }
}
