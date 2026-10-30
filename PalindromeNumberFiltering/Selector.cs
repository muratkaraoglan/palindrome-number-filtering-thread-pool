using System.Collections.Concurrent;

namespace PalindromeNumberFiltering
{
    /// <summary>
    /// A static class containing methods for filtering palindrome numbers from a collection of integers.
    /// </summary>
    public static class Selector
    {
        /// <summary>
        /// Retrieves a collection of palindrome numbers from the given list of integers using sequential filtering.
        /// </summary>
        /// <param name="numbers">The list of integers to filter.</param>
        /// <returns>A collection of palindrome numbers.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the input list 'numbers' is null.</exception>
        public static IList<int> GetPalindromeInSequence(IList<int>? numbers)
        {
            ArgumentNullException.ThrowIfNull(numbers);
            return numbers.Where(number => IsPalindrome(number)).ToList();
        }

        /// <summary>
        /// Retrieves a collection of palindrome numbers from the given list of integers using parallel filtering.
        /// </summary>
        /// <param name="numbers">The list of integers to filter.</param>
        /// <returns>A collection of palindrome numbers.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the input list 'numbers' is null.</exception>
        public static IList<int> GetPalindromeInParallel(IList<int> numbers)
        {
            ArgumentNullException.ThrowIfNull(numbers);

            var count = numbers.Count;
            if (count == 0)
            {
                return Array.Empty<int>();
            }

            var result = new ConcurrentBag<int>();

            _ = Parallel.ForEach(numbers, number =>
            {
                if (IsPalindrome(number))
                {
                    result.Add(number);
                }
            });

            return result.ToList();
        }

        /// <summary>
        /// Checks whether the given integer is a palindrome number.
        /// </summary>
        /// <param name="number">The integer to check.</param>
        /// <returns>True if the number is a palindrome, otherwise false.</returns>
        private static bool IsPalindrome(int number)
        {
            if (number < 0)
            {
                return false;
            }

            int diveder = (int)Math.Pow(10, GetLength(number) - 1);

            return IsPositiveNumberPalindrome(number, diveder);
        }

        /// <summary>
        /// Recursively checks whether a positive number is a palindrome.
        /// </summary>
        /// <param name="number">The positive number to check.</param>
        /// <param name="divider">The divider used in the recursive check.</param>
        /// <returns>True if the positive number is a palindrome, otherwise false.</returns>
        private static bool IsPositiveNumberPalindrome(int number, int divider)
        {
            if (number < 10)
            {
                return true;
            }

            int firstDigit = number / divider;
            int lastDigit = number % 10;

            if (firstDigit != lastDigit)
            {
                return false;
            }

            int rem = number % divider;
            number = rem / 10;
            divider /= 100;

            return IsPositiveNumberPalindrome(number, divider);
        }

        /// <summary>
        /// Gets the number of digits in the given integer.
        /// </summary>
        /// <param name="number">The integer to count digits for.</param>
        /// <returns>The number of digits in the integer.</returns>
        private static byte GetLength(int number)
        {
            return number switch
            {
                < 10 => 1,
                < 100 => 2,
                < 1_000 => 3,
                < 10_000 => 4,
                < 100_000 => 5,
                < 1_000_000 => 6,
                < 10_000_000 => 7,
                < 100_000_000 => 8,
                < 1_000_000_000 => 9,
                _ => 10,
            };
        }
    }
}
