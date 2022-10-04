using Vectors;
using Matrices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lab1;

namespace Methods {
	internal class SIM {
		private Vector X;
		private IOModule io;

		public Vector Answer {
			get { return X; }
		}

		public SIM(Matrix A, Vector B, double eps, IOModule io) {
			this.io = io;

			Vector X_Prev = new Vector(B.Length);
			Vector X_Curr = new Vector(B.Length);

			double stopCriterion = 0;
			double tau = 0.9 * 2 / A.EuclideNorm;

			int iter = 0;

			X_Prev.Fill(1);

			do {
				Tuple<Vector, double, double> res = SIMStep(A, B, X_Curr, X_Prev, tau);
				Vector X = res.Item1;
				double q = res.Item2;
				double residualNorm = res.Item3;

				Vector diff = X - X_Curr;
				double criterionCurr = q * diff.EnergyNorm(A) / (1 - q);

				X_Prev = X_Curr;
				X_Curr = X;

				stopCriterion = residualNorm;

				WriteStep(iter, tau, q, residualNorm, criterionCurr, X);

				iter++;
			} while(stopCriterion > eps);

			this.X = X_Curr;
		}

		private Tuple<Vector, double, double> SIMStep(Matrix A, Vector B, Vector X_Curr, Vector X_Prev, double tau) {
			Vector X = new Vector(B.Length);

			for(int i = 0; i < A.Rows; i++) {
				double sum = 0;

				for(int j = 0; j < A.Cols; j++) {
					sum += A[i, j] * X_Curr[j];
				}

				X[i] = X_Curr[i] + tau * (B[i] - sum);
			}

			Vector residual = A * X - B;
			Vector Q_Numerator = X - X_Curr;
			Vector Q_Denumerator = X_Curr - X_Prev;

			double Q_NumEnergyNorm = Q_Numerator.EnergyNorm(A);
			double Q_DenumEnergyNorm = Q_Denumerator.EnergyNorm(A);
			double q = Q_NumEnergyNorm / Q_DenumEnergyNorm;

			double residualNorm = residual.EnergyNorm(A);

			return new Tuple<Vector, double, double>(X, q, residualNorm);
		}

		private void WriteStep(int iter, double tau, double q, double residualNorm, double criterionCurr, Vector X) {
			string str = string.Format("| {0,5} | {1,12:0.0000E0;-0.0000E0;0} | {2,12:0.0000E0;-0.0000E0;0} | " +
				"{3,12:0.0000E0;-0.0000E0;0} | {3,12:0.0000E0;-0.0000E0;0} | " + X.ToString(), iter, tau, q, residualNorm, criterionCurr);

			io.WriteLine(str);
		}
	}
}
