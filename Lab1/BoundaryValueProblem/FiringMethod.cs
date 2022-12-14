using Lab1;
using System;

namespace BoundaryValueProblem {
	class FiringMethod {
		int var;
		ABC abc;
		IOModule io;	
		public FiringMethod(double eps, int var, ABC abc, IOModule io) {
			this.abc = abc;
			this.var = var;
			this.io = io;
			int iter = 1;
			double a0 = 0;
			double a1 = 3;
			double y1B = 2;
			double est = 0;
			double a;
			double y = 0;
			WriteHeadFiringMethod();
			while(Math.Abs(y - y1B) > eps)  {
				a = (a0 + a1) / 2;

				Tuple<double, double> t = RungeKutte(a, 1, false);

				est = t.Item2;

				y = t.Item1;

				if(y < y1B)
					a0 = a;
				else
					a1 = a;

				WriteStep(iter, a, y, est);

				iter++;

			} 
			RungeKutte(1.0000269412994385, 1, true);
		}
		public Tuple<double, double> RungeKutte(double a, double _x, bool table) {
			if(table)
				WriteHeadRungeKutte();
			double delta = 0;
			double x = 0;
			double y = 1;
			double z = a;
			double yPrev = y;
			double zPrev = z;
			double y1= 0, y2 = 0, y3 = 0;
			double z1 = 0, z2 = 0, z3 = 0;
			while(x < _x) {
				double h = 0.1;
				do {
					yPrev = y;
					zPrev = z;
					Tuple<double, double> t1 = find_h(h, x, yPrev, zPrev);
					y1 = t1.Item1;
					z1 = t1.Item2;
					t1 = find_h(h / 2, x, yPrev, zPrev);
					y2 = t1.Item1;
					z2 = t1.Item2;
					t1 = find_h(h / 2, x, y2, z2);
					y3 = t1.Item1;
					z3 = t1.Item2;
					h /= 2;
				} while(Math.Abs(y1 - y3) > 1E-5);
				double err = Math.Abs(Yderiv(x) - yPrev);
				if(err > delta)
					delta = err;
				if(table)
					WriteStepRungeKutte(x, y, Yderiv(x), z, err);
					yPrev = y;
				h *= 2;
				Tuple<double, double> t = find_h(h, x, y, z);
				y = t.Item1;
				z = t.Item2;
				x += h;
			}
			return new Tuple<double, double>(yPrev, delta);
		}

		public double Yderiv(double x) {
			return 1 + x + 10 * Math.Log(var + 1) * Math.Pow(x, 3) * Math.Pow(1 - x, 3);
		}
		public double YderivDx(double x) {
			return 1 - 30 * Math.Log(var + 1) * Math.Pow(x, 2) * Math.Pow(1 - x, 2) * (2 * x - 1);
		}
		public double YderivDx2(double x) {
			return -60 * Math.Log(var + 1) * x * (x - 1) * (5 * Math.Pow(x, 2) - 5 * x + 1);
		}
		public double F(double x, double A, double B, double C) {
			return YderivDx2(x) + A * YderivDx(x) - B * Yderiv(x) + C * Math.Sin(Yderiv(x));
		}
		public double f(double x, double y, double z) {
			double A = abc.A(x);
			double B = abc.B(x);
			double C = abc.C(x);
			return -A * z + B * y - C * Math.Sin(y) + F(x, A, B, C);
		}
		public Tuple<double, double> find_h(double h, double x, double y, double z) {
			double k1 = h * z;
			double l1 = h * f(x, y, z);
			double k2 = h * (z + l1 / 2);
			double l2 = h * f(x + h / 2, y + k1 / 2, z + l1 / 2);
			double k3 = h * (z + l2 / 2);
			double l3 = h * f(x + h / 2, y + k2 / 2, z + l2 / 2);
			double k4 = h * (z + l3);
			double l4 = h * f(x + h, y + k3, z + l3);
			y = y + (k1 + 2 * k2 + 2 * k3 + k4) / 6;
			z = z + (l1 + 2 * l2 + 2 * l3 + l4) / 6;
			return new Tuple<double, double>(y, z);
		}
		private void WriteHeadFiringMethod() { // печать шапки
			string iterStr = io.CenterString("Iter", 7);
			string zStr = io.CenterString("z(0)", 20);
			string yStr = io.CenterString("y(1)", 20);
			string estStr = io.CenterString("Est", 20);

			string head = $"|{iterStr}|{zStr}|{yStr}|{estStr}";

			io.WriteLine(head);
		}
		private void WriteStep(int iter, double z, double y, double est) { // печать шага итерации
			string iterStr = string.Format("{0,5}", iter);
			string zStr = io.PrettyfyDouble(z, 18);
			string yStr = io.PrettyfyDouble(y, 18, 14);
			string errStr = io.PrettyfyDouble(est, 18, 14);

			string str = $"| {iterStr} | {zStr} | {yStr} | {errStr}";

			io.WriteLine(str);
		}

		private void WriteHeadRungeKutte() { // печать шапки
			string iterStr = io.CenterString("x", 14);
			string yStr = io.CenterString("y(x)", 14);
			string yDerivStr = io.CenterString("Yderiv(x)", 20);
			string zStr = io.CenterString("z(x)", 20);
			string estErr = io.CenterString("Est", 20);

			string head = $"|{iterStr}|{yStr}|{yDerivStr}|{zStr}|{estErr}";

			io.WriteLine(head);
		}
		private void WriteStepRungeKutte(double x, double y, double yD, double z, double est) { // печать шага итерации
			string xStr = io.PrettyfyDouble(x, 12);
			string yStr = io.PrettyfyDouble(y, 12);
			string yDStr = io.PrettyfyDouble(yD, 18, 14);
			string zStr = io.PrettyfyDouble(z, 18, 14);
			string estStr = io.PrettyfyDouble(est, 18, 14);

			string str = $"| {xStr} | {yStr} | {yDStr} | {zStr} | {estStr}";

			io.WriteLine(str);
		}
	}
}
