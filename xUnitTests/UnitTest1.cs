namespace xUnitTests
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            #region Arrange
            MyMath myMath = new MyMath();
            int input1 = 10, input2 = 5, expectedValue1 = 15;
            #endregion

            #region Act
            int acutal = myMath.Add(input1, input2);
            #endregion

            #region Assert
            Assert.Equal(expectedValue1, acutal);
            #endregion

        }
    }
}