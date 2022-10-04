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
			Vector X_Prev = new Vector(B.Length);
			Vector X_Curr = new Vector(B.Length);

			double stopCriterion = 0;
			double tau = 0.9 * 2 / A.EuclideNorm;

			int iter = 0;

			this.io = io;

			X_Prev.Fill(1);

			do {

			} while(stopCriterion > eps);
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
			double Q_NumEnergyNorm = Math.Sqrt(Vector.Dot(A * Q_Numerator, Q_Numerator));
			double Q_DenumEnergyNorm = Math.Sqrt(Vector.Dot(A * Q_Denumerator, Q_Denumerator));
			double q = Q_NumEnergyNorm / Q_DenumEnergyNorm;
			double residualNorm = Math.Sqrt(Vector.Dot(A * residual, residual));
		}
	}
}
