using Lab1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vectors;

namespace BoundaryValueProblem {
	class FiniteDifferencesExplicit {
		double var;
		IOModule io;
		public FiniteDifferencesExplicit(int var, IOModule io) {
			this.var = var / 10.0;
			this.io = io;
			double hi = 0.1;
			for(int i = 3; i <= 5; i++) {
				int n = (int)Math.Pow(2, i);
				io.WriteLine("N = " + n);
				double h = 1.0 / n;
				WriteHead(n, h);
				double tau = h * h / 4 / hi;
				Vector X = new Vector(n+1);
				for(int j = 0; j < n + 1; j++)
					X[j] = h * j;
				double t = tau;
				Vector prevX = (Vector)X.Clone();
				Vector curX = new Vector(n+1);
				double maxDelta = 0;
				while(t <= 1) {
					curX[0] = 0;
					curX[n] = 1;
					for(int j = 1; j < n; j++)
						curX[j] = prevX[j] + tau * (hi * (prevX[j + 1] - 2 * prevX[j] + prevX[j - 1]) / h / h + f(t - tau, X[j], hi));
					double curMax = double.MinValue;
					for(int k = 0; k <= n; k++) {
						if(Math.Abs(curX[k] - u(t, X[k])) > curMax)
							curMax = Math.Abs(curX[k] - u(t, X[k]));
					}
					maxDelta = Math.Max(maxDelta, curMax);
					WriteStep(t, maxDelta, curX);
					t += tau;
					prevX = (Vector)curX.Clone();
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
				head += $"|{io.CenterString((h*i).ToString(), 7)}";

			io.WriteLine(head);
		}
		private void WriteStep(double t, double delta, Vector X) { // печать шага итерации
			string tStr = io.PrettyfyDouble(t, 8);
			string deltaStr = io.PrettyfyDouble(delta, 8);

			string str = $"| {tStr} | {deltaStr} ";

			for(int i = 0; i < X.Length; i++)
				str += $"| {io.PrettyfyDouble(X[i], 8, 3)} ";


			io.WriteLine(str);
		}
	}
}
