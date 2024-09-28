namespace Matrices
{
    internal class Matrix(float[,] values) : IEquatable<Matrix>
    {
        protected float[,] matrix { get; } = values;

        public int Width => matrix.GetLength(0);
        public int Height => matrix.GetLength(1);

        public virtual Matrix Transposed
        {
            get
            {
                float[,] values = new float[Height, Width];
                Iterate(values, (int i, int k) => {
                    values[k, i] = matrix[i, k];
                });
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
            Iterate(matrix, (ref float value) => {
                value = 0;
            });
        }

        protected static void Iterate(int i, int k, Action<int, int> action)
        {
            for (int ii = 0; ii < i; ii++)
            {
                for (int kk = 0; kk < k; kk++)
                {
                    action(ii, kk);
                }
            }
        }

        protected static void Iterate(Matrix matrix, Action<int, int> action)
        {
            Iterate(matrix.Width, matrix.Height, action);
        }

        protected static void Iterate(float[,] values, Action<int, int> action)
        {
            Iterate(values.GetLength(0), values.GetLength(1), action);
        }

        protected static void Iterate(float[,] values, Action<float> action)
        {
            Iterate(values, (int i, int k) => {
                action(values[i, k]);
            });
        }

        protected static void Iterate(float[,] values, ReferentialAction action)
        {
            Iterate(values, (int i, int k) => {
                action(ref values[i, k]);
            });
        }

        protected delegate void ReferentialAction(ref float value);

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

        static Matrix ArithmetizeNewMatrix(int width, int height, Action<float[,]> action)
        {
            float[,] values = new float[width, height];
            action(values);
            return new Matrix(values);
        }

        public bool Equals(Matrix? other) => matrix.Equals(other);

        public static Matrix operator +(Matrix left, Matrix right) => PerformCommutativeOperation(left, right, (left, right) => left + right, CheckSizes, "Cannot calculate sum of two matrices of differing sizes.");

        public static Matrix operator -(Matrix left, Matrix right) => PerformCommutativeOperation(left, right, (left, right) => left - right, CheckSizes, "Cannot calculate difference of two matrices of differing sizes.");

        public static Matrix operator *(Matrix left, Matrix right)
        {
            if (left.Width != right.Height || left.Height != right.Width)
            {
                throw new ArgumentException("Cannot multiply two matrices where the left matrix's height doesn't equal the right matrix's width and vice versa.");
            }
            return ArithmetizeNewMatrix(right.Width, left.Height, (float[,] values) => {
                // For formality
                //FillWith0(values);
                for (int leftK = 0; leftK < left.Height; leftK++)
                {
                    Iterate(right.Width, right.Height, (int rightI, int rightK) => {
                        values[rightI, leftK] += left[rightK, rightI] * right[rightI, rightK];
                    });
                }
            });
        }

        public static Matrix operator *(Matrix matrix, float lambda) => ArithmetizeNewMatrix(matrix.Width, matrix.Height, (float[,] values) => {
            Iterate(values, (ref float value) => {
                value *= lambda;
            });
        });

        public static Matrix operator *(float lambda, Matrix matrix) => matrix * lambda;
    }
}