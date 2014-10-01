using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExecutionEnvironment;

namespace UnitTestHelpers
{
    public class AssertEx
    {
        public static void AreEqual(int[] expected, int[] actual)
        {
            Assert.AreEqual(expected.Length, actual.Length);
            for (int i = 0; i < expected.Length; i++)
                Assert.AreEqual(expected[i], actual[i], "at index " + i);
        }

        public static void AreEqual(int[] expected, Arr<int> actual)
        {
            Assert.AreEqual(expected.Length, actual.Length);
            for (int i = 0; i < expected.Length; i++)
                Assert.AreEqual(expected[i], actual[i], "at index " + i);
        }

        public static void EqualToOne(List<int[]> expected,  int[] actual)
        {
            foreach (int[] candidate in expected)
                if (AreEqualEx(candidate, actual))
                    return;
            throw new AssertFailedException("Value did not match any of the expected values.");
        }

        public static void EqualToOne(List<int[]> expected, Arr<int> actual)
        {
            foreach (int[] candidate in expected)
                if (AreEqualEx(candidate, actual))
                    return;
            throw new AssertFailedException("Value did not match any of the expected values. Expected:\r\n" + expected.ToString());
        }

        public static bool AreEqualEx(int[] expected, int[] actual)
        {
            if (expected.Length != actual.Length) return false;
            for (int i = 0; i < expected.Length; i++)
                if (expected[i] != actual[i])
                    return false;
            return true;
        }

        public static bool AreEqualEx(int[] expected, Arr<int> actual)
        {
            if (expected.Length != actual.Length) return false;
            for (int i = 0; i < expected.Length; i++)
                if (expected[i] != actual[i])
                    return false;
            return true;
        }

        public static void IsGreaterThanOrEqualTo(double actual, double value)
        {
            Assert.IsTrue(actual >= value, actual + " is less than " + value);
        }

        public static void IsLessThanOrEqualTo(double actual, double value)
        {
            Assert.IsTrue(actual <= value, actual + " is greater than " + value);
        }
    }
}
