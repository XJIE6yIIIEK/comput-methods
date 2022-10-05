using System;
using Vectors;
using Matrices;

namespace Methods
{

	internal class SOR
	{
		private Vector X;
		//private Matrix D;
		//private Matrix A1;
		//public Matrix A2;

		public Vector Answer
		{
			get { return X; }
		}

		public int SOR_Iteration(double w, Vector X, Vector startVector, Matrix A, Vector B, double eps, bool output)
		{
			int iter = 0;
			Vector Z = new Vector(startVector.Length);
			Vector newX = new Vector(startVector.Length);
			Z = (Vector)startVector.Clone();
			newX = (Vector)startVector.Clone();
			while ((X - newX).EnergyNorm(A) >= eps)
			{
				iter++;
				double sumNew = 0;
				double sumOld = 0;

				for (int i = 0; i < Z.Length; i++)
				{
					sumNew = 0;
					sumOld = 0;
					for (int j = 0; j < i; j++)
						sumNew += A[i, j] * newX[j];
					for (int j = i + 1; j < newX.Length; j++)
						sumOld += A[i, j] * newX[j];
					Z[i] = 1 / A[i, i] * (B[i] - sumNew - sumOld);
					newX[i] = newX[i] + w * (Z[i] - newX[i]);
				}

				if(output)
					Console.WriteLine(iter + "  " + newX.ToString());
			}
			this.X = (Vector)newX.Clone();
			return iter;
		}

		public SOR(Matrix A, Vector B, Vector X)
		{
			//D = new Matrix(A.Rows, A.Cols);
			//A1 = new Matrix(A.Rows, A.Cols);
			//A2 = new Matrix(A.Rows, A.Cols);

			//for (int i = 0; i < A.Rows; i++)
			//	for (int j = 0; j < A.Cols; j++)
			//	{
			//		if (i > j)
			//			A1[i, j] = A[i, j];
			//		if (i < j)
			//			A2[i, j] = A[i, j];
			//		if (i == j)
			//			D[i, j] = A[i, j];
			//	}

			int min = int.MaxValue;
			double w0 = 0;
			for (double i = 0.1; i <= 1.9; i += 0.1)
			{
				int cur = this.SOR_Iteration(i, X, B, A, B, 1E-2, false);
				if (cur < min)
				{
					min = cur;
					w0 = i;
				}
			}

			 SOR_Iteration(w0, X, B, A, B, 1E-4, true);

		}

	}
}
