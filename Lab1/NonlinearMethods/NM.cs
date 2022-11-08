using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vectors;
using Matrices;
using Lab1;

namespace NonlinearMethods {
    class NM { // метод Ньютона
        IOModule io;                        // модуль ввода-вывода
        Vector curVector;                   // текущее значение вектора
        Vector prevVector;                  // предыдущее значение вектора
        Vector startVector;                 // стартовое значение вектора 
        IFunctions F1;                      // первое уравнение системы F
        IFunctions F2;                      // второе уравнение системы F
        Matrix derivF = new Matrix(2, 2);   // матрица производных
        Vector F = new Vector(2);           // решение системы F

        public Vector Answer{ // получить ответ
            get { return curVector; }
        }
        public NM(Vector startVector, IOModule io, IFunctions[] functions) {  // конструктор
            this.io = io;
            this.prevVector = startVector;
            this.startVector = startVector;

            F1 = functions[0];
            F2 = functions[1];

            NewtonMethod();
        } 
        private void FillDerivF(Vector X) { // заполнение матрицы производных
            derivF[0, 0] = F1.EvaluateDerivativeX(X);
            derivF[0, 1] = F1.EvaluateDerivativeY(X);
            derivF[1, 0] = F2.EvaluateDerivativeX(X);
            derivF[1, 1] = F2.EvaluateDerivativeY(X);
            derivF = derivF.Inverse;
        }

        private void FillF(Vector X) { // заполнение вектора F
            F[0] = F1.Evaluate(X);
            F[1] = F2.Evaluate(X);
        }
        private void NewtonMethod() {
            int iter = 0; // текущая итерация
            double resNorm;

			WriteHead(); // вывод шапки
            do {
                FillDerivF(prevVector); // пересчитать значения матрицы производных
                FillF(prevVector); // пересчитать значения F

                curVector = prevVector - derivF * F; // подсчет нового значения вектора решений

                FillF(curVector); // пересчитать значения F

                prevVector = curVector; // передать текущее значения в предыдущее для след итерации

                resNorm = F.Norm(); // норма

                double err = (curVector - startVector).Norm();

                WriteStep(++iter, curVector, resNorm, F, err);
            }
            while(resNorm > 1E-12); // подсчет с точностью 10^-12
        } 

        private void WriteStep(int iter, Vector X, double residualNorm, Vector F, double err) { // печать шага итерации
            string iterStr = string.Format("{0,5}", iter);
            string x = io.PrettyfyDouble(X[0], 12);
            string y = io.PrettyfyDouble(X[1], 12);
            string resNorm = io.PrettyfyDouble(residualNorm, 12);
            string F1 = io.PrettyfyDouble(F[0], 12);
            string F2 = io.PrettyfyDouble(F[1], 12);
			string _err = io.PrettyfyDouble(err, 12);

			string str = $"| {iterStr} | {x} | {y} | {resNorm} | {F1} | {F2} | {_err}";

            io.WriteLine(str);
        }

        private void WriteHead() { // печать шапки
            string centeredIter = io.CenterString("Iter", 7);
            string centeredX = io.CenterString("x", 14);
            string centeredY = io.CenterString("y", 14);
            string centeredResNorm = io.CenterString("Residual norm", 14);
            string centeredF1 = io.CenterString("F1", 14);
            string centeredF2 = io.CenterString("F2", 14);

            string head = $"|{centeredIter}|{centeredX}|{centeredY}|{centeredResNorm}|{centeredF1}|{centeredF2}";

            io.WriteLine(head);
        }

    }
}
