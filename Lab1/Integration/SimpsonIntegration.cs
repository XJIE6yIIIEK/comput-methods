using System;
using ApproximationTheory;
using Lab1;

namespace Integration {
	internal class SimpsonIntegration {
		IOModule io;
		double answer;

		public double Answer {
			get {
				return answer;
			}
		}

		/// <summary>
		/// Метод Симпсона
		/// </summary>
		/// <param name="func"></param>
		/// <param name="exactVal"></param>
		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <param name="eps"></param>
		/// <param name="io"></param>
		public SimpsonIntegration(IFunction func, double exactVal, double left, double right, double eps, IOModule io) {
			this.io = io;

			int segmentsNumber = 1;
			double width;
			double previousValue;
			double currentValue = 0;
			double nextValue = 0;
			double theta = 1.0 / 15;
			double error;
			double estErr = 0;
			double funcLeft = func.Solve(left);
			int counter;

			WriteHead();

			do {
				double x1 = left;
				previousValue = currentValue;
				currentValue = nextValue;
				double previousFuncValue = funcLeft;

				nextValue = 0;
				width = (right - left) / segmentsNumber;
				counter = 1;

				for(int step = 0; step < segmentsNumber; step++) {
					double x2 = x1 + width;
					double newFuncValue = func.Solve(x2);
					nextValue += (x2 - x1) / 6 * (previousFuncValue + 4 * func.Solve(0.5 * (x1 + x2)) + newFuncValue);
					previousFuncValue = newFuncValue;
					x1 = x2;
					counter += 2;
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
			} while(error > eps);

			answer = currentValue;

			io.WriteLine("Kобр = " + counter);
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
