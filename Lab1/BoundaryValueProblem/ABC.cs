using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoundaryValueProblem {
	interface ABC {
		double A(double x);
		double B(double x);
		double C(double x);
	}
	class ABC2 : ABC {
		public double A(double x) {
			return 40 * (x + 1);
		}
		public double B(double x) {
			return x * x + 2;
		}
		public double C(double x) {
			return 2 * x + 1;
		}
	}
	class ABC28 : ABC {
		public double A(double x) {
			return 50 * (-x + 0.5);
		}
		public double B(double x) {
			return Math.Pow(x, 2) + 1;
		}
		public double C(double x) {
			return x + 2;
		}
	}
}
