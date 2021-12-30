using System;

namespace Lab5.Domain
{
    public partial class Polynomial
    {

        // Simple addition. No need for parallelization.
        public static Polynomial Add(Polynomial a, Polynomial b)
        {
            var min = Math.Min(a.Degree, b.Degree);
            var max = Math.Max(a.Degree, b.Degree);

            var coefficients = new int[max + 1];

            for (var i = 0; i <= min; ++i)
                coefficients[i] = a.Coefficients[i] + b.Coefficients[i];

            for (var i = min + 1; i <= max; ++i)
                if (i <= a.Degree)
                    coefficients[i] = a.Coefficients[i];
                else
                    coefficients[i] = b.Coefficients[i];

            return new Polynomial(coefficients);
        }

        // Simple subtraction. No need for parallelization.
        public static Polynomial Subtract(Polynomial a, Polynomial b)
        {
            var min = Math.Min(a.Degree, b.Degree);
            var max = Math.Max(a.Degree, b.Degree);

            var coefficients = new int[max + 1];

            for (var i = 0; i <= min; ++i)
                coefficients[i] = a.Coefficients[i] - b.Coefficients[i];

            for (var i = min + 1; i <= max; ++i)
                if (i <= a.Degree)
                    coefficients[i] = a.Coefficients[i];
                else
                    coefficients[i] = -b.Coefficients[i];

            var degree = coefficients.Length - 1;
            while (coefficients[degree] == 0 && degree > 0)
                degree--;

            var clean = new int[degree + 1];
            Array.Copy(coefficients, 0, clean, 0, degree + 1);
            return new Polynomial(clean);
        }

        // Shifts a polynomial to the left by a given offset.
        // Equivalent to multiplying with "X^offset".
        // For example, X^2 shifted by offset 3 becomes X^5.
        // "^" denotes the power function.
        public static Polynomial Shift(Polynomial p, int offset)
        {
            if (offset < 0)
                throw new InvalidOperationException("Invalid offset specification!");

            var coefficients = new int[p.Degree + 1 + offset];
            Array.Copy(p.Coefficients, 0, coefficients, offset, p.Degree + 1);
            return new Polynomial(coefficients);
        }
    }
}
