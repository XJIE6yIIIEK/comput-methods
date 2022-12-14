using Lab1;
using Lab1.Matrices;
using Matrices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vectors;

namespace BoundaryValueProblem {
	class FiniteDifferencesImplicit {
		double var;
		IOModule io;
		public FiniteDifferencesImplicit(int var, IOModule io) {
			this.var = var / 10.0;
			this.io = io;
			double hi = 0.1;
			for(int i = 3; i <= 5; i++) {
				int n = (int)Math.Pow(2, i);
				io.WriteLine("N = " + n);
				double h = 1.0 / n;
				WriteHead(n, h);
				double tau = h;
				Vector X = new Vector(n + 1);

				for(int j = 0; j < n + 1; j++)
					X[j] = h * j;

				double d = tau * hi / h / h;
				Matrix A = new Matrix(n - 1, n - 1);

				for(int k = 0; k < n - 1; k++) {
					A[k, k] = 1 + 2 * d;
					if(k != 0)
						A[k, k - 1] = -d;
					if(k != n - 2)
						A[k, k + 1] = -d;
				}

				Vector curX = (Vector)X.Clone();
				double t = tau;
				double maxDelta = 0;

				while(t <= 1) {
					Vector B = new Vector(n - 1);
					for(int k = 0; k < n - 1; k++)
						B[k] = curX[k + 1] + tau * f(t, X[k + 1], hi);
					B[n - 2] += d;
					SweepMethod sm = new SweepMethod(A, B);
					Vector next = sm.Answer;
					for(int k = 0; k < n - 1; k++)
						curX[k + 1] = next[k];
					double curMax = double.MinValue;
					for(int k = 0; k <= n; k++) {
						if(Math.Abs(curX[k] - u(t, X[k])) > curMax)
							curMax = Math.Abs(curX[k] - u(t, X[k]));
					}
					maxDelta = Math.Max(maxDelta, curMax);
					WriteStep(t, maxDelta, curX);
					t += tau;
				}
			}
		}
		private double f(double t, double x, double hi) {
			return var * Math.Sin(Math.PI * x) * (1 + hi * t * Math.PI * Math.PI);
		}

		private double u(double t, double x) {
			return x + var * t * Math.Sin(Math.PI * x);
		}

		private void WriteHead(int n, double h) { // печать шапки
			string t = io.CenterString("t", 10);
			string deltaStr = io.CenterString("delta", 10);
			string head = $"|{t}|{deltaStr}";
			for(int i = 0; i <= n; i++)
				head += $"|{io.CenterString((h * i).ToString(), 7)}";

			io.WriteLine(head);
		}
		private void WriteStep(double t, double delta, Vector X) { // печать шага итерации
			string tStr = io.PrettyfyDouble(t, 8);
			string deltaStr = io.PrettyfyDouble(delta, 8);

			string str = $"| {tStr} | {deltaStr} ";

			for(int i = 0; i < X.Length; i++)
				str += $"| {io.PrettyfyDouble(X[i], 5, 3)} ";


			io.WriteLine(str);
		}
	}
}
