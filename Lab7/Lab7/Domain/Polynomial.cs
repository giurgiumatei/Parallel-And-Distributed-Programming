using System;
using System.Text;

namespace Lab7.Domain
{
    public partial class Polynomial
    {
        public static readonly Random Random = new();

        public readonly int Degree;
        public readonly int[] Coefficients;

        public Polynomial(params int[] coefficients)
        {
            if (coefficients == null || coefficients.Length == 0)
                throw new InvalidOperationException("Invalid coefficients specification!");

            Degree = coefficients.Length - 1;
            Coefficients = coefficients;
        }

        public override string ToString()
        {
            var result = new StringBuilder();

            for (var i = Degree; i >= 0; --i)
                result.AppendFormat("{0}{1}*X^{2}", i != Degree ? " + " : "", Coefficients[i], i);

            return result.ToString();
        }

        public override bool Equals(object obj)
        {
            var polynomial = obj as Polynomial;

            if (Degree != polynomial?.Degree)
                return false;

            for (var i = 0; i <= Degree; ++i)
                if (Coefficients[i] != polynomial.Coefficients[i])
                    return false;

            return true;
        }


        public override int GetHashCode()
        {
            unchecked
            {
                return (Degree * 397) ^ Coefficients.GetHashCode();
            }
        }

        public static Polynomial RandomPolynomial(int degree)
        {
            if (degree < 0)
                throw new InvalidOperationException("Invalid degree specification!");

            var coefficients = new int[degree + 1];

            for (var i = 0; i <= degree; ++i)
                coefficients[i] = Random.Next(1, 10);

            return new Polynomial(coefficients);
        }

        public static Polynomial operator <<(Polynomial polynomial, int offset)
        {
            return Shift(polynomial, offset);
        }

        public static Polynomial operator +(Polynomial firstPolynomial, Polynomial secondPolynomial)
        {
            return Add(firstPolynomial, secondPolynomial);
        }

        public static Polynomial operator -(Polynomial firstPolynomial, Polynomial secondPolynomial)
        {
            return Subtract(firstPolynomial, secondPolynomial);
        }

        public static Polynomial operator *(Polynomial firstPolynomial, Polynomial secondPolynomial)
        {
            return Multiply(firstPolynomial, secondPolynomial);
        }
    }
}
