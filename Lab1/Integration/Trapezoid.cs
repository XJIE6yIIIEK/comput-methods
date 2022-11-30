using ApproximationTheory;
using Lab1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Integration {
	class Trapezoid {
		IOModule io;
		public Trapezoid(IFunction func, double a, double b, double exactValue, double eps, IOModule io) {
			this.io = io;
			int segmentsNumber = 1;
			double segmentWidth;
			double err;
			double theta = 1.0 / 15;
			double errEst = 0;
			double previousValue;
			double currentValue = 0;
			double nextValue = 0;
			double leftFunc;
			double rightFunc;
			int counter;

			WriteHead();

			do {
				double x1 = a;
				double x2;
				previousValue = currentValue;
				currentValue = nextValue;

				leftFunc = func.Solve(x1);
				counter = 1;

				nextValue = 0;
				segmentWidth = 1.0 * (b - a) / segmentsNumber;


				for(int i = 0; i < segmentsNumber; i++) {
					x2 = x1 + segmentWidth;
					rightFunc = func.Solve(x2);
					counter++;
					nextValue += segmentWidth * (leftFunc + rightFunc) / 2;
					leftFunc = rightFunc;
					x1 = x2;
				}

				err = nextValue - exactValue;

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
