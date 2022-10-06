using Matrices;
using Vectors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lab1;

namespace Methods{
    internal class CGM {
        private Vector X;           //Вычисленный вектор X
		private IOModule io;        //Модуль IO

		private int theoretical;    //Теоретическое количество итераций

		public int Theoretical {
			get { return theoretical; }
		}

		public Vector Answer {
            get { return X; }
        }

        private Vector getR(Matrix A, Vector B, Vector curX) {
            return A * curX - B;
        }

        private double getTau(Matrix A, Vector R) {
            return Vector.Dot(R, R) / Vector.Dot(A * R, R);
        }

        private double getA(double tau1, double tau, Vector R1, Vector R, double a) {
            double res = 1 - tau1 / tau / a * Vector.Dot(R1, R1) / Vector.Dot(R, R);
            return 1 / res;
        }

        public CGM(Matrix A, Vector B, Vector X, double eps, IOModule io) {
            this.io = io;

            theoretical = (int)(Math.Sqrt(A.EuclideCondition) * Math.Log(1 / eps) / 2);

			int iter = 1;
            double a = 1;

            Vector startVector = (Vector)B.Clone();

            Vector R = getR(A, B, startVector);
            Vector RPrev;
            Vector curX = (Vector)startVector.Clone();
            Vector prevX = (Vector)startVector.Clone();
            Vector newX;
            double tau = getTau(A, R);
            double tauPrev;
            double residualNorm = (B - A * curX).EnergyNorm(A);


            curX = a * curX - tau * a * R;
            this.WriteStep(iter, tau, residualNorm, (X - curX).EnergyNorm(A), curX, a);

            tauPrev = tau;
            RPrev = (Vector)R.Clone();

            while ((X - curX).EnergyNorm(A) >= eps) {
                iter++;

                residualNorm = (B - A * curX).EnergyNorm(A);

                R = getR(A, B, curX);
                tau = getTau(A, R);
                a = getA(tau, tauPrev, R, RPrev, a);

                newX = a * curX + (1 - a) * prevX - tau * a * R;

                this.WriteStep(iter, tau, residualNorm, (X - newX).EnergyNorm(A), newX, a);

                tauPrev = tau;
                RPrev = R;

                prevX = (Vector)curX.Clone();
                curX = (Vector)newX.Clone();

            }

            this.X = (Vector)curX.Clone();
        }

        private void WriteStep(int iter, double tau, double residualNorm, double errNorm, Vector X, double alpha)
        {
            string iterStr = string.Format("{0,5}", iter);
            string tauIter = io.PrettyfyDouble(tau, 12);
            string residualNormStr = io.PrettyfyDouble(residualNorm, 12);
            string errNormStr = io.PrettyfyDouble(errNorm, 12);
            string a = io.PrettyfyDouble(alpha, 12);

            string str = $"| {iterStr} | {tauIter} | {residualNormStr} | {errNormStr} | {X.ToString()} | {a}";

            io.WriteLine(str);
        }

		private void WriteHead(int count) {
			string centeredIter = io.CenterString("Iter", 7);
			string centeredTau = io.CenterString("Tau", 14);
			string centeredQ = io.CenterString("Q", 14);
			string centeredResNorm = io.CenterString("Residual norm", 14);
			string centeredErrNorm = io.CenterString("Error norm", 14);
			string centeredErrEst = io.CenterString("Error estimate", 14);

			string x = " ";
			for(int i = 0; i < count; i++) {
				string centeredX = io.CenterString($"X[{i + 1}]", 14);
				x = x + centeredX;
			}

			string head = $"|{centeredIter}|{centeredTau}|{centeredQ}|{centeredResNorm}|{centeredErrNorm}|{centeredErrEst}|{x}";

			io.WriteLine(head);
		}
	}
}
