using ADS_lab_2;
using NUnit.Framework;

namespace lab_2_tests
{
    public class MD5AlgorythmTests
    {
        // Можливо тут потім (ніколи) напишу ще й для файлу перевірки
        //[SetUp]
        //public void Setup()
        //{
        //}

        [TestCase("", ExpectedResult = "D41D8CD98F00B204E9800998ECF8427E")]
        [TestCase("a", ExpectedResult = "0CC175B9C0F1B6A831C399E269772661")]
        [TestCase("abc", ExpectedResult = "900150983CD24FB0D6963F7D28E17F72")]
        [TestCase("message digest", ExpectedResult = "F96B697D7CB7938D525A2F31AAF161D0")]
        [TestCase("abcdefghijklmnopqrstuvwxyz", ExpectedResult = "C3FCD3D76192E4007DFB496CCA67E13B")]
        [TestCase("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789", ExpectedResult = "D174AB98D277D9F5A5611C2C9F419D9F")]
        [TestCase("12345678901234567890123456789012345678901234567890123456789012345678901234567890", ExpectedResult = "57EDF4A22BE3C955AC49DA2E2107B67A")]
        public string MD5AlgorythmTests_Algorythm_ReturnCorrectHash(string massage)
        {
            return MD5Algorythm.Algorythm(massage).ToUpperInvariant();
        }
    }
}