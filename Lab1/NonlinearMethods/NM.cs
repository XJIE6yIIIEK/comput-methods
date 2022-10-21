using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vectors;
using Matrices;
using Lab1;

namespace NonlinearMethods {
    class NM {
        IOModule io;
        Vector curVector;
        Vector prevVector;
        Vector startVector;
        Functions f = new Functions();

        public Vector Answer{
            get { return curVector; }
        }
        public NM(Vector startVector, IOModule io) {
            this.io = io;
            this.prevVector = startVector;
            this.startVector = startVector;
            NewtonMethod();
        } 
        private void NewtonMethod() {
            int iter = 0;
            WriteHead();
            do {
                curVector = prevVector - f.derivF(prevVector) * f.F(prevVector);
                double resNorm = f.F(curVector).Norm();
                prevVector = curVector;

                WriteStep(++iter, curVector, resNorm, f.F(curVector));
            }
            while(f.F(curVector).Norm() > 1E-12);
        }

        private void WriteStep(int iter, Vector X, double residualNorm, Vector F) {
            string iterStr = string.Format("{0,5}", iter);
            string x = io.PrettyfyDouble(X[0], 12);
            string y = io.PrettyfyDouble(X[1], 12);
            string resNorm = io.PrettyfyDouble(residualNorm, 12);
            string F1 = io.PrettyfyDouble(F[0], 12);
            string F2 = io.PrettyfyDouble(F[1], 12);

            string str = $"| {iterStr} | {x} | {y} | {resNorm} | {F1} | {F2}";

            io.WriteLine(str);
        }

        private void WriteHead() { 
            string centeredIter = io.CenterString("Iter", 7);
            string centeredX = io.CenterString("x", 14);
            string centeredY = io.CenterString("y", 14);
            string centeredResNorm = io.CenterString("Residual norm", 14);
            string centeredF1 = io.CenterString("F1", 14);
            string centeredF2 = io.CenterString("F2", 14);

            string head = $"|{centeredIter}|{centeredX}|{centeredY}|{centeredResNorm}|{centeredF1}|{centeredF2}";

            Console.WriteLine(head);
        }

    }
}
