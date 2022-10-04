﻿using Vectors;
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

			double stopCriterion;
			double tau = 0.9 * 2 / A.EuclideNorm;

			int iter = 0;

			X_Curr.Fill(new double[] { 1, 2, 3, 4 });

			do {
				Tuple<Vector, double, double> res = SIMStep(A, B, X_Curr, X_Prev, tau);
				Vector X = res.Item1;
				double q = res.Item2;
				double residualNorm = res.Item3;

				Vector diff = X - X_Curr;
				double errNorm = diff.EnergyNorm(A);
				double errAss = q * errNorm / (1 - q);

				X_Prev = X_Curr;
				X_Curr = X;

				stopCriterion = residualNorm;


				WriteStep(iter, tau, q, residualNorm, errNorm, errAss, X);

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

		private void WriteStep(int iter, double tau, double q, double residualNorm, double errNorm, double errAss, Vector X) {
			string iterStr = string.Format("{0,5}", iter);
			string tauStr = io.PrettyfyDouble(tau, 12);
			string qStr = io.PrettyfyDouble(q, 12);
			string residualNormStr = io.PrettyfyDouble(residualNorm, 12);
			string errNormStr = io.PrettyfyDouble(errNorm, 12);
			string errAssStr = io.PrettyfyDouble(errAss, 12);

			string str = $"| {iterStr} | {tauStr} | {qStr} | {residualNormStr} | {errNormStr} | {errAssStr} | {X.ToString()}";

			io.WriteLine(str);
		}
	}
}
