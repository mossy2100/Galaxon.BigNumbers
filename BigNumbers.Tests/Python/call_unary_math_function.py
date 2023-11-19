import sys
import mpmath

def main():
    if len(sys.argv) != 4:
        print("Usage: python3 call_unary_math_function.py <function_name> <x> <precision>")
        sys.exit(1)

    function_name = sys.argv[1]
    x = mpmath.mpf(sys.argv[2])
    precision = int(sys.argv[3])

    mpmath.mp.dps = precision

    # Get the function based on its name
    try:
        func = getattr(mpmath, function_name)
    except AttributeError:
        print(f"Function {function_name} not found in mpmath.")
        sys.exit(1)

    result = func(x)
    print(result)

if __name__ == "__main__":
    main()
