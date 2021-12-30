using System;

namespace Lab5.Domain
{
    public  partial class Polynomial
    {
        // Serial multiplication.
        public static Polynomial Multiply(Polynomial firstPolynomial, Polynomial secondPolynomial)
        {
            var coefficients = new int[firstPolynomial.Degree + secondPolynomial.Degree + 1];

            for (var i = 0; i <= firstPolynomial.Degree; ++i)
            for (var j = 0; j <= secondPolynomial.Degree; ++j)
                coefficients[i + j] += firstPolynomial.Coefficients[i] * secondPolynomial.Coefficients[j];

            return new Polynomial(coefficients);
        }

        public static Polynomial MultiplyKaratsubaIterative(Polynomial firstPolynomial, Polynomial secondPolynomial)
        {
            if (firstPolynomial.Degree != secondPolynomial.Degree)
                throw new InvalidOperationException("Only works for polynomials of same degree!");

            var minimumDegree = Math.Min(firstPolynomial.Degree, secondPolynomial.Degree);
            var n = minimumDegree + 1;

            var di = new int[n];
            var dpq = new int[n, n];

            for (var i = 0; i <= n - 1; ++i)
            {
                di[i] = firstPolynomial.Coefficients[i] * secondPolynomial.Coefficients[i];
            }

            for (var i = 0; i <= 2 * n - 3; ++i)
            for (var p = 0; p <= i; ++p)
            {
                var q = i - p;
                if (p < n && q < n && q > p)
                    dpq[p, q] = (firstPolynomial.Coefficients[p] + firstPolynomial.Coefficients[q]) * (secondPolynomial.Coefficients[p] + secondPolynomial.Coefficients[q]);
            }

            var result = new int[2 * n - 1];

            result[0] = di[0];
            result[2 * n - 2] = di[n - 1];
            
            for (var i = 1; i <= 2 * n - 3; ++i)
            {
                for (var p = 0; p <= i; ++p)
                {
                    var q = i - p;
                    if (p < n && q < n && q > p)
                        result[i] += dpq[p, q] - (di[p] + di[q]);
                }

                if (i % 2 == 0)
                {
                    result[i] += di[i / 2];
                }
            }

            return new Polynomial(result);
        }
    }
}
