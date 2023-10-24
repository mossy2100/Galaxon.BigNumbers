using System.Numerics;

namespace Galaxon.BigNumbers;

public partial struct BigComplex
{
    #region Properties

    public BigDecimal Real { get; set; }

    public BigDecimal Imaginary { get; set; }

    public readonly BigDecimal Magnitude => Abs(this);

    public readonly BigDecimal Phase => BigDecimal.Atan2(Imaginary, Real);

    #endregion Properties

    #region Static properties

    public static BigComplex Zero => new (0, 0);

    public static BigComplex One => new (1, 0);

    // Same as in System.Numerics.Complex.
    public static BigComplex ImaginaryOne => new (0, 1);

    // Convenient shorthand equal to ImaginaryOne.
    public static BigComplex I => ImaginaryOne;

    #endregion Static properties

    #region Constructor

    public BigComplex(BigDecimal real, BigDecimal imaginary)
    {
        Real = real;
        Imaginary = imaginary;
    }

    #endregion Constructor

    #region Overridden methods

    public override bool Equals(object? obj)
    {
        if (obj is not BigComplex z)
        {
            return false;
        }

        return Real == z.Real && Imaginary == z.Imaginary;
    }

    public readonly override int GetHashCode()
    {
        return HashCode.Combine(Real, Imaginary);
    }

    /// <summary>
    /// Express the complex number as a string in the usual algebraic format.
    /// This differs from Complex.ToString(), which outputs strings like (x, y).
    /// </summary>
    /// <returns>The complex number as a string.</returns>
    public readonly override string ToString()
    {
        var realPart = Real == 0 && Imaginary != 0 ? "" : $"{Real}";

        var sign = "";
        if (Real == 0)
        {
            if (Imaginary < 0)
            {
                sign = "-";
            }
        }
        else
        {
            if (Imaginary < 0)
            {
                sign = " - ";
            }
            else if (Imaginary > 0)
            {
                sign = " + ";
            }
        }

        string imagPart;
        var absImag = BigDecimal.Abs(Imaginary);
        if (absImag == 0)
        {
            imagPart = "";
        }
        else if (absImag == 1)
        {
            imagPart = "i";
        }
        else
        {
            imagPart = $"{absImag}i";
        }

        return $"{realPart}{sign}{imagPart}";
    }

    #endregion Overridden methods

    #region Methods relating to magnitude and phase

    /// <summary>
    /// Calculate absolute value (also known as magnitude).
    /// Using:
    /// sqrt(a^2 + b^2) = |a| * sqrt(1 + (b/a)^2)
    /// we can factor out the larger component to dodge overflow even when a^2
    /// would overflow.
    /// </summary>
    /// <param name="z">A BigComplex number.</param>
    /// <returns>The magnitude of the argument.</returns>
    public static BigDecimal Abs(BigComplex z)
    {
        var a = BigDecimal.Abs(z.Real);
        var b = BigDecimal.Abs(z.Imaginary);

        BigDecimal small, large;
        if (a < b)
        {
            small = a;
            large = b;
        }
        else
        {
            small = b;
            large = a;
        }

        if (small == 0)
        {
            return large;
        }

        var ratio = small / large;
        return large * BigDecimal.Sqrt(1 + BigDecimal.Sqr(ratio));
    }

    /// <summary>
    /// Construct a complex number from the magnitude and phase.
    /// <see cref="System.Numerics.Complex.FromPolarCoordinates" />
    /// </summary>
    /// <param name="magnitude">The magnitude (or absolute value) of the complex number.</param>
    /// <param name="phase">The phase angle in radians.</param>
    /// <returns>The new BigComplex number.</returns>
    public static BigComplex FromPolarCoordinates(BigDecimal magnitude, BigDecimal phase)
    {
        if (magnitude < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(magnitude), "Cannot be less than zero.");
        }

        // Optimization.
        if (magnitude == 0)
        {
            return Zero;
        }

        // Calculate parts.
        var real = magnitude * BigDecimal.Cos(phase);
        var imag = magnitude * BigDecimal.Sin(phase);
        return new BigComplex(real, imag);
    }

    #endregion Methods relating to magnitude and phase

    #region Exponentiation methods

    /// <summary>
    /// Square a complex number.
    /// </summary>
    /// <param name="z">A complex value.</param>
    /// <returns>The square of the supplied value.</returns>
    public static BigComplex Sqr(BigComplex z)
    {
        // Optimizations.
        if (z.Imaginary == 0)
        {
            if (z.Real == 0)
            {
                return 0;
            }

            if (z.Real == 1)
            {
                return 1;
            }

            return z.Real * z.Real;
        }

        if (z.Real == 0)
        {
            if (z.Real == 1)
            {
                return -1;
            }
            return -z.Imaginary * z.Imaginary;
        }

        return z * z;
    }

    /// <summary>
    /// Calculate the square root of a BigComplex number.
    /// The second root can be found by the negative of the result, as with
    /// other Sqrt() methods.
    /// You can use this method to get the square root of a negative BigDecimal.
    /// e.g. BigComplex z = BigComplex.Sqrt(-5);
    /// (There will be an implicit cast of the -5 to a BigComplex.)
    /// TODO Test.
    /// <see cref="System.Math.Sqrt" />
    /// <see cref="System.Numerics.Complex.Sqrt" />
    /// <see cref="BigDecimal.Sqrt" />
    /// </summary>
    /// <param name="z">A BigComplex number.</param>
    /// <returns>The positive square root as a BigComplex number.</returns>
    public static BigComplex Sqrt(BigComplex z)
    {
        if (z == 0)
        {
            return 0;
        }
        if (z == 1)
        {
            return 1;
        }
        if (z == -1)
        {
            return I;
        }

        var a = z.Real;
        var b = z.Imaginary;
        if (b == 0)
        {
            return a > 0
                ? new BigComplex(BigDecimal.Sqrt(a), 0)
                : new BigComplex(0, BigDecimal.Sqrt(-a));
        }

        var m = Abs(z);
        var x = BigDecimal.Sqrt((m + a) / 2);
        var y = b / BigDecimal.Abs(b) * BigDecimal.Sqrt((m - a) / 2);
        return new BigComplex(x, y);
    }

    /// <summary>
    /// Natural logarithm of a complex number.
    /// <see cref="Math.Log(double)" />
    /// <see cref="Complex.Log(Complex)" />
    /// </summary>
    /// <param name="z">A complex number.</param>
    /// <returns>The natural logarithm of the given value.</returns>
    /// <exception cref="ArgumentOutOfRangeException">If z == 0.</exception>
    public static BigComplex Log(BigComplex z)
    {
        if (z.Imaginary == 0)
        {
            if (z.Real == 0)
            {
                // Log(0) is undefined.
                // Math.Log(0) returns -Infinity, which BigDecimal can't represent.
                throw new ArgumentOutOfRangeException(nameof(z), "Logarithm of 0 is undefined.");
            }

            if (z.Real < 0)
            {
                // ln(x) = ln(|x|) + πi, for x < 0
                return new BigComplex(BigDecimal.Log(-z.Real), BigDecimal.Pi);
            }

            // For positive real values, pass to the BigDecimal method.
            return BigDecimal.Log(z.Real);
        }

        return new BigComplex(BigDecimal.Log(z.Magnitude), z.Phase);
    }

    /// <summary>
    /// Logarithm of a complex number in a specified base.
    /// <see cref="Log(BigComplex)" />
    /// <see cref="BigDecimal.Log(BigDecimal, BigDecimal)" />
    /// </summary>
    /// <param name="z">The complex arg.</param>
    /// <param name="b">The base.</param>
    /// <returns>The logarithm of z in base b.</returns>
    /// <exception cref="ArgumentOutOfRangeException">If the complex value is 0.</exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// If the base is less than or equal to 0, or equal to 1.
    /// </exception>
    public static BigComplex Log(BigComplex z, BigDecimal b)
    {
        if (b == 1)
        {
            throw new ArgumentOutOfRangeException(nameof(b),
                "Logarithms are undefined for a base of 1.");
        }

        return Log(z) / BigDecimal.Log(b);
    }

    /// <summary>
    /// Logarithm of a complex number in base 2.
    /// <see cref="BigDecimal.Log2" />
    /// </summary>
    /// <param name="z">The complex arg.</param>
    /// <returns>The logarithm of z in base 2.</returns>
    public static BigComplex Log2(BigComplex z)
    {
        return Log(z, 2);
    }

    /// <summary>
    /// Logarithm of a complex number in base 10.
    /// <see cref="BigDecimal.Log10" />
    /// <see href="https://en.wikipedia.org/wiki/Euler%27s_identity" />
    /// <see href="https://tauday.com/tau-manifesto#sec-euler_s_identity" />
    /// </summary>
    /// <param name="z">The complex arg.</param>
    /// <returns>The logarithm of z in base 10.</returns>
    public static BigComplex Log10(BigComplex z)
    {
        return Log(z, 10);
    }

    public static BigComplex Exp(BigComplex z)
    {
        // Optimizations.
        if (z.Real == 0)
        {
            // e^0 = 1
            if (z.Imaginary == 0)
            {
                return 1;
            }

            // Euler's identity with π: e^πi = -1
            if (z.Imaginary == BigDecimal.Pi)
            {
                return -1;
            }

            // Euler's identity with τ: e^τi = 1
            if (z.Imaginary == BigDecimal.Tau)
            {
                return 1;
            }
        }

        if (z.Imaginary == 0)
        {
            if (z.Real == 1)
            {
                // e^1 == e
                return BigDecimal.E;
            }

            if (z.Real == BigDecimal.Ln10)
            {
                // e^ln10 == 10
                return 10;
            }
        }

        // Euler's formula.
        var r = BigDecimal.Exp(z.Real);
        var x = r * BigDecimal.Cos(z.Imaginary);
        var y = r * BigDecimal.Sin(z.Imaginary);
        return new BigComplex(x, y);
    }

    /// <summary>
    /// Calculate 2 raised to a complex power.
    /// <see cref="BigDecimal.Exp2" />
    /// </summary>
    /// <param name="z">A complex value.</param>
    /// <returns>2^z</returns>
    public static BigComplex Exp2(BigComplex z)
    {
        return 2 ^ z;
    }

    /// <summary>
    /// Calculate 10 raised to a complex power.
    /// <see cref="BigDecimal.Exp10" />
    /// </summary>
    /// <param name="z">A complex value.</param>
    /// <returns>10^z</returns>
    public static BigComplex Exp10(BigComplex z)
    {
        return 10 ^ z;
    }

    /// <summary>
    /// Complex exponentiation.
    /// Only the principal value is returned.
    /// <see href="https://en.wikipedia.org/wiki/Exponentiation#Complex_exponentiation" />
    /// </summary>
    /// <param name="z">The base.</param>
    /// <param name="w">The exponent.</param>
    /// <returns>The result.</returns>
    /// <exception cref="ArithmeticException">
    /// If the base is 0 and the exponent
    /// is negative or imaginary.
    /// </exception>
    public static BigComplex Pow(BigComplex z, BigComplex w)
    {
        // Guards.
        if (z == 0)
        {
            // 0 raised to a negative real value is undefined.
            // Math.Pow() returns double.Infinity, but BigDecimal doesn't have this.
            if (w.Imaginary == 0 && w.Real < 0)
            {
                throw new ArithmeticException("0 raised to a negative power is undefined.");
            }

            // 0 to the power of an imaginary number is also undefined.
            if (w.Imaginary != 0)
            {
                throw new ArithmeticException("0 raised to an imaginary power is undefined.");
            }
        }

        // Any non-zero value (real or complex) raised to the 0 power is 1.
        // 0^0 has no agreed-upon value, but some programming languages,
        // including C#, return 1 (i.e. Math.Pow(0, 0) == 1). Rather than throw
        // an exception, we'll do that here, too, for consistency.
        if (w == 0)
        {
            return 1;
        }

        // Any value (real or complex) raised to the power of 1 is itself.
        if (w == 1)
        {
            return z;
        }

        // 1 raised to any real value is 1.
        // 1 raised to any complex value has multiple results, including 1.
        // We'll just return 1 (the principal value) for simplicity and
        // consistency with Complex.Pow().
        if (z == 1)
        {
            return 1;
        }

        // i^2 == -1 by definition.
        if (z == I && w == 2)
        {
            return -1;
        }

        // If the values are both real, pass it to the BigDecimal calculation.
        if (z.Imaginary == 0 && w.Imaginary == 0)
        {
            return BigDecimal.Pow(z.Real, w.Real);
        }

        // Use formula for principal value.
        return Exp(w * Log(z));
    }

    /// <summary>
    /// Exponentiation operator.
    /// </summary>
    /// <param name="z">The base.</param>
    /// <param name="w">The exponent.</param>
    /// <returns>The first operand raised to the power of the second.</returns>
    /// <exception cref="ArithmeticException">
    /// If the base is 0 and the exponent
    /// is negative or imaginary.
    /// </exception>
    public static BigComplex operator ^(BigComplex z, BigComplex w)
    {
        return Pow(z, w);
    }

    #endregion Exponentiation methods

    #region Trigonometic methods

    public static BigComplex Sin(BigComplex z)
    {
        var x = z.Real;
        var y = z.Imaginary;
        var a = BigDecimal.Sin(x) * BigDecimal.Cosh(y);
        var b = BigDecimal.Cos(x) * BigDecimal.Sinh(y);
        return new BigComplex(a, b);
    }

    public static BigComplex Cos(BigComplex z)
    {
        var x = z.Real;
        var y = z.Imaginary;
        var a = BigDecimal.Cos(x) * BigDecimal.Cosh(y);
        var b = BigDecimal.Sin(x) * BigDecimal.Sinh(y);
        return new BigComplex(a, -b);
    }

    public static BigComplex Tan(BigComplex z)
    {
        var x = z.Real;
        var y = z.Imaginary;
        BigComplex a = new (BigDecimal.Sin(2 * x), BigDecimal.Sinh(2 * y));
        var b = BigDecimal.Cos(2 * x) + BigDecimal.Cosh(2 * y);
        return a / b;
    }

    /// <summary>
    ///     <see href="https://en.wikipedia.org/wiki/Inverse_trigonometric_functions#Logarithmic_forms" />
    /// </summary>
    /// <param name="z"></param>
    /// <returns></returns>
    public static BigComplex Asin(BigComplex z)
    {
        return I * Log(Sqrt(1 - z * z) - I * z);
    }

    /// <summary>
    ///     <see href="https://en.wikipedia.org/wiki/Inverse_trigonometric_functions#Logarithmic_forms" />
    /// </summary>
    /// <param name="z"></param>
    /// <returns></returns>
    public static BigComplex Acos(BigComplex z)
    {
        return -I * Log(z + I * Sqrt(1 - z * z));
    }

    /// <summary>
    ///     <see href="https://en.wikipedia.org/wiki/Inverse_trigonometric_functions#Logarithmic_forms" />
    /// </summary>
    /// <param name="z"></param>
    /// <returns></returns>
    public static BigComplex Atan(BigComplex z)
    {
        return -I / 2 * Log((I - z) / (I + z));
    }

    public static BigComplex Sinh(BigComplex z)
    {
        var x = z.Real;
        var y = z.Imaginary;
        var a = BigDecimal.Sinh(x) * BigDecimal.Cos(y);
        var b = BigDecimal.Cosh(x) * BigDecimal.Sin(y);
        return new BigComplex(a, b);
    }

    public static BigComplex Cosh(BigComplex z)
    {
        var x = z.Real;
        var y = z.Imaginary;
        var a = BigDecimal.Cosh(x) * BigDecimal.Cos(y);
        var b = BigDecimal.Sinh(x) * BigDecimal.Sin(y);
        return new BigComplex(a, b);
    }

    public static BigComplex Tanh(BigComplex z)
    {
        var x = z.Real;
        var y = z.Imaginary;
        var a = BigDecimal.Tanh(x);
        var b = BigDecimal.Tan(y);
        return (a + I * b) / (1 + I * a * b);
    }

    #endregion Trigonometic methods

    #region Comparison operators

    /// <summary>
    /// Equality comparison operator.
    /// </summary>
    /// <param name="z1">The left-hand BigComplex number.</param>
    /// <param name="z2">The right-hand BigComplex number.</param>
    /// <returns>If they are equal.</returns>
    /// TODO Test.
    public static bool operator ==(BigComplex z1, BigComplex z2)
    {
        return z1.Equals(z2);
    }

    /// <summary>
    /// Inequality comparison operator.
    /// </summary>
    /// <param name="z1">The left-hand BigComplex number.</param>
    /// <param name="z2">The right-hand BigComplex number.</param>
    /// <returns>If they are not equal.</returns>
    public static bool operator !=(BigComplex z1, BigComplex z2)
    {
        return !(z1 == z2);
    }

    #endregion Comparison operators

    #region Arithmetic operators and methods

    /// <summary>
    /// Negate method.
    /// </summary>
    /// <returns>The negation of the argument.</returns>
    public static BigComplex Negate(BigComplex z)
    {
        return new BigComplex(-z.Real, -z.Imaginary);
    }

    /// <summary>
    /// Unary negation operator.
    /// </summary>
    /// <param name="z">A BigComplex number.</param>
    /// <returns>The negation of the operand.</returns>
    public static BigComplex operator -(BigComplex z)
    {
        return Negate(z);
    }

    /// <summary>
    /// Complex conjugate method.
    /// </summary>
    /// <returns>The complex conjugate of the argument.</returns>
    public static BigComplex Conjugate(BigComplex z)
    {
        return new BigComplex(z.Real, -z.Imaginary);
    }

    /// <summary>
    /// Complex conjugate operator.
    /// The use of the tilde (~) for this operator is non-standard, but it seems
    /// a good fit and it could be useful.
    /// </summary>
    /// <returns>The complex conjugate of the operand.</returns>
    public static BigComplex operator ~(BigComplex z)
    {
        return Conjugate(z);
    }

    /// <summary>
    /// Calculate reciprocal.
    /// </summary>
    /// <returns>The reciprocal of the argument.</returns>
    public static BigComplex Reciprocal(BigComplex z)
    {
        return 1 / z;
    }

    /// <summary>
    /// Addition method.
    /// </summary>
    /// <param name="z1">The left-hand BigComplex number.</param>
    /// <param name="z2">The right-hand BigComplex number.</param>
    /// <returns>The addition of the arguments.</returns>
    public static BigComplex Add(BigComplex z1, BigComplex z2)
    {
        return new BigComplex(z1.Real + z2.Real, z1.Imaginary + z2.Imaginary);
    }

    /// <summary>
    /// Addition operator.
    /// </summary>
    /// <param name="z1">The left-hand BigComplex number.</param>
    /// <param name="z2">The right-hand BigComplex number.</param>
    /// <returns>The addition of the operands.</returns>
    public static BigComplex operator +(BigComplex z1, BigComplex z2)
    {
        return Add(z1, z2);
    }

    /// <summary>
    /// Subtraction method.
    /// </summary>
    /// <param name="z1">The left-hand BigComplex number.</param>
    /// <param name="z2">The right-hand BigComplex number.</param>
    /// <returns>The subtraction of the arguments.</returns>
    public static BigComplex Subtract(BigComplex z1, BigComplex z2)
    {
        return new BigComplex(z1.Real - z2.Real, z1.Imaginary - z2.Imaginary);
    }

    /// <summary>
    /// Subtraction operator.
    /// </summary>
    /// <param name="z1">The left-hand BigComplex number.</param>
    /// <param name="z2">The right-hand BigComplex number.</param>
    /// <returns>The subtraction of the operands.</returns>
    public static BigComplex operator -(BigComplex z1, BigComplex z2)
    {
        return Subtract(z1, z2);
    }

    /// <summary>
    /// Multiply two BigComplex values.
    /// </summary>
    /// <param name="z1">The left-hand BigComplex number.</param>
    /// <param name="z2">The right-hand BigComplex number.</param>
    /// <returns>The multiplication of the arguments.</returns>
    public static BigComplex Multiply(BigComplex z1, BigComplex z2)
    {
        var a = z1.Real;
        var b = z1.Imaginary;
        var c = z2.Real;
        var d = z2.Imaginary;
        return new BigComplex(a * c - b * d, a * d + b * c);
    }

    /// <summary>
    /// Multiply a BigComplex by a BigDecimal.
    /// </summary>
    /// <param name="z">The BigComplex number.</param>
    /// <param name="m">The BigDecimal number.</param>
    /// <returns>The multiplication of the arguments.</returns>
    public static BigComplex Multiply(BigComplex z, BigDecimal m)
    {
        return new BigComplex(z.Real * m, z.Imaginary * m);
    }

    /// <summary>
    /// Multiply a BigDecimal by a BigComplex.
    /// </summary>
    /// <param name="m">The BigDecimal number.</param>
    /// <param name="z">The BigComplex number.</param>
    /// <returns>The multiplication of the arguments.</returns>
    public static BigComplex Multiply(BigDecimal m, BigComplex z)
    {
        return Multiply(z, m);
    }

    /// <summary>
    /// Multiplication operator.
    /// </summary>
    /// <param name="z1">The left-hand BigComplex number.</param>
    /// <param name="z2">The right-hand BigComplex number.</param>
    /// <returns>The multiplication of the operands.</returns>
    public static BigComplex operator *(BigComplex z1, BigComplex z2)
    {
        return Multiply(z1, z2);
    }

    /// <summary>
    /// Multiply a BigComplex by a BigDecimal.
    /// </summary>
    /// <param name="z">The BigComplex number.</param>
    /// <param name="m">The BigDecimal number.</param>
    /// <returns>The multiplication of the operands.</returns>
    public static BigComplex operator *(BigComplex z, BigDecimal m)
    {
        return Multiply(z, m);
    }

    /// <summary>
    /// Multiply a BigDecimal by a BigComplex.
    /// </summary>
    /// <param name="m">The BigDecimal number.</param>
    /// <param name="z">The BigComplex number.</param>
    /// <returns>The multiplication of the operands.</returns>
    public static BigComplex operator *(BigDecimal m, BigComplex z)
    {
        return Multiply(m, z);
    }

    /// <summary>
    /// Divide a BigComplex by a BigComplex.
    /// </summary>
    /// <param name="z1">The left-hand BigComplex number.</param>
    /// <param name="z2">The right-hand BigComplex number.</param>
    /// <returns>The division of the arguments.</returns>
    /// <exception cref="System.DivideByZeroException">If z2 == 0</exception>
    public static BigComplex Divide(BigComplex z1, BigComplex z2)
    {
        var a = z1.Real;
        var b = z1.Imaginary;
        var c = z2.Real;
        var d = z2.Imaginary;
        if (d == 0)
        {
            return z1 / c;
        }
        return new BigComplex(a * c + b * d, b * c - a * d) / (c * c + d * d);
    }

    /// <summary>
    /// Divide a BigComplex by a BigDecimal.
    /// </summary>
    /// <param name="z">The BigComplex value.</param>
    /// <param name="m">The BigDecimal value.</param>
    /// <returns>The division of the arguments.</returns>
    /// <exception cref="System.DivideByZeroException">If d == 0.</exception>
    public static BigComplex Divide(BigComplex z, BigDecimal m)
    {
        return new BigComplex(z.Real / m, z.Imaginary / m);
    }

    /// <summary>
    /// Divide a BigDecimal by a BigComplex.
    /// </summary>
    /// <param name="m">The BigDecimal value.</param>
    /// <param name="z">The BigComplex value.</param>
    /// <returns>The division of the arguments.</returns>
    /// <exception cref="System.DivideByZeroException">If z == 0.</exception>
    public static BigComplex Divide(BigDecimal m, BigComplex z)
    {
        var a = z.Real;
        var b = z.Imaginary;
        return new BigComplex(m * a, -m * b) / (a * a + b * b);
    }

    /// <summary>
    /// Divide a BigComplex by a BigComplex.
    /// </summary>
    /// <param name="z1">The left-hand BigComplex number.</param>
    /// <param name="z2">The right-hand BigComplex number.</param>
    /// <returns>The division of the operands.</returns>
    public static BigComplex operator /(BigComplex z1, BigComplex z2)
    {
        return Divide(z1, z2);
    }

    /// <summary>
    /// Divide a BigComplex by a BigDecimal.
    /// </summary>
    /// <param name="z">The BigComplex value.</param>
    /// <param name="m">The BigDecimal value.</param>
    /// <returns>The division of the operands.</returns>
    /// <exception cref="System.DivideByZeroException">If d == 0.</exception>
    public static BigComplex operator /(BigComplex z, BigDecimal m)
    {
        return Divide(z, m);
    }

    /// <summary>
    /// Divide a BigDecimal by a BigComplex.
    /// </summary>
    /// <param name="m">The BigDecimal value.</param>
    /// <param name="z">The BigComplex value.</param>
    /// <returns>The division of the operands.</returns>
    /// <exception cref="System.DivideByZeroException">If z == 0.</exception>
    public static BigComplex operator /(BigDecimal m, BigComplex z)
    {
        return Divide(m, z);
    }

    #endregion Arithmetic operators and methods

    #region Testing methods

    /// <summary>
    /// Helper function to test if a Complex equals a BigComplex.
    /// </summary>
    /// <param name="expected">Expected Complex value</param>
    /// <param name="actual">Actual BigComplex value</param>
    public static void AssertAreEqual(Complex expected, BigComplex actual)
    {
        BigDecimal.AssertAreEqual(expected.Real, actual.Real);
        BigDecimal.AssertAreEqual(expected.Imaginary, actual.Imaginary);
    }

    #endregion Testing methods
}
