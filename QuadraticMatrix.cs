namespace Matrices
{
    internal class QuadraticMatrix : Matrix
    {
        Func<bool, float>? getDeterminant { get; set; }

        public QuadraticMatrix(float[,] values) : base(values.GetLength(0) == values.GetLength(1) ? values : throw new ArgumentException("The width and height of a quadratic matrix has to be equal."))
        {
            getDeterminant = InitGetDeterminant();
        }

        public QuadraticMatrix(int n) : this(new float[n, n])
        {

        }

        QuadraticMatrix(float[,] values, bool initGetDeterminant = false) : base(values)
        {
            getDeterminant = initGetDeterminant ? InitGetDeterminant() : null;
        }

        public float GetDeterminant(bool checkFor0RowOrColumn = true) => getDeterminant?.Invoke(checkFor0RowOrColumn) ?? 0;

        Func<bool, float> InitGetDeterminant() => (bool checkFor0RowOrColumn) => {
            bool calculateDeterminant = true;
            if (checkFor0RowOrColumn)
            {
                
            }
            float determinant = 0;
            if (calculateDeterminant)
            {

            }
            getDeterminant = (bool _) => determinant;
            return determinant;
        };
    }
}
