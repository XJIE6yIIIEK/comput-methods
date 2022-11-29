using Lab1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vectors;

namespace ApproximationTheory {
	class NewtonMethod {
		Vector X; // сетка
		List<Vector> dd; // таблица разделенных разностей
		Vector F; // вектор значений функции в сетке
		IFunction func; // функция
		double h; // шаг для получения значений сетки
		IOModule io; 
		public NewtonMethod(IFunction func, int a, int b, int n, IOModule io) {
			this.io = io;
			this.func = func;

			h = 1.0*(b - a) / n;

			X = new Vector(n + 1);
			for(int i = 0; i < n + 1; i++) {
				X[i] = a + h * i;
			}

			F = new Vector(n + 1);
			for(int i = 0; i < n + 1; i++) {
				F[i] = func.Solve(X[i]);
			}

			DividedDifferences ddA = new DividedDifferences(X); // получение разделенных разностей
			dd = ddA.GetDiffsList(F);

			io.WriteLine("Divided Differences Table:");
			WriteDDTable();

			io.WriteLine(" ");

			WriteHead();
			for(int i = 0; i < n; i++) {
				double x = 1 + (i + 0.5) * 0.2;
				double f = func.Solve(x);
				double pn = evalPolValue(x);
				double delta = Math.Abs(f - pn);
				double est = Math.Pow(3, x) * Math.Pow(Math.Log(3), 6) * Math.Abs(evalW(x, n+1)) / fact(n+1);
				WriteStep(x, f, pn, delta, est);
			}
		}
		private int fact(int n) { // факториал
			if(n == 1)
				return 1;
			else 
				return fact(n - 1) * n;
		}
		private double evalPolValue(double evalX) { // посчитать значение полинома
			 double summ = 0;
			 for(int i = 0; i < X.Length; i++) {
				summ += dd[i][0] * evalW(evalX, i);
			}
			return summ;
		}

		private double evalW(double evalX, int n) { // посчитать значение омега
			double p = 1;
			for(int i = 0; i < n; i++) {
				p *= evalX - X[i];
			}
			return p;
		}

		private void WriteStep(double x, double f, double pn, double delta, double est) { // печать шага итерации
			string xStr = io.PrettyfyDouble(x, 12);
			string fStr = io.PrettyfyDouble(f, 12);
			string pnStr = io.PrettyfyDouble(pn, 12);
			string deltaStr = io.PrettyfyDouble(delta, 12);
			string estStr = io.PrettyfyDouble(est, 12);

			string str = $"| {xStr} | {fStr} | {pnStr} | {deltaStr} | {estStr}";

			io.WriteLine(str);
		}

		private void WriteHead() { // вывод шапки
			string centeredX = io.CenterString("X", 14);
			string centeredF = io.CenterString("f(x)", 14);
			string centeredPn = io.CenterString("pn(x)", 14);
			string centeredDelta = io.CenterString("delta", 14);
			string centeredEst = io.CenterString("estimate", 14);

			string head = $"|{centeredX}|{centeredF}|{centeredPn}|{centeredDelta}|{centeredEst}";

			io.WriteLine(head);
		}

		private void WriteDDTable() { // вывод таблицы разделенных разностей
			for(int i = 0; i < dd.Count; i++) {
				string str = "";
				str += io.CenterString(X[i].ToString(), 8);
				for(int j = 0; j < dd.Count - i; j++) {
					string s = io.CenterString(dd[j][i].ToString(), 18);
					str += s;
				}
				io.WriteLine(str);
			}
		}
	}
}
