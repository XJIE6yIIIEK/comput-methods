using Matrices;
using Vectors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1.Methods{
    internal class CGM{
        private Vector X;
        
        private Vector getR(Matrix A, Vector B, Vector curX){
            return A * curX - B;
        }

        private double getTau(Matrix A, Vector R){
            return Vector.Dot(R, R) / Vector.Dot(A * R, R);
        }

        private double getA(double tau1, double tau, Vector R1, Vector R, double a){
            double res = 0;
            res = 1 - tau1 / tau / a * Vector.Dot(R1, R1) / Vector.Dot(R, R);
            return 1 / res;
        }
        public CGM(Matrix A, Vector B, Vector X, Vector startVector, double eps){
            Vector a = new Vector(B.Length);
            a.Fill(new double[4] {1, 1, 1, 1});
            Vector R = getR(A, B, X);
            double tau = getTau(A, R);

        }
    }
}
