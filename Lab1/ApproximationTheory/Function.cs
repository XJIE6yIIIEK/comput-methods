using System;

namespace ApproximationTheory {
	/// <summary>
	/// Интерфейс функций
	/// </summary>
	interface IFunction {
		/// <summary>
		/// Вычисление значения функции от аргумента
		/// </summary>
		/// <param name="x"></param>
		/// <returns></returns>
		double Solve(double x);

		/// <summary>
		/// Первая производная
		/// </summary>
		/// <param name="x"></param>
		/// <returns></returns>
		double d1x(double x);

		/// <summary>
		/// Четвёртая производная
		/// </summary>
		/// <param name="x"></param>
		/// <returns></returns>
		double d4x(double x);

		/// <summary>
		/// Пятая производная
		/// </summary>
		/// <param name="x"></param>
		/// <returns></returns>
		double d5x(double x);

		double GetIntegrFG(int g, int a, int b);

		double GetIntegrGG(int g1, int g2, int a, int b);

		double GetIntegrFG1(int a, int b);

		double GetIntegrFG2(int a, int b);

		double GetIntegrFG3(int a, int b);

		double GetIntegrG1G1(int a, int b);

		double GetIntegrG1G2(int a, int b);

		double GetIntegrG1G3(int a, int b);

		double GetIntegrG2G2(int a, int b);

		double GetIntegrG2G3(int a, int b);

		double GetIntegrG3G3(int a, int b);
	}

	class F_30 : IFunction {
		public F_30() { }

		public double Solve(double x) {
			return Math.Pow(3, x) + 5 * x - 2;
		}

		public double d1x(double x) {
			return Math.Pow(3, x) * Math.Log(3) + 5;
		}

		public double d4x(double x) {
			return Math.Pow(3, x) * Math.Pow(Math.Log(3), 4);
		}

		public double d5x(double x) {
			return Math.Pow(3, x) * Math.Pow(Math.Log(3), 5);
		}
		public double GetIntegrFG(int g, int a, int b) {
			switch(g) {
				case 1:
				return GetIntegrFG1(a, b);
				case 2:
				return GetIntegrFG2(a, b);
				case 3:
				return GetIntegrFG3(a, b);
				default:
				return 0;
			}
		}
		public double GetIntegrGG(int g1, int g2, int a, int b) {
			if(g1 == 1 && g2 == 1) {
				return GetIntegrG1G1(a, b);
			}
			if(g1 == 1 && g2 == 2 || g1 == 2 && g2 == 1) {
				return GetIntegrG1G2(a, b);
			}
			if(g1 == 1 && g2 == 3 || g1 == 3 && g2 == 1) {
				return GetIntegrG1G3(a, b);
			}
			if(g1 == 2 && g2 == 2) {
				return GetIntegrG2G2(a, b);
			}
			if(g1 == 2 && g2 == 3 || g1 == 3 && g2 == 2) {
				return GetIntegrG2G3(a, b);
			}
			return GetIntegrG3G3(a, b);
		}
		public double GetIntegrFG1(int a, int b) {
			double _b = Math.Pow(3, b) / Math.Log(3) + 2.5 * Math.Pow(b, 2) - 2 * b;
			double _a = Math.Pow(3, a) / Math.Log(3) + 2.5 * Math.Pow(a, 2) - 2 * a;
			return _b - _a;
		}

		public double GetIntegrFG2(int a, int b) {
			double _b = Math.Pow(b, 3) * 5 / 3 - Math.Pow(b, 2) + Math.Pow(3, b) * (b * Math.Log(3) - 1) / Math.Pow(Math.Log(3), 2);
			double _a = Math.Pow(a, 3) * 5 / 3 - Math.Pow(a, 2) + Math.Pow(3, a) * (a * Math.Log(3) - 1) / Math.Pow(Math.Log(3), 2);
			return _b - _a;
		}

		public double GetIntegrFG3(int a, int b) {
			double _b = Math.Pow(b, 4) * 5 / 4 - Math.Pow(b, 3) * 2 / 3 + Math.Pow(3, b) * (Math.Pow(b, 2) * Math.Pow(Math.Log(3), 2) - b * Math.Log(9) + 2) / Math.Pow(Math.Log(3), 3);
			double _a = Math.Pow(a, 4) * 5 / 4 - Math.Pow(a, 3) * 2 / 3 + Math.Pow(3, a) * (Math.Pow(a, 2) * Math.Pow(Math.Log(3), 2) - a * Math.Log(9) + 2) / Math.Pow(Math.Log(3), 3);
			return _b - _a;
		}
		public double GetIntegrG1G1(int a, int b) {
			return b - a;
		}

		public double GetIntegrG1G2(int a, int b) {
			return (Math.Pow(b, 2) - Math.Pow(a, 2)) / 2;
		}

		public double GetIntegrG2G2(int a, int b) {
			return (Math.Pow(b, 3) - Math.Pow(a, 3)) / 3;
		}

		public double GetIntegrG1G3(int a, int b) {
			return (Math.Pow(b, 3) - Math.Pow(a, 3)) / 3;
		}

		public double GetIntegrG2G3(int a, int b) {
			return (Math.Pow(b, 4) - Math.Pow(a, 4)) / 4;
		}
		public double GetIntegrG3G3(int a, int b) {
			return (Math.Pow(b, 5) - Math.Pow(a, 5)) / 5;
		}

	}

	class F_2 : IFunction {
		public F_2() { }

		public double Solve(double x) {
			return Math.Exp(x) + 5 * x - 3;
		}

		public double d1x(double x) {
			return Math.Exp(x) + 5;
		}

		public double d4x(double x) {
			return Math.Exp(x);
		}

		public double d5x(double x) {
			return Math.Exp(x);
		}
		public double GetIntegrFG(int g, int a, int b) {
			switch(g) {
				case 1:
				return GetIntegrFG1(a, b);
				case 2:
				return GetIntegrFG2(a, b);
				case 3:
				return GetIntegrFG3(a, b);
				default:
				return 0;
			}
		}
		public double GetIntegrGG(int g1, int g2, int a, int b) {
			if(g1 == 1 && g2 == 1) {
				return GetIntegrG1G1(a, b);
			}
			if(g1 == 1 && g2 == 2 || g1 == 2 && g2 == 1) {
				return GetIntegrG1G2(a, b);
			}
			if(g1 == 1 && g2 == 3 || g1 == 3 && g2 == 1) {
				return GetIntegrG1G3(a, b);
			}
			if(g1 == 2 && g2 == 2) {
				return GetIntegrG2G2(a, b);
			}
			if(g1 == 2 && g2 == 3 || g1 == 3 && g2 == 2) {
				return GetIntegrG2G3(a, b);
			}
			return GetIntegrG3G3(a, b);
		}
		public double GetIntegrFG1(int a, int b) {
			double _b = Math.Exp(b) - 2 * b + 5.0 / 2 * b * b;
			double _a = Math.Exp(a) - 2 * a + 5.0 / 2 * a * a; ;
			return _b - _a;
		}

		public double GetIntegrFG2(int a, int b) {
			double _b = Math.Exp(b) * (b - 1) + 1.0 / 3 * (5 * b - 3) * b * b;
			double _a = Math.Exp(a) * (a - 1) + 1.0 / 3 * (5 * a - 3) * a * a;
			return _b - _a;
		}

		public double GetIntegrFG3(int a, int b) {
			double _b = 1.0 / 12 * (15 * b - 8) * Math.Pow(b, 3) + Math.Exp(b)*(Math.Pow(b, 2) - 2 * b + 2);
			double _a = 1.0 / 12 * (15 * a - 8) * Math.Pow(a, 3) + Math.Exp(a) * (Math.Pow(a, 2) - 2 * a + 2); ;
			return _b - _a;
		}
		public double GetIntegrG1G1(int a, int b) {
			return b - a;
		}

		public double GetIntegrG1G2(int a, int b) {
			return (Math.Pow(b, 2) - Math.Pow(a, 2)) / 2;
		}

		public double GetIntegrG2G2(int a, int b) {
			return (Math.Pow(b, 3) - Math.Pow(a, 3)) / 3;
		}

		public double GetIntegrG1G3(int a, int b) {
			return (Math.Pow(b, 3) - Math.Pow(a, 3)) / 3;
		}

		public double GetIntegrG2G3(int a, int b) {
			return (Math.Pow(b, 4) - Math.Pow(a, 4)) / 4;
		}
		public double GetIntegrG3G3(int a, int b) {
			return (Math.Pow(b, 5) - Math.Pow(a, 5)) / 5;
		}
	}

	class F_28 : IFunction {
		public F_28() { }

		public double Solve(double x) {
			return Math.Pow(3, x) - 2 * x + 5;
		}
		public double d1x(double x) {
			return Math.Pow(3, x) * Math.Log(3) - 2;
		}
		public double d4x(double x) {
			return Math.Pow(3, x) * Math.Pow(Math.Log(3), 4);
		}

		public double d5x(double x) {
			return Math.Pow(3, x) * Math.Pow(Math.Log(3), 5);
		}
		public double GetIntegrFG(int g, int a, int b) {
			switch(g) {
				case 1:
				return GetIntegrFG1(a, b);
				case 2:
				return GetIntegrFG2(a, b);
				case 3:
				return GetIntegrFG3(a, b);
				default:
				return 0;
			}
		}
		public double GetIntegrGG(int g1, int g2, int a, int b) {
			if(g1 == 1 && g2 == 1) {
				return GetIntegrG1G1(a, b);
			}
			if(g1 == 1 && g2 == 2 || g1 == 2 && g2 == 1) {
				return GetIntegrG1G2(a, b);
			}
			if(g1 == 1 && g2 == 3 || g1 == 3 && g2 == 1) {
				return GetIntegrG1G3(a, b);
			}
			if(g1 == 2 && g2 == 2) {
				return GetIntegrG2G2(a, b);
			}
			if(g1 == 2 && g2 == 3 || g1 == 3 && g2 == 2) {
				return GetIntegrG2G3(a, b);
			}
			return GetIntegrG3G3(a, b);
		}
		public double GetIntegrFG1(int a, int b) {
			double _b = Math.Pow(3, b) / Math.Log(3) - Math.Pow(b, 2) + 5 * b;
			double _a = Math.Pow(3, a) / Math.Log(3) - Math.Pow(a, 2) + 5 * a;
			return _b - _a;
		}

		public double GetIntegrFG2(int a, int b) {
			double _b = -Math.Pow(b, 3) * 2.0 / 3 + 5.0 / 2 * Math.Pow(b, 2) + Math.Pow(3, b) * (b * Math.Log(3) - 1) / Math.Pow(Math.Log(3), 2);
			double _a = -Math.Pow(a, 3) * 2.0 / 3 + 5.0 / 2 * Math.Pow(a, 2) + Math.Pow(3, a) * (a * Math.Log(3) - 1) / Math.Pow(Math.Log(3), 2);
			return _b - _a;
		}

		public double GetIntegrFG3(int a, int b) {
			double _b = -Math.Pow(b, 4) * 1.0 / 2 + Math.Pow(b, 3) * 5.0 / 3 + Math.Pow(3, b) * (Math.Pow(b, 2) * Math.Pow(Math.Log(3), 2) - b * Math.Log(9) + 2) / Math.Pow(Math.Log(3), 3);
			double _a = -Math.Pow(a, 4) * 1.0 / 2 + Math.Pow(a, 3) * 5.0 / 3 + Math.Pow(3, a) * (Math.Pow(a, 2) * Math.Pow(Math.Log(3), 2) - a * Math.Log(9) + 2) / Math.Pow(Math.Log(3), 3);
			return _b - _a;
		}
		public double GetIntegrG1G1(int a, int b) {
			return b - a;
		}

		public double GetIntegrG1G2(int a, int b) {
			return (Math.Pow(b, 2) - Math.Pow(a, 2)) / 2;
		}

		public double GetIntegrG2G2(int a, int b) {
			return (Math.Pow(b, 3) - Math.Pow(a, 3)) / 3;
		}

		public double GetIntegrG1G3(int a, int b) {
			return (Math.Pow(b, 3) - Math.Pow(a, 3)) / 3;
		}

		public double GetIntegrG2G3(int a, int b) {
			return (Math.Pow(b, 4) - Math.Pow(a, 4)) / 4;
		}
		public double GetIntegrG3G3(int a, int b) {
			return (Math.Pow(b, 5) - Math.Pow(a, 5)) / 5;
		}
	}
}
