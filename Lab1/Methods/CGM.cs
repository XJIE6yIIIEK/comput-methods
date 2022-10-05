using Matrices;
using Vectors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Methods{
    internal class CGM{
        private Vector X;
        
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
        public CGM(Matrix A, Vector B, Vector X, Vector startVector, double eps) {
            double a = 1;
            Vector R = getR(A, B, startVector);
            Vector RPrev;
            Vector curX = (Vector)startVector.Clone();
            Vector prevX = (Vector)startVector.Clone();
            Vector newX = new Vector(X.Length);
            double tau = getTau(A, R);
            double tauPrev;

            curX = a * curX - tau * a * R;
            tauPrev = tau;
            RPrev = (Vector)R.Clone();

            while ((X - curX).EnergyNorm(A) >= eps) {
                R = getR(A, B, curX);
                tau = getTau(A, R);
                a = getA(tau, tauPrev, R, RPrev, a);

                newX = a * curX + (1 - a) * prevX - tau * a * R;

                tauPrev = tau;
                RPrev = R;

                prevX = (Vector)curX.Clone();
                curX = (Vector)newX.Clone();

            }

            this.X = (Vector)curX.Clone();
        }
    }
}
