using System;
using ApproximationTheory;
using Lab1;

namespace Integration {
	internal class Gauss {
		IOModule io;
		double answer;

		public double Answer {
			get {
				return answer;
			}
		}

		public Gauss(IFunction func, double exactVal, double left, double right, double eps, IOModule io) {
			this.io = io;

			int segmentsNumber = 1;
			double width;
			double previousValue;
			double currentValue = 0;
			double nextValue = 0;
			double theta = 1.0 / 63;
			double error;
			double estErr = 0;

			WriteHead();

			do {
				previousValue = currentValue;
				currentValue = nextValue;

				nextValue = 0;
				width = (right - left) / segmentsNumber;

				for(int step = 0; step < segmentsNumber; step++) {
					double x = left + 0.5 * (1 + 2 * step) * width;

					nextValue += width / 2.0 * (func.Solve(x - width * Math.Sqrt(3.0 / 5) / 2.0) * (5 / 9.0) 
						+ (8 / 9.0) * func.Solve(x) 
						+ (5 / 9.0) * func.Solve(x + width * Math.Sqrt(3 / 5.0) / 2.0));
				}

				error = nextValue - exactVal;

				if(segmentsNumber > 1) {
					estErr = theta * (nextValue - currentValue);
				}

				if(segmentsNumber < 4) {
					WriteStep(segmentsNumber, width, nextValue, error, estErr, 0);
				} else {
					WriteStep(segmentsNumber, width, nextValue, error, estErr, KValue(previousValue, currentValue, nextValue));
				}

				segmentsNumber *= 2;
			} while(error > eps || segmentsNumber <= 4);

			answer = currentValue;
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