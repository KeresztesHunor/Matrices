namespace Matrices
{
    internal class Matrix(float[,] values)
    {
        float[,] matrix { get; } = values;

        public int Width => matrix.GetLength(0);
        public int Height => matrix.GetLength(1);

        public virtual Matrix Transposed
        {
            get
            {
                float[,] values = new float[Height, Width];
                for (int i = 0; i < Width; i++)
                {
                    for (int k = 0; k < Height; k++)
                    {
                        values[k, i] = matrix[i, k];
                    }
                }
                return new Matrix(values);
            }
        }

        public float this[int i, int k] => matrix[i, k];

        public Matrix(int i, int k) : this(new float[i, k])
        {
            // For formality:
            //FillWith0(matrix);
        }

        static void FillWith0(float[,] matrix)
        {
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for(int k = 0; k < matrix.GetLength(1); k++)
                {
                    matrix[i, k] = 0;
                }
            }
        }

        static Matrix PerformCommutativeOperation(Matrix left, Matrix right, Func<float, float, float> operation, Func<Matrix, Matrix, bool>? error = null, string? errorMessage = null)
        {
            if (error?.Invoke(left, right) ?? false)
            {
                throw new ArgumentException(errorMessage);
            }
            float[,] values = new float[left.Width, left.Height];
            for (int i = 0; i < left.Width; i++)
            {
                for (int k = 0; k < left.Height; k++)
                {
                    values[i, k] = operation(left[i, k], right[i, k]);
                }
            }
            return new Matrix(values);
        }

        static bool CheckSizes(Matrix left, Matrix right) => left.Width != right.Width || left.Height != right.Height;

        public static Matrix operator +(Matrix left, Matrix right) => PerformCommutativeOperation(left, right, (left, right) => left + right, CheckSizes, "Cannot calculate sum of two matrices of differing sizes.");

        public static Matrix operator -(Matrix left, Matrix right) => PerformCommutativeOperation(left, right, (left, right) => left - right, CheckSizes, "Cannot calculate difference of two matrices of differing sizes.");

        public static Matrix operator *(Matrix left, Matrix right)
        {
            if (left.Width != right.Height || left.Height != right.Width)
            {
                throw new ArgumentException("Cannot multiply two matrices where the left matrix's height doesn't equal the right matrix's width and vice versa.");
            }
            float[,] values = new float[right.Width, left.Height];
            //For formality
            //FillWith0(values);
            for (int leftK = 0; leftK < left.Height; leftK++)
            {
                for (int rightI = 0; rightI < right.Width; rightI++)
                {
                    for (int rightK = 0; rightK < right.Height; rightK++)
                    {
                        values[rightI, leftK] += left[rightK, rightI] * right[rightI, rightK];
                    }
                }
            }
            return new Matrix(values);
        }
    }
}