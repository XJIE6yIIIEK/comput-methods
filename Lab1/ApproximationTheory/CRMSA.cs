using Lab1;
using Matrices;
using Methods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vectors;

namespace ApproximationTheory {
	class CRMSA {
		int a, b, n;
		double h;
		Vector F, X;
		IFunction func;
		Vector ans;
		IOModule io;

		public Vector Answer {
			get { return ans; }
		}
		public CRMSA(IFunction func, int a, int b, int n, IOModule io) {
			this.io = io;
			this.a = a;
			this.b = b;
			this.n = n;
			this.h =  1.0 * (b - a) / n;
			//Function func = new Function(a, b, n);
			this.func = func;

			X = new Vector(n + 1);
			for(int i = 0; i < n + 1; i++) {
				X[i] = a + h * i;
			}

			F = new Vector(n + 1);
			for(int i = 0; i < n + 1; i++) {
				F[i] = func.Solve(X[i]);
			}

			getPolsConsts();
		}
		private void getPolsConsts() {
			Matrix A = new Matrix(3, 3);
			Vector B = new Vector(3);

			for(int i = 1; i <= 3; i++) { // подсчет матриц А и В
				for(int j = 1; j <= 3; j++) {
					A[i - 1, j - 1] = func.GetIntegrGG(i, j, a, b);
					}
			}

			for(int i = 1; i <= 3; i++) {
				B[i - 1] = func.GetIntegrFG(i, a, b);
			}

			io.WriteLine("Matrix:");
			io.WriteLine(A.ToString());
			io.WriteLine("Vector: ");
			io.WriteLine(B.ToString());

			SSRM ssrm = new SSRM(A, B);
			ans = ssrm.Answer;

			io.WriteLine($"P2(X) = ({ans[0]}) + ({ans[1]})*x + ({ans[2]})*x^2");
			io.WriteLine("\n");

			Vector g = new Vector(F.Length);

			WriteHead();
			for(int i = 0; i <= n; i++) {
				double x = a + h * i;
				g[i] = ans[0] + ans[1] * x + ans[2] * Math.Pow(x, 2);
				WriteStep(x, g[i] - F[i]);
			}

			io.WriteLine("Error Norm: " + Math.Sqrt(Math.Pow(F.Norm(), 2) - Math.Pow(g.Norm(), 2)).ToString());

		}

		private void WriteHead() { // печать шапки
			string x = io.CenterString("X", 12);
			string err = io.CenterString("Err", 12);

			string head = $"{x} | {err}";
			io.WriteLine(head);
		}
		private void WriteStep(double x, double err) { // печать шага
			string xStr = io.PrettyfyDouble(x, 12);
			string errStr = io.PrettyfyDouble(err, 12);
			io.WriteLine($"{xStr} | {errStr}");
		}
	}
}
