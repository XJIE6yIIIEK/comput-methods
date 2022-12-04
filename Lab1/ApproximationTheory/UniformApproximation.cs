using System;
using Lab1;

namespace ApproximationTheory {
	internal class UniformApproximationP2 {
		IOModule io;

		IFunction func;
		double a0;
		double a1;

		public UniformApproximationP2(IFunction func, double a, double b, int n, IOModule io) {
			this.func = func;
			this.io = io;

			a1 = (func.Solve(b) - func.Solve(a)) / (b - a);
			double d = HalfDivideMethod(a1, a, b, 1E-12);
			a0 = (func.Solve(a) + func.Solve(d) - a1 * (a + d)) / 2;

			double La = func.Solve(a) - (a0 + a1 * a);
			double Ld = func.Solve(d) - (a0 + a1 * d);
			double Lb = func.Solve(b) - (a0 + a1 * b);

			io.WriteLine($"P1(x) = {io.PrettyfyDouble(a0, 6)} + ({a1}) * x");
			io.WriteLine($"d = {io.PrettyfyDouble(d, 6)}");
			io.WriteLine($"L(a) = {io.PrettyfyDouble(La, 6)}");
			io.WriteLine($"L(d) = {io.PrettyfyDouble(Ld, 6)}");
			io.WriteLine($"L(b) = {io.PrettyfyDouble(Lb, 6)}");
			io.WriteLine();

			WriteHead();

			for(int i = 0; i < n + 1; i++) {
				double x = a + 0.2 * i;
				double err = (a0 + a1 * x) - func.Solve(x);

				WriteStep(x, err);
			}
		}

		/// <summary>
		/// Метод половинного деления
		/// </summary>
		/// <param name="a1"></param>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <param name="eps"></param>
		/// <returns></returns>
		private double HalfDivideMethod(double a1, double a, double b, double eps) {
			double c;

			do {
				c = (a + b) / 2;

				if((func.d1x(a) - a1) * (func.d1x(c) - a1) > 0) {
					a = c;
				} else {
					b = c;
				}
			} while(Math.Abs(a - b) > eps);

			return c;
		}

		/// <summary>
		/// Вычисление полученного полинома первой степени
		/// </summary>
		/// <param name="x"></param>
		/// <returns></returns>
		public double Solve(double x) {
			return a0 + a1 * x;
		}

		private void WriteStep(double x, double err) { // печать шага итерации
			string xStr = io.PrettyfyDouble(x, 5);
			string errStr = io.PrettyfyDouble(err, 12);

			string str = $"| {xStr} | {errStr} |";

			io.WriteLine(str);
		}

		private void WriteHead() { // печать шапки
			string centeredX = io.CenterString("X", 10);
			string centeredErr = io.CenterString("Estimate error", 14);

			string head = $"|{centeredX}|{centeredErr}|";

			io.WriteLine(head);
		}
	}
}
