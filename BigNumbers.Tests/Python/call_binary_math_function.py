import sys
import mpmath

def main():
    if len(sys.argv) != 5:
        print("Usage: python3 call_binary_math_function.py <function_name> <x> <y> <precision>")
        sys.exit(1)

    function_name = sys.argv[1]
    x = mpmath.mpf(sys.argv[2])
    y = mpmath.mpf(sys.argv[3])
    precision = int(sys.argv[4])

    mpmath.mp.dps = precision

    # Get the function based on its name
    try:
        func = getattr(mpmath, function_name)
    except AttributeError:
        print(f"Function {function_name} not found in mpmath.")
        sys.exit(1)

    result = func(x, y)
    print(result)

if __name__ == "__main__":
    main()
