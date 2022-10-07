using System;
using Vectors;
using Matrices;
using Lab1;

namespace Methods
{

	internal class SOR
	{
		private Vector X;
		IOModule io;
		public Vector Answer
		{
			get { return X; }
		}

		public int SOR_Iteration(double w, Vector X, Vector startVector, Matrix A, Vector B, double eps, bool output)
		{
			int iter = 0;
			double residualNorm;
			Vector Z = new Vector(startVector.Length);
			Vector newX = new Vector(startVector.Length);
			Z = (Vector)startVector.Clone();
			newX = (Vector)startVector.Clone();
			while ((X - newX).EnergyNorm(A) >= eps)
			{
				residualNorm = (B - A * newX).EnergyNorm(A);
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

				if (output)
					this.WriteStep(iter, w, residualNorm, (X - newX).EnergyNorm(A), newX);
			}
			this.X = (Vector)newX.Clone();
			return iter;
		}

		public SOR(Matrix A, Vector B, Vector X, IOModule io)
		{
			this.io = io;

			int min = int.MaxValue;
			double w0 = 0;
			for (double i = 0.1; i <= 1.9; i += 0.1)
			{
				int cur = this.SOR_Iteration(i, X, B, A, B, 1E-2, false);
				this.WriteStep(i, cur);
				if (cur < min)
				{
					min = cur;
					w0 = i;
				}
			}

			 SOR_Iteration(w0, X, B, A, B, 1E-4, true);

		}

		private void WriteStep(double w, int iter) {
			string iterStr = string.Format("{0,5}", iter);
			string wIter = io.PrettyfyDouble(w, 12);

			string str = $"| {wIter} | {iterStr}  ";

			io.WriteLine(str);
		}
		private void WriteStep(int iter, double w, double residualNorm, double errNorm, Vector X) {
			string iterStr = string.Format("{0,5}", iter);
			string wIter = io.PrettyfyDouble(w, 12);
			string residualNormStr = io.PrettyfyDouble(residualNorm, 12);
			string errNormStr = io.PrettyfyDouble(errNorm, 12);

			string str = $"| {iterStr} | {wIter} | {residualNormStr} | {errNormStr} | {X.ToString()}";

			io.WriteLine(str);
		}

	}
}
