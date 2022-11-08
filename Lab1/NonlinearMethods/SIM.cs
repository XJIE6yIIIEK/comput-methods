using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lab1;
using Vectors;
using Matrices;

namespace NonlinearMethods {
    class SIM {
		IOModule io;							// модуль ввода - вывода
        Vector NMsolution;						// точное решение	
        Vector startVector;						// вектор начальных значений
        Vector curVector;						// текущее значение вектора
        Vector prevVector;						// предыдущее значение вектора
		IFunctions Fi1;							// первое уравнение системы Fi
		IFunctions Fi2;							// второе уравнение системы Fi
		IFunctions F1;							// первое уравнение системы F
		IFunctions F2;							// второе уравнение системы F
		Matrix jacobian = new Matrix(2, 2);		// якобиан
		Vector Fi = new Vector(2);				// система Fi
		double errResNorm;                      // норма невязки
		double errEst;

		public SIM(Vector solution, Vector startVector, IOModule io, double E, IFunctions[] functions) { // конструктор
			this.io = io;
			this.NMsolution = solution;
			this.startVector = startVector;
			prevVector = startVector;

			F1 = functions[0];
			F2 = functions[1];
			Fi1 = functions[2];
			Fi2 = functions[3];

			SimpleIterationMethod(E);
		}
		private Matrix GetJacobian(Vector X) { // получить пересчитанный якобиан
			Matrix m = new Matrix(2, 2);
			m[0, 0] = Fi1.EvaluateDerivativeX(X);
			m[0, 1] = Fi1.EvaluateDerivativeY(X);
			m[1, 0] = Fi2.EvaluateDerivativeX(X);
			m[1, 1] = Fi2.EvaluateDerivativeY(X);
			return m;
		}
		private Vector GetFi(Vector X) { // получить вектор Fi
			Vector v = new Vector(2);
			v[0] = Fi1.Evaluate(X);
			v[1] = Fi2.Evaluate(X);
			return v;
		}
		private Vector GetF(Vector X) { // получить вектор F
			Vector v = new Vector(2);
			v[0] = F1.Evaluate(X);
			v[1] = F2.Evaluate(X);
			return v;
		}
		private void SimpleIterationMethod(double E) { // Метод Простых Итераций
			int itr = 1; // номер итерации
			WriteHead(); // печать шапки
			do {
				curVector = GetFi(prevVector); // пересчитать значение текущего вектора
				jacobian = GetJacobian(curVector); // якобиан
				errEst = jacobian.EuclideNorm * (curVector - prevVector).Norm() / (1 - jacobian.EuclideNorm);
				prevVector = curVector; // передать текущее значение в предыдущее для след итерации

				errResNorm = (GetF(curVector) - GetF(NMsolution)).Norm(); // норма невязки
				double err = (curVector - NMsolution).Norm(); // погрешность
				
				WriteStep(itr++, curVector, errResNorm, errEst, err, jacobian.EuclideNorm);
			}
			while(errResNorm >= E); // условие выхода - норма невязки < E
		}

		private void WriteStep(int iter, Vector X, double residualNorm, double errEstimate, double errNorm, double jNorm) { // печать шага итерации
			string iterStr = string.Format("{0,5}", iter);
			string x = io.PrettyfyDouble(X[0], 12);
			string y = io.PrettyfyDouble(X[1], 12);
			string resNorm = io.PrettyfyDouble(residualNorm, 12);
			string errEst = io.PrettyfyDouble(errEstimate, 12);
			string errN = io.PrettyfyDouble(errNorm, 12);
			string jacobianNorm = io.PrettyfyDouble(jNorm, 12);

			string str = $"| {iterStr} | {x} | {y} | {resNorm} | {errEst} | {errN} | {jacobianNorm}";

			io.WriteLine(str);
		}

		private void WriteHead() { // печать шапки
			string centeredIter = io.CenterString("Iter", 7);
			string centeredX = io.CenterString("x", 14);
			string centeredY = io.CenterString("y", 14);
			string centeredResNorm = io.CenterString("Residual norm", 14);
			string errEstimate = io.CenterString("Error Estimate", 14);
			string errNorm = io.CenterString("Error norm", 14);
			string jNorm = io.CenterString("Jacobian norm", 14);

			string head = $"|{centeredIter}|{centeredX}|{centeredY}|{centeredResNorm}|{errEstimate}|{errNorm}|{jNorm}";

			io.WriteLine(head);
		}
	}
}
