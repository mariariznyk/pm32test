using AnalaizerClassLibrary;
using ErrorLibrary;

namespace UnitTests
{
    [TestFixture]
    public class AnalaizerClassTests
    {
        [SetUp]
        public void Setup()
        {
            AnalaizerClass.expression = string.Empty;
        }

        [Test]
        public void Format_ShouldReturnEmptyString_WhenExpressionIsEmpty()
        {
            AnalaizerClass.expression = "";
            string result = AnalaizerClass.Format();
            Assert.AreEqual("", result);
        }

        [Test]
        public void Format_ShouldReturnError_WhenExpressionExceedsMaxLength()
        {
            AnalaizerClass.expression = new string('1', 65537); 
            string result = AnalaizerClass.Format();
            Assert.AreEqual("&" + ErrorsExpression.ERROR_07, result);
        }

        [Test]
        public void Format_ShouldReturnError_WhenExpressionContainsUnknownOperator()
        {
            AnalaizerClass.expression = "5+3a";
            string result = AnalaizerClass.Format();
            Assert.IsTrue(result.StartsWith("&" + ErrorsExpression.GetFullStringError(ErrorsExpression.ERROR_02, 3)));
        }

        [Test]
        public void Format_ShouldReturnError_WhenExpressionStartsWithInvalidSymbol()
        {
            AnalaizerClass.expression = "*3+5";
            string result = AnalaizerClass.Format();
            Assert.AreEqual("&" + ErrorsExpression.ERROR_03, result);
        }

        [Test]
        public void Format_ShouldReturnError_WhenExpressionEndsWithInvalidSymbol()
        {
            AnalaizerClass.expression = "3+5+";
            string result = AnalaizerClass.Format();
            Assert.AreEqual("&" + ErrorsExpression.ERROR_05, result);
        }

        [Test]
        public void Format_ShouldReturnError_WhenTwoOperatorsAreInSequence()
        {
            AnalaizerClass.expression = "5++3";
            string result = AnalaizerClass.Format();
            Assert.IsTrue(result.StartsWith("&" + ErrorsExpression.GetFullStringError(ErrorsExpression.ERROR_04, 2)));
        }

        [Test]
        public void Format_ShouldReturnError_WhenUnaryOperatorIsFollowedByInvalidSymbol()
        {
            AnalaizerClass.expression = "-*3+5";
            string result = AnalaizerClass.Format();
            Assert.AreEqual("&" + ErrorsExpression.ERROR_03, result);
        }

        [Test]
        public void Format_ShouldReturnError_WhenCloseBracketIsFollowedByInvalidSymbol()
        {
            AnalaizerClass.expression = "(3+5)2";
            string result = AnalaizerClass.Format();
            Assert.AreEqual("&" + ErrorsExpression.ERROR_03, result);
        }

        [Test]
        public void Format_ShouldReturnError_WhenOpenBracketIsFollowedByInvalidSymbol()
        {
            AnalaizerClass.expression = "(+3+5)";
            string result = AnalaizerClass.Format();
            Assert.AreEqual("&" + ErrorsExpression.ERROR_03, result);
        }

        [Test]
        public void Format_ShouldReturnError_WhenUnaryOperatorIsAtEndOfExpression()
        {
            AnalaizerClass.expression = "3+5-";
            string result = AnalaizerClass.Format();
            Assert.AreEqual("&" + ErrorsExpression.ERROR_05, result);
        }

        [Test]
        public void Format_ShouldReturnFormattedExpression_WhenExpressionIsValid()
        {
            AnalaizerClass.expression = "3 + 5 - (2 * 7)";
            string result = AnalaizerClass.Format();
            Assert.AreEqual("3+5-(2*7)", result);
        }

        [Test]
        public void Format_ShouldReturnFormattedExpression_WhenExpressionIsAtMaxLength()
        {
            AnalaizerClass.expression = new string('1', 65536); 
            string result = AnalaizerClass.Format();
            Assert.AreEqual(new string('1', 65536), result); 
        }

        [Test]
        public void Format_ShouldReturnError_WhenConsecutiveUnaryOperatorsUsed()
        {
            AnalaizerClass.expression = "--5+3";
            string result = AnalaizerClass.Format();
            Assert.AreEqual("&" + ErrorsExpression.ERROR_03, result); 
        }

        [Test]
        public void Format_ShouldReturnError_WhenExpressionContainsNonNumericNonOperatorSymbols()
        {
            AnalaizerClass.expression = "5+3#2";
            string result = AnalaizerClass.Format();
            Assert.IsTrue(result.StartsWith("&" + ErrorsExpression.GetFullStringError(ErrorsExpression.ERROR_02, 3)));
        }

        [Test]
        public void Format_ShouldReturnFormattedExpression_WhenComplexValidExpressionIsGiven()
        {
            AnalaizerClass.expression = "3 * (5 + 2) - m(7) + p(4)";
            string result = AnalaizerClass.Format();
            Assert.AreEqual("3*(5+2)-m(7)+p(4)", result);
        }

        [Test]
        public void Format_ShouldReturnErrorWithCorrectPosition_WhenInvalidSymbolFoundInMiddle()
        {
            AnalaizerClass.expression = "5+2&3";
            string result = AnalaizerClass.Format();
            Assert.AreEqual("&" + ErrorsExpression.GetFullStringError(ErrorsExpression.ERROR_02, 3), result);
        }

        [Test]
        public void Format_ShouldReturnErrorWithCorrectPosition_WhenMultipleOperatorsFoundInSequence()
        {
            AnalaizerClass.expression = "5++2";
            string result = AnalaizerClass.Format();
            Assert.AreEqual("&" + ErrorsExpression.GetFullStringError(ErrorsExpression.ERROR_04, 2), result);
        }

        
    }
}