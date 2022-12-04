using Vectors;
using Matrices;
using Lab1;

namespace ApproximationTheory {
	internal class ReverseRootMethod {
		IFunction func; //Функция
		Vector X; //Узлы интерполяции
		Matrix dt; //Таблица разностей
		double answer; //Ответ

		public double Answer {
			get {
				return answer;
			}
		}

		public ReverseRootMethod(IFunction func, double a, double b, double y, int n, IOModule io) {
			this.func = func;

			double step = (b - a) / n;
			X = new Vector(n + 1);

			for(int i = 0; i < n + 1; i++) {
				X[i] = a + step * i;
			}

			CalculateDT(y, n + 1);

			io.WriteLine($"Difference table");
			io.WriteLine(dt.ToString());
			io.WriteLine();
			io.WriteLine($"c = {io.PrettyfyDouble(y, 6)}");


			CalculatePolynom(y, y, n + 1);

			io.WriteLine($"Root = {io.PrettyfyDouble(answer, 6)}");

			double residual = func.Solve(answer) - y;

			io.WriteLine($"Residual = {residual}");
		}

		/// <summary>
		/// Расчёт таблицы разностей
		/// </summary>
		/// <param name="c"></param>
		/// <param name="n"></param>
		void CalculateDT(double c, int n) {
			dt = new Matrix(n, n + 1);

			for(int i = 0; i < n; i++) {
				dt[i, 1] = X[i];
				dt[i, 0] = func.Solve(X[i]) - c;
			}

			for(int j = 2, k = -1; j < n + 1; j++) {
				k++;

				for(int i = 0; i < n; i++) {
					if(i + j < n + 1) {
						dt[i, j] = (dt[i + 1, j - 1] - dt[i, j - 1]) / (dt[i + k + 1, 0] - dt[i, 0]);
					}
				}
			}
		}

		/// <summary>
		/// Вычисление значения полученного полинома от аргумента
		/// </summary>
		/// <param name="x"></param>
		/// <param name="c"></param>
		/// <param name="n"></param>
		void CalculatePolynom(double x, double c, int n) {
			double px = dt[0, 1];

			for(int i = 2; i < n + 1; i++) {
				double newP = 1;

				for(int j = 0, str = 0; j < i - 1; j++, str++) {
					newP *= x - dt[str, 0] - c;
				}

				px += dt[0, i] * newP;
			}

			answer = px;
		}
	}
}
