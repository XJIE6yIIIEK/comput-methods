using Lab1;
using Lab1.Matrices;
using Matrices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vectors;

namespace ApproximationTheory {
	class CubicSplinesMethod {
		IOModule io; 
		double h; // шаг сетки
		IFunction func; // функция
		Vector f; // значение функции 
		Vector dF; // значение производной функции 
		Vector m; // m[i]
		Vector X; // узлы сетки
		int n;
		public Vector Answer {
			get { return m; }
		}
		public CubicSplinesMethod(IFunction func, double a, double b, int n, IOModule io) {
			this.io = io;
			h = 1.0 * (b - a) / n;
			this.n = n;
			this.func = func;

			X = new Vector(n + 1);
			for(int i = 0; i < n + 1; i++) {
				X[i] = a + h * i;
			}

			f = new Vector(n + 1);
			for(int i = 0; i < n + 1; i++) {
				f[i] = func.Solve(X[i]);
			}

			dF = new Vector(n + 1);
			for(int i = 0; i < n + 1; i++) {
				dF[i] = func.d1x(X[i]);
			}

			m = evalM(n);

			int indM5 = 0; //агрумент максимума из значений производной 5го порядка по узлам сетки
			for(int i = 0; i <= n; i++) { 
				if(func.d5x(X[i]) >
					func.d5x(X[indM5])){
					indM5 = i;
				}
			}

			int indM4 = 0; // агрумент максимума из значений производной 4го порядка по узлам сетки
			for(int i = 0; i <= n; i++) {
				if(func.d4x(X[i]) >
					func.d4x(X[indM4])) {
					indM4 = i;
				}
			}
			double M4 = func.d4x(X[indM4]);
			double M5 = func.d5x(X[indM5]);
			double estM = M5 * Math.Pow(0.2, 4) / 60; // погрешность
			double estSpline = (M4 / 384 + M5 / 240 * h) *Math.Pow(h, 4); // погрешность

			io.WriteLine("M4: " + M4);
			io.WriteLine("M5: " + M5);
			io.WriteLine(" ");

			WriteHeadM();

			for(int i = 0; i < n + 1; i++) {
				double delta = Math.Abs(dF[i] - m[i]);
				WriteStep(X[i], dF[i], m[i], delta, estM);
			}
			io.WriteLine("\n");

			WriteHeadF();
			for(int i = 0; i < n; i++) {
				double x = a + (i + 0.5) * h;
				double f = func.Solve(x);
				double splineValue = evalSpline(x);
				double delta = Math.Abs(f - splineValue);
				WriteStep(x, f, splineValue, delta, estSpline);
			}
		}

		private double evalSpline(double x) { // посчитать значение в сплайне
			int curI = 0;
			for(int i = 0; i < n; i++) {
				if(x > X[i] && x < X[i + 1]) {
					curI = i;
					break;
				}
			}
			double t = (x - X[curI]) / h;
			return f[curI] * Fi0(t) + Fi0(1 - t) * f[curI + 1] +
					h * (Fi1(t) * m[curI] - Fi1(1 - t) * m[curI + 1]);
		}

		private double Fi0(double t) { // фи0
			return (1 + 2 * t) * (1 - t) * (1 - t);
		}

		private double Fi1(double t) { // фи1
			return t * (1 - t) * (1 - t);
		}
		private Vector evalM(int n) { // посчитать вектор m
			Matrix a = new Matrix(n - 1, n - 1); // трехдиагональная матрица
			Vector b = new Vector(n - 1);
			a[0, 0] = 2;
			a[0, 1] = 0.5;
			b[0] = 1.5 * (f[2] - f[0]) / h - dF[0] / 2;

			for(int i = 1; i < a.Cols - 1; i++) { 
				a[i, i - 1] = 0.5;
				a[i, i] = 2;
				a[i, i + 1] = 0.5;

				b[i] = 1.5 * (f[i + 2] - f[i]) / h;
			}
			a[a.Cols - 1, a.Cols - 2] = 0.5;
			a[a.Cols - 1, a.Cols - 1] = 2;
			b[b.Length - 1] = 1.5 * (f[n] - f[n - 2]) / h - dF[n] / 2;

			SweepMethod sw = new SweepMethod(a, b); 
			Vector ans = new Vector(sw.Answer.Length + 2); // решение трехдиагональной матрицы
			ans[0] = dF[0];
			for(int i = 1; i < ans.Length - 1; i++) {
				ans[i] = sw.Answer[i - 1];
			}
			ans[ans.Length - 1] = dF[n];
			return ans;
		}

		private void WriteStep(double x, double df, double m, double delta, double est) { // печать шага итерации
			string xStr = io.PrettyfyDouble(x, 12);
			string dfStr = io.PrettyfyDouble(df, 12);
			string mStr = io.PrettyfyDouble(m, 12);
			string deltaStr = io.PrettyfyDouble(delta, 12);
			string estStr = io.PrettyfyDouble(est, 12);

			string str = $"| {xStr} | {dfStr} | {mStr} | {deltaStr} | {estStr}";

			io.WriteLine(str);
		}
		private void WriteHeadM() { // вывод шапки для m
			string centeredX = io.CenterString("X[i]", 14);
			string centeredDF = io.CenterString("f'(x)", 14);
			string centeredM = io.CenterString("m[i]", 14);
			string centeredDelta = io.CenterString("delta", 14);
			string centeredEst = io.CenterString("estimate", 14);

			string head = $"|{centeredX}|{centeredDF}|{centeredM}|{centeredDelta}|{centeredEst}";

			io.WriteLine(head);
		}
		private void WriteHeadF() { // вывод шапки для f
			string centeredX = io.CenterString("X[i]", 14);
			string centeredF = io.CenterString("f(x)", 14);
			string centeredS = io.CenterString("S31(f;x)", 14);
			string centeredDelta = io.CenterString("delta", 14);
			string centeredEst = io.CenterString("estimate", 14);

			string head = $"|{centeredX}|{centeredF}|{centeredS}|{centeredDelta}|{centeredEst}";

			io.WriteLine(head);
		}
	}
}
