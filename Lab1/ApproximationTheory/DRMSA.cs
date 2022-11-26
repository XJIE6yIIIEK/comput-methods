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
	class DRMSA {
		Vector F, X; // вектор значений функции и вектор значений аргументов
		double h, a, b, n; // шаг, левая граница, правая граница, количество отрезков
		IOModule io;
		Vector ans; // ответ

		public Vector Answer {
			get { return ans; }
		}

		public DRMSA(int a, int b, int n, IOModule io) { // дискретная среднеквадратичная апроксимация
			this.io = io;
			h = 1.0 * (b - a) / n;
			this.n = n;
			this.a = a;
			this.b = b;
			Function func = new Function(a, b, n);
			this.X = func.GetVectorX();
			this.F = func.GetVectorF(X);
			getPolConsts(); // получить константы полинома
		}
		public void getPolConsts() {
			Matrix A = new Matrix(3, 3);
			Vector B = new Vector(3);

			for(int i = 0; i < X.Length; i++) { // подсчет матриц А и В
				for(int j = 0; j < 3; j++) {
					for(int k = j; k < 3; k++) {
						A[j, k] += Basis.g(j + 1, X[i]) * Basis.g(k + 1, X[i]);
						if(k != j)
							A[k, j] += Basis.g(k + 1, X[i]) * Basis.g(j + 1, X[i]);
					}
				}

				for(int j = 0; j < 3; j++) {
					B[j] += F[i] * Basis.g(j + 1, X[i]);
				}
			}

			io.WriteLine("Matrix:");
			io.WriteLine(A.ToString());
			io.WriteLine("Vector: ");
			io.WriteLine(B.ToString());

			SSRM ssrm = new SSRM(A, B); // решение СЛАУ
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
