using ApproximationTheory;
using Lab1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vectors;

namespace Integration {
	class TrapezoidSpline {
		IOModule io;
		Vector X;
		public TrapezoidSpline(IFunction func, double a, double b, double exactValue, double eps, IOModule io) {
			this.io = io;
			int segmentsNumber = 1;
			double segmentWidth;
			double err;
			double previousValue;
			double currentValue = 0;
			double nextValue = 0;
			double errEst = 0;
			double theta = 1.0 / 15;
			int counter;

			WriteHead();

			do {
				previousValue = currentValue;
				currentValue = nextValue;

				nextValue = 0;

				X = new Vector(segmentsNumber + 1);
				segmentWidth = (b - a) / segmentsNumber;
				for(int i = 0; i <= segmentsNumber; i++)
					X[i] = a + i * segmentWidth;

				nextValue += (func.Solve(X[0]) + func.Solve(X[segmentsNumber])) / 2.0;
				counter = 2;

				for(int i = 1; i < segmentsNumber; i++) {
					nextValue += func.Solve(X[i]);
					counter++;
				}
				nextValue *= segmentWidth;
				nextValue += Math.Pow(segmentWidth, 2) / 12 * (func.d1x(X[0]) - func.d1x(X[segmentsNumber]));
				counter += 2;

				err = Math.Abs(nextValue - exactValue);

				if(segmentsNumber > 1) {
					errEst = theta * (nextValue - currentValue);
				}

				if(segmentsNumber < 4)
					WriteStep(segmentsNumber, segmentWidth, nextValue, err, errEst, 0);
				else
					WriteStep(segmentsNumber,
						segmentWidth,
						nextValue,
						err,
						errEst,
						KValue(previousValue, currentValue, nextValue));


				segmentsNumber *= 2;
			}
			while(err > eps);

			io.WriteLine("Kобр = " + counter);

		}

		private double IntegFi0(double a, double b) {
			double _b = -Math.Pow(b, 4) - Math.Pow(b, 3) + Math.Pow(b, 2) + b;
			double _a = -Math.Pow(a, 4) - Math.Pow(a, 3) + Math.Pow(a, 2) + a;
			return _b - _a;
		}
		private double KValue(double prev, double cur, double next) {
			return Math.Log((next - prev) / (cur - prev) - 1) / Math.Log(0.5);
		}
		private void WriteStep(int N, double H, double integral, double err, double estErr, double k) { // печать шага итерации
			string NStr = string.Format("{0,5}", N);
			string HStr = io.PrettyfyDouble(H, 12);
			string _err = io.PrettyfyDouble(err, 18, 14);
			string estErrStr = io.PrettyfyDouble(estErr, 18, 14);
			string integralStr = io.PrettyfyDouble(integral, 18, 14);
			string KStr = io.PrettyfyDouble(k, 12);

			string str = $"| {NStr} | {HStr} | {integralStr} | {_err} | {estErrStr} | {KStr} |";

			io.WriteLine(str);
		}

		private void WriteHead() { // печать шапки
			string centeredN = io.CenterString("N", 7);
			string centeredH = io.CenterString("h", 14);
			string centeredInt = io.CenterString("Integral", 20);
			string centeredErr = io.CenterString("Error", 20);
			string centeredEstErr = io.CenterString("Estimated Error", 20);
			string centeredK = io.CenterString("K", 14);

			string head = $"|{centeredN}|{centeredH}|{centeredInt}|{centeredErr}|{centeredEstErr}|{centeredK}";

			io.WriteLine(head);
		}
	}
}

